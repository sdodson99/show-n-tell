using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShowNTell.Domain.Exceptions;
using ShowNTell.Domain.Models;
using ShowNTell.Domain.Services;
using ShowNTell.EntityFramework.ShowNTellDbContextFactories;

namespace ShowNTell.EntityFramework.Services
{
    public class EFCommentService : ICommentService
    {
        private readonly IShowNTellDbContextFactory _contextFactory;

        public EFCommentService(IShowNTellDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Comment> Create(Comment comment)
        {
            using(ShowNTellDbContext context = _contextFactory.CreateDbContext())
            {
                context.Comments.Add(comment);
                await context.SaveChangesAsync();

                return comment;
            }
        }

        public async Task<Comment> Update(int id, string content)
        {
            using(ShowNTellDbContext context = _contextFactory.CreateDbContext())
            {
                Comment comment = await context.Comments.FirstOrDefaultAsync(c => c.Id == id);
                
                if(comment == null)
                {
                    throw new EntityNotFoundException<int>(id, typeof(Comment));
                }

                comment.Content = content;

                context.Update(comment);
                await context.SaveChangesAsync();

                return comment;
            }
        }

        public async Task<bool> Delete(int commentId)
        {
            using(ShowNTellDbContext context = _contextFactory.CreateDbContext())
            {
                bool success = false;

                Comment comment = new Comment() { Id = commentId };

                try
                {
                    context.Remove(comment);

                    await context.SaveChangesAsync();
                    success = true;
                }
                catch (System.Exception)
                {
                    success = false;
                }

                return success;
            }
        }

        public async Task<bool> IsCommentOwner(int commentId, string userEmail)
        {
            using(ShowNTellDbContext context = _contextFactory.CreateDbContext())
            {
                bool success = false;

                try
                {
                    Comment comment = await context.Comments.FirstOrDefaultAsync(c => c.Id == commentId);

                    success = comment.UserEmail == userEmail;
                }
                catch (System.Exception)
                {
                    success = false;
                }

                return success;
            }
        }

        public async Task<bool> CanDelete(int commentId, string userEmail)
        {
            using(ShowNTellDbContext context = _contextFactory.CreateDbContext())
            {
                bool success = false;

                try
                {
                    Comment comment = await context.Comments
                        .Include(c => c.ImagePost)
                        .FirstOrDefaultAsync(c => c.Id == commentId);

                    success = comment.UserEmail == userEmail || comment.ImagePost.UserEmail == userEmail;
                }
                catch (System.Exception)
                {
                    success = false;
                }

                return success;
            }
        }
    }
}