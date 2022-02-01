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

        public CatalogController(CatalogContext context, IOptions<CatalogOptions> options)
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
        public async Task<IActionResult> GetById(int id)
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
        public async Task<IActionResult> Get([FromQuery] string name, [FromQuery] int? catalogBrandId,
            [FromQuery] int? catalogTypeId, [FromQuery] int pageSize = 6, [FromQuery] int pageIndex = 0)
        {
            var totalIems = await this.context.CatalogItems.LongCountAsync();
            var itemsQuery = this.context.CatalogItems.AsQueryable();
            if (string.IsNullOrEmpty(name) is not true)
            {
                itemsQuery = itemsQuery.Where(c => c.Name.StartsWith(name));
            }

            if (catalogBrandId.HasValue)
            {
                itemsQuery = itemsQuery.Where(c => catalogBrandId.Value == c.CatalogBrandId);
            }

            if (catalogTypeId.HasValue)
            {
                itemsQuery = itemsQuery.Where(c => catalogTypeId.Value == c.CatalogTypeId);
            }

            itemsQuery = itemsQuery.OrderBy(c => c.Name).Skip(pageSize * pageIndex).Take(pageSize);
            var items = await itemsQuery.ToListAsync();
            items.ForEach(item => item.PictureUrl = item.PictureUrl.Replace("http://externalcatalogbaseurltobereplaced",
                this.options.ExternalCatalogBaseUrl));
            var model = new PaginatedItemsViewModel<CatalogItem>(pageIndex, totalIems, items, pageSize);
            return Ok(model);
        }

        // items2/type/{int}/brand{int}
        [HttpGet("items2/type/{catalogTypeId?}/brand/{catalogBrandId?}")]
        public async Task<IActionResult> Get2([FromRoute] int? catalogBrandId, [FromRoute] int? catalogTypeId,
            [FromQuery] string name, [FromQuery] int pageSize = 6, [FromQuery] int pageIndex = 0)
        {
            var totalIems = await this.context.CatalogItems.LongCountAsync();
            var itemsQuery = this.context.CatalogItems.AsQueryable();
            if (string.IsNullOrEmpty(name) is not true)
            {
                itemsQuery = itemsQuery.Where(c => c.Name.StartsWith(name));
            }

            if (catalogBrandId.HasValue)
            {
                itemsQuery = itemsQuery.Where(c => catalogBrandId.Value == c.CatalogBrandId);
            }

            if (catalogTypeId.HasValue)
            {
                itemsQuery = itemsQuery.Where(c => catalogTypeId.Value == c.CatalogTypeId);
            }

            itemsQuery = itemsQuery.OrderBy(c => c.Name).Skip(pageSize * pageIndex).Take(pageSize);
            var items = await itemsQuery.ToListAsync();
            items.ForEach(item => item.PictureUrl = item.PictureUrl.Replace("http://externalcatalogbaseurltobereplaced",
                this.options.ExternalCatalogBaseUrl));
            var model = new PaginatedItemsViewModel<CatalogItem>(pageIndex, totalIems, items, pageSize);
            return Ok(model);
        }

        [HttpPost("items")]
        public async Task<IActionResult> CreateProduct([FromBody] CatalogItem product)
        {
            var item = new CatalogItem()
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CatalogBrandId = product.CatalogBrandId,
                CatalogTypeId = product.CatalogTypeId,
                PictureFileName = product.PictureFileName,
            };
            
            await this.context.CatalogItems.AddAsync(item);
            await this.context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = item.Id });
        }

        [HttpPut("items")]
        public async Task<IActionResult> UpdateProduct([FromBody] CatalogItem productToUpdate)
        {
            var catalogItem = await this.context.CatalogItems.FindAsync(productToUpdate.Id);
            if (catalogItem is null)
            {
                return NotFound(new { message = $"The product with id: {productToUpdate.Id.ToString()} was not found."} );
            }

            var item = new CatalogItem()
            {
                Name = productToUpdate.Name,
                Description = productToUpdate.Description,
                Price = productToUpdate.Price,
                CatalogBrandId = productToUpdate.CatalogBrandId,
                CatalogTypeId = productToUpdate.CatalogTypeId,
                PictureFileName = productToUpdate.PictureFileName,
            };
            
            // try this later
            // catalogItem = productToUpdate;
            // this.context.Update(catalogItem);
            
            this.context.Update(productToUpdate);
            await this.context.SaveChangesAsync();
            return NoContent();
        }
        
        [HttpDelete("items/{id:int}")]
        public async Task<IActionResult> UpdateProduct(int id)
        {
            var catalogItem = await this.context.CatalogItems.FindAsync(id);
            if (catalogItem is null)
            {
                return NotFound(new { message = $"The product with id: {id.ToString()} was not found."} );
            }
            
            this.context.CatalogItems.Remove(catalogItem);
            await this.context.SaveChangesAsync();
            return NoContent();
        }
    }
}