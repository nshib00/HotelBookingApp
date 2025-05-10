using Microsoft.EntityFrameworkCore;
using BookingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BookingApp.Infrastructure.DB
{
    public class BookingDbContext : IdentityDbContext<User>
    {
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<RoomService> RoomServices { get; set; }
        public DbSet<HotelService> HotelServices { get; set; }

        public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new BookingConfiguration());
            modelBuilder.ApplyConfiguration(new RoomConfiguration());
            modelBuilder.ApplyConfiguration(new HotelConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.HasIndex(u => u.Email).IsUnique();
        }
    }

    public class RoomConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.HasMany(r => r.Services)
                .WithOne(s => s.Room)
                .HasForeignKey(s => s.RoomId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasOne(b => b.Room)
               .WithMany()
               .HasForeignKey(b => b.RoomId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.HasKey(h => h.Id);
        }
    }

    public class BookingDbContextFactory : IDesignTimeDbContextFactory<BookingDbContext>
    {
        public BookingDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("D:/projects/WebLabs/BookingService/BookingService/appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<BookingDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseNpgsql(connectionString);

            return new BookingDbContext(optionsBuilder.Options);
        }
    }
}