using Nvisibl.DataLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace Nvisibl.DataLibrary.Contexts
{
    public class ChatContext : DbContext
    {
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        public ChatContext(DbContextOptions options)
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Friend> Friends { get; set; }

        public DbSet<Chatroom> Chatrooms { get; set; }

        public DbSet<ChatroomUser> ChatroomUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Friend>().HasKey(e => new { e.User1Id, e.User2Id });

            builder.Entity<User>()
                .HasMany(u => u.Friends)
                .WithOne(f => f.User1)
                .HasForeignKey(f => f.User1Id);

            builder.Entity<User>()
                .HasMany(u => u.FriendedBy)
                .WithOne(f => f.User2)
                .HasForeignKey(f => f.User2Id)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ChatroomUser>().HasKey(e => new { e.ChatroomId, e.UserId });

            builder.Entity<ChatroomUser>()
                .HasOne(cu => cu.Chatroom)
                .WithMany(c => c.Users)
                .HasForeignKey(cu => cu.ChatroomId);

            builder.Entity<ChatroomUser>()
                .HasOne(cu => cu.User)
                .WithMany(u => u.Chatrooms)
                .HasForeignKey(cu => cu.UserId);
        }
    }
}
