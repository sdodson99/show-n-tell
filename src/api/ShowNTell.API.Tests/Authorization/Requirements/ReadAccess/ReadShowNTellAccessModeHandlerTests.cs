using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Moq;
using NUnit.Framework;
using ShowNTell.API.Authorization.Requirements.ReadAccess;

namespace ShowNTell.API.Tests.Authorization.Requirements.ReadAccess
{
    [TestFixture]
    public class ReadShowNTellAccessModeHandlerTests
    {
        private AuthorizationHandlerContext _context;

        [SetUp]
        public void SetUp()
        {
            _context = new AuthorizationHandlerContext(new []{ new ReadAccessRequirement() }, null, null);
        }

        [Test]
        public async Task HandleAsync_WithReadAccessEnabled_Succeeds()
        {
            ReadShowNTellAccessModeHandler handler = new ReadShowNTellAccessModeHandler(true);

            await handler.HandleAsync(_context);

            Assert.IsTrue(_context.HasSucceeded);
        }

        [Test]
        public async Task HandleAsync_WithReadAccessDisabled_NotDoesSucceed()
        {
            ReadShowNTellAccessModeHandler handler = new ReadShowNTellAccessModeHandler(false);

            await handler.HandleAsync(_context);

            Assert.IsFalse(_context.HasSucceeded);
        }
    }
}