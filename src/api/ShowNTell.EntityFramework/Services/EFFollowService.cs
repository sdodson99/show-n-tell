using Microsoft.EntityFrameworkCore;
using ShowNTell.Domain.Models;
using ShowNTell.Domain.Services;
using ShowNTell.EntityFramework.ShowNTellDbContextFactories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShowNTell.EntityFramework.Services
{
    public class EFFollowService : IFollowService
    {
        private readonly IShowNTellDbContextFactory _contextFactory;

        public EFFollowService(IShowNTellDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Follow> FollowUser(string userEmail, string followerEmail)
        {
            using(ShowNTellDbContext context = _contextFactory.CreateDbContext())
            {
                Follow newFollow = new Follow()
                {
                    UserEmail = userEmail,
                    FollowerEmail = followerEmail
                };

                context.Follows.Add(newFollow);
                await context.SaveChangesAsync();

                return newFollow;
            }
        }

        public async Task<bool> UnfollowUser(string userEmail, string followerEmail)
        {
            using (ShowNTellDbContext context = _contextFactory.CreateDbContext())
            {
                bool success = false;

                Follow existingFollow = new Follow()
                {
                    UserEmail = userEmail,
                    FollowerEmail = followerEmail
                };

                try
                {
                    context.Entry(existingFollow).State = EntityState.Deleted;
                    await context.SaveChangesAsync();

                    success = true;
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
