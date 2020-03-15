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
        }

        public DbSet<ImagePost> ImagePosts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
