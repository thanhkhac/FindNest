namespace FindNest.Data.Models
{
    public class BaseModel
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public DateTime CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}