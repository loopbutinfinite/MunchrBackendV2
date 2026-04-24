using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MunchrBackendV2.Context;
using MunchrBackendV2.Models;

namespace MunchrBackendV2.Services
{
    public class FavoriteServices
    {
        private readonly DataContext _dataContext;
        public FavoriteServices(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<ActionResult<IEnumerable<FavoritesModel>>> GetAll()
        {
            return await _dataContext.FavoriteBusinesses.ToListAsync();
        }
        public async Task<ActionResult<IEnumerable<FavoritesModel>>> GetFavoritesByUserId(int id)
        {
            return await _dataContext.FavoriteBusinesses.Where(favorite => favorite.UserId == id).ToListAsync();
        }

        public async Task<ActionResult<IEnumerable<FavoritesModel>>> GetFavoritesById(int id)
        {
            return await _dataContext.FavoriteBusinesses.Where(favorite => favorite.Id == id).ToListAsync();
        }

        public async Task<ActionResult<IEnumerable<FavoritesModel>>> GetFavoritesByBusinessId(int id)
        {
            return await _dataContext.FavoriteBusinesses.Where(favorite => favorite.BusinessId == id).ToListAsync();
        }

        public async Task<bool> AddFavorite(FavoritesModel favorite)
        {
            await _dataContext.FavoriteBusinesses.AddAsync(favorite);
            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<bool> RemoveFavorite([FromBody] FavoritesModel favorite)
        {
            _dataContext.FavoriteBusinesses.Remove(favorite);
            return await _dataContext.SaveChangesAsync() != 0;
        }
    }
}