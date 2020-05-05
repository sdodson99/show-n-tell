using System;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using ShowNTell.API.Filters.AccessModes;
using ShowNTell.API.Models.Results;
using ShowNTell.API.Tests.BaseFixtures;

namespace ShowNTell.API.Tests.Filters.AccessModes
{
    public class RequireReadAccessModeFilterTests : ResourceFilterTests
    {
        [Test]
        public void OnResourceExecuting_WithReadAccessModeFalse_HasForbiddenObjectResult()
        {
            RequireReadAccessModeFilter filter = new RequireReadAccessModeFilter(false);
            Type expectedResultType = typeof(ForbiddenObjectResult);

            filter.OnResourceExecuting(_context);
            IActionResult actualResult = _context.Result;

            Assert.IsAssignableFrom(expectedResultType, _context.Result);
        }

        [Test]
        public void OnResourceExecuting_WithReadAccessModeTrue_HasNullResult()
        {
            RequireReadAccessModeFilter filter = new RequireReadAccessModeFilter(true);
            
            filter.OnResourceExecuting(_context);
            IActionResult actualResult = _context.Result;

            Assert.IsNull(actualResult);
        }
    }
}