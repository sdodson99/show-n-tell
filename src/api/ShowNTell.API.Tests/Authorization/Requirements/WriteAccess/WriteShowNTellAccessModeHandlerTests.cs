using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NUnit.Framework;
using ShowNTell.API.Authorization.Requirements.WriteAccess;

namespace ShowNTell.API.Tests.Authorization.Requirements.WriteAccess
{
    [TestFixture]
    public class WriteShowNTellAccessModeHandlerTests
    {
        private AuthorizationHandlerContext _context;

        [SetUp]
        public void SetUp()
        {
            _context = new AuthorizationHandlerContext(new []{ new WriteAccessRequirement() }, null, null);
        }

        [Test]
        public async Task HandleAsync_WithWriteAccessEnabled_Succeeds()
        {
            WriteShowNTellAccessModeHandler handler = new WriteShowNTellAccessModeHandler(true);

            await handler.HandleAsync(_context);

            Assert.IsTrue(_context.HasSucceeded);
        }

        [Test]
        public async Task HandleAsync_WithWriteAccessDisabled_NotDoesSucceed()
        {
            WriteShowNTellAccessModeHandler handler = new WriteShowNTellAccessModeHandler(false);

            await handler.HandleAsync(_context);

            Assert.IsFalse(_context.HasSucceeded);
        }
    }
}