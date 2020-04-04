using System;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using ShowNTell.EntityFramework.ShowNTellDbContextFactories;

namespace ShowNTell.EntityFramework.Tests.BaseFixtures
{
    [TestFixture]
    public abstract class EFTest
    {
        private DbContextOptions _dbContextOptions;
        
        protected IShowNTellDbContextFactory _contextFactory;

        [SetUp]
        public void BaseSetup()
        {
            _dbContextOptions = new DbContextOptionsBuilder()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .Options;

            SeedDbContext();    

            Mock<IShowNTellDbContextFactory> contextFactory = new Mock<IShowNTellDbContextFactory>();
            contextFactory.Setup(c => c.CreateDbContext()).Returns(() => GetDbContext());

            _contextFactory = contextFactory.Object;
        }

        protected ShowNTellDbContext GetDbContext()
        {
            return new ShowNTellDbContext(_dbContextOptions);
        }

        private void SeedDbContext()
        {
            using(ShowNTellDbContext context = GetDbContext())
            {
                Seed(context);
                context.SaveChanges();
            }
        }

        protected abstract void Seed(ShowNTellDbContext context);
    }
}