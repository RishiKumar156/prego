using API.Models.Foods;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderNowController : ControllerBase
    {
        private readonly IMongoCollection<Foods> _foodCollection;
        public OrderNowController(IMongoClient mongoClient)
        {
            var Database = mongoClient.GetDatabase("pregopantry");
            _foodCollection = Database.GetCollection<Foods>("Foods");
        }

        [HttpGet("Foods")]
        public IActionResult GetAllFoods()
        {
            var Foods = _foodCollection.Find(new BsonDocument()).ToList();
            return Ok(Foods);
        }
    }
}
