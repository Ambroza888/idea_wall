﻿// <auto-generated />
using System;
using CS_proj.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CS_proj.Migrations
{
    [DbContext(typeof(MyContext))]
    [Migration("20191018163438_FirstMigration")]
    partial class FirstMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("CS_proj.Models.Idea", b =>
                {
                    b.Property<int>("IdeaId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("IdeaText")
                        .IsRequired();

                    b.Property<DateTime>("UpdatedAt");

                    b.Property<int>("UserId");

                    b.HasKey("IdeaId");

                    b.HasIndex("UserId");

                    b.ToTable("Ideas");
                });

            modelBuilder.Entity("CS_proj.Models.Like", b =>
                {
                    b.Property<int>("LikeId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int>("IdeaId");

                    b.Property<DateTime>("UpdatedAt");

                    b.Property<int>("UserId");

                    b.HasKey("LikeId");

                    b.HasIndex("IdeaId");

                    b.HasIndex("UserId");

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("CS_proj.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<DateTime>("UpdatedAt");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CS_proj.Models.Idea", b =>
                {
                    b.HasOne("CS_proj.Models.User", "User")
                        .WithMany("Ideas")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CS_proj.Models.Like", b =>
                {
                    b.HasOne("CS_proj.Models.Idea", "Idea")
                        .WithMany("Likes")
                        .HasForeignKey("IdeaId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CS_proj.Models.User", "User")
                        .WithMany("Likes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
