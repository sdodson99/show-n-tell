using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShowNTell.Domain.Exceptions;
using ShowNTell.Domain.Models;
using ShowNTell.Domain.Services;
using ShowNTell.EntityFramework.ShowNTellDbContextFactories;

namespace ShowNTell.EntityFramework.Services
{
    public class EFLikeService : ILikeService
    {
        private readonly IShowNTellDbContextFactory _contextFactory;

        public EFLikeService(IShowNTellDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Like> LikeImagePost(int imagePostId, string userEmail)
        {
            using(ShowNTellDbContext context = _contextFactory.CreateDbContext())
            {
                ImagePost existingImagePost = await context.ImagePosts
                    .Include(p => p.Likes)
                    .FirstOrDefaultAsync(p => p.Id == imagePostId);
                if(existingImagePost == null)
                {
                    throw new EntityNotFoundException<int>(imagePostId);
                }

                User existingUser = await context.Users.FindAsync(userEmail);
                if(existingUser == null)
                {
                    throw new EntityNotFoundException<string>(userEmail);
                }

                if(existingImagePost.UserEmail == userEmail)
                {
                    throw new OwnImagePostLikeException(existingImagePost, userEmail);
                }

                if(existingImagePost.Likes.Any(l => l.UserEmail == userEmail))
                {
                    throw new DuplicateLikeException(existingImagePost, userEmail);
                }

                Like like = new Like() 
                {
                    ImagePostId = imagePostId,
                    UserEmail = userEmail,
                    DateCreated = DateTime.Now
                };

                context.Likes.Add(like);
                await context.SaveChangesAsync();

                return like;
            }
        }

        public async Task<bool> UnlikeImagePost(int imagePostId, string userEmail)
        {
            using(ShowNTellDbContext context = _contextFactory.CreateDbContext())
            {
                bool success = false;

                try
                {
                    Like like = await context.Likes
                        .FirstOrDefaultAsync(l => l.ImagePostId == imagePostId && l.UserEmail == userEmail);

                    if(like != null) 
                    {
                        context.Likes.Remove(like);
                        await context.SaveChangesAsync();

                        success = true;
                    }
                }
                catch (Exception)
                {
                    success = false;
                }

                return success;                
            }
        }
    }
}