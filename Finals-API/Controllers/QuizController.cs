using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Finals_API.Data;
using Finals_API.Models;

namespace Finals_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly FinalDatabaseContext _context;

        public QuizController(FinalDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Quiz
        [HttpGet]
        public async Task<ActionResult<Quiz>> GetQuiz()
        {
            if (_context.Quizzes == null)
            {
                return NotFound();
            }

            Quiz? quiz = await _context.Quizzes.FindAsync(0);

            if(quiz == null)
            {
                quiz = QuizGenerator.GenerateNew(new Quiz());
                _context.Quizzes.Add(quiz);
            }
            else if(DateTime.Today - quiz.Date == TimeSpan.FromDays(1))
            {
                quiz = QuizGenerator.GenerateNew(quiz);
                _context.Quizzes.Update(quiz);
            }
            else
            {
                // no changes, return daily quiz
                return Ok(new
                {
                    operators = quiz.Operators,
                    results = quiz.Results,
                    numbers = quiz.Numbers
                });
            }

            // changes was made, save it.
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return Ok(new
            {
                operators = quiz.Operators,
                results = quiz.Results,
                numbers = quiz.Numbers
            });
        }

        [HttpGet("Check/{numbers}")]
        public async Task<ActionResult<bool>> CheckQuiz(string numbers)
        {
            if (_context.Quizzes == null)
            {
                return NotFound();
            }

            Quiz? quiz = await _context.Quizzes.FindAsync(0);

            if (quiz == null)
                return Problem("Quiz missing");

            bool solved = CountNumbers(numbers, quiz.Results, quiz.Operators);

            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var UserId = HttpContext.User.Claims.FirstOrDefault(i => i.Type == "UserId");
                if(UserId != null)
                {
                    if (solved)
                        await AddScore(UserId.Value);
                    else
                        await AddAttempt(UserId.Value);
                }    
            }


            return Ok(solved);
        }

        private bool CountNumbers(string _numbers, string _results, string _operators)
        {
            if (!_numbers.All(char.IsDigit))
                return false;

            int[] numbers = _numbers.Where(char.IsDigit).Select(x => int.Parse(x.ToString())).ToArray();
            int[] results = _results.Split(',').Select(x => int.Parse(x)).ToArray();
            char[] operators = _operators.Split(',').Select(x => char.Parse(x)).ToArray();

            var res1 = QuizGenerator.Calculate(new int[]{ numbers[0], numbers[1], numbers[2] }, new char[] { operators[0], operators[1] });
            var res2 = QuizGenerator.Calculate(new int[]{ numbers[3], numbers[4], numbers[5] }, new char[] { operators[5], operators[6] });
            var res3 = QuizGenerator.Calculate(new int[]{ numbers[6], numbers[7], numbers[8] }, new char[] { operators[10], operators[11] });
            var res4 = QuizGenerator.Calculate(new int[]{ numbers[0], numbers[3], numbers[6] }, new char[] { operators[2], operators[7] });
            var res5 = QuizGenerator.Calculate(new int[]{ numbers[1], numbers[4], numbers[7] }, new char[] { operators[3], operators[8] });
            var res6 = QuizGenerator.Calculate(new int[]{ numbers[2], numbers[5], numbers[8] }, new char[] { operators[4], operators[9] });


            return res1 == results[0]
                && res2 == results[1]
                && res3 == results[2]
                && res4 == results[3]
                && res5 == results[4]
                && res6 == results[5];
        }

        public async Task<bool> AddScore(string UserId)
        {
            Score? score = await _context.Scores.FindAsync(UserId);

            if (score == null || score?.LastScored?.Date == DateTime.Today)
                return false;

            score.Solved += 1;
            score.LastScored = DateTime.Now;

            _context.Entry(score).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> AddAttempt(string UserId)
        {
            Score? score = await _context.Scores.FindAsync(UserId);

            if (score == null || score?.LastScored?.Date == DateTime.Today)
                return false;

            score.Attempts += 1;

            _context.Entry(score).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

            return true;
        }

    }
}
