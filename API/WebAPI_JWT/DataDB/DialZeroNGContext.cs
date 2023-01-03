using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace WebAPI_JWT.DataDB
{
    public partial class DialZeroNGContext : IdentityDbContext<IdentityUser>
	{
        public DialZeroNGContext()
        {
        }
		
		public DialZeroNGContext(DbContextOptions<DialZeroNGContext> options)
            : base(options)
        {

        }

        
        public virtual DbSet<Product> Products { get; set; } = null!;
        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.ProductId).ValueGeneratedNever();

                entity.Property(e => e.ProductDescription).HasMaxLength(50);

                entity.Property(e => e.ProductName).HasMaxLength(50);
            });

            

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
