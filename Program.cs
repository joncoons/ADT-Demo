using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using Azure;
using Azure.DigitalTwins.Core;
using Azure.Identity;
using System;

namespace DigitalTwinsDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //string clientId = "5c694aad-e4ed-43a3-95cc-3d45af6685e5";
            //string tenantId = "72f988bf-86f1-41af-91ab-2d7cd011db47";
            string adtInstanceUrl = "https://digital-twin-demo-jc.api.wcus.digitaltwins.azure.net";
            var credOpts = new DefaultAzureCredentialOptions()
            {
                ExcludeSharedTokenCacheCredential = true,
                ExcludeVisualStudioCodeCredential = true
            };
            var cred = new DefaultAzureCredential(credOpts);
            DigitalTwinsClient client = new DigitalTwinsClient(new Uri(adtInstanceUrl), cred);
            //var credentials = new InteractiveBrowserCredential(tenantId, clientId);
            // DigitalTwinsClient client = new DigitalTwinsClient(new Uri(adtInstanceUrl), credentials);
            Console.WriteLine($"Service client created – ready to go");
            Console.WriteLine();
            Console.WriteLine($"Upload a model");
            
            var typeList = new List<string>();
            string dtdl = File.ReadAllText("SampleModel.json");
            typeList.Add(dtdl);
            // Upload the model to the service
            await client.CreateModelsAsync(typeList);

            // Read a list of models back from the service
            AsyncPageable<ModelData> modelDataList = client.GetModelsAsync();
            await foreach (ModelData md in modelDataList)
            {
                Console.WriteLine($"Type name: {md.DisplayName}: {md.Id}");
            }
        }
    }
}
