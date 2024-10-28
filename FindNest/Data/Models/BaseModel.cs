using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FindNest.Data.Models
{
    public class BaseModel
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        [ForeignKey(nameof(CreatedUser))]
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        
        public string? DeleteBy { get; set; }
        public bool IsDeleted { get; set; }
        
        public User? CreatedUser { get; set; }
    }
}