using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MunchrBackendV2.Models.DTOs
{
    public class UserDTO
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public bool IsBusinessOwner { get; set; }
        public List<FavoritesModel>? Favorites { get; set; } = [];
    }
}