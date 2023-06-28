using Microsoft.EntityFrameworkCore;

namespace Services {
    public sealed class ApplicationDbContext : DbContext
    {
        public DbSet<ParsingRule> ParsingRules { get; set; } = null!;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            SQLitePCL.Batteries_V2.Init();
            Database.EnsureCreated();
        }
        
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            Database.OpenConnection();
            Database.ExecuteSqlRaw("PRAGMA foreign_keys=ON;");

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<ParsingRule>()
                .HasKey(x => x.Id)
                .HasName("PrimaryKey_Id");
        }
    }
}
