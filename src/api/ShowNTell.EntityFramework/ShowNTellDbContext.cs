using Microsoft.EntityFrameworkCore;
using ShowNTell.Domain.Models;
using System;

namespace ShowNTell.EntityFramework
{
    public class ShowNTellDbContext : DbContext
    {
        public ShowNTellDbContext(DbContextOptions options) : base(options) { }

        public DbSet<ImagePost> ImagePosts { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
