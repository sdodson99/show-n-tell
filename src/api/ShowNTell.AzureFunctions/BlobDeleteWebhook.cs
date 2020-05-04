using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.EventGrid.Models;
using ShowNTell.EntityFramework.Services;
using Azure.Security.KeyVault.Secrets;
using Azure.Identity;
using ShowNTell.EntityFramework.ShowNTellDbContextFactories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ShowNTell.AzureFunctions.Services.EventGridValidations;
using ShowNTell.AzureFunctions.Services.EventGridImageBlobDeletes;
using ShowNTell.AzureFunctions.Handlers;

namespace ShowNTell.AzureFunctions
{
    public class BlobDeleteWebhook
    {
        private readonly BlobDeleteWebhookHandler _handler;

        public BlobDeleteWebhook()
        {
            string accessToken = System.Environment.GetEnvironmentVariable("ACCESS_TOKEN");

            string keyVaultName = System.Environment.GetEnvironmentVariable("KEY_VAULT_NAME");
            string keyVaultUri = $"https://{keyVaultName}.vault.azure.net";
            SecretClient keyVaultClient = new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential());
            string connectionString = keyVaultClient.GetSecret("DATABASE-CONNECTION-STRING").Value.Value;

            IEventGridValidationService eventGridValidationService = new EventGridValidationService();
            IEventGridImageBlobDeleteService eventGridImageBlobDeleteService = new EventGridImageBlobDeleteService(
                new EFImagePostService(new ShowNTellDbContextFactory(
                    new DbContextOptionsBuilder().UseSqlServer(connectionString).Options)), accessToken);

            _handler = new BlobDeleteWebhookHandler(eventGridValidationService, eventGridImageBlobDeleteService);
        }

        [FunctionName("BlobDeleteWebhook")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger logger)
        {
            try
            {
                logger.LogInformation("Received image blob delete event.");

                string token = req.Query["token"];
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                EventGridEvent[] events = JsonConvert.DeserializeObject<EventGridEvent[]>(requestBody);

                return await _handler.Handle(events, token, logger);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new StatusCodeResult(500);
            }
        }
    }
}
