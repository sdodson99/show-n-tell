using System;
using System.IO;
using System.Threading.Tasks;
using Azure;
using Moq;
using NUnit.Framework;
using ShowNTell.AzureStorage.Services;
using ShowNTell.AzureStorage.Services.BlobClientFactories;
using ShowNTell.AzureStorage.Services.BlobClients;
using ShowNTell.AzureStorage.Tests.MockResponses;

namespace ShowNTell.AzureStorage.Tests
{
    public class AzureBlobImageStorageTest
    {
        private const string _blobClientBaseUri = "http://test.com/";
        private const string _existingBlobName = "image.png";
        private const string _nonExistingBlobName = "fake.png";

        private FileStream _imageStream;
        private string _imageExtension;
        private AzureBlobImageStorage _imageStorage;

        [SetUp]
        public void Setup()
        {
            _imageStream = new FileStream("MockFiles/test.txt", FileMode.Open);
            _imageExtension = ".txt";

            Mock<IBlobClient> mockBlobClient = new Mock<IBlobClient>();
            mockBlobClient.Setup(c => c.Uri).Returns(new Uri(_blobClientBaseUri));
            mockBlobClient.Setup(c => c.DeleteBlobAsync(_existingBlobName)).ReturnsAsync(new SuccessResponse());
            mockBlobClient.Setup(c => c.DeleteBlobAsync(It.Is<string>(n => n != _existingBlobName))).ReturnsAsync(new ErrorResponse());

            Mock<IBlobClientFactory> mockBlobClientFactory = new Mock<IBlobClientFactory>();
            mockBlobClientFactory.Setup(m => m.CreateBlobClient()).ReturnsAsync(mockBlobClient.Object);

            _imageStorage = new AzureBlobImageStorage(mockBlobClientFactory.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _imageStream.Dispose();
        }

        [Test]
        public async Task SaveImage_WithImageStream_ReturnsImageUriWithBaseUri()
        {
            string expectedBaseUri = _blobClientBaseUri;

            string actualUri = await _imageStorage.SaveImage(_imageStream, _imageExtension);

            Assert.IsTrue(actualUri.StartsWith(expectedBaseUri));
        }

        [Test]
        public async Task SaveImage_WithImageStream_ReturnsImageUriWithGuid()
        {
            string actualUri = await _imageStorage.SaveImage(_imageStream, _imageExtension);
            // Get Guid part of Uri which is after the BaseUri and is as long as a typical Guid.
            string guidUri = actualUri.Substring(_blobClientBaseUri.Length, Guid.Empty.ToString().Length);

            Assert.IsTrue(Guid.TryParse(guidUri, out Guid result));
        }

        [Test]
        public async Task SaveImage_WithImageStream_ReturnsImageUriWithExtension()
        {
            string expectedExtension = _imageExtension;

            string actualUri = await _imageStorage.SaveImage(_imageStream, _imageExtension);
            string actualExtension = actualUri.Substring(actualUri.LastIndexOf('.'));

            Assert.AreEqual(expectedExtension, actualExtension);
        }

        [Test]
        public async Task DeleteImage_WithExistingImage_ReturnsTrue()
        {
            string fileUri = Path.Combine(_blobClientBaseUri, _existingBlobName);

            bool success = await _imageStorage.DeleteImage(fileUri);

            Assert.IsTrue(success);
        }

        [Test]
        public async Task DeleteImage_WithNonExistingImage_ReturnsFalse()
        {
            string fileUri = Path.Combine(_blobClientBaseUri, _nonExistingBlobName);

            bool success = await _imageStorage.DeleteImage(fileUri);

            Assert.IsFalse(success);
        }
    }
}