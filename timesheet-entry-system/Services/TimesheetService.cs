using timesheet_entry_system.Data;
using timesheet_entry_system.Interfaces;
using timesheet_entry_system.Models;

namespace timesheet_entry_system.Services
{
    public class TimesheetService : ITimesheetService
    {
        private readonly ApplicationDbContext _context;

        public TimesheetService(ApplicationDbContext context)
        {
            _context = context;    
        }

        public void AddEntry(TimesheetEntry entry)
        {
            _context.TimesheetEntries.Add(entry);
            _context.SaveChanges();
        }

    }
}
