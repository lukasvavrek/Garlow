using Garlow.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Garlow.API.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Value> Values { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Location> Locations { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    }
}