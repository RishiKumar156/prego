using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMongoCollection<UserRegister> _newuserCollections;
        private readonly IConfiguration _configuration;

       // public static UserRegister userRegister = new UserRegister();

        public AuthController(IMongoClient client , IConfiguration configuration )
        {
            var database = client.GetDatabase("pregopantry");
            _newuserCollections = database.GetCollection<UserRegister>("newusers");
            _configuration = configuration;
        }

        [HttpPost("Register")]
        public ActionResult<UserRegister> CreateUser(UserRegisterDTO request)
        {
            var userRegister = new UserRegister();
            var haspassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            userRegister.UserEmail = request.UserEmail;
            userRegister.Password = haspassword;
            _newuserCollections.InsertOne(userRegister);
            return Ok(userRegister);
        }


        [HttpPost("login")]
        public ActionResult<UserRegister> LoginUser(UserRegisterDTO request)
        {
            var user = _newuserCollections.Find( c => c.UserEmail == request.UserEmail ).FirstOrDefault();
            if(user == null)
            {
                return BadRequest("User name not found");
            }
            if(!BCrypt.Net.BCrypt.Verify(request.Password , user.Password))
            {
                return Unauthorized("Credential are mismatch");
            }

            var token = CreateToken(user);
            var filter = Builders<UserRegister>.Filter.Eq("_id", user.Id);
            var update = Builders<UserRegister>.Update.Set("authToken", user.AuthToken);
            _newuserCollections.UpdateOne(filter, update);
            return Ok(user);
        }

        private string CreateToken(UserRegister userRegister)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name , userRegister.UserEmail) 
            };

            var key = new Byte[64];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(key);
            }
            var symmetrickey = new SymmetricSecurityKey(key);
            var cred = new SigningCredentials(symmetrickey, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: cred);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
