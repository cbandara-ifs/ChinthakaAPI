using ChinthakaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ChinthakaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public LocationController(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        [HttpGet("{ipAddress}")]
        public async Task<IActionResult> getLocationByIP(string ipAddress)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                var httpResponseMessage = await httpClient.GetAsync(
                $"http://api.ipstack.com/{ipAddress}?access_key={_config.GetSection("ipstack").GetValue<string>("access_key")}");

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    using var contentStream =
                        await httpResponseMessage.Content.ReadAsStreamAsync();

                    var location = await JsonSerializer.DeserializeAsync
                        <Location>(contentStream);

                    if (location == null)
                        return NotFound();

                    return Ok(location.city);
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
