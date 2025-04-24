using Microsoft.EntityFrameworkCore;
using practise.Models;

namespace practise.data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<DailyContext> Daily { get; set; }
        public DbSet<NonDailyContext> Weekly { get; set; }
        public DbSet<MemberContext> Member { get; set; }
    }
}
