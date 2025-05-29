using EventProject.Models;
using Microsoft.EntityFrameworkCore;

namespace EventProject.AppContext
{
    public class Context : DbContext
    {
        public Context(DbContextOptions options) : base(options)
        {
        }

        public DbSet<EventRegistrations> eventRegistrations { get; set; }
    }

}
