using System;
using Microsoft.Azure.EventGrid.Models;
using NUnit.Framework;
using ShowNTell.API.Services.EventGridValidations;

namespace ShowNTell.API.Tests.Services.EventGridValidations
{
    [TestFixture]
    public class EventGridValidationServiceTest
    {
        private EventGridValidationService _validationService;

        [SetUp]
        public void SetUp()
        {
            _validationService = new EventGridValidationService();
        }

        [Test]
        public void Validate_WithValidationResponseData_ReturnsValidationResponse()
        {
            string expectedValidationCode = "test-validation-code123";
            EventGridEvent gridEvent = new EventGridEvent() 
            { 
                Data = $"{{ \"validationCode\": \"{expectedValidationCode}\" }}"
            };

            SubscriptionValidationResponse actual = _validationService.Validate(gridEvent);
            string actualValidationCode = actual.ValidationResponse;

            Assert.AreEqual(expectedValidationCode, actualValidationCode);
        }

        [Test]
        public void Validate_WithNoValidationResponseData_ThrowsException()
        {
            EventGridEvent gridEvent = new EventGridEvent() { Data = string.Empty };

            Assert.Throws<Exception>(() => _validationService.Validate(gridEvent));
        }
    }
}