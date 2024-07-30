using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using minimalAPIMongo.Domains;
using minimalAPIMongo.Services;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;

namespace minimalAPIMongo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ClientController : ControllerBase
    {
        private readonly IMongoCollection<Client>? _client;
        private readonly IMongoCollection<User>? _user;

        public ClientController(MongoDbService mongoDbService)
        {
            _client = mongoDbService.GetDatabase.GetCollection<Client>("client");
            _user = mongoDbService.GetDatabase.GetCollection<User>("user");
        }

        [HttpGet]
        public async Task<ActionResult<List<Client>>> Get()
        {
            try
            {
                var clients = await _client.Find(FilterDefinition<Client>.Empty).ToListAsync();
                return Ok(clients);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetById(string id)
        {
            try
            {
                //var client = await _client.Find(Builders<Client>.Filter.Eq(p => p.Id, id)).FirstOrDefaultAsync();
                var client = await _client.Find(x => x.Id == id).FirstOrDefaultAsync();

                return client is not null ? Ok(client) : NotFound("Produto não encontrado.");
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Client>> Post(Client newClient)
        {
            try
            {
                await _user!.InsertOneAsync(newClient.User!);
                newClient.User = new User
                {
                    Id = newClient.User!.Id,
                    Name = newClient.User!.Name,
                    Email = newClient.User.Email,
                };
                newClient.Id = newClient.User.Id;
                await _client!.InsertOneAsync(newClient);
                return Ok(newClient);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Client>> Delete(string id)
        {
            try
            {
                var client = await _client.Find(x => x.Id == id).FirstOrDefaultAsync();
                var user = await _user.Find(x => x.Id == id).FirstOrDefaultAsync();
                if (client is not null)
                {
                    await _client.DeleteOneAsync(x => x.Id == id);
                    await _user.DeleteOneAsync(x => x.Id == id);
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
        public async Task<ActionResult<Client>> Update(string id, Client updatedProduct)
        {
            try
            {
                var client = await _client.Find(x => x.Id == id).FirstOrDefaultAsync();
                var user = await _user.Find(x => x.Id == id).FirstOrDefaultAsync();
                if (client is not null)
                {

                    updatedProduct.Id = id;
                    updatedProduct.User!.Id = id;
                    await _user.ReplaceOneAsync(x => x.Id == id, updatedProduct.User!);
                    await _client.ReplaceOneAsync(x => x.Id == id, updatedProduct);
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
