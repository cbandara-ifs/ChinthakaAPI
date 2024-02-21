using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChinthakaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        [HttpGet]
        public IActionResult getPerson()
        {
            var person = new
            {
                name = "test",
                phone = "test"
            };

            return Ok(person);
        }
    }
}
