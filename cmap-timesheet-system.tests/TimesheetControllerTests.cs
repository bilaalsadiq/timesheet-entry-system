using cmap_timesheet_system.tests.Fixtures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using timesheet_entry_system.Controllers;
using timesheet_entry_system.Data;
using timesheet_entry_system.Interfaces;
using timesheet_entry_system.Models;


namespace cmap_timesheet_system.tests
{
    public class TimesheetControllerTests : IClassFixture<TestFixture>
    {
        private readonly TimesheetController _controller;
        private readonly ApplicationDbContext _context;
        
        public TimesheetControllerTests(TestFixture fixture)
        {
            _context = fixture.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var service = fixture.ServiceProvider.GetRequiredService<ITimesheetService>();
            _controller = new TimesheetController(service);
        }

        [Fact]
        public void DownloadCSV_EntryExists_CreatesValidCsvFile()
        {
            // Arrange
            var userName = "John Smith";
            var date = "2014-10-22";
            var project = "Project Alpha";
            var description = "Developed new feature X";
            var hoursWorked = 4;

            var entry = new TimesheetEntry()
            {
                UserName = userName,
                Date = date != null ? DateTime.Parse(date) : default,
                ProjectName = project,
                TaskDescription = description,
                HoursWorked = hoursWorked
            };

            _context.TimesheetEntries.Add(entry);
            _context.SaveChanges();

            // Act
            var result = _controller.DownloadCsv() as FileContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("text/csv", result.ContentType);
            Assert.Equal("TimesheetEntries.csv", result.FileDownloadName);
        }

    }
}
