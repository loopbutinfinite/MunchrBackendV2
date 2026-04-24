using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MunchrBackendV2.Models.DTOs
{
    public class ReviewDTO
    {
        public int Id { get; set; } //The id of the review
        public int BusinessId { get; set; } //The id of the business that the review is on
        public DateTime Date { get; set; }
        public string? ReviewerName { get; set; }
        public string? ReviewTitle { get; set; }
        public string? ReviewDescription { get; set; }
        public int ReviewScore { get; set; }
        public int UserId { get; set; } //The id of the user who made the review
        public UserDTO? UserReview { get; set; }
    }
}