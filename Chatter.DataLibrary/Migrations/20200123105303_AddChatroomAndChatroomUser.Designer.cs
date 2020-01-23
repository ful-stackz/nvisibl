﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Nvisibl.DataLibrary.Contexts;

namespace Nvisibl.DataLibrary.Migrations
{
    [DbContext(typeof(ChatContext))]
    [Migration("20200123105303_AddChatroomAndChatroomUser")]
    partial class AddChatroomAndChatroomUser
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Nvisibl.DataLibrary.Models.Chatroom", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("Chatrooms");
                });

            modelBuilder.Entity("Nvisibl.DataLibrary.Models.ChatroomUser", b =>
                {
                    b.Property<int>("ChatroomId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ChatroomId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("ChatroomUsers");
                });

            modelBuilder.Entity("Nvisibl.DataLibrary.Models.Friend", b =>
                {
                    b.Property<int>("User1Id")
                        .HasColumnType("int");

                    b.Property<int>("User2Id")
                        .HasColumnType("int");

                    b.HasKey("User1Id", "User2Id");

                    b.HasIndex("User2Id");

                    b.ToTable("Friends");
                });

            modelBuilder.Entity("Nvisibl.DataLibrary.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Nvisibl.DataLibrary.Models.ChatroomUser", b =>
                {
                    b.HasOne("Nvisibl.DataLibrary.Models.Chatroom", "Chatroom")
                        .WithMany("Users")
                        .HasForeignKey("ChatroomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Nvisibl.DataLibrary.Models.User", "User")
                        .WithMany("Chatrooms")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Nvisibl.DataLibrary.Models.Friend", b =>
                {
                    b.HasOne("Nvisibl.DataLibrary.Models.User", "User1")
                        .WithMany("Friends")
                        .HasForeignKey("User1Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Nvisibl.DataLibrary.Models.User", "User2")
                        .WithMany("FriendedBy")
                        .HasForeignKey("User2Id")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
