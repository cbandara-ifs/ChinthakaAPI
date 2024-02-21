using ChinthakaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Xml.Linq;

namespace ChinthakaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public VehicleController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("{passengers}")]
        public async Task<ActionResult<IEnumerable<vehicleListing>>> getAvailableFlights(int passengers)
        {
            try
            {
                using var httpClient = _httpClientFactory.CreateClient();
                var httpResponseMessage = await httpClient.GetAsync("https://jayridechallengeapi.azurewebsites.net/api/QuoteRequest");

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    using var contentStream =
                        await httpResponseMessage.Content.ReadAsStreamAsync();

                    var vehicleList = await JsonSerializer.DeserializeAsync
                        <vehicleListing>(contentStream);

                    if (vehicleList == null)
                        return NotFound("No vehicle listings found.");
                    /*
                    var filteredResult = vehicleList.listings.Where(x => x.vehicleType.maxPassengers >= passengers)
                        .OrderBy(a => a.pricePerPassenger*passengers)
                        .ToList();
                    */

                    var filteredResult = vehicleList.listings
                        .Where(x => x.vehicleType.maxPassengers >= passengers)
                        .OrderBy(a => a.pricePerPassenger * passengers)
                        .Select(y => new { name = y.name,
                            TotalPrice = y.pricePerPassenger * passengers, 
                            pricePerPassenger = y.pricePerPassenger, 
                            VehicleType = y.vehicleType.name,
                            maxPassengers = y.vehicleType.maxPassengers
                        })
                        .ToList();
                    
                        return Ok(filteredResult);
                }
                else
                {
                    return BadRequest("Something went wrong");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");

            }
        }
    }
}
