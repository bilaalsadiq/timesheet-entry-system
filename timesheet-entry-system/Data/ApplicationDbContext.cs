using Microsoft.EntityFrameworkCore;
using timesheet_entry_system.Models;

namespace timesheet_entry_system.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<TimesheetEntry> TimesheetEntries { get; set; }
    }
}
