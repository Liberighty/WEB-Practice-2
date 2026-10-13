using Microsoft.EntityFrameworkCore;

namespace GodSimulatorAPI.Data
{
    public class MyApplicationContext : DbContext
    {
        public DbSet<Update> Updates { get; set; }
        public DbSet<WorldEvent> WorldEvents { get; set; }  
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=moderation.db");
            }
        }
    }
}
