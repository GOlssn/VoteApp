using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VoteApp.Data;
using VoteApp.Models;

namespace VoteApp.Controllers
{
    public class PollsController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PollsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult View(int id)
        {
            Poll poll = new Poll();

            poll = _context.Polls.Include(p => p.Options).SingleOrDefault(p => p.Id == id);

            return View(poll);
        }

        public IActionResult ViewRandom()
        {
            Random rand = new Random();
            Poll poll = null;

            while(poll == null) {
                poll = _context.Polls.FirstOrDefault(p => p.Id == rand.Next(_context.Polls.Max(p2 => p2.Id) + 1));
            }

            return Redirect("/Polls/View/" + poll.Id);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(Poll poll, string[] options)
        {
            foreach(string option in options)
            {
                poll.Options.Add(new Option() { Value = option, TimesSelected = 0 });
            }

            poll.ApplicationUserId = _userManager.GetUserId(HttpContext.User);

            _context.Polls.Add(poll);

            await _context.SaveChangesAsync();

            return Redirect("view/" + poll.Id);
        }

        [HttpPost]
        public async Task<IActionResult> Vote(int option)
        {
            var optionToSave = _context.Options.Include(o => o.Poll).SingleOrDefault(o => o.Id == option);
            optionToSave.TimesSelected++;
            optionToSave.Poll.TotalVotes++;
            await _context.SaveChangesAsync();

            return Redirect("View/" + optionToSave.Poll.Id);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Edit(int id)
        {
            Poll poll = _context.Polls.Include(p => p.Options).SingleOrDefault(p => p.Id == id);
            return View(poll);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(Poll poll, string[] options)
        {
            Poll pollToSave = _context.Polls.Include(p => p.Options).SingleOrDefault(p => p.Id == poll.Id);

            pollToSave.Question = poll.Question;

            for(int i = 0; i < pollToSave.Options.Count; i++)
            {
                pollToSave.Options.ElementAt(i).Value = options[i];
            }

            if (options.Length < pollToSave.Options.Count)
            {
                pollToSave.Options.RemoveRange(options.Length - 1, pollToSave.Options.Count - options.Length);
            }
            else if(options.Length > pollToSave.Options.Count)
            {
                for(int i = options.Length - (options.Length - pollToSave.Options.Count); i < options.Length; i++)
                {
                    pollToSave.Options.Add(new Option() { Value = options[i], TimesSelected = 0 });
                }
            }

            await _context.SaveChangesAsync();

            return Redirect("/Polls/View/" + poll.Id);
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            Poll poll = _context.Polls.SingleOrDefault(p => p.Id == id);
            _context.Polls.Remove(poll);

            await _context.SaveChangesAsync();

            return Redirect("/Account/Polls");
        }
    }
}