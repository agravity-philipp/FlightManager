using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace fhhagenberg
{
    public static class HttpFlights
    {
        [FunctionName("HttpFlights")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get",  Route = "flight")] HttpRequest req,
            ILogger log)
        {
            return new OkObjectResult(Data.Flights);
        }
    }
}
