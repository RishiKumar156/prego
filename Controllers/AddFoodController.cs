using API.Models.Foods;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Reflection.Metadata;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddFoodController : ControllerBase
    {
        
        private readonly IMongoCollection<Foods> _foodCollection;
        public AddFoodController(IMongoClient mongoClient)
        {
            var Database = mongoClient.GetDatabase("pregopantry");
            _foodCollection = Database.GetCollection<Foods>("Foods");
        }

        [HttpPost("CreateFood")]
        public IActionResult CreateFood(Foods foods)
        {
            var Find = _foodCollection.Find( c => c.FoodName== foods.FoodName ).FirstOrDefault();
            if (Find != null)
            {
                return BadRequest("Food Exists");
            }
            _foodCollection.InsertOne(foods);
            return Ok(foods);
        }

        [HttpGet("GetFoods")]
        public IActionResult GetAllfoods()
        {
            var Foods = _foodCollection.Find(new BsonDocument()).ToList();
            return Ok(Foods);
        }
    }
}
