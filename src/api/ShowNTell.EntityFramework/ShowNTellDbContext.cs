using Microsoft.EntityFrameworkCore;
using ShowNTell.Domain.Models;
using System;

namespace ShowNTell.EntityFramework
{
    public class ShowNTellDbContext : DbContext
    {
        public ShowNTellDbContext(DbContextOptions options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Like>().HasKey(l => new {l.ImagePostId, l.UserEmail });

            modelBuilder.Entity<User>().HasKey(u => u.Email);

            modelBuilder.Entity<Follow>().HasKey(f => new { f.UserEmail, f.FollowerEmail });
            modelBuilder.Entity<Follow>()
                .HasOne(f => f.User)
                .WithMany(u => u.Followers)
                .HasForeignKey(f => f.UserEmail)
                .IsRequired();
            modelBuilder.Entity<Follow>()
                .HasOne(f => f.Follower)
                .WithMany(u => u.Following)
                .HasForeignKey(f => f.FollowerEmail)
                .IsRequired();
        }

        public DbSet<ImagePost> ImagePosts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Follow> Follows { get; set; }
    }
}
