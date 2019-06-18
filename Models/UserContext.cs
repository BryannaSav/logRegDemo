using Microsoft.EntityFrameworkCore;
 
namespace logRegDemo.Models
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions options) : base(options) { }

        public DbSet<User> users { get; set; }
    }
}