using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
namespace Company.Function
{
    public static class ZoomServerHttpTrigger
    {
        [FunctionName("ZoomServerHttpTrigger")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            //string name = req.Query["name"];
            string responseMessage = "Datos recibidos.";
            string requestHeader = req.Headers["Authorization"];


            if (requestHeader!=Environment.GetEnvironmentVariable("ZoomToken")){
                return new OkObjectResult("Fuente desconocida.");
            }
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            //log.LogInformation("Primero: "+requestBody+".");
            //dynamic data = JsonConvert.DeserializeObject(requestBody);
            //name = name ?? data?.name;

            //string.IsNullOrEmpty(name)
                //? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                //: $"Hello, {name}. This HTTP triggered function executed successfully.";
            
            // Create a producer client that you can use to send events to an event hub

            try{
            /*
            await using (var producerClient = new EventHubProducerClient(Environment.GetEnvironmentVariable("EventHubConnectionSender"), Environment.GetEnvironmentVariable("EventHubName")))
            {
                // Create a batch of events 
                log.LogInformation("C# Message sent to Event Hub.");
                using EventDataBatch eventBatch = await producerClient.CreateBatchAsync();
                log.LogInformation("Enviando Mensaje: "+requestBody);
                // Add events to the batch. An event is a represented by a collection of bytes and metadata. 
                eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(requestBody)));

                // Use the producer client to send the batch of events to the event hub
                await producerClient.SendAsync(eventBatch);
            }
            */
            await using (var producerClient = new EventHubProducerClient(Environment.GetEnvironmentVariable("EventHub2"), Environment.GetEnvironmentVariable("EventHubName2")))
            {
                // Create a batch of events 
                log.LogInformation("C# Message sent to Event Hub.");
                using EventDataBatch eventBatch = await producerClient.CreateBatchAsync();
                log.LogInformation("Enviando Mensaje: "+requestBody);
                // Add events to the batch. An event is a represented by a collection of bytes and metadata. 
                eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(requestBody)));

                // Use the producer client to send the batch of events to the event hub
                await producerClient.SendAsync(eventBatch);
            }

            }
            catch (Exception){
                responseMessage="No se pudo acceder al Event Hub.";
            }
            return new OkObjectResult(responseMessage);
        }
    }
}
