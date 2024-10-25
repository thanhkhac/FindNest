using FindNest.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FindNest.Data
{
    public class FindNestDbContext : IdentityDbContext<User>
    {
        public FindNestDbContext(DbContextOptions<FindNestDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Check if the context has already been configured
            if (!optionsBuilder.IsConfigured)
            {
                // Configure the database connection
                optionsBuilder.UseSqlServer("Server=(local);Database=FindNest;uid=sa;pwd=123456;TrustServerCertificate=True;MultipleActiveResultSets=true");
            }
        }

        public FindNestDbContext()
            : base()
        {
        }

        public DbSet<ReceivedRequest> Requests { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Utility> Utilities { get; set; }
        public DbSet<Media> Media { get; set; }
        public DbSet<RentPost> RentPosts { get; set; }
        public DbSet<RentCategory> RentCategories { get; set; }
        public DbSet<RentPostRoom> RentPostRooms { get; set; }
        public DbSet<Room> Rooms { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RentCategory>().HasData(
                new RentCategory { Id = 1, Name = "Phòng trọ" },
                new RentCategory { Id = 2, Name = "Căn hộ" },
                new RentCategory { Id = 3, Name = "Nguyên Căn" }
            );

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "Admin", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "User", Name = "User", NormalizedName = "USER" }
            );

            modelBuilder.Entity<Room>().HasData(
                new Room
                {
                    Id = 1,
                    Name = "Phòng ngủ"
                }, new Room
                {
                    Id = 2,
                    Name = "Phòng khách"
                }, new Room
                {
                    Id = 3,
                    Name = "Nhà vệ sinh"
                },
                new Room
                {
                    Id = 4,
                    Name = "Nhà bếp riêng"
                }
            );
        }
    }
}