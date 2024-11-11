using FindNest.Constants;
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

        public DbSet<Region> Regions { get; set; }
        public DbSet<Media> Media { get; set; }
        public DbSet<RentPost> RentPosts { get; set; }
        public DbSet<RentCategory> RentCategories { get; set; }
        public DbSet<RentPostRoom> RentPostRooms { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Like> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configure RentPost -> RentPostRooms relationship with Cascade delete

            // RentPost và RentPostRoom có quan hệ 1-nhiều
            modelBuilder.Entity<RentPost>()
                .HasMany(rp => rp.RentPostRooms) 
                .WithOne(rpr => rpr.RentPost) 
                .HasForeignKey(rpr => rpr.RentPostId) 
                .OnDelete(DeleteBehavior.Cascade); 

   
            modelBuilder.Entity<RentPost>()
                .HasMany(rp => rp.Mediae) 
                .WithOne(m => m.RentPost) 
                .HasForeignKey(m => m.RentPostId) 
                .OnDelete(DeleteBehavior.Cascade); 

    
            modelBuilder.Entity<RentPost>()
                .HasMany(rp => rp.Likes) 
                .WithOne(l => l.RentPost) 
                .HasForeignKey(l => l.RentPostId) 
                .OnDelete(DeleteBehavior.Cascade); 


            modelBuilder.Entity<User>()
                .HasMany(u => u.Likes) 
                .WithOne(l => l.User) 
                .HasForeignKey(l => l.UserId) 
                .OnDelete(DeleteBehavior.Cascade); 
                


            modelBuilder.Entity<RentCategory>().HasData(
                new RentCategory { Id = 1, Name = "Phòng trọ" },
                new RentCategory { Id = 2, Name = "Căn hộ" },
                new RentCategory { Id = 3, Name = "Nguyên Căn" }
            );

            // Seed IdentityRole data
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "Admin", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "User", Name = "User", NormalizedName = "USER" }
            );

            // Seed Room data
            modelBuilder.Entity<Room>().HasData(
                new Room
                {
                    Id = RoomConst.Bedroom,
                    Name = "Phòng ngủ"
                },
                new Room
                {
                    Id = RoomConst.Livingroom,
                    Name = "Phòng khách"
                },
                new Room
                {
                    Id = RoomConst.Bathroom,
                    Name = "Nhà vệ sinh"
                },
                new Room
                {
                    Id = RoomConst.Kitchen,
                    Name = "Nhà bếp riêng"
                }
            );
        }
    }
}
