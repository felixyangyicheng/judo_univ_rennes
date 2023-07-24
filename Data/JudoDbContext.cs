

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace judo_univ_rennes.Data
{
    public class JudoDbContext : IdentityDbContext
    {

        public DbSet<Post> Posts { get; set; }
        public DbSet<Command> Commands { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Devise> Devises { get; set; }

        public JudoDbContext(DbContextOptions<JudoDbContext> options)
           : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole
            {
                Name = "Client",
                NormalizedName = "CLIENT",
                Id = "b5a136a0-dc53-4e4e-b5e0-68d10b70fe02"
            },
            new IdentityRole
            {
                Name = "Admin",
                NormalizedName = "ADMIN",
                Id = "d9e1208e-5301-4fc9-8db0-f2562714a991"
            },
            new IdentityRole
            {
                Name = "SuperAdmin",
                NormalizedName = "SUPERADMIN",
                Id = "94c965f5-41f4-4c0f-ba55-61f9ac99d622"
            }
         );

            var hasher = new PasswordHasher<ApiUser>();

            modelBuilder.Entity<ApiUser>().HasData(
                new ApiUser
                {
                    Id = "43c38655-9aa0-48b4-aab1-7cd175500f09",
                    Email = "y.yang@iia-formation.fr",
                    NormalizedEmail = "Y.YANG@IIA-FORMATION.FR",
                    UserName = "YANG.YICHENG",
                    NormalizedUserName = "YANG.YICHENG",
                    FirstName = "Yicheng",
                    LastName = "YANG",
                    PasswordHash = hasher.HashPassword(null, "P@ssword1")
                }

            );

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "d9e1208e-5301-4fc9-8db0-f2562714a991",//admin
                    UserId = "43c38655-9aa0-48b4-aab1-7cd175500f09" //yicheng
                },
                new IdentityUserRole<string>
                {
                    RoleId = "94c965f5-41f4-4c0f-ba55-61f9ac99d622",//super admin
                    UserId = "43c38655-9aa0-48b4-aab1-7cd175500f09" //yicheng
                }

            );
        }
    }
}
