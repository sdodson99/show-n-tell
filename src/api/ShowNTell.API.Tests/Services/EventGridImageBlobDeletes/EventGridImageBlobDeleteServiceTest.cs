using System.Threading.Tasks;
using Microsoft.Azure.EventGrid.Models;
using Moq;
using NUnit.Framework;
using ShowNTell.API.Services.EventGridImageBlobDeletes;
using ShowNTell.Domain.Exceptions;
using ShowNTell.Domain.Services;

namespace ShowNTell.API.Tests.Services.EventGridImageBlobDeletes
{
    [TestFixture]
    public class EventGridImageBlobDeleteServiceTest
    {
        private Mock<IImagePostService> _mockImagePostService;
        private string _token;
        private EventGridImageBlobDeleteService _deleteService;

        [SetUp]
        public void SetUp()
        {
            _mockImagePostService = new Mock<IImagePostService>();
            _token = "Test123";
            _deleteService = new EventGridImageBlobDeleteService(_mockImagePostService.Object, _token);
        }
        
        [Test]
        public async Task DeleteImagePost_WithExistingUriAndValidToken_ReturnsTrue()
        {
            string uri = "https://test.com";
            EventGridEvent gridEvent = new EventGridEvent()
            {
                Data = $"{{ \"Url\": \"{uri}\" }}"
            };
            _mockImagePostService.Setup(p => p.DeleteByUri(uri)).ReturnsAsync(true);

            bool actual = await _deleteService.DeleteImagePost(gridEvent, _token);

            Assert.IsTrue(actual);
        }

        [Test]
        public void DeleteImagePost_WithInvalidToken_ThrowsInvalidTokenExceptionForToken()
        {
            string expectedToken = "bad-token";

            InvalidTokenException actual = Assert.ThrowsAsync<InvalidTokenException>(() => _deleteService.DeleteImagePost(It.IsAny<EventGridEvent>(), expectedToken));
            string actualToken = actual.Token;

            Assert.AreEqual(expectedToken, actualToken);
        }

        [Test]
        public async Task DeleteImagePost_WithNonExistingUriAndValidToken_ReturnsFalse()
        {
            string uri = "https://test.com";
            EventGridEvent gridEvent = new EventGridEvent()
            {
                Data = $"{{ \"Url\": \"{uri}\" }}"
            };
            _mockImagePostService.Setup(p => p.DeleteByUri(uri)).ReturnsAsync(false);

            bool actual = await _deleteService.DeleteImagePost(gridEvent, _token);

            Assert.IsFalse(actual);
        }
    }
}