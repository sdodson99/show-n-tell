using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShowNTell.Prototype.Models
{
    public class ShowNTellDbContext : DbContext
    {
        public ShowNTellDbContext(DbContextOptions options) : base(options) { }

        public DbSet<ImagePost> ImagePosts { get; set; }
    }
}
