using Microsoft.AspNetCore.Mvc;
using minimalAPIMongo.Domains;
using minimalAPIMongo.Services;
using minimalAPIMongo.ViewModels;
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
        private readonly IMongoCollection<Client>? _client;

        public OrderController(MongoDbService mongoDbService)
        {
            _order = mongoDbService.GetDatabase.GetCollection<Order>("order");
            _product = mongoDbService.GetDatabase.GetCollection<Product>("product");
            _client = mongoDbService.GetDatabase.GetCollection<Client>("client");
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

                order.Client = _client.Find(x => x.Id == order.ClientId).FirstOrDefaultAsync().Result;

                //if(order.ProductsIds != null)
                //{
                //    var filter = Builders<Product>.Filter.In(p => p.Id, order.ProductsIds);
                //    order.Products = await _product.Find(filter).ToListAsync();
                //}

                order.Products = new List<Product>();
                order.ProductsIds!.ForEach(pId =>
                {
                    var p = _product.Find(x => x.Id == pId).FirstOrDefaultAsync().Result;
                    order.Products!.Add(p);
                });

                return order is not null ? Ok(order) : NotFound("Produto não encontrado.");
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Order>> Post(OrderViewModel newOrderModel)
        {
            try
            {
                Order newOrder = new Order
                {
                    ClientId = newOrderModel.ClientId,
                    ProductsIds = newOrderModel.ProductsIds,
                    Status = newOrderModel.Status,
                    Date = newOrderModel.Date,
                };
                await _order!.InsertOneAsync(newOrder);
                return Ok(newOrder);
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
        public async Task<ActionResult<Order>> Update(string id, Order updatedOrder)
        {
            try
            {
                var order = await _order.Find(x => x.Id == id).FirstOrDefaultAsync();
                if (order is not null)
                {
                    updatedOrder.Id = id;
                    await _order.ReplaceOneAsync(x => x.Id == id, updatedOrder);
                    return Ok(updatedOrder);
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
