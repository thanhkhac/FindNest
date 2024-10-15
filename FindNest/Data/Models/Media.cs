using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FindNest.Data.Models
{
    public class Media : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("RentPost")]
        public int? RentPostId { get; set; }
        public required string MediaType { get; set; }
        public required string Path { get; set; }
        public virtual RentPost RentPost { get; set; } 
    }
}