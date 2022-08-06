using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SD_330_W22SD_Assignment.Data;
using SD_330_W22SD_Assignment.Models;
using SD_330_W22SD_Assignment.Models.ViewModels;

namespace SD_330_W22SD_Assignment.Controllers
{
    [Authorize]
    public class QuestionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuestionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(int Page = 1, int SortOrder = 0)
        {
            List<Question> questions;
            switch (SortOrder)
            {
                case 0:
                    questions = await _context.Questions.Skip((Page - 1) * 10).Take(10).OrderBy(q => q.CreatedAt).ToListAsync();
                    break;
                case 1:
                    questions = await _context.Questions.Include(q => q.Answers).Skip((Page - 1) * 10).Take(10).OrderByDescending(q => q.Answers.Count).ToListAsync();
                    break;
                default:
                    return NotFound();
            }

            var maxPage = _context.Questions.Count() / 10 + 1;
            var vm = new QuestionsViewModel(questions, Page, maxPage);

            return View(vm);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Questions == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .Include(q => q.User)
                .Include(q => q.Answers)
                .ThenInclude(a => a.Comments)
                .Include(q => q.Answers)
                .ThenInclude(a => a.Votes)
                .Include(q => q.Comments)
                .Include(q => q.Votes)
                .Include(q => q.CorrectAnswer)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (question == null)
            {
                return NotFound();
            }

            List<Answer> answers;
            if (question.CorrectAnswerId != null)
            {
                answers = question.Answers.Where(a => a.Id != question.CorrectAnswerId).Prepend(question.CorrectAnswer).ToList() as List<Answer>;
            }
            else
            {
                answers = question.Answers.ToList();
            }

            var tags = _context.QuestionTags.Include(qt => qt.Tag).Where(qt => qt.QuestionId == id).Select(qt => qt.Tag).ToList();
            var voteCount = question.Votes.Count(v => v.Up) - question.Votes.Count(v => !v.Up);
            var vm = new QuestionDetailsViewModel(question, voteCount, answers, tags);

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Upvote(int QuestionId)
        {
            var user = await _context.Users.FirstAsync(u => u.UserName == User.Identity!.Name);
            var questioner = (await _context.Questions.Include(q => q.User).FirstAsync(q => q.Id == QuestionId)).User!;

            if (user.Id == questioner.Id)
            {
                return BadRequest();
            }

            var existingVote = await _context.Votes.FirstOrDefaultAsync(v => v.QuestionId == QuestionId && v.UserId == user.Id);

            if (existingVote != null)
            {
                if (existingVote.Up)
                {
                    _context.Remove(existingVote);
                    questioner.Reputation -= 5;
                    _context.Update(questioner);
                }
                else
                {
                    existingVote.Up = true;
                    _context.Update(existingVote);
                    questioner.Reputation += 10;
                    _context.Update(questioner);
                }
            }
            else
            {
                var vote = new Vote();
                vote.QuestionId = QuestionId;
                vote.UserId = user.Id;
                vote.Up = true;
                await _context.AddAsync(vote);
                questioner.Reputation += 5;
                _context.Update(questioner);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = QuestionId });
        }

        [HttpPost]
        public async Task<IActionResult> Downvote(int QuestionId)
        {
            var user = await _context.Users.FirstAsync(u => u.UserName == User.Identity!.Name);
            var questioner = (await _context.Questions.Include(q => q.User).FirstAsync(q => q.Id == QuestionId)).User!;

            if (user.Id == questioner.Id)
            {
                return BadRequest();
            }

            var existingVote = await _context.Votes.FirstOrDefaultAsync(v => v.QuestionId == QuestionId && v.UserId == user.Id);

            if (existingVote != null)
            {
                if (existingVote.Up)
                {
                    existingVote.Up = false;
                    _context.Update(existingVote);
                    questioner.Reputation -= 10;
                    _context.Update(questioner);
                }
                else
                {
                    _context.Remove(existingVote);
                    questioner.Reputation += 5;
                    _context.Update(questioner);
                }
            }
            else
            {
                var vote = new Vote();
                vote.QuestionId = QuestionId;
                vote.UserId = user.Id;
                vote.Up = false;
                await _context.AddAsync(vote);
                questioner.Reputation -= 5;
                _context.Update(questioner);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = QuestionId });
        }

        // GET: Questions/Create
        public IActionResult Create()
        {
            var vm = new CreateQuestionViewModel("", "", _context.Tags.ToList());
            return View(vm);
        }

        // POST: Questions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string Title, string Body, List<string> TagIds)
        {
            var user = _context.Users.First(u => u.UserName == User.Identity!.Name);

            var question = new Question();
            question.Title = Title;
            question.Body = Body;
            question.CreatedAt = DateTime.Now;
            question.UserId = user.Id;

            _context.Add(question);
            await _context.SaveChangesAsync();

            foreach (var tagId in TagIds)
            {
                var qt = new QuestionTag();
                qt.QuestionId = question.Id;
                qt.TagId = int.Parse(tagId);
                _context.Add(qt);
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Questions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Questions == null)
            {
                return NotFound();
            }

            var question = await _context.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", question.UserId);
            return View(question);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Body,CreatedAt,UserId")] Question question)
        {
            if (id != question.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(question);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionExists(question.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", question.UserId);
            return View(question);
        }

        // GET: Questions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Questions == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .Include(q => q.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Questions == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Questions'  is null.");
            }
            var question = await _context.Questions.FindAsync(id);
            if (question != null)
            {
                _context.Questions.Remove(question);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionExists(int id)
        {
            return (_context.Questions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
