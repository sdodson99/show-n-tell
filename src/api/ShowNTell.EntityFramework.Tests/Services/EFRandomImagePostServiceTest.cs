using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using ShowNTell.Domain.Models;
using ShowNTell.EntityFramework.Services;
using ShowNTell.EntityFramework.ShowNTellDbContextFactories;
using ShowNTell.EntityFramework.Tests.BaseFixtures;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShowNTell.EntityFramework.Tests.Services
{
    [TestFixture]
    public class EFRandomImagePostServiceTest : EFTest
    {
        private EFRandomImagePostService _randomImagePostService;

        [SetUp]
        public void Setup()
        {
            _randomImagePostService = new EFRandomImagePostService(_contextFactory);
        }

        [Test]
        public async Task GetRandom_WithExistingImagePosts_ReturnsImagePost()
        {
            using(ShowNTellDbContext context = GetDbContext())
            {
                context.ImagePosts.Add(new ImagePost());
                context.SaveChanges();
            }

            ImagePost actual = await _randomImagePostService.GetRandom();

            Assert.IsNotNull(actual);
        }

        [Test]
        public async Task GetRandom_WithNoExistingImagePosts_ReturnsNull()
        {
            ImagePost actual = await _randomImagePostService.GetRandom();

            Assert.IsNull(actual);
        }

        protected override void Seed(ShowNTellDbContext context){}
    }
}
