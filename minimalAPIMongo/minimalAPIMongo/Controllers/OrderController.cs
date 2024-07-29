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
    public class OrderController : ControllerBase
    {
        private readonly IMongoCollection<Order>? _order;
        private readonly IMongoCollection<Product>? _product;

        public OrderController(MongoDbService mongoDbService)
        {
            _order = mongoDbService.GetDatabase.GetCollection<Order>("order");
            _product = mongoDbService.GetDatabase.GetCollection<Product>("product");
        }

        [HttpGet]
        public async Task<ActionResult<List<Order>>> Get()
        {
            try
            {
                var orders = await _order.Find(FilterDefinition<Order>.Empty).ToListAsync();
                return Ok(orders);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetById(string id)
        {
            try
            {
                //var order = await _order.Find(Builders<Order>.Filter.Eq(p => p.Id, id)).FirstOrDefaultAsync();
                var order = await _order.Find(x => x.Id == id).FirstOrDefaultAsync();

                return order is not null ? Ok(order) : NotFound("Produto não encontrado.");
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Order>> Post(Order newProduct)
        {
            try
            {
                await _order!.InsertOneAsync(newProduct);
                return Ok(newProduct);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Order>> Delete(string id)
        {
            try
            {
                var order = await _order.Find(x => x.Id == id).FirstOrDefaultAsync();
                if (order is not null)
                {
                    await _order.DeleteOneAsync(x => x.Id == id);
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
        public async Task<ActionResult<Order>> Update(string id, Order updatedProduct)
        {
            try
            {
                var order = await _order.Find(x => x.Id == id).FirstOrDefaultAsync();
                if (order is not null)
                {
                    updatedProduct.Id = id;
                    await _order.ReplaceOneAsync(x => x.Id == id, updatedProduct);
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
