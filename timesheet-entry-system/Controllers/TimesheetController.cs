using Microsoft.AspNetCore.Mvc;
using System.Text;
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

        [HttpGet]
        public IActionResult DownloadCsv()
        {
            var entries = _service.GetEntriesWithTotalHours();

            if (!entries.Any())
            {
                ViewBag.ErrorMessage = "No timesheet entries found.";
                return View("Index");
            }

            var csv = new StringBuilder();
            csv.AppendLine("UserName,Date,ProjectName,TaskDescription,HoursWorked,TotalHoursWorked");
            foreach (var entry in entries)
            {
                csv.AppendLine($"{entry.UserName}," +
                    $"{entry.Date.ToString("yyyy-MM-dd")}," +
                    $"{entry.ProjectName}," +
                    $"{entry.TaskDescription}," +
                    $"{entry.HoursWorked}," +
                    $"{entry.TotalHours}"
                    );
            }

            var byteArray = Encoding.ASCII.GetBytes(csv.ToString());
            var fileContent = new FileContentResult(byteArray, "text/csv")
            {
                FileDownloadName = "TimesheetEntries.csv"
            };

            return fileContent;
        }
    }
}
