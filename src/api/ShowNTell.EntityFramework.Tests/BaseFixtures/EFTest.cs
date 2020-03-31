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
        private string _databaseName;
        private bool _isSeeded;
        
        protected IShowNTellDbContextFactory _contextFactory;

        [SetUp]
        public void BaseSetup()
        {
            _databaseName = Guid.NewGuid().ToString();
            _isSeeded = false;

            Mock<IShowNTellDbContextFactory> contextFactory = new Mock<IShowNTellDbContextFactory>();
            contextFactory.Setup(c => c.CreateDbContext()).Returns(() => GetDbContext());

            _contextFactory = contextFactory.Object;
        }

        protected ShowNTellDbContext GetDbContext()
        {
            DbContextOptions options = new DbContextOptionsBuilder().UseInMemoryDatabase(_databaseName).Options;

            ShowNTellDbContext context = new ShowNTellDbContext(options);

            if(!_isSeeded)
            {
                Seed(context);
                context.SaveChanges();

                _isSeeded = true;
            }

            return context;
        }

        protected abstract void Seed(ShowNTellDbContext context);
    }
}