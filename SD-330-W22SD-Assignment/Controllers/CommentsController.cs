using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SD_330_W22SD_Assignment.Data;
using SD_330_W22SD_Assignment.Models;

namespace SD_330_W22SD_Assignment.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddCommentToQuestion(int QuestionId, string Body)
        {
            var comment = new Comment();
            var user = _context.Users.First(u => u.UserName == User.Identity!.Name);

            comment.Body = Body;
            comment.QuestionId = QuestionId;
            comment.UserId = user.Id;

            _context.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Questions", new { id = QuestionId });
        }

        public async Task<IActionResult> AddCommentToAnswer(int QuestionId, int AnswerId, string Body)
        {
            var comment = new Comment();
            var user = _context.Users.First(u => u.UserName == User.Identity!.Name);

            comment.Body = Body;
            comment.AnswerId = AnswerId;
            comment.UserId = user.Id;

            _context.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Questions", new { id = QuestionId });
        }
    }
}
