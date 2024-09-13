using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebGuardBackend.Models;

namespace WebGuardBackend.Controllers
{
    [Route("api/controller")]
    [ApiController]
    [Authorize]
    public class UrlController : ControllerBase
    {
        private static List<Url> Urls = new List<Url>();

        [HttpPost("submit")]
        public IActionResult SubmitUrl([FromBody] Url url) 
        {
            url.Id = Urls.Count + 1;
            Urls.Add(url);
            return Ok(new { id = url.Id, status = "submitted" });
        }

        [HttpGet("results/id")]
        public IActionResult GetResults(int id)
        {
            var url = Urls.FirstOrDefault(x => x.Id == id);

            if (url == null) return NotFound(new {message = "URL not found"});

            url.Status = "completed";
            return Ok(url);
        }
    }
}
