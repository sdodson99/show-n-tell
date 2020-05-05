using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;

namespace ShowNTell.API.Tests.BaseFixtures
{
    public class ResourceFilterTests
    {
        protected ResourceExecutingContext _context;

        [SetUp]
        public void BaseSetUp()
        {
            Mock<HttpContext> mockContext = new Mock<HttpContext>();
            Mock<RouteData> mockRoute = new Mock<RouteData>();
            Mock<ActionDescriptor> mockDescriptor = new Mock<ActionDescriptor>();
            
            _context = new ResourceExecutingContext(new ActionContext(mockContext.Object, mockRoute.Object, mockDescriptor.Object), 
                new List<IFilterMetadata>(), new List<IValueProviderFactory>());
        }
    }
}