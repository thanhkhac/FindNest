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
        public string Name { get; set; }
        public int Level { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(ParentId)}: {ParentId}, {nameof(Name)}: {Name}, {nameof(Level)}: {Level}";
        }
    }
}