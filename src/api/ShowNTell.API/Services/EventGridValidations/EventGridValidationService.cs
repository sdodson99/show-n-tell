using System;
using Microsoft.Azure.EventGrid.Models;
using Newtonsoft.Json;

namespace ShowNTell.API.Services.EventGridValidations
{
    public class EventGridValidationService
    {
        /// <summary>
        /// Validate an event grid subscription event.
        /// </summary>
        /// <param name="gridEvent">The event to validate.</param>
        /// <returns>The validation response with the validation code.</returns>
        /// <exception cref="Exception">Thrown if validation failed.</exception>
        public SubscriptionValidationResponse Validate(EventGridEvent gridEvent)
        {
            string data = gridEvent.Data.ToString();
            SubscriptionValidationEventData eventData = JsonConvert.DeserializeObject<SubscriptionValidationEventData>(data);

            if(eventData == null || string.IsNullOrEmpty(eventData.ValidationCode))
            {
                throw new Exception();
            }

            return new SubscriptionValidationResponse(eventData.ValidationCode);
        }
    }
}