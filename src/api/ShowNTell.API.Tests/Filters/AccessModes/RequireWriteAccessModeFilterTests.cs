using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;
using ShowNTell.API.Filters.AccessModes;
using ShowNTell.API.Tests.BaseFixtures;

namespace ShowNTell.API.Tests.Filters.AccessModes
{
    [TestFixture]
    public class RequireWriteAccessModeFilterTests : ResourceFilterTests
    {
        [Test]
        public void OnResourceExecuting_WithWriteAccessModeFalse_HasBadRequestObjectResult()
        {
            RequireWriteAccessModeFilter filter = new RequireWriteAccessModeFilter(false);
            Type expectedResultType = typeof(BadRequestObjectResult);

            filter.OnResourceExecuting(_context);
            IActionResult actualResult = _context.Result;

            Assert.IsAssignableFrom(expectedResultType, _context.Result);
        }

        [Test]
        public void OnResourceExecuting_WithWriteAccessModeTrue_HasNullResult()
        {
            RequireWriteAccessModeFilter filter = new RequireWriteAccessModeFilter(true);
            
            filter.OnResourceExecuting(_context);
            IActionResult actualResult = _context.Result;

            Assert.IsNull(actualResult);
        }
    }
}