using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Quantum;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Quantum.Providers.IonQ.Targets;
using QApp.Qsharp;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Function.Csharp
{
    public static class CreateRandomNumberJob
    {
        [FunctionName("CreateRandomNumberJob")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var validTargetIds = new string[]{"ionq.simulator", "ionq.qpu"};

            string targetId = req.Query["targetId"];
            string nQubitsStr = req.Query["n"];;
            int nQubits = 3;

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            targetId = targetId ?? data?.targetId;

            if (!int.TryParse(nQubitsStr, out nQubits))
            {
                return new BadRequestObjectResult("No valid number of qubits (n) was passed.");
            }
            if (Array.IndexOf(validTargetIds, targetId) < 0)
            {
                return new BadRequestObjectResult("No valid target ID (targetId) was passed. Valid IDs are: " + string.Join(", ", validTargetIds));
            }

            log.LogInformation($"Number of qubits specified in request: {nQubits}");
            log.LogInformation($"Target specified in request (id): {targetId}");

            // define the workspace that manages the target machines.
            var workspace = new Workspace
            (
                subscriptionId: Environment.GetEnvironmentVariable("SubscriptionId"),
                resourceGroupName: Environment.GetEnvironmentVariable("ResourceGroup"),
                workspaceName: Environment.GetEnvironmentVariable("WorkspaceName"),
                location: Environment.GetEnvironmentVariable("WorkspaceLocation")
            );

            var quantumMachine = new IonQQuantumMachine(
                target: targetId,
                workspace: workspace);

            // Submit the random number job to the target machine.
            var randomNumberJob = quantumMachine.SubmitAsync(SampleRandomNumberInRange.Info, nQubits);

            log.LogInformation($"Job submitted to target (id): {randomNumberJob.Result.Id}");

            return new OkObjectResult(randomNumberJob.Result.Id);
        }
    }
}
