using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MunchrBackendV2.Context;
using MunchrBackendV2.Models;
using MunchrBackendV2.Models.DTOs;

namespace MunchrBackendV2.Services
{
    public class MenuItemServices
    {
        private readonly DataContext _dataContext;
        public MenuItemServices(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<(bool Success, string Message, MenuItemModel Item)> CreateMenuItem(CreateMenuItemDTO dto)
        {
            if (dto == null)
                return (false, "Invalid menu item data.", null);

            if (dto.BusinessId == 0)
                return (false, "A valid business is required.", null);

            if (string.IsNullOrWhiteSpace(dto.Name))
                return (false, "Item name cannot be empty.", null);

            if (dto.Price < 0)
                return (false, "Price cannot be negative.", null);

            var item = new MenuItemModel
            {
                BusinessId = dto.BusinessId,
                Name = dto.Name.Trim(),
                Description = dto.Description?.Trim() ?? string.Empty,
                Price = dto.Price,
                Category = string.IsNullOrWhiteSpace(dto.Category) ? "Other" : dto.Category.Trim(),
                ImageUrl = string.IsNullOrWhiteSpace(dto.ImageUrl) ? null : dto.ImageUrl.Trim(),
            };

            await _dataContext.MenuItems.AddAsync(item);
            var saved = await _dataContext.SaveChangesAsync() != 0;

            return saved
                ? (true, "Menu item created.", item)
                : (false, "Could not save the menu item.", null);
        }

        public async Task<List<MenuItemModel>> GetMenuItemsByBusinessId(int businessId)
        {
            return await _dataContext.MenuItems
                .Where(item => item.BusinessId == businessId)
                .ToListAsync();
        }

        public async Task<(bool Success, string Message)> UpdateMenuItem(MenuItemDTO dto)
        {
            var existing = await _dataContext.MenuItems.FindAsync(dto.Id);
            if (existing == null)
                return (false, "Menu item not found.");

            if (string.IsNullOrWhiteSpace(dto.Name))
                return (false, "Item name cannot be empty.");

            if (dto.Price < 0)
                return (false, "Price cannot be negative.");

            existing.Name = dto.Name.Trim();
            existing.Description = dto.Description?.Trim() ?? string.Empty;
            existing.Price = dto.Price;
            existing.Category = string.IsNullOrWhiteSpace(dto.Category) ? "Other" : dto.Category.Trim();
            existing.ImageUrl = string.IsNullOrWhiteSpace(dto.ImageUrl) ? null : dto.ImageUrl.Trim();

            _dataContext.MenuItems.Update(existing);
            await _dataContext.SaveChangesAsync();
            return (true, "Menu item updated.");
        }

        public async Task<bool> DeleteMenuItem(int id)
        {
            var item = await _dataContext.MenuItems.FindAsync(id);
            if (item == null) return false;

            _dataContext.MenuItems.Remove(item);
            return await _dataContext.SaveChangesAsync() != 0;
        }
    }
}
