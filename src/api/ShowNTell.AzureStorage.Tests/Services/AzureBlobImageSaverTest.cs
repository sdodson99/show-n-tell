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
        private string _imageExtension;
        private AzureBlobImageStorage _imageSaver;

        [SetUp]
        public void Setup()
        {
            _imageStream = new FileStream("MockFiles/test.txt", FileMode.Open);
            _imageExtension = ".txt";

            Mock<IBlobClient> mockBlobClient = new Mock<IBlobClient>();
            mockBlobClient.Setup(c => c.Uri).Returns(new Uri(_blobClientBaseUri));

            Mock<IBlobClientFactory> mockBlobClientFactory = new Mock<IBlobClientFactory>();
            mockBlobClientFactory.Setup(m => m.CreateBlobClient()).ReturnsAsync(mockBlobClient.Object);

            _imageSaver = new AzureBlobImageStorage(mockBlobClientFactory.Object);
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

            string actualUri = await _imageSaver.SaveImage(_imageStream, _imageExtension);

            Assert.IsTrue(actualUri.StartsWith(expectedBaseUri));
        }

        [Test]
        public async Task SaveImage_WithImageStream_ReturnsImageUriWithGuid()
        {
            string actualUri = await _imageSaver.SaveImage(_imageStream, _imageExtension);
            // Get Guid part of Uri which is after the BaseUri and is as long as a typical Guid.
            string guidUri = actualUri.Substring(_blobClientBaseUri.Length, Guid.Empty.ToString().Length);

            Assert.IsTrue(Guid.TryParse(guidUri, out Guid result));
        }

        [Test]
        public async Task SaveImage_WithImageStream_ReturnsImageUriWithExtension()
        {
            string expectedExtension = _imageExtension;

            string actualUri = await _imageSaver.SaveImage(_imageStream, _imageExtension);
            string actualExtension = actualUri.Substring(actualUri.LastIndexOf('.'));

            Assert.AreEqual(expectedExtension, actualExtension);
        }
    }
}