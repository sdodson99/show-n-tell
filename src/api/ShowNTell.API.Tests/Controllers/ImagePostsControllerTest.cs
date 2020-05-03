using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using ShowNTell.API.Controllers;
using ShowNTell.API.Models.Requests;
using ShowNTell.API.Models.Responses;
using ShowNTell.API.Services.ImageOptimizations;
using ShowNTell.API.Tests.BaseFixtures;
using ShowNTell.Domain.Exceptions;
using ShowNTell.Domain.Models;
using ShowNTell.Domain.Services;
using ShowNTell.Domain.Services.ImageStorages;

namespace ShowNTell.API.Tests.Controllers
{
    [TestFixture]
    public class ImagePostsControllerTest : ControllerTest<ImagePostsController>
    {
        private Mock<IImagePostService> _mockImagePostService;
        private Mock<IRandomImagePostService> _mockRandomImagePostService;
        private Mock<ISearchService> _mockSearchService;
        private Mock<IImageStorage> _mockImageStorage;
        private Mock<IImageOptimizationService> _mockImageOptimizationService;
        private ImagePostsController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockImagePostService = new Mock<IImagePostService>();
            _mockRandomImagePostService = new Mock<IRandomImagePostService>();
            _mockSearchService = new Mock<ISearchService>();
            _mockImageStorage = new Mock<IImageStorage>();
            _mockImageOptimizationService = new Mock<IImageOptimizationService>();

            _controller = new ImagePostsController(_mockImagePostService.Object, _mockRandomImagePostService.Object,
                _mockSearchService.Object, _mockImageStorage.Object, _mockImageOptimizationService.Object, _mapper, _logger, _currentUserService);
        }

        [Test]
        public async Task Search_WithNonNullSearch_ReturnsOk()
        {
            _mockSearchService.Setup(s => s.SearchImagePosts(It.IsNotNull<string>())).ReturnsAsync(new List<ImagePost>());
            Type expectedType = typeof(OkObjectResult);

            ActionResult<IEnumerable<ImagePostResponse>> actual = await _controller.Search(It.IsNotNull<string>());
            ActionResult actualResult = actual.Result;
            
            Assert.IsAssignableFrom(expectedType, actualResult);
        }

        [Test]
        public async Task Search_WithNullSearch_ReturnsOk()
        {
            _mockSearchService.Setup(s => s.SearchImagePosts(It.IsNotNull<string>())).ReturnsAsync(new List<ImagePost>());
            Type expectedType = typeof(OkObjectResult);

            ActionResult<IEnumerable<ImagePostResponse>> actual = await _controller.Search(null);
            ActionResult actualResult = actual.Result;
            
            Assert.IsAssignableFrom(expectedType, actualResult);
        }

        [Test]
        public async Task GetRandom_WithAvailableImagePosts_ReturnsOk()
        {
            _mockRandomImagePostService.Setup(s => s.GetRandom()).ReturnsAsync(new ImagePost());
            Type expectedType = typeof(OkObjectResult);

            ActionResult<ImagePostResponse> actual = await _controller.GetRandom();
            ActionResult actualResult = actual.Result;
            
            Assert.IsAssignableFrom(expectedType, actualResult);
        }

        [Test]
        public async Task GetRandom_WithNoAvailableImagePosts_ReturnsNotFound()
        {
            Type expectedType = typeof(NotFoundResult);

            ActionResult<ImagePostResponse> actual = await _controller.GetRandom();
            ActionResult actualResult = actual.Result;
            
            Assert.IsAssignableFrom(expectedType, actualResult);
        }

        [Test]
        public async Task GetById_WithExistingImagePostId_ReturnsOk()
        {
            _mockImagePostService.Setup(s => s.GetById(It.IsAny<int>())).ReturnsAsync(new ImagePost());
            Type expectedType = typeof(OkObjectResult);

            ActionResult<ImagePostResponse> actual = await _controller.GetById(It.IsAny<int>());
            ActionResult actualResult = actual.Result;
            
            Assert.IsAssignableFrom(expectedType, actualResult);
        }

        [Test]
        public async Task GetById_WithNonExistingImagePostId_ReturnsNotFound()
        {
            Type expectedType = typeof(NotFoundResult);

            ActionResult<ImagePostResponse> actual = await _controller.GetById(It.IsAny<int>());
            ActionResult actualResult = actual.Result;
            
            Assert.IsAssignableFrom(expectedType, actualResult);
        }

        [Test]
        public async Task Create_ReturnsCreated()
        {
            _mockImagePostService.Setup(s => s.Create(It.IsAny<ImagePost>())).ReturnsAsync(new ImagePost());
            Type expectedType = typeof(CreatedResult);
            CreateImagePostRequest createImagePostRequest = new CreateImagePostRequest()
            {
                Image = new Mock<IFormFile>().Object,
                Tags = new List<string>()
            };

            ActionResult<ImagePostResponse> actual = await _controller.Create(createImagePostRequest);
            ActionResult actualResult = actual.Result;
            
            Assert.IsAssignableFrom(expectedType, actualResult);
        }

        [Test]
        public async Task Update_WithExistingImagePost_ReturnsOk()
        {
            _mockImagePostService.Setup(s => s.Update(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<Tag>>())).ReturnsAsync(new ImagePost());
            _mockImagePostService.Setup(s => s.IsAuthor(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(true);
            Type expectedType = typeof(OkObjectResult);
            UpdateImagePostRequest updateImagePostRequest = new UpdateImagePostRequest()
            {
                Tags = new List<string>()
            };

            ActionResult<ImagePostResponse> actual = await _controller.Update(It.IsAny<int>(), updateImagePostRequest);
            ActionResult actualResult = actual.Result;
            
            Assert.IsAssignableFrom(expectedType, actualResult);
        }

        [Test]
        public async Task Update_WithNonExistingImagePost_ReturnsNotFound()
        {
            _mockImagePostService.Setup(s => s.Update(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<IEnumerable<Tag>>())).ThrowsAsync(new EntityNotFoundException());
            _mockImagePostService.Setup(s => s.IsAuthor(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(true);
            Type expectedType = typeof(NotFoundResult);
            UpdateImagePostRequest updateImagePostRequest = new UpdateImagePostRequest()
            {
                Tags = new List<string>()
            };

            ActionResult<ImagePostResponse> actual = await _controller.Update(It.IsAny<int>(), updateImagePostRequest);
            ActionResult actualResult = actual.Result;
            
            Assert.IsAssignableFrom(expectedType, actualResult);
        }

        [Test]
        public async Task Update_WithImagePostNotOwnedByUser_ReturnsForbid()
        {
            _mockImagePostService.Setup(s => s.IsAuthor(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(false);
            Type expectedType = typeof(ForbidResult);

            ActionResult<ImagePostResponse> actual = await _controller.Update(It.IsAny<int>(), It.IsAny<UpdateImagePostRequest>());
            ActionResult actualResult = actual.Result;
            
            Assert.IsAssignableFrom(expectedType, actualResult);
        }

        [Test]
        public async Task Delete_WithExistingImagePost_ReturnsNoContent()
        {
            _mockImagePostService.Setup(s => s.GetById(It.IsAny<int>())).ReturnsAsync(new ImagePost() { UserEmail = CurrentUser.Email });
            _mockImagePostService.Setup(s => s.Delete(It.IsAny<int>())).ReturnsAsync(true);
            _mockImageStorage.Setup(s => s.DeleteImage(It.IsAny<string>())).ReturnsAsync(true);
            Type expectedType = typeof(NoContentResult);

            IActionResult actual = await _controller.Delete(It.IsAny<int>());
            
            Assert.IsAssignableFrom(expectedType, actual);
        }

        [Test]
        public async Task Delete_WithNonExistingImagePost_ReturnsNotFound()
        {
            Type expectedType = typeof(NotFoundResult);

            IActionResult actual = await _controller.Delete(It.IsAny<int>());
            
            Assert.IsAssignableFrom(expectedType, actual);
        }

        [Test]
        public async Task Delete_WithImagePostNotOwnedByUser_ReturnsForbid()
        {
            _mockImagePostService.Setup(s => s.GetById(It.IsAny<int>())).ReturnsAsync(new ImagePost() { UserEmail = It.IsNotIn<string>(CurrentUser.Email)});
            Type expectedType = typeof(ForbidResult);

            IActionResult actual = await _controller.Delete(It.IsAny<int>());
            
            Assert.IsAssignableFrom(expectedType, actual);
        }

        [Test]
        public async Task Delete_WithFailedDelete_ReturnsNotFound()
        {
            _mockImagePostService.Setup(s => s.GetById(It.IsAny<int>())).ReturnsAsync(new ImagePost() { UserEmail = CurrentUser.Email });
            _mockImagePostService.Setup(s => s.Delete(It.IsAny<int>())).ReturnsAsync(false);
            Type expectedType = typeof(NotFoundResult);

            IActionResult actual = await _controller.Delete(It.IsAny<int>());
            
            Assert.IsAssignableFrom(expectedType, actual);
        }

        [Test]
        public async Task Delete_WithFailedStorageDelete_ReturnsBadRequest()
        {
            _mockImagePostService.Setup(s => s.GetById(It.IsAny<int>())).ReturnsAsync(new ImagePost() { UserEmail = CurrentUser.Email });
            _mockImagePostService.Setup(s => s.Delete(It.IsAny<int>())).ReturnsAsync(true);
            _mockImageStorage.Setup(s => s.DeleteImage(It.IsAny<string>())).ReturnsAsync(false);
            Type expectedType = typeof(BadRequestResult);

            IActionResult actual = await _controller.Delete(It.IsAny<int>());
            
            Assert.IsAssignableFrom(expectedType, actual);
        }
    }
}