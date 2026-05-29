using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MunchrBackendV2.Context;
using MunchrBackendV2.Models;
using MunchrBackendV2.Models.DTOs;

namespace MunchrBackendV2.Services
{
    public class BusinessServices
    {
        private readonly DataContext _dataContext;
        public BusinessServices(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<(bool Success, string Message, BusinessModel Business)> CreateBusiness(BusinessModel newBusiness)
        {
            if (newBusiness == null)
                return (false, "Invalid business data.", null);

            // No null / whitespace business names.
            if (string.IsNullOrWhiteSpace(newBusiness.BusinessName))
                return (false, "Business name cannot be empty.", null);

            // An owner must be supplied so we can tie the business to an account.
            if (newBusiness.OwnerId == null || newBusiness.OwnerId == 0)
                return (false, "A valid owner is required to create a business.", null);

            // One business per owner.
            if (await DoesOwnerHaveBusiness(newBusiness.OwnerId.Value))
                return (false, "You already have a registered business.", null);

            // No duplicate business names (case-insensitive at the DB collation level).
            var trimmedName = newBusiness.BusinessName.Trim();
            if (await DoesBusinessNameExist(trimmedName))
                return (false, "That business name is already taken.", null);

            newBusiness.BusinessName = trimmedName;

            await _dataContext.Business.AddAsync(newBusiness);
            var saved = await _dataContext.SaveChangesAsync() != 0;

            return saved
                ? (true, "Business created.", newBusiness)
                : (false, "Could not save the business. Please try again.", null);
        }

        public async Task<(bool Success, string Message)> EditBusinessAsync(BusinessModel business)
        {
            if (business == null)
                return (false, "Invalid business data.");

            var businessToEdit = await GetBusinessByIdAsync(business.BusinessId);
            if (businessToEdit == null)
                return (false, "Business not found.");

            // No null / whitespace business names.
            if (string.IsNullOrWhiteSpace(business.BusinessName))
                return (false, "Business name cannot be empty.");

            // No duplicate names — but allow this business to keep its own name.
            var trimmedName = business.BusinessName.Trim();
            if (await DoesBusinessNameExist(trimmedName, business.BusinessId))
                return (false, "That business name is already taken.");

            businessToEdit.BusinessDescription = business.BusinessDescription;
            businessToEdit.BusinessHours = business.BusinessHours;
            businessToEdit.BusinessName = trimmedName;
            businessToEdit.BusinessPhoneNumber = business.BusinessPhoneNumber;
            businessToEdit.Category = business.Category;
            businessToEdit.City = business.City;
            businessToEdit.State = business.State;
            businessToEdit.ZipCode = business.ZipCode;
            businessToEdit.StreetName = business.StreetName;
            // OwnerId is intentionally NOT reassigned here — ownership cannot change via edit.

            _dataContext.Business.Update(businessToEdit);
            await _dataContext.SaveChangesAsync();

            // Treat a successful, exception-free save as success even if the values
            // were unchanged (SaveChanges would otherwise return 0 rows affected).
            return (true, "Business updated.");
        }

        public async Task<BusinessModel> GetBusinessByOwnerId(int ownerId)
        {
            return await _dataContext.Business
                .FirstOrDefaultAsync(business => business.OwnerId == ownerId);
        }

        public async Task<BusinessModel> GetBusinessByIdAsync(int id)
        {
            return await _dataContext.Business.FindAsync(id);
        }

        public async Task<List<BusinessModel>> GetAllBusinesses()
        {
            return await _dataContext.Business.ToListAsync();
        }

        public async Task<BusinessModel> GetBusinessInfoByBusinessNameAsync(string businessName) => await _dataContext.Business.SingleOrDefaultAsync(business => business.BusinessName == businessName);

        public async Task<BusinessModel> GetBusinessByBusinessName(string businessName)
        {
            var currentBusiness = await _dataContext.Business.SingleOrDefaultAsync(business => business.BusinessName == businessName);

            BusinessModel business = new();
            business.BusinessId = currentBusiness.BusinessId;
            business.BusinessName = currentBusiness.BusinessName;
            return business;
        }

        public async Task<List<BusinessModel>> GetBusinessByState(string stateName)
        {
            return await _dataContext.Business.Where(business => business.State == stateName).ToListAsync();
        }

        public async Task<List<BusinessModel>> GetBusinessByPostalCode(int postalCode)
        {
            return await _dataContext.Business.Where(business => business.ZipCode == postalCode).ToListAsync();
        }

        public async Task<List<BusinessModel>> GetBusinessByCity(string cityName)
        {
            return await _dataContext.Business.Where(business => business.City == cityName).ToListAsync();
        }

        public async Task<List<BusinessModel>> GetBusinessByCategory(string foodCategory)
        {
            return await _dataContext.Business.Where(business => business.Category == foodCategory).ToListAsync();
        }

        // Returns true if any *other* business already uses this name.
        // Pass excludeBusinessId when editing so a business doesn't clash with itself.
        private async Task<bool> DoesBusinessNameExist(string businessName, int? excludeBusinessId = null)
        {
            if (string.IsNullOrWhiteSpace(businessName)) return false;

            var name = businessName.Trim();
            return await _dataContext.Business.AnyAsync(business =>
                business.BusinessName == name &&
                (excludeBusinessId == null || business.BusinessId != excludeBusinessId.Value));
        }

        private async Task<bool> DoesOwnerHaveBusiness(int ownerId)
        {
            return await _dataContext.Business.AnyAsync(business => business.OwnerId == ownerId);
        }
    }
}