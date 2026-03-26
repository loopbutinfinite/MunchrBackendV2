using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MunchrBackendV2.Context;
using MunchrBackendV2.Models;
using MunchrBackendV2.Models.DTOs;

namespace MunchrBackendV2.Services
{
    public class UserServices
    {
        private readonly DataContext _dataContext;
        private readonly IConfiguration _config;//To access our appsettings.json to access "JWT"
        public UserServices(DataContext dataContext, IConfiguration config)
        {
            _dataContext = dataContext;
            _config = config;
        }

        // public async Task<ActionResult<UserModel>> CreateAccount([FromBody] UserModel userModel)
        // {
        //     await _dataContext.Users.AddAsync(userModel);
        //     await _dataContext.Users.SaveChangesAsync();
        // }

        // public async Task<bool> DoesUserExist(string username)
        // {
        //     return await _dataContext.Users.SingleOrDefaultAsync(user => user.Username == username) != null;
        // }

    }
}