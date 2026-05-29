using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MunchrBackendV2.Context;
using MunchrBackendV2.Models;
using MunchrBackendV2.Models.DTOs;

namespace MunchrBackendV2.Services
{
    public class UserServices
    {
        private readonly DataContext _dataContext;
        private readonly IConfiguration _config;
        public UserServices(DataContext dataContext, IConfiguration config)
        {
            _dataContext = dataContext;
            _config = config;
        }

        public async Task<bool> CreateAccount(UserDTO newUser)
        {
            if (await DoesUserExist(newUser.Username)) return false;

            UserModel user = new();
            PasswordDTO EncryptedPassword = HashPassword(newUser.Password);
            user.Username = newUser.Username;
            user.Email = newUser.Email;
            user.FirstName = newUser.FirstName;
            user.LastName = newUser.LastName;
            user.PhoneNumber = newUser.PhoneNumber;
            user.IsBusinessOwner = newUser.IsBusinessOwner;
            user.Hash = EncryptedPassword.Hash;
            user.Salt = EncryptedPassword.Salt;

            await _dataContext.User.AddAsync(user);
            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<bool> ChangePassword(ChangePasswordDTO request)
        {
            var user = await _dataContext.User
                .FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null)
                return false;

            bool isPasswordCorrect = VerifyPassword(
                request.CurrentPassword,
                user.Salt,
                user.Hash
            );

            if (!isPasswordCorrect)
                return false;

            PasswordDTO encryptedPassword = HashPassword(request.NewPassword);

            user.Salt = encryptedPassword.Salt;
            user.Hash = encryptedPassword.Hash;

            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<bool> UpdateUserProfile(int userId, UpdateUserProfileDTO updatedUser)
        {
            if (string.IsNullOrWhiteSpace(updatedUser.Username))
                return false;

            if (string.IsNullOrWhiteSpace(updatedUser.Email))
                return false;

            var user = await _dataContext.User
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
                return false;

            var usernameTaken = await _dataContext.User
                .AnyAsync(u => u.Username == updatedUser.Username && u.UserId != userId);

            if (usernameTaken)
                return false;

            var emailTaken = await _dataContext.User
                .AnyAsync(u => u.Email == updatedUser.Email && u.UserId != userId);

            if (emailTaken)
                return false;

            user.Username = updatedUser.Username.Trim();
            user.Email = updatedUser.Email.Trim();

            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<string> Login(UserDTO user)
        {
            UserModel currentUser = await GetUserInfoByUsernameAsync(user.Username);

            if (currentUser == null) return null;

            if (!VerifyPassword(user.Password, currentUser.Salt, currentUser.Hash)) return null;

            return GenerateJWT(new List<Claim>());
        }

        public async Task<bool> DoesUserExist(string username)
        {
            return await _dataContext.User.SingleOrDefaultAsync(user => user.Username == username) != null;
        }

        private static PasswordDTO HashPassword(string password)
        {
            byte[] SaltBytes = RandomNumberGenerator.GetBytes(64);

            string salt = Convert.ToBase64String(SaltBytes);

            string hash;

            using (var derivedBytes = new Rfc2898DeriveBytes(password, SaltBytes, 310000, HashAlgorithmName.SHA256))
            {
                hash = Convert.ToBase64String(derivedBytes.GetBytes(32));
            }

            return new PasswordDTO
            {
                Salt = salt,
                Hash = hash
            };
        }

        public async Task<bool> DeleteAccount(UserModel userToDelete)
        {
            _dataContext.User.Remove(userToDelete);
            return await _dataContext.SaveChangesAsync() != 0;
        }

        private string GenerateJWT(List<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
            var SigningCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: "https://csa-2526-munchr-a8dbh8ckfddrewh7.westus3-01.azurewebsites.net/", // This is coming from hosted web app
                audience: "https://csa-2526-munchr-a8dbh8ckfddrewh7.westus3-01.azurewebsites.net/",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: SigningCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private static bool VerifyPassword(string password, string salt, string hash)
        {
            byte[] saltByte = Convert.FromBase64String(salt);

            string checkHash;

            using (var derivedBytes = new Rfc2898DeriveBytes(password, saltByte, 310000, HashAlgorithmName.SHA256))
            {
                checkHash = Convert.ToBase64String(derivedBytes.GetBytes(32));
                return hash == checkHash;
            }
        }

        public async Task<UserModel> GetUserInfoByUsernameAsync(string username) => await _dataContext.User.SingleOrDefaultAsync(user => user.Username == username);

        public async Task<UserModel> GetUserById(int id)
        {
            return await _dataContext.User.SingleOrDefaultAsync(user => user.UserId == id);
        }

        public async Task<List<UserModel>> GetAllUsers()
        {
            return await _dataContext.User.ToListAsync();
        }

        public async Task<UserInfo> GetUserByUsername(string username)
        {
            var currentUser = await _dataContext.User.SingleOrDefaultAsync(user => user.Username == username);

            UserInfo user = new();
            user.Id = currentUser.UserId;
            user.Username = currentUser.Username;
            return user;
        }
    }
}