using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Finals_API.Data;
using Finals_API.Models;
using Microsoft.AspNetCore.Authorization;

namespace Finals_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScoreController : ControllerBase
    {
        private readonly FinalDatabaseContext _context;

        public ScoreController(FinalDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Score
        [HttpGet("highscore")]
        public async Task<ActionResult<IEnumerable<Score>>> GetScores()
        {
            if (_context.Scores == null)
            {
                return NotFound();
            }

            var scores = await _context.Users.Include(s => s.Score)
                .Include(p => p.Profile)
                .OrderByDescending(x => x.Score!.Solved)
                .Take(10)
                .Select(u => new
                {
                    u.Profile!.UserName,
                    u.Score!.Solved,
                    u.Score.Attempts,
                })
                .ToListAsync();

            return Ok(scores);
        }

        // GET: api/Scores/5
        [Authorize]
        [HttpGet("personal")]
        public async Task<ActionResult<Score>> GetScore()
        {
            if (_context.Scores == null)
            {
                return NotFound();
            }

            var UserId = HttpContext.User.Claims.FirstOrDefault(i => i.Type == "UserId");

            if (UserId == null)
            {
                return Unauthorized();
            }

            var score = await _context.Scores.FindAsync(UserId.Value);

            if (score == null)
            {
                return NotFound();
            }

            return Ok(new { 
                score.Attempts,
                score.Solved
            });
        }


    }
}
