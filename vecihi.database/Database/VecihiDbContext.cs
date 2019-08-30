using Microsoft.EntityFrameworkCore;
using System.Linq;
using vecihi.database.model;

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

            // Remove OneToManyCascadeDeleteConvention
            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            };

            // Init Seed Data
            builder.Seed();
        }

        /// <summary> 
        /// ToDo: Translate - DataSet objeleri test tarafında ezilip in-memory olarak kullanılacağı için virtual olarak tanımlanmıştır.
        /// </summary>
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<AutoCode> AutoCode { get; set; }
        public virtual DbSet<AutoCodeLog> AutoCodeLog { get; set; }
        public virtual DbSet<File> File { get; set; }
    }
}
