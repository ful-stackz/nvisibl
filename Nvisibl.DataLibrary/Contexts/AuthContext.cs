using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Nvisibl.DataLibrary.Contexts
{
    public class AuthContext : IdentityDbContext
    {
        public AuthContext(DbContextOptions<AuthContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityUser>(entity =>
            {
                entity.Property(m => m.Id).HasMaxLength(80);
                entity.Property(m => m.Email).HasMaxLength(250);
                entity.Property(m => m.NormalizedEmail).HasMaxLength(250);
                entity.Property(m => m.UserName).HasMaxLength(250);
                entity.Property(m => m.NormalizedUserName).HasMaxLength(250);
            });

            builder.Entity<IdentityRole>(entity =>
            {
                entity.Property(m => m.Id).HasMaxLength(80);
                entity.Property(m => m.Name).HasMaxLength(250);
                entity.Property(m => m.NormalizedName).HasMaxLength(250);
            });

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.Property(m => m.LoginProvider).HasMaxLength(80);
                entity.Property(m => m.ProviderKey).HasMaxLength(80);
            });

            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.Property(m => m.UserId).HasMaxLength(80);
                entity.Property(m => m.RoleId).HasMaxLength(80);
            });

            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.Property(m => m.UserId).HasMaxLength(80);
                entity.Property(m => m.LoginProvider).HasMaxLength(80);
                entity.Property(m => m.Name).HasMaxLength(80);
            });
        }
    }
}
