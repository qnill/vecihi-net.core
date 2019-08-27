using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace vecihi.database
{
    public class VecihiDbContext : DbContext
    {
        public VecihiDbContext(DbContextOptions<VecihiDbContext> options)
          : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            };
        }
    }
}
