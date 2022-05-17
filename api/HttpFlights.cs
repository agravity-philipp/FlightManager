using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;

namespace fhhagenberg
{
    public static class HttpFlights
    {
        [FunctionName("HttpFlight")]
        public static async Task<IActionResult> HttpFlight(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get",  Route = "flight")] HttpRequest req,
            ILogger log)
        {
            return new OkObjectResult(Data.Flights);
        }

        [FunctionName("HttpFlightById")]
        public static async Task<IActionResult> HttpFlightById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get",  Route = "flight/{Id}")] HttpRequest req,
            ILogger log, int Id)
        {
          var flight = Data.Flights.FirstOrDefault(f => f.Id == Id);

          if (flight == null) {
            return new NotFoundResult();
          }

            return new OkObjectResult(flight);
        }
    }
}
