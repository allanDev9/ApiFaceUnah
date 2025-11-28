using Microsoft.EntityFrameworkCore;

namespace ApiFaceUnah
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        { 
        }
    }
}
