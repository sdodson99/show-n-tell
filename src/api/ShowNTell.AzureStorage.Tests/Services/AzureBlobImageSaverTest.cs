using System;
using System.IO;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using ShowNTell.AzureStorage.Services;
using ShowNTell.AzureStorage.Services.BlobClientFactories;
using ShowNTell.AzureStorage.Services.BlobClients;

namespace ShowNTell.AzureStorage.Tests
{
    public class AzureBlobImageSaverTest
    {
        private const string _blobClientBaseUri = "http://test.com/";

        private FileStream _imageStream;
        private AzureBlobImageSaver _imageSaver;

        [SetUp]
        public void Setup()
        {
            _imageStream = new FileStream("MockFiles/test.txt", FileMode.Open);

            Mock<IBlobClient> mockBlobClient = new Mock<IBlobClient>();
            mockBlobClient.Setup(c => c.Uri).Returns(new Uri(_blobClientBaseUri));

            Mock<IBlobClientFactory> mockBlobClientFactory = new Mock<IBlobClientFactory>();
            mockBlobClientFactory.Setup(m => m.CreateBlobClient()).ReturnsAsync(mockBlobClient.Object);

            _imageSaver = new AzureBlobImageSaver(mockBlobClientFactory.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _imageStream.Dispose();
        }

        [Test]
        public async Task SaveImage_WithImageStream_ReturnsImageStreamUriWithBaseUri()
        {
            string expectedBaseUri = _blobClientBaseUri;

            string actualUri = await _imageSaver.SaveImage(_imageStream);

            Assert.IsTrue(actualUri.StartsWith(expectedBaseUri));
        }

        [Test]
        public async Task SaveImage_WithImageStream_ReturnsImageStreamUriWithGuid()
        {
            string actualUri = await _imageSaver.SaveImage(_imageStream);
            string uniqueUri = actualUri.Substring(_blobClientBaseUri.Length);

            Assert.IsTrue(Guid.TryParse(uniqueUri, out Guid result));
        }
    }
}