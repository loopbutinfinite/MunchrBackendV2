using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MunchrBackendV2.Models.DTOs
{
    public class BusinessDTO
    {
        public int BusinessId { get; set; }
        public string? BusinessName { get; set; }
        public string? BusinessHours { get; set; }
        public string? BusinessPhoneNumber { get; set; }
        public string? BusinessDescription { get; set; }
        public string? Category { get; set; }
        public string? StreetName { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public int ZipCode { get; set; }

        public int UserId { get; set; }
        public string? OwnerFirstName { get; set; }
        public string? OwnerLastName { get; set; }
        public string? OwnerUsername { get; set; }

        public List<ReviewDTO> BusinessReviews { get; set; } = [];
        public List<FavoritesDTO>? Favorites { get; set; } = [];
    }
}