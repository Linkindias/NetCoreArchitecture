using DAL.Table;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;

namespace DAL
{
    public partial class TwoDbContext : DbContext
    {
        string connectionString = string.Empty;

        public TwoDbContext(DbContextOptions<TwoDbContext> options) : base(options)
        {
            var sql = options.FindExtension<SqlServerOptionsExtension>();
            if (sql != null) connectionString = sql.ConnectionString;
        }

        public virtual DbSet<TwoAccount> Account { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TwoAccount>()
                .Property(e => e.IdNo)
                .IsUnicode(false);

            modelBuilder.Entity<TwoAccount>()
                .Property(e => e.Tel)
                .IsUnicode(false);

            modelBuilder.Entity<TwoAccount>()
                .Property(e => e.Phone)
                .IsUnicode(false);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString, providerOptions => providerOptions.CommandTimeout(60));
        }
    }
}
