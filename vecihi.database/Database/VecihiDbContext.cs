using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using vecihi.database.model;

namespace vecihi.database
{
    public class VecihiDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
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
        /// DataSet objects are defined as virtual since they 
        /// will be crushed on the unit-test side and used as in-memory.
        /// </summary>
        public virtual DbSet<RefreshToken> RefreshToken { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<AutoCode> AutoCode { get; set; }
        public virtual DbSet<AutoCodeLog> AutoCodeLog { get; set; }
        public virtual DbSet<File> File { get; set; }
    }
}