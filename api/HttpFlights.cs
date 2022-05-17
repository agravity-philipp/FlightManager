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
    [FunctionName(nameof(HttpGetAllFlights))]
    public static async Task<IActionResult> HttpGetAllFlights(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "flight")] HttpRequest req,
        ILogger log)
    {
      var flights = Data.Flights;

      string from = req.Query["from"];
      if (!string.IsNullOrEmpty(from))
      {
        flights = flights.Where(f => f.From.Equals(from.Trim(), StringComparison.OrdinalIgnoreCase)).ToList();
      }

      string to = req.Query["to"];
      if (!string.IsNullOrEmpty(to))
      {
        flights = flights.Where(f => f.To.Equals(to.Trim(), StringComparison.OrdinalIgnoreCase)).ToList();
      }

      return new OkObjectResult(flights);
    }

    [FunctionName(nameof(HttpGetSingleFlightById))]
    public static async Task<IActionResult> HttpGetSingleFlightById(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "flight/{Id}")] HttpRequest req,
        ILogger log, int Id)
    {
      var flight = Data.Flights.FirstOrDefault(f => f.Id == Id);

      if (flight == null)
      {
        return new NotFoundResult();
      }

      return new OkObjectResult(flight);
    }

    [FunctionName(nameof(HttpPostSingleFlight))]
    public static async Task<IActionResult> HttpPostSingleFlight(
           [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "flight")] HttpRequest req,
           ILogger log)
    {

      Flight input = null;
      // log.LogInformation("Creating a new collection list item");
      using (var streamReader = new StreamReader(req.Body))
      {
        var requestBody = await streamReader.ReadToEndAsync().ConfigureAwait(false);

        try
        {
          input = JsonConvert.DeserializeObject<Flight>(requestBody);
        }
        catch (JsonException e)
        {
          var message = "Object is not a valid Flight.";
          log.LogError(message);
          return new BadRequestObjectResult(new { message, exception = e.Message });
        }
      }

      var newId = Data.Flights.Max(f => f.Id);
      input.Id = newId + 1;
      Data.Flights.Add(input);

      return new ObjectResult(input) { StatusCode = StatusCodes.Status201Created };
    }

    [FunctionName(nameof(HttpPutSingleFlight))]
    public static async Task<IActionResult> HttpPutSingleFlight(
              [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "flight/{id}")] HttpRequest req,
              int id,
              ILogger log)
    {

      Flight input = null;
      // log.LogInformation("Creating a new collection list item");
      using (var streamReader = new StreamReader(req.Body))
      {
        var requestBody = await streamReader.ReadToEndAsync().ConfigureAwait(false);

        try
        {
          input = JsonConvert.DeserializeObject<Flight>(requestBody);
        }
        catch (JsonException e)
        {
          var message = "Object is not a valid Flight.";
          log.LogError(message);
          return new BadRequestObjectResult(new { message, exception = e.Message });
        }
      }

      input.Id = id;

      Data.Flights[Data.Flights.FindIndex(ind => ind.Id == id)] = input;

      return new ObjectResult(input);
    }

    [FunctionName(nameof(HttpDeleteSingleFlight))]
    public static IActionResult HttpDeleteSingleFlight(
              [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "flight/{id}")] HttpRequest req,
              int id,
              ILogger log)
    {

      var flight = Data.Flights.FirstOrDefault(f => f.Id == id);
      if (flight != null)
      {
        Data.Flights.Remove(flight);
      }

      return new NoContentResult();
    }

  }
}
