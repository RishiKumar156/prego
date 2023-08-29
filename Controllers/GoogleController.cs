using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoogleController : ControllerBase
    {
        private readonly IMongoCollection<GoogleRegister> _GoogleRegisterCcollection;
        public GoogleController(IMongoClient mongoClient )
        {
            var database = mongoClient.GetDatabase("pregopantry");
            _GoogleRegisterCcollection = database.GetCollection<GoogleRegister>("googlelogin");
        }

        [HttpPost("newuser")]
        public IActionResult RegisterUser(GoogleRegister googleRegister)
        {
            _GoogleRegisterCcollection.InsertOne(googleRegister);
            if(googleRegister == null)
            {
                return BadRequest("no data found");
            }
            return Ok(googleRegister);
        }
    }
}
