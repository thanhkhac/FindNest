using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FindNest.Data.Models
{
    public class BaseModel
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public string? CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
        
        [NotMapped]
        public User? User { get; set; }
    }
}