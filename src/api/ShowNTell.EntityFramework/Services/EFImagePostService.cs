using Microsoft.EntityFrameworkCore;
using ShowNTell.Domain.Exceptions;
using ShowNTell.Domain.Models;
using ShowNTell.Domain.Services;
using ShowNTell.EntityFramework.ShowNTellDbContextFactories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowNTell.EntityFramework.Services
{
    public class EFImagePostService : IImagePostService
    {
        private readonly IShowNTellDbContextFactory _contextFactory;

        public EFImagePostService(IShowNTellDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<IEnumerable<ImagePost>> GetAllByUserEmail(string email)
        {
            using (ShowNTellDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.ImagePosts
                    .Include(p => p.User)
                    .Where(p => p.User.Email == email)
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<ImagePost>> GetAllByTagId(int id)
        {
            using (ShowNTellDbContext context = _contextFactory.CreateDbContext())
            {
                throw new NotImplementedException();
            }
        }

        public async Task<ImagePost> GetById(int id)
        {
            using (ShowNTellDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.ImagePosts.FindAsync(id);
            }
        }

        public async Task<ImagePost> Create(ImagePost imagePost)
        {
            using(ShowNTellDbContext context = _contextFactory.CreateDbContext())
            {
                context.ImagePosts.Add(imagePost);
                await context.SaveChangesAsync();

                return imagePost;
            }
        }

        public async Task<ImagePost> Update(int id, ImagePost imagePost)
        {
            using (ShowNTellDbContext context = _contextFactory.CreateDbContext())
            {
                ImagePost storedImagePost = await GetByIdFromContext(id, context);

                // Do not update Id or DateCreated on stored post.
                imagePost.Id = storedImagePost.Id;
                imagePost.DateCreated = storedImagePost.DateCreated;

                context.Entry(storedImagePost).CurrentValues.SetValues(imagePost);
                await context.SaveChangesAsync();

                return storedImagePost;
            }
        }
        public async Task<ImagePost> UpdateDescription(int id, string description)
        {
            using (ShowNTellDbContext context = _contextFactory.CreateDbContext())
            {
                ImagePost storedImagePost = await GetByIdFromContext(id, context);

                storedImagePost.Description = description;
                await context.SaveChangesAsync();

                return storedImagePost;
            }
        }

        public async Task<bool> Delete(int id)
        {
            using (ShowNTellDbContext context = _contextFactory.CreateDbContext())
            {
                bool success = false;

                ImagePost storedImagePost = await context.ImagePosts.FindAsync(id);

                if (storedImagePost != null)
                {
                    context.ImagePosts.Remove(storedImagePost);
                    await context.SaveChangesAsync();

                    success = true;
                }

                return success;
            }
        }

        private static async Task<ImagePost> GetByIdFromContext(int id, ShowNTellDbContext context)
        {
            ImagePost storedImagePost = await context.ImagePosts.FindAsync(id);

            if (storedImagePost == null)
            {
                throw new EntityNotFoundException<int>(id);
            }

            return storedImagePost;
        }
    }
}
