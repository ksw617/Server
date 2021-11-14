using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
namespace Server
{
    class AppDbContext : DbContext
    {
        public DbSet<AccountDb> Accounts { get; set; }
        public DbSet<PlayerDb> Players { get; set; }

        static readonly ILoggerFactory logger = LoggerFactory.Create(
            builder => { builder.AddConsole(); }
            );

        string ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=GameDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseLoggerFactory(logger).UseSqlServer(ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //유니티 ID 설정하기
            builder.Entity<AccountDb>().HasIndex(a => a.AccountName).IsUnique();
            builder.Entity<PlayerDb>().HasIndex(p => p.PlayerName).IsUnique();
        }
    }
}
