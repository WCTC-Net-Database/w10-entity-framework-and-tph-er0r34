using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using W9_assignment_template.Models;

namespace W9_assignment_template.Data
{
    public class GameContext : DbContext
    {
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Ability> Abilities { get; set; }

        public GameContext(DbContextOptions<GameContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure TPH for Character hierarchy
            modelBuilder.Entity<Character>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue<Player>("Player")
                .HasValue<Goblin>("Goblin");

            // Configure TPH for Ability hierarchy
            modelBuilder.Entity<Ability>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue<PlayerAbility>("PlayerAbility")
                .HasValue<GoblinAbility>("GoblinAbility");

            base.OnModelCreating(modelBuilder);
        }
    }
}
