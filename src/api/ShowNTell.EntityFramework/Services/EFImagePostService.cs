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

        public async Task<ImagePost> GetById(int id)
        {
            using (ShowNTellDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.ImagePosts
                    .Include(p => p.User)
                    .Include(p => p.Likes)
                    .Include(p => p.Comments)
                        .ThenInclude(c => c.User)
                    .Include(p => p.Tags)
                        .ThenInclude(t => t.Tag)
                    .FirstOrDefaultAsync(p => p.Id == id);
            }
        }

        public async Task<ImagePost> Create(ImagePost imagePost)
        {
            using(ShowNTellDbContext context = _contextFactory.CreateDbContext())
            {
                if(imagePost.Tags != null)
                {
                    // Get all the tags on the image post.
                    IEnumerable<Tag> newImagePostTags = imagePost.Tags.Select(t => t.Tag);

                    // Find the new image post tags that are already in the database.
                    IEnumerable<Tag> existingTags = await context.Tags
                        .Where(t => newImagePostTags.Select(t => t.Content).Contains(t.Content))
                        .ToListAsync();
                    HashSet<string> existingTagContent = existingTags.Select(t => t.Content).ToHashSet();

                    // Merge new image post tags in with the existing tags.
                    List<Tag> mergedImagePostTags = new List<Tag>(existingTags);
                    foreach(Tag newTag in newImagePostTags)
                    {
                        if(!existingTagContent.Contains(newTag.Content))
                        {
                            existingTagContent.Add(newTag.Content);
                            mergedImagePostTags.Add(newTag);
                        }
                    }

                    // Set the image post tags to the list of existing database tags and new tags.
                    imagePost.Tags = new List<ImagePostTag>(mergedImagePostTags.Select(t => t.Id == 0 ? new ImagePostTag()
                    {
                        Tag = new Tag()
                        {
                            Id = t.Id,
                            Content = t.Content
                        }
                    } : new ImagePostTag()
                    {
                        TagId = t.Id
                    }));
                }

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
        public async Task<ImagePost> Update(int id, string description)
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

        public async Task<bool> IsAuthor(int id, string email)
        {
            using (ShowNTellDbContext context = _contextFactory.CreateDbContext())
            {
                ImagePost storedImagePost = await context.ImagePosts.FindAsync(id);

                return storedImagePost != null && storedImagePost.UserEmail == email;
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
