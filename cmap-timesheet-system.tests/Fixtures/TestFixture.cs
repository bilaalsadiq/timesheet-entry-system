using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using timesheet_entry_system.Data;
using timesheet_entry_system.Interfaces;
using timesheet_entry_system.Services;

namespace cmap_timesheet_system.tests.Fixtures
{
    public class TestFixture : IDisposable
    {
        public ServiceProvider ServiceProvider { get; private set; }

        public TestFixture()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase(Guid.NewGuid().ToString()));

            serviceCollection.AddTransient<ITimesheetService, TimesheetService>();

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        public void Setup()
        {
            var context = ServiceProvider.GetService<ApplicationDbContext>();
            context.TimesheetEntries.RemoveRange(context.TimesheetEntries);
            context.SaveChanges();
        }

        public void Dispose()
        {
            ServiceProvider?.Dispose();
        }
    }
}
