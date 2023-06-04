using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleTeleBot {
    public sealed class ApplicationDbContext : DbContext {
        
        public DbSet<ParsingRule> ParsingRules { get; set; } = null!;
        public ApplicationDbContext() => Database.EnsureCreated();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlite("Data Source=app.db;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<ParsingRule>()
                .HasKey(x => x.Id)
                .HasName("PrimaryKey_Id");
        }
    }
}
