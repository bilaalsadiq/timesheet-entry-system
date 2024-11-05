using Microsoft.AspNetCore.Mvc;
using timesheet_entry_system.Interfaces;
using timesheet_entry_system.Models;

namespace timesheet_entry_system.Controllers
{
    public class TimesheetController : Controller
    {
        private readonly ITimesheetService _service;

        public TimesheetController(ITimesheetService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddEntry(TimesheetEntry entry)
        {
            try
            {
                _service.AddEntry(entry);
                return RedirectToAction("Index");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View("Index");
        }
    }
}
