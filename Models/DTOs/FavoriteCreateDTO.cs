using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MunchrBackendV2.Models.DTOs
{
    public class FavoriteCreateDTO
    {
        public int UserId { get; set; }
        public int BusinessId { get; set; }   
    }
}