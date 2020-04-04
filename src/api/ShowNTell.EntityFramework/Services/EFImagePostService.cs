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
                ICollection<ImagePostTag> mergedTags = null;

                if(imagePost.Tags != null && imagePost.Tags.Count > 0)
                {
                    mergedTags = await GetMergedNewAndExistingTagsFromContext(imagePost.Tags, context);
                    imagePost.Tags = ConvertImagePostTagsForSave(mergedTags);
                }

                context.ImagePosts.Add(imagePost);
                await context.SaveChangesAsync();

                imagePost.Tags = mergedTags;

                return imagePost;
            }
        }

        public async Task<ImagePost> Update(int id, string description)
        {
            return await Update(id, description, null);
        }

        public async Task<ImagePost> Update(int id, string description, IEnumerable<Tag> tags)
        {
            using (ShowNTellDbContext context = _contextFactory.CreateDbContext())
            {
                ImagePost storedImagePost = new ImagePost()
                {
                    Id = id
                };

                context.Attach(storedImagePost);
                storedImagePost.Description = description;

                ICollection<ImagePostTag> mergedTags = null;
                
                if(tags != null && tags.Count() > 0)
                {
                    mergedTags = await GetMergedNewAndExistingTagsFromContext(tags, context);
                    storedImagePost.Tags = ConvertImagePostTagsForSave(mergedTags);
                }

                try
                {
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw new EntityNotFoundException<int>(id);
                }

                storedImagePost.Tags = mergedTags;

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

        private async Task<ICollection<ImagePostTag>> GetMergedNewAndExistingTagsFromContext(IEnumerable<ImagePostTag> newImagePostTags, ShowNTellDbContext context)
        {
            return await GetMergedNewAndExistingTagsFromContext(newImagePostTags.Select(t => t.Tag), context);
        }

        private async Task<ICollection<ImagePostTag>> GetMergedNewAndExistingTagsFromContext(IEnumerable<Tag> newTags, ShowNTellDbContext context)
        {
            // Find the new image post tags that are already in the database.
            IEnumerable<Tag> existingTags = await context.Tags
                .Where(t => newTags.Select(t => t.Content)
                .Contains(t.Content))
                .ToListAsync();
            HashSet<string> existingTagContent = existingTags.Select(t => t.Content).ToHashSet();

            // Merge new image post tags in with the existing tags.
            List<Tag> mergedImagePostTags = new List<Tag>(existingTags);
            foreach(Tag tag in newTags)
            {
                if(!existingTagContent.Contains(tag.Content))
                {
                    existingTagContent.Add(tag.Content);
                    mergedImagePostTags.Add(tag);
                }
            }

            // Select into a list of existing database tags and new tags.
            return new List<ImagePostTag>(mergedImagePostTags.Select(t => new ImagePostTag()
            {
                TagId = t.Id,
                Tag = new Tag()
                {
                    Content = t.Content
                }
            }));
        }

        private ICollection<ImagePostTag> ConvertImagePostTagsForSave(ICollection<ImagePostTag> tags)
        {
            return tags.Select(t => t.TagId == 0 ? new ImagePostTag()
            {
                Tag = t.Tag
            } : new ImagePostTag()
            {
                TagId = t.TagId
            }).ToList();
        }
    }
}
