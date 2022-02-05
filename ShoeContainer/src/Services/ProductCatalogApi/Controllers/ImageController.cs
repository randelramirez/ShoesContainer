using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace ShoesOnContainers.Services.ProductCatalogApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        [HttpGet("{id:int}")]
        public IActionResult GetImage(int id,[FromServices] IWebHostEnvironment environment)
        {
            var webRooth = environment.WebRootPath;
            var path = Path.Combine(webRooth, "images","shoes", $"shoes-{id.ToString()}.png");
            var buffer = System.IO.File.ReadAllBytes(path);
            return File(buffer, "image/png");
        }
    }
}