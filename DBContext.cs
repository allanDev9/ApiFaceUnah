using Microsoft.EntityFrameworkCore;

namespace ApiFaceUnah
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        {
        }

        public DbSet<Models.UserModel> Users { get; set; }
    }
}
