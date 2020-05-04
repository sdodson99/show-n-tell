using Microsoft.Azure.EventGrid.Models;
using System;

namespace ShowNTell.AzureFunctions.Services.EventGridValidations
{
    public interface IEventGridValidationService
    {
        /// <summary>
        /// Validate an event grid subscription event.
        /// </summary>
        /// <param name="gridEvent">The event to validate.</param>
        /// <returns>The validation response with the validation code.</returns>
        /// <exception cref="Exception">Thrown if validation failed.</exception>
        SubscriptionValidationResponse Validate(EventGridEvent gridEvent);
    }
}