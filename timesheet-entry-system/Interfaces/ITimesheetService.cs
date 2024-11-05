using timesheet_entry_system.Models;

namespace timesheet_entry_system.Interfaces
{
    public interface ITimesheetService
    {
        void AddEntry(TimesheetEntry entry);
    }
}
