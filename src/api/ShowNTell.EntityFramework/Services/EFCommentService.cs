using System.Threading.Tasks;
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
    }
}