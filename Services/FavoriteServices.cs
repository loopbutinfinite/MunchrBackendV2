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
        
        // public async Task<ActionResult<IEnumerable<FavoritesModel>>> GetFavoritesByUserId(int id)
        // {
        //     return await _dataContext.FavoriteBusinesses.Where(favorite => favorite.UserId == id).ToListAsync();
        // }

        public async Task<List<FavoritesModel>> GetFavoritesByUserId(int id)
        {
            return await _dataContext.FavoriteBusinesses
                .Where(favorite => favorite.UserId == id)
                .Select(favorite => new FavoritesModel
                {
                    Id = favorite.Id,
                    UserId = favorite.UserId,
                    BusinessId = favorite.BusinessId,

                    User = new UserModel
                    {
                        UserId = favorite.User.UserId,
                        UserProfileImage = favorite.User.UserProfileImage,
                        FirstName = favorite.User.FirstName,
                        LastName = favorite.User.LastName,
                        Username = favorite.User.Username,
                        PhoneNumber = favorite.User.PhoneNumber,
                        Email = favorite.User.Email,
                        IsBusinessOwner = favorite.User.IsBusinessOwner,

                        // Important: do NOT include Favorites here.
                        // This prevents circular JSON loops.
                        Favorites = null,
                        Reviews = null
                    },

                    Business = new BusinessModel
                    {
                        BusinessId = favorite.Business.BusinessId,
                        BusinessName = favorite.Business.BusinessName,
                        BusinessHours = favorite.Business.BusinessHours,
                        BusinessPhoneNumber = favorite.Business.BusinessPhoneNumber,
                        BusinessDescription = favorite.Business.BusinessDescription,
                        Category = favorite.Business.Category,
                        StreetName = favorite.Business.StreetName,
                        City = favorite.Business.City,
                        State = favorite.Business.State,
                        ZipCode = favorite.Business.ZipCode,

                        // Important: do NOT include Favorites here.
                        // This prevents circular JSON loops.
                        Favorites = null,
                        BusinessReviews = null
                    }
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

        public async Task<FavoritesModel?> AddFavorite(FavoritesModel favorite)
        {
            bool alreadyFavorited = await _dataContext.FavoriteBusinesses
                .AnyAsync(f => f.UserId == favorite.UserId && f.BusinessId == favorite.BusinessId);

            if (alreadyFavorited)
            {
                return await _dataContext.FavoriteBusinesses
                    .Include(f => f.User)
                    .Include(f => f.Business)
                    .FirstOrDefaultAsync(f =>
                        f.UserId == favorite.UserId &&
                        f.BusinessId == favorite.BusinessId
                    );
            }

            FavoritesModel favoriteToAdd = new FavoritesModel
            {
                UserId = favorite.UserId,
                BusinessId = favorite.BusinessId
            };

            await _dataContext.FavoriteBusinesses.AddAsync(favoriteToAdd);
            await _dataContext.SaveChangesAsync();

            return await _dataContext.FavoriteBusinesses
                .Include(f => f.User)
                .Include(f => f.Business)
                .FirstOrDefaultAsync(f => f.Id == favoriteToAdd.Id);
        }
        public async Task<bool> RemoveFavorite([FromBody] FavoritesModel favorite)
        {
            _dataContext.FavoriteBusinesses.Remove(favorite);
            return await _dataContext.SaveChangesAsync() != 0;
        }
    }
}