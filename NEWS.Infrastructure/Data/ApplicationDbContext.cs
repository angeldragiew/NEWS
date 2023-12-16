using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NEWS.Infrastructure.Data.Models;

namespace NEWS.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<News> News { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(new IdentityRole { Id = "8881f953-e7cc-4d0d-8937-9a74413e60c5", Name = "Admin", NormalizedName = "ADMIN".ToUpper() });

            var user = new ApplicationUser
            {
                Id = "bcc9e639-b998-466b-8d67-5e7dda1dfe5a", // primary key
                UserName = "myadmin@gmail.com",
                NormalizedUserName = "MYADMIN@GMAIL.COM",
                Email = "myadmin@gmail.com",
                NormalizedEmail = "MYADMIN@GMAIL.COM",
                FirstName = "Admin",
                LastName = "Adminov"
            };

            var hasher = new PasswordHasher<ApplicationUser>();
            var hashed = hasher.HashPassword(user, "MyAdmin12345.");
            user.PasswordHash = hashed;

            builder.Entity<ApplicationUser>().HasData(user);

            builder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                RoleId = "8881f953-e7cc-4d0d-8937-9a74413e60c5",
                UserId = "bcc9e639-b998-466b-8d67-5e7dda1dfe5a"
            });

            base.OnModelCreating(builder);
        }
    }
}