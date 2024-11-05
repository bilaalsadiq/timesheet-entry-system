using Microsoft.AspNetCore.Mvc;

namespace timesheet_entry_system.Controllers
{
    public class TimesheetController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
