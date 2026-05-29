using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MunchrBackendV2.Context;
using MunchrBackendV2.Models;
using MunchrBackendV2.Models.DTOs;

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

        // public async Task<ActionResult<IEnumerable<FavoritesModel>>> GetFavoritesByUserId(int id)
        // {
        //     return await _dataContext.FavoriteBusinesses.Where(favorite => favorite.UserId == id).ToListAsync();
        // }

        public async Task<List<FavoritesDTO>> GetFavoritesByUserId(int userId)
        {
            return await _dataContext.FavoriteBusinesses
                .Where(favorite => favorite.UserId == userId)
                .Select(favorite => new FavoritesDTO
                {
                    Id = favorite.Id,
                    UserId = favorite.UserId,
                    BusinessId = favorite.BusinessId
                })
                .ToListAsync();
        }

        public async Task<ActionResult<IEnumerable<FavoritesModel>>> GetFavoritesById(int id)
        {
            return await _dataContext.FavoriteBusinesses.Where(favorite => favorite.Id == id).ToListAsync();
        }

        public async Task<ActionResult<IEnumerable<FavoritesModel>>> GetFavoritesByBusinessId(int id)
        {
            return await _dataContext.FavoriteBusinesses.Where(favorite => favorite.BusinessId == id).ToListAsync();
        }

        // public async Task<bool> AddFavorite(FavoritesModel favorite)
        // {
        //     await _dataContext.FavoriteBusinesses.AddAsync(favorite);
        //     return await _dataContext.SaveChangesAsync() != 0;
        // }

        public async Task<bool> AddFavorite(FavoriteCreateDTO favorite)
        {
            var alreadyExists = await _dataContext.FavoriteBusinesses
                .AnyAsync(f => f.UserId == favorite.UserId && f.BusinessId == favorite.BusinessId);

            if (alreadyExists)
            {
                return true;
            }

            var newFavorite = new FavoritesModel
            {
                UserId = favorite.UserId,
                BusinessId = favorite.BusinessId
            };

            await _dataContext.FavoriteBusinesses.AddAsync(newFavorite);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        // public async Task<bool> RemoveFavorite([FromBody] FavoritesModel favorite)
        // {
        //     _dataContext.FavoriteBusinesses.Remove(favorite);
        //     return await _dataContext.SaveChangesAsync() != 0;
        // }

        public async Task<bool> DeleteFavorite(int userId, int businessId)
        {
            var favorite = await _dataContext.FavoriteBusinesses
                .FirstOrDefaultAsync(f => f.UserId == userId && f.BusinessId == businessId);

            if (favorite == null)
            {
                return false;
            }

            _dataContext.FavoriteBusinesses.Remove(favorite);
            return await _dataContext.SaveChangesAsync() > 0;
        }

    }
}