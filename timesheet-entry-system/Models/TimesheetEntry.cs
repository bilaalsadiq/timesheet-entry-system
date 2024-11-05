namespace timesheet_entry_system.Models
{
    public class TimesheetEntry
    {
        public int id { get; set; }
        public string UserName { get; set; }
        public DateTime Date { get; set; }
        public string ProjectName { get; set; }
        public string TaskDescription { get; set; }
        public int HoursWorked { get; set; }
    }
}
