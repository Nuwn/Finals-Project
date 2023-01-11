using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Finals_API.Models;
using Finals_API.Data;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Finals_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly FinalDatabaseContext _context;

        private readonly JwtTokenGenerator _jwtTokenGenerator;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;


        const int keySize = 64;
        const int iterations = 350000;
        readonly HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;


        public UsersController(FinalDatabaseContext context, JwtTokenGenerator jwtTokenGenerator, JwtSecurityTokenHandler jwtSecurityTokenHandler)
        {
            _context = context;
            _jwtTokenGenerator = jwtTokenGenerator;
            _jwtSecurityTokenHandler = jwtSecurityTokenHandler;
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthSuccessResponse>> Register(Account account)
        {
            if (_context.Users == null)
            {
                 return Problem("Entity set 'Users'  is null.");
            }

            (string, string) HashedPassword = HashPassword(account.Password);

            string id = Guid.NewGuid().ToString();
            User user = new()
            {
                Id = id,
                Email = account.Email,
                EmailNormalized = account.Email.ToLower(),
                Password = HashedPassword.Item1,
                Salt = HashedPassword.Item2,
                Activated = true,
                Locked = false,
                Score = new Score(),
                Profile = new Profile() { UserName = account.Username ?? "NoName" }
            };

            _context.Users.Add(user);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserExists(user.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            //Generate a JWT token
            var token = _jwtTokenGenerator.GenerateToken(user);

            return new AuthSuccessResponse
            {
                Token = token
            };
        }

        [HttpPost("Login")]
        public async Task<ActionResult<AuthSuccessResponse>> Login(Account account)
        {
            // Check if the email and password combination is correct
            var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == account.Email);

            if (dbUser == null || !VerifyPassword(account.Password, dbUser.Password, Convert.FromHexString(dbUser.Salt!)))
            {
                return Unauthorized();
            }

            //Generate a JWT token
            var token = _jwtTokenGenerator.GenerateToken(dbUser);

            //Return the user and the JWT token
            return new AuthSuccessResponse
            {
                Token = token
            };
        }



        // DELETE: api/Users/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Profile>> DeleteUser(string Id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(Id);

            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<ActionResult<AuthSuccessResponse>> GetProfile()
        {
            var idClaim = HttpContext.User.Claims.FirstOrDefault(i => i.Type == "UserId");

            if (idClaim == null)
            {
                return BadRequest("ID claim not found in JWT.");
            }

            var dbprofile = await _context.Profiles.FirstOrDefaultAsync(u => u.UserId == idClaim.Value);

            if(dbprofile == null)
            {
                return BadRequest("Profile not found");
            }

            // Return user information in response
            return Ok(new {
                Username = dbprofile.UserName
            });

        }



        private bool UserExists(string id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private (string, string) HashPassword(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(keySize);

            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                iterations,
                hashAlgorithm,
                keySize);

            return (Convert.ToHexString(hash), Convert.ToHexString(salt));
        }

        private bool VerifyPassword(string password, string hash, byte[] salt)
        {
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                iterations,
                hashAlgorithm,
                keySize);

            return hashToCompare.SequenceEqual(Convert.FromHexString(hash));
        }
    }

    public class AuthSuccessResponse
    {
        public string Token { get; set; }
    }
}
