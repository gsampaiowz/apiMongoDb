using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using minimalAPIMongo.Domains;
using minimalAPIMongo.Services;
using MongoDB.Driver;

namespace minimalAPIMongo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProductController : ControllerBase
    {
        private readonly IMongoCollection<Product>? _product;

        public ProductController(MongoDbService mongoDbService)
        {
            _product = mongoDbService.GetDatabase.GetCollection<Product>("product");
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> Get()
        {
            try
            {
                var products = await _product.Find(FilterDefinition<Product>.Empty).ToListAsync();
                return Ok(products);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetById(string id)
        {
            try
            {
                //var product = await _product.Find(Builders<Product>.Filter.Eq(p => p.Id, id)).FirstOrDefaultAsync();
                var product = await _product.Find(x => x.Id == id).FirstOrDefaultAsync();

                return product is not null ? Ok(product) : NotFound("Produto não encontrado.");
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Post(Product newProduct)
        {
            try
            {
                await _product!.InsertOneAsync(newProduct);
                return Ok(newProduct);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> Delete(string id)
        {
            try
            {
                var product = await _product.Find(x => x.Id == id).FirstOrDefaultAsync();
                if(product is not null)
                {
                    await _product.DeleteOneAsync(x => x.Id == id);
                    return StatusCode(204, "Sucesso ao deletar.");
                }
                return NotFound("Produto não encontrado.");
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> Update(string id, Product updatedProduct)
        {
            try
            {
                var product = await _product.Find(x => x.Id == id).FirstOrDefaultAsync();
                if(product is not null)
                {
                    updatedProduct.Id = id;
                    await _product.ReplaceOneAsync(x => x.Id == id, updatedProduct);
                    return Ok(updatedProduct);
                }
                return NotFound("Produto não encontrado");

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }
    }
}
