using Microsoft.EntityFrameworkCore;
using ShowNTell.Domain.Exceptions;
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

        public async Task<Follow> FollowUser(string userUsername, string followerEmail)
        {
            using(ShowNTellDbContext context = _contextFactory.CreateDbContext())
            {
                User user = await context.Users.FirstOrDefaultAsync(u => u.Username == userUsername);

                if(user == null)
                {
                    throw new EntityNotFoundException<string>(userUsername, typeof(User));
                }

                if(user.Email == followerEmail)
                {
                    throw new OwnProfileFollowException(followerEmail);
                }

                Follow newFollow = new Follow()
                {
                    UserEmail = user.Email,
                    FollowerEmail = followerEmail
                };

                context.Follows.Add(newFollow);
                await context.SaveChangesAsync();

                return newFollow;
            }
        }

        public async Task<bool> UnfollowUser(string userUsername, string followerEmail)
        {
            using (ShowNTellDbContext context = _contextFactory.CreateDbContext())
            {
                bool success = false;

                try
                {
                    User user = await context.Users.FirstOrDefaultAsync(u => u.Username == userUsername);

                    if(user != null)
                    {
                        Follow existingFollow = new Follow()
                        {
                            UserEmail = user.Email,
                            FollowerEmail = followerEmail
                        };

                        context.Entry(existingFollow).State = EntityState.Deleted;
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
