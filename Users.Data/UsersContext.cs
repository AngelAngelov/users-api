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

        public DbSet<User> Users { get; set; }        
    }
}
