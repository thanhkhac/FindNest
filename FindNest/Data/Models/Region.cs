using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FindNest.Data.Models
{
    public class Region
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public int? ParentId { get; set; }
        [MaxLength(255)]
        public string? FullName { get; set; }

        [MaxLength(255)]
        public string? Name { get; set; }

        [MaxLength(255)]
        public string? EnglishName { get; set; }
        
        [MaxLength(255)]
        public string? EnglishFullName { get; set; }
        
        [MaxLength(255)]
        public string? CodeName { get; set; }
        public int Level { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(ParentId)}: {ParentId}, {nameof(FullName)}: {FullName}, {nameof(Level)}: {Level}";
        }
    }
}