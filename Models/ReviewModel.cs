using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MunchrBackendV2.Models
{
    public class ReviewModel
    {
        [Key]
        public int Id { get; set; }
        public int BusinessId { get; set; }
        public DateTime Date { get; set; }
        public string? ReviewerName { get; set; }
        public string? ReviewTitle { get; set; }
        public string? ReviewDescription { get; set; }
        public int ReviewScore { get; set; }
        public int UserId { get; set; }
        public UserModel? UserReview { get; set; } //A one-to-one connection tying the one review to the one user
    }
}