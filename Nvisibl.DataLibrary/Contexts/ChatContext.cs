﻿using Nvisibl.DataLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace Nvisibl.DataLibrary.Contexts
{
    public class ChatContext : DbContext
    {
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        public ChatContext(DbContextOptions<ChatContext> options)
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Friend> Friends { get; set; }

        public DbSet<Chatroom> Chatrooms { get; set; }

        public DbSet<ChatroomUser> ChatroomUsers { get; set; }

        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Friend>().HasKey(e => e.Id);
            builder.Entity<Friend>().HasAlternateKey(e => new { e.User1Id, e.User2Id });

            builder.Entity<Friend>()
                .HasOne(f => f.User1)
                .WithMany(u => u!.Friends)
                .HasForeignKey(f => f.User1Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Friend>()
                .HasOne(f => f.User2)
                .WithMany(u => u!.FriendedBy)
                .HasForeignKey(f => f.User2Id)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ChatroomUser>().HasKey(e => new { e.ChatroomId, e.UserId });

            builder.Entity<ChatroomUser>()
                .HasOne(cu => cu.Chatroom)
                .WithMany(c => c!.Users)
                .HasForeignKey(cu => cu.ChatroomId);

            builder.Entity<ChatroomUser>()
                .HasOne(cu => cu.User)
                .WithMany(u => u!.Chatrooms)
                .HasForeignKey(cu => cu.UserId);

            builder.Entity<Message>()
                .HasIndex(m => new { m.AuthorId, m.ChatroomId });

            builder.Entity<Message>()
                .HasOne(m => m.Author)
                .WithMany(u => u!.Messages)
                .HasForeignKey(m => m.AuthorId);

            builder.Entity<Message>()
                .HasOne(m => m.Chatroom)
                .WithMany(c => c!.Messages)
                .HasForeignKey(m => m.ChatroomId);
        }
    }
}
