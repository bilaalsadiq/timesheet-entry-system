using cmap_timesheet_system.tests.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using timesheet_entry_system.Data;
using timesheet_entry_system.Interfaces;
using timesheet_entry_system.Models;

namespace cmap_timesheet_system.tests
{
    public class TimesheetServiceTests : IClassFixture<TestFixture>
    {
        private readonly ITimesheetService _service;
        private readonly ApplicationDbContext _context;

        public TimesheetServiceTests(TestFixture fixture)
        {
            _service = fixture.ServiceProvider.GetService<ITimesheetService>();
            _context = fixture.ServiceProvider.GetService<ApplicationDbContext>();
        }


        [Theory]
        [InlineData("John Smith", "2014-10-22", "Project Alpha", "Developed new feature X", 4)]
        public void AddSingleEntry_ShouldAddEntryToDB(string userName, string date, string project, string description, int hoursWorked)
        {
            //arrange
            var entry = new TimesheetEntry()
            {
                UserName = userName,
                Date = date != null ? DateTime.Parse(date) : default,
                ProjectName = project,
                TaskDescription = description,
                HoursWorked = hoursWorked
            };

            //act
            _service.AddEntry(entry);

            //assert
            Assert.Single(_context.TimesheetEntries);
        }
    }
}