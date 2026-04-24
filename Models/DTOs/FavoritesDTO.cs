using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MunchrBackendV2.Models.DTOs
{
    public class FavoritesDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BusinessId { get; set; }
        public UserDTO? User { get; set; }
        public BusinessDTO? Business { get; set; }
    }
}