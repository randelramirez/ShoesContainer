using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProductCatalogApi.Controllers
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
            var path = Path.Combine(webRooth, "Pics", $"shoes-{id.ToString()}.png");
            var buffer = System.IO.File.ReadAllBytes(path);
            return File(buffer, "image/png");
        }
    }
}