using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SD_330_W22SD_Assignment.Data;

namespace SD_330_W22SD_Assignment.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var questions = _context.Questions.ToList();
            return View(questions);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int QuestionId)
        {
            var question = await _context.Questions
                .Include(q => q.Comments)
                .Include(q => q.Answers)
                .ThenInclude(a => a.Comments)
                .FirstOrDefaultAsync(q => q.Id == QuestionId);

            if (question == null)
            {
                return BadRequest($"Question with {QuestionId} does not exist.");
            }

            _context.RemoveRange(question.Comments);
            _context.RemoveRange(question.Answers.SelectMany(a => a.Comments));
            await _context.SaveChangesAsync();

            _context.RemoveRange(question.Answers);
            await _context.SaveChangesAsync();

            _context.Remove(question);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
