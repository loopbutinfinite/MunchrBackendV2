using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MunchrBackendV2.Models;
using MunchrBackendV2.Services;

namespace MunchrBackendV2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly ReviewServices _reviewServices;
        public ReviewController(ReviewServices reviewServices)
        {
            _reviewServices = reviewServices;
        }

        [HttpGet("GetReviews")]
        public async Task<ActionResult<IEnumerable<ReviewModel>>> GetAllReviews()
        {
            var reviews = await _reviewServices.GetReviewsAsync();
            if(reviews == null)
            {
                return NotFound("No Reviews found.");
            }
            return Ok(reviews);
        }

        [HttpGet("GetReviewsById/{id}")]
        public async Task<ActionResult<IEnumerable<ReviewModel>>> GetReviewsById(int id)
        {
            var reviews = await _reviewServices.GetReviewByIdAsync(id);
            if (reviews == null)
                return NotFound("No reviews found.");
            return Ok(reviews);
        }

        [HttpGet("GetReviewsByScore/{score}")]
        public async Task<ActionResult<IEnumerable<ReviewModel>>> GetReviewsByScore(int score)
        {
            var reviews = await _reviewServices.GetReviewsByScoreAsync(score);
            if (reviews == null)
                return NotFound("No reviews found.");
            return Ok(reviews);
        }

        [HttpGet("GetReviewsByBusiness/{id}")]
        public async Task<ActionResult<IEnumerable<ReviewModel>>> GetReviewsByBusiness(int id)
        {
            var reviews = await _reviewServices.GetReviewsByBusinessAsync(id);
            if (reviews == null)
                return NotFound("No reviews found.");
            return Ok(reviews);
        }

        [HttpPost("AddReview")]
        public async Task<ActionResult<bool>> AddReview([FromBody] ReviewModel review)
        {
            var result = await _reviewServices.AddReviewAsync(review);
            if (!result)
                return BadRequest("Failed to add your review.");
            
            return Ok(result);
        }

        [HttpPut("EditReview")]
        public async Task<ActionResult<bool>> EditReview([FromBody] ReviewModel review)
        {
            var result = await _reviewServices.EditReviewAsync(review);
            if (!result)
                return NotFound("Failed to update review.");
            
            return Ok(result);
        }
    }
}