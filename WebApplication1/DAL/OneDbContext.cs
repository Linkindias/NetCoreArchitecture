using DAL.Table;
using DAL.TableModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;

namespace DAL
{
    public partial class OneDbContext : DbContext
    {
        string connectionString = string.Empty;

        public OneDbContext(DbContextOptions<OneDbContext> options) : base(options)
        {
            var sql = options.FindExtension<SqlServerOptionsExtension>();
            if (sql != null) connectionString = sql.ConnectionString;
        }

        public virtual DbSet<OneAccount> Account { get; set; }
        public virtual DbSet<UserEventLog> UserEventLog { get; set; }
        public virtual DbSet<ExceptionLog> ExceptionLog { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OneAccount>()
                .Property(e => e.IdNo)
                .IsUnicode(false);

            modelBuilder.Entity<ExceptionLog>()
                .Property(e => e.IP)
                .IsUnicode(false);

            modelBuilder.Entity<ExceptionLog>()
                .Property(e => e.ROUTE)
                .IsUnicode(false);

            modelBuilder.Entity<ExceptionLog>()
                .Property(e => e.METHOD)
                .IsUnicode(false);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString, providerOptions => providerOptions.CommandTimeout(60));
        }
    }
}
