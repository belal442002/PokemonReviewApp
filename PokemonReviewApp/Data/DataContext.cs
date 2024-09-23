using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Pockemon> Pockemon { get; set; }
        public DbSet<PockemonCategory> PockemonCategories { get; set; }
        public DbSet<PockemonOwner> PockemonOwners { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Reviewer> Reviewers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //for PockemonCategory(Many To Many)
            modelBuilder.Entity<PockemonCategory>()
                .HasKey(pc => new { pc.PockemonId, pc.CategoryId });

            modelBuilder.Entity<PockemonCategory>()
                .HasOne(pc => pc.Pockemon)
                .WithMany(p => p.PockemonCategories)
                .HasForeignKey(pc => pc.PockemonId);

            modelBuilder.Entity<PockemonCategory>()
                .HasOne(pc => pc.Category)
                .WithMany(c => c.pockemonCategories)
                .HasForeignKey(pc => pc.CategoryId);


            //for PockemonOwner(Many To Many)
            modelBuilder.Entity<PockemonOwner>()
                .HasKey(po => new { po.PockemonId, po.OwnerId });

            modelBuilder.Entity<PockemonOwner>()
                .HasOne(po => po.Pockemon)
                .WithMany(p => p.PockemonOwners)
                .HasForeignKey(po => po.PockemonId);

            modelBuilder.Entity<PockemonOwner>()
                .HasOne(po => po.Owner)
                .WithMany(o => o.PockemonOwners)
                .HasForeignKey(po => po.OwnerId);
        }
    }
}
