using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MunchrBackendV2.Models;
using MunchrBackendV2.Models.DTOs;
using MunchrBackendV2.Services;

namespace MunchrBackendV2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserServices _userServices;
        public UserController(UserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpGet("GetUserByUsername/{username}")]
        public async Task<ActionResult<UserModel>> GetUserByUsername(string username)
        {
            var user = await _userServices.GetUserInfoByUsernameAsync(username);

            if(user != null) return Ok(user);

            return BadRequest(new {Message = "No user found"});
        }

        [HttpGet("GetUserById/{id}")]
        public async Task<ActionResult<UserModel>> GetUserById(int id)
        {
            var user = await _userServices.GetUserById(id);
            return user;
        }

        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetAllUsers()
        {
            var users = await _userServices.GetAllUsers();
            return users;
        }

        [HttpPost("CreateAccount")]
        public async Task<ActionResult<UserModel>> CreateAccount([FromBody] UserDTO user)
        {
            var success = await _userServices.CreateAccount(user);

            if(success) return Ok(new {Success = true, Message = "User Created."});

            return BadRequest(new {Success = false, Message = "User Creation failed. Username is already in use."});
        }
        
        [HttpPost("Login")]
        public async Task<ActionResult<UserModel>> Login(UserDTO user)
        {
            var success = await _userServices.Login(user);

            if(success != null) return Ok(new {Token = success});

            return Unauthorized(new {Message = "Login was unsuccessful"});
        }

        [HttpDelete("DeleteUser")]
        public async Task<ActionResult> DeleteAccount(UserModel user)
        {
            var success = await _userServices.DeleteAccount(user);

            if(success) return Ok(new{success});

            return BadRequest(new{success});
        }
    }
}