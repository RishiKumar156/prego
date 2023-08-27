﻿using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMongoCollection<UserRegister> _newuserCollections;
        private readonly IConfiguration _configuration;

        public static UserRegister userRegister = new UserRegister();

        public AuthController(IMongoClient client , IConfiguration configuration )
        {
            var database = client.GetDatabase("pregopantry");
            _newuserCollections = database.GetCollection<UserRegister>("newusers");
            _configuration = configuration;
        }

        [HttpPost("Register")]
        public ActionResult<UserRegister> CreateUser(UserRegisterDTO request)
        {
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
            return Ok(user);
        }
    }
}
