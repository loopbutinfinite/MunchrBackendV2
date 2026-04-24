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
    public class FavoriteController : ControllerBase
    {
        private readonly FavoriteServices _favoriteServices;
        public FavoriteController(FavoriteServices favoriteServices)
        {
            _favoriteServices = favoriteServices;
        }

        [HttpGet("GetFavorites")]
        public async Task<ActionResult<IEnumerable<FavoritesModel>>> GetAllFavorites()
        {
            var favorites = await _favoriteServices.GetAll();
            if(favorites == null)
            {
                return NotFound("No Favorites found.");
            }
            return Ok(favorites);
        }

        [HttpGet("GetFavoritesById/{id}")]
        public async Task<ActionResult<IEnumerable<FavoritesModel>>> GetFavoritesById(int id)
        {
            var favorites = await _favoriteServices.GetFavoritesById(id);
            if (favorites == null)
            {
                return NotFound("No Favorites found.");
            }
            return Ok(favorites);
        }

        [HttpGet("GetFavoritesByUserId/{id}")]
        public async Task<ActionResult<IEnumerable<FavoritesModel>>> GetFavoritesByUserId(int id)
        {
            var favorites = await _favoriteServices.GetFavoritesByUserId(id);
            if (favorites == null)
            {
                return NotFound("No Favorites found.");
            }
            return Ok(favorites);
        }

        [HttpGet("GetFavoritesByBusinessId/{id}")]
        public async Task<ActionResult<IEnumerable<FavoritesModel>>> GetFavoritesByBusinessId(int id)
        {
            var favorites = await _favoriteServices.GetFavoritesByBusinessId(id);
            if (favorites == null)
            {
                return NotFound("No Favorites found.");
            }
            return Ok(favorites);
        }

        [HttpPost("AddFavorites")]
        public async Task<ActionResult<FavoritesModel>> AddFavorite([FromBody] FavoritesModel favorite)
        {
            var favoriteToAdd = await _favoriteServices.AddFavorite(favorite);
            if (!favoriteToAdd)
            {
                return BadRequest("Failed to add your favorite.");
            }
            return Ok(favoriteToAdd);
        }

        [HttpDelete("RemoveFavorite")]
        public async Task<ActionResult<bool>> RemoveFavorite(FavoritesModel favorite)
        {
            var success = await _favoriteServices.RemoveFavorite(favorite);
            
            if(success) return Ok(new{success});

            return BadRequest(new{success});
        }
    }
}