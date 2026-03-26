using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MunchrBackendV2.Context;
using MunchrBackendV2.Models;

namespace MunchrBackendV2.Services
{
    public class ReviewServices
    {
        private readonly DataContext _dataContext;
        public ReviewServices(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<ReviewModel>> GetReviewsAsync () => await _dataContext.Review.ToListAsync();

        public async Task<bool> AddReviewAsync(ReviewModel review)
        {
            await _dataContext.Review.AddAsync(review);
            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<bool> EditReviewAsync (ReviewModel review)
        {
            var reviewToEdit = await GetReviewByIdAsync(review.Id);

            if(reviewToEdit == null) return false;

            reviewToEdit.ReviewTitle = review.ReviewTitle;
            reviewToEdit.ReviewDescription = review.ReviewDescription;
            reviewToEdit.Date = review.Date;
            reviewToEdit.ReviewerName = review.ReviewerName;
            reviewToEdit.ReviewScore = review.ReviewScore;

            _dataContext.Review.Update(reviewToEdit);
            return await _dataContext.SaveChangesAsync() != 0;
        }

        private async Task<ReviewModel> GetReviewByIdAsync(int id)
        {
            return await _dataContext.Review.FindAsync(id);
        }

        public async Task<List<ReviewModel>> GetReviewByUserIdAsync(int id) => await _dataContext.Review.Where(review => review.UserId == id).ToListAsync();

        public async Task<List<ReviewModel>> GetReviewsByScoreAsync(int reviewScore) => await _dataContext.Review.Where(review => review.ReviewScore == reviewScore).ToListAsync();

        public async Task<List<ReviewModel>> GetReviewsByBusinessAsync(int id) => await _dataContext.Review.Where(review => review.BusinessId == id).ToListAsync();

        public async Task<List<ReviewModel>> GetReviewsByBusinessAsync(DateTime date) => await _dataContext.Review.Where(review => review.Date == date).ToListAsync();
    }
}