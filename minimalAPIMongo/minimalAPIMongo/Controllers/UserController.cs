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
    public class UserController : ControllerBase
    {
        private readonly IMongoCollection<User>? _user;

        public UserController(MongoDbService mongoDbService)
        {
            _user = mongoDbService.GetDatabase.GetCollection<User>("user");
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> Get()
        {
            try
            {
                var users = await _user.Find(FilterDefinition<User>.Empty).ToListAsync();
                return Ok(users);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(string id)
        {
            try
            {
                //var user = await _user.Find(Builders<User>.Filter.Eq(p => p.Id, id)).FirstOrDefaultAsync();
                var user = await _user.Find(x => x.Id == id).FirstOrDefaultAsync();

                return user is not null ? Ok(user) : NotFound("Produto não encontrado.");
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<User>> Post(User newProduct)
        {
            try
            {
                await _user!.InsertOneAsync(newProduct);
                return Ok(newProduct);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> Delete(string id)
        {
            try
            {
                var user = await _user.Find(x => x.Id == id).FirstOrDefaultAsync();
                if (user is not null)
                {
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
        public async Task<ActionResult<User>> Update(string id, User updatedProduct)
        {
            try
            {
                var user = await _user.Find(x => x.Id == id).FirstOrDefaultAsync();
                if (user is not null)
                {
                    updatedProduct.Id = id;
                    await _user.ReplaceOneAsync(x => x.Id == id, updatedProduct);
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
