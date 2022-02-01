using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProductCatalogApi.Data;
using ProductCatalogApi.Domain;
using ProductCatalogApi.ViewModels;

namespace ProductCatalogApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly CatalogContext context;
        private readonly CatalogOptions options;

        public CatalogController(CatalogContext context,
            IOptions<CatalogOptions> options)
        {
            this.context = context;
            this.options = options.Value;

            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> CatalogTypes()
        {
            var items = await this.context.CatalogTypes.ToListAsync();
            return Ok(items);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> CatalogBrands()
        {
            var items = await this.context.CatalogBrands.ToListAsync();
            return Ok(items);
        }

        [HttpGet("items/{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }

            var item = await this.context.CatalogItems.FindAsync(id);
            if (item is not null)
            {
                item.PictureUrl = item.PictureUrl.Replace("http://externalcatalogbaseurltobereplaced",
                    this.options.ExternalCatalogBaseUrl);
                return Ok(item);
            }

            return NotFound();
        }

        [HttpGet("items")]
        public async Task<IActionResult> Get([FromQuery] string name,[FromQuery] int pageSize = 6,
            [FromQuery] int pageIndex = 0)
        {
            var totalIems = await this.context.CatalogItems.LongCountAsync();
            var itemsQuery =  this.context.CatalogItems.AsQueryable();
            if (string.IsNullOrEmpty(name) is not true)
            {
                itemsQuery = itemsQuery.Where(c => c.Name.StartsWith(name));
            }

            itemsQuery = itemsQuery.OrderBy(c => c.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize);
            
            var items = await itemsQuery.ToListAsync();
            items.ForEach(item => item.PictureUrl = item.PictureUrl.Replace(
                "http://externalcatalogbaseurltobereplaced",
                this.options.ExternalCatalogBaseUrl));

            var model = new PaginatedItemsViewModel<CatalogItem>(pageIndex,
                totalIems,
                items,
                pageSize);
            
            return Ok(model);
        }
    }
}