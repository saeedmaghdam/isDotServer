using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace isDotServer.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=isDotGame;User ID=sa;Password=Abcd@123456;Connection Timeout=30;Integrated Security=False;Persist Security Info=False;;");
            //optionsBuilder.UseSqlServer("Server=88.99.137.107\\MSSQLSERVER2016,51016;Database=ebiasgmj_isDotGame;User ID=ebiasgmj_isDotGame;Password=Abcd@123456;Connection Timeout=30;Integrated Security=False;Persist Security Info=False;;");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<GameSession> GameSessions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configure default schema
            //modelBuilder.HasDefaultSchema("Admin");

            //Map entity to table
            //modelBuilder.Entity<GameSession>()
            //.HasKey(x => new { x.HostId, x.GuestId });

            modelBuilder.Entity<GameSession>()
                        .HasOne(x => x.Host)
                        .WithMany(x => x.GamesAsHost)
                        .HasForeignKey(x => x.HostId)
                        .IsRequired()
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GameSession>()
                        .HasOne(x => x.Guest)
                        .WithMany(x => x.GamesAsGuest)
                        .HasForeignKey(x => x.GuestId)
                        .IsRequired()
                        .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
