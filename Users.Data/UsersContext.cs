using Microsoft.EntityFrameworkCore;
using Users.Core.Models;

namespace Users.Data
{
    public sealed class UsersContext : DbContext
    {
        public UsersContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=blogging.db");
        }

        public DbSet<User> Users { get; set; }        
    }
}
