using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MunchrBackendV2.Models
{
    public class UserModel
    {
        [Key]
        public int UserId { get; set; }
        public string? UserProfileImage { get; set; }
        public string? Username { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Salt { get; set; }
        public string? Hash { get; set; }
        public bool IsBusinessOwner { get; set; }
    }
}