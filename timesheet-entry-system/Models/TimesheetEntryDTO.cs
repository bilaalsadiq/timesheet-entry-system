namespace timesheet_entry_system.Models
{
    public class TimesheetEntryDTO
    {
        public string UserName { get; set; }
        public DateTime Date { get; set; }
        public string ProjectName { get; set; }
        public string TaskDescription { get; set; }
        public int HoursWorked { get; set; }
        public int TotalHours { get; set; }
    }
}
