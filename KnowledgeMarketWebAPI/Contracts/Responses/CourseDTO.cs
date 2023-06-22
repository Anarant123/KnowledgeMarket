using KnowledgeMarketWebAPI.Data.Models.db;
using System;

namespace KnowledgeMarketWebAPI.Contracts.Responses
{
    public class CourseDTO
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public double? Price { get; set; }

        public string? Author { get; set; }

        public string? Description { get; set; }

        public string? Photo { get; set; }

        public int? UserId { get; set; }

        public bool? IsDeleted { get; set; }

        public virtual User? User { get; set; }
        
        public bool IsPurchated { get; set; }

        public static CourseDTO Map(Course ad, bool isPurchated)
        {
            return new CourseDTO
            {
                Id = ad.Id,
                Price = ad.Price,
                Name = ad.Name,
                Description = ad.Description,
                UserId = ad.UserId,
                User = ad.User,
                IsPurchated = isPurchated
            };
        }
    }
}
