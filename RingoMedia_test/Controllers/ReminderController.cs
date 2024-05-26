using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RingoMedia_test.Data;
using RingoMedia_test.Models;

namespace RingoMedia_test.Controllers
{
    public class ReminderController : Controller
    {
        private readonly AppDbcontext _dbcontext;

        public ReminderController(AppDbcontext context)
        {
            _dbcontext = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _dbcontext.Reminders.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReminderId,Title,DateTime,ToEmail")] Reminder reminder)
        {
            if (ModelState.IsValid)
            {
                #region Code to fix server datetime issue
                var indiaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

                // Specify the kind of DateTime as Unspecified and then convert to IST
                reminder.DateTime = DateTime.SpecifyKind(reminder.DateTime, DateTimeKind.Unspecified);

                // Convert from unspecified (assuming IST) to UTC
                var dateTimeInIst = TimeZoneInfo.ConvertTime(reminder.DateTime, indiaTimeZone);
                reminder.DateTime = TimeZoneInfo.ConvertTimeToUtc(dateTimeInIst, indiaTimeZone);
                #endregion
                _dbcontext.Add(reminder);
                await _dbcontext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reminder);
        }
    }
}
