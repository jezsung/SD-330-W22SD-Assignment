using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SD_330_W22SD_Assignment.Data;
using SD_330_W22SD_Assignment.Models;

namespace SD_330_W22SD_Assignment.Controllers
{
    public class AnswersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnswersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create(int questionId)
        {
            ViewBag.QuestionId = questionId;
            return View();
        }

        [HttpPost]
        public IActionResult Create(int questionId, string body)
        {
            var answer = new Answer();
            var user = _context.Users.First(u => u.UserName == User.Identity!.Name);

            answer.UserId = user.Id;
            answer.CreatedAt = DateTime.Now;
            answer.QuestionId = questionId;
            answer.Body = body;

            _context.Add(answer);
            _context.SaveChanges();

            return RedirectToAction("Index", "Questions");
        }

        [HttpPost]
        public async Task<IActionResult> Upvote(int QuestionId, int AnswerId)
        {
            var user = await _context.Users.FirstAsync(u => u.UserName == User.Identity!.Name);
            var answerer = (await _context.Answers.Include(q => q.User).FirstAsync(q => q.Id == AnswerId)).User!;

            if (user.Id == answerer.Id)
            {
                return BadRequest();
            }

            var existingVote = await _context.AnswerVotes.FirstOrDefaultAsync(v => v.AnswerId == AnswerId && v.UserId == user.Id);

            if (existingVote != null)
            {
                if (existingVote.Up)
                {
                    _context.Remove(existingVote);
                    answerer.Reputation -= 5;
                    _context.Update(answerer);
                }
                else
                {
                    existingVote.Up = true;
                    _context.Update(existingVote);
                    answerer.Reputation += 10;
                    _context.Update(answerer);
                }
            }
            else
            {
                var vote = new AnswerVote();
                vote.AnswerId = AnswerId;
                vote.UserId = user.Id;
                vote.Up = true;
                await _context.AddAsync(vote);
                answerer.Reputation += 5;
                _context.Update(answerer);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Questions", new { id = QuestionId });
        }

        [HttpPost]
        public async Task<IActionResult> Downvote(int QuestionId, int AnswerId)
        {
            var user = await _context.Users.FirstAsync(u => u.UserName == User.Identity!.Name);
            var answerer = (await _context.Answers.Include(q => q.User).FirstAsync(q => q.Id == AnswerId)).User!;

            if (user.Id == answerer.Id)
            {
                return BadRequest();
            }

            var existingVote = await _context.AnswerVotes.FirstOrDefaultAsync(v => v.AnswerId == AnswerId && v.UserId == user.Id);

            if (existingVote != null)
            {
                if (existingVote.Up)
                {
                    existingVote.Up = false;
                    _context.Update(existingVote);
                    answerer.Reputation -= 10;
                    _context.Update(answerer);
                }
                else
                {
                    _context.Remove(existingVote);
                    answerer.Reputation += 5;
                    _context.Update(answerer);
                }
            }
            else
            {
                var vote = new AnswerVote();
                vote.AnswerId = AnswerId;
                vote.UserId = user.Id;
                vote.Up = false;
                await _context.AddAsync(vote);
                answerer.Reputation -= 5;
                _context.Update(answerer);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Questions", new { id = QuestionId });
        }
    }
}