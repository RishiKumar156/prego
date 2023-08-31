using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailRegisterController : ControllerBase
    {
        private readonly IMongoCollection<Email> _mongoCollection;

        private static Email EmailRegister = new Email();
        
        public EmailRegisterController(IMongoClient mongoClient)
        {
            var Database = mongoClient.GetDatabase("pregopantry");
            _mongoCollection = Database.GetCollection<Email>("emailregister");
        }

        [HttpPost("EmailRegister")]
        public IActionResult RegisterUser(EmailDTO email)
        {
            var Hashpassword = BCrypt.Net.BCrypt.HashPassword(email.Password);
            EmailRegister.UserName = email.UserName;
            EmailRegister.Password = Hashpassword;
            if (email == null)
            {
                return BadRequest("Data Not founnd"); 
            }
           _mongoCollection.InsertOne(EmailRegister);
            return Ok(email);
        }

        [HttpPost("EmailLogin")]
        public IActionResult LoginUser(EmailDTO email)
        {
            var user = _mongoCollection.Find(c => c.UserName == email.UserName).FirstOrDefault();
            if(!BCrypt.Net.BCrypt.Verify(email.Password , user.Password))
            {
                return BadRequest();
            }
            return Ok(email);
        }
    }
}
