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
        public void AddSingleEntry_ShouldAddEntryToContext(string userName, string date, string project, string description, int hoursWorked)
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


        [Fact]
        public void AddMultipleEntries_ToDB_ShouldContainBothEntries()
        {
            // Arrange

            var entries = new List<TimesheetEntry>
            {
                new TimesheetEntry { UserName = "John Smith", Date = new DateTime(2014, 10, 22), ProjectName = "Project Alpha", TaskDescription = "Developed new feature X", HoursWorked = 4 },
                new TimesheetEntry { UserName = "Jane Doe", Date = new DateTime(2014, 10, 22), ProjectName = "Project Gamma", TaskDescription = "Conducted User Testing", HoursWorked = 6 }
            };

            var entry1 = new TimesheetEntry
            {
                UserName = "John Smith",
                Date = new DateTime(2014, 10, 22),
                ProjectName = "Project Alpha",
                TaskDescription = "Developed new feature X",
                HoursWorked = 4
            };

            var entry2 = new TimesheetEntry
            {
                UserName = "Jane Doe",
                Date = new DateTime(2014, 10, 22),
                ProjectName = "Project Gamma",
                TaskDescription = "Conducted User Testing",
                HoursWorked = 6
            };

            // Act
            _service.AddEntry(entry1);
            _service.AddEntry(entry2);

            // Assert
            var retrievedEntry1 = _context.TimesheetEntries.FirstOrDefault(e => e.UserName == "John Smith");
            Assert.NotNull(retrievedEntry1);
            Assert.Equal(new DateTime(2014, 10, 22), retrievedEntry1.Date);
            Assert.Equal("Project Alpha", retrievedEntry1.ProjectName);
            Assert.Equal("Developed new feature X", retrievedEntry1.TaskDescription);
            Assert.Equal(4, retrievedEntry1.HoursWorked);

            var retrievedEntry2 = _context.TimesheetEntries.FirstOrDefault(e => e.UserName == "Jane Doe");
            Assert.NotNull(retrievedEntry2);
            Assert.Equal(new DateTime(2014, 10, 22), retrievedEntry2.Date);
            Assert.Equal("Project Gamma", retrievedEntry2.ProjectName);
            Assert.Equal("Conducted User Testing", retrievedEntry2.TaskDescription);
            Assert.Equal(6, retrievedEntry2.HoursWorked);
        }


    }
}