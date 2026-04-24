using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MunchrBackendV2.Models
{
    public class FavoritesModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BusinessId { get; set; }
        public UserModel? User { get; set; }
        public BusinessModel? Business { get; set; }
    }
}