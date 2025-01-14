using Microsoft.EntityFrameworkCore;
using practise.Models;

namespace practise.data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<HomeContext> Test1 { get; set; }
        public DbSet<DailyContext> Daily { get; set; }
    }
}
