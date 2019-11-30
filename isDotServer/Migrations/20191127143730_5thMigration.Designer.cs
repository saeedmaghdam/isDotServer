﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using isDotServer.Models;

namespace isDotServer.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20191127143730_5thMigration")]
    partial class _5thMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("isDotServer.Models.GameSession", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("FinishedTime");

                    b.Property<int>("GuestId");

                    b.Property<int>("HostId");

                    b.Property<DateTime>("StartTime");

                    b.Property<Guid>("ViewId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("WhosTurn");

                    b.Property<int>("Winner");

                    b.HasKey("Id");

                    b.HasIndex("GuestId");

                    b.HasIndex("HostId");

                    b.ToTable("GameSessions");
                });

            modelBuilder.Entity("isDotServer.Models.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Amount");

                    b.Property<string>("Coins");

                    b.Property<string>("RefId");

                    b.Property<int>("UserId");

                    b.Property<Guid>("ViewId")
                        .ValueGeneratedOnAdd();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("isDotServer.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Avatar");

                    b.Property<int>("Coins");

                    b.Property<string>("ConnectionId");

                    b.Property<int>("FailsCount");

                    b.Property<string>("PhoneNumber");

                    b.Property<int>("Rate");

                    b.Property<int>("UnfinishedCount");

                    b.Property<string>("UniqueId");

                    b.Property<string>("Username");

                    b.Property<Guid>("ViewId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("WinsCount");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("isDotServer.Models.GameSession", b =>
                {
                    b.HasOne("isDotServer.Models.User", "Guest")
                        .WithMany("GamesAsGuest")
                        .HasForeignKey("GuestId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("isDotServer.Models.User", "Host")
                        .WithMany("GamesAsHost")
                        .HasForeignKey("HostId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("isDotServer.Models.Payment", b =>
                {
                    b.HasOne("isDotServer.Models.User", "User")
                        .WithMany("Payments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
