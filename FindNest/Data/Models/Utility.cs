using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FindNest.Data.Models
{
    [Table("Utilities")]
    public class Utility : BaseModel
    {
        [Key]       
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string? Name { get; set; }
        public virtual ICollection<RentPost> RentPosts { get; set; }

    }
}