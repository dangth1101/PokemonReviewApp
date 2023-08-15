using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        public DbSet<Category> categories { get; set; }
        public DbSet<Country> countries { get; set; }
        public DbSet<Owner> owners { get; set; }
        public DbSet<Pokemon> pokemon { get; set; }
        public DbSet<PokemonCategory> PokemonCategories { get; set; }
        public DbSet<PokemonOwner> PokemonOwners { get; set; }
        public DbSet<Review> reviews { get; set; }
        public DbSet<Reviewer> reviewers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PokemonCategory>().HasKey(pc => new { pc.pokemonID, pc.categoryID }); // Set primary key
            modelBuilder.Entity<PokemonCategory>() // One-Many relationship from Pokemon to PokemonCategory through same ID
                .HasOne(p => p.pokemon)
                .WithMany(pc => pc.PokemonCategories)
                .HasForeignKey(p => p.pokemonID);
            modelBuilder.Entity<PokemonCategory>() // One-Many relationship from Category to PokemonCategory through same ID
                .HasOne(c => c.category)
                .WithMany(pc => pc.PokemonCategories)
                .HasForeignKey(c => c.categoryID);

            modelBuilder.Entity<PokemonOwner>() // Set primary key
                .HasKey(po => new {po.pokemonID, po.ownerID }); 
            modelBuilder.Entity<PokemonOwner>() // One-Many relationship from Pokemon to PokemonOwner through same ID
                .HasOne(p => p.pokemon)
                .WithMany(po => po.PokemonOwners)
                .HasForeignKey(p => p.pokemonID);
            modelBuilder.Entity<PokemonOwner>() // One-Many relationship from Owner to PokemonOwner through same ID
                .HasOne(o => o.owner)
                .WithMany(po => po.PokemonOwners)
                .HasForeignKey(o => o.ownerID);

        }
    }
}
