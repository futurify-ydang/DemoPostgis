using Microsoft.EntityFrameworkCore;

namespace DemoPostgres
{
    public class CSContext : DbContext
    {
        public CSContext(DbContextOptions<CSContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasPostgresExtension("postgis");
        }
    }
}