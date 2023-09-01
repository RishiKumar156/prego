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
            var UserExists = _GoogleRegisterCcollection.Find(c => c.GmailId == googleRegister.GmailId).FirstOrDefault();
            if(UserExists != null)
            {
                return Ok(UserExists);
            }
            _GoogleRegisterCcollection.InsertOne(googleRegister);
            return Ok(googleRegister);
        }
    }
}
