﻿using timesheet_entry_system.Data;
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
            if (string.IsNullOrWhiteSpace(entry.UserName))
                throw new ArgumentException("Username is required");

            if (entry.Date == default)
                throw new ArgumentException("Date is required");

            if (string.IsNullOrWhiteSpace(entry.ProjectName))
                throw new ArgumentException("Project Name is required");

            if (string.IsNullOrWhiteSpace(entry.TaskDescription))
                throw new ArgumentException("Task Description is required");

            if (entry.HoursWorked <= 0)
                throw new ArgumentException("Hours Worked must be greater than 0");

            _context.TimesheetEntries.Add(entry);
            _context.SaveChanges();
        }

        public List<TimesheetEntryDTO> GetEntriesWithTotalHours()
        {
            var result = _context.TimesheetEntries
                .AsEnumerable()
                .GroupBy(e => new { e.UserName, e.Date })
                .SelectMany(g => g.Select(e => new TimesheetEntryDTO
                {
                    UserName = e.UserName,
                    Date = e.Date,
                    ProjectName = e.ProjectName,
                    TaskDescription = e.TaskDescription,
                    HoursWorked = e.HoursWorked,
                    TotalHours = g.Sum(e => e.HoursWorked)
                }));

            return result.ToList();
        }
    }
}
