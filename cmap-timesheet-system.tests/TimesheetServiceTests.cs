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

        [Theory]
        [InlineData(null, "2014-10-22", "Project Alpha", "Developed new feature X", 4, "Username is required")]
        [InlineData("John Smith", null, "Project Alpha", "Developed new feature X", 4, "Date is required")]
        [InlineData("John Smith", "2014-10-22", null, "Developed new feature X", 4, "Project Name is required")]
        [InlineData("John Smith", "2014-10-22", "Project Alpha", null, 4, "Task Description is required")]
        [InlineData("John Smith", "2014-10-22", "Project Alpha", "Developed new feature X", 0, "Hours Worked must be greater than 0")]
        public void AddSingleEntry_ShouldThrowException_WhenFieldsAreInvalid(string userName, string date, string project, string description, int hoursWorked, string expectedMessage)
        {
            // Arrange
            var entry = new TimesheetEntry
            {
                UserName = userName,
                Date = date != null ? DateTime.Parse(date) : default,
                ProjectName = project,
                TaskDescription = description,
                HoursWorked = hoursWorked
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _service.AddEntry(entry));
            Assert.Equal(expectedMessage, exception.Message);
        }
    }
}