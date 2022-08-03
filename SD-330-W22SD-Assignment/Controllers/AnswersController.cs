using Microsoft.AspNetCore.Mvc;
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
    }
}