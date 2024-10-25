using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FindNest.Data.Models
{
    public class RentPost : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Title { get; set; }
        [ForeignKey("Region")]
        public int? RegionId { get; set; }
        [ForeignKey("RentCategory")]
        public int? RentCategoryId { get; set; }
        public long Price { get; set; }
        public int Area { get; set; }
        public string? Address { get; set; }
        public bool IsNegotiatedPrice { get; set; }
        public string? Thumbnail { get; set; }
        public bool IsHidden { get; set; }
        public virtual Region? Region { get; set; }
        public virtual RentCategory? RentCategory { get; set; }

        public virtual ICollection<Utility> Utilities { get; set; }

        public virtual ICollection<RentPostRoom> RentPostRooms { get; set; }


        [NotMapped]
        public string RegionAddress { get; set; }
        [NotMapped]
        public int BedRoomCount => RentPostRooms != null ? RentPostRooms.Count(x => x.RoomId == 1) : 0;
        public int BathRoomCount => RentPostRooms != null ? RentPostRooms.Count(x => x.RoomId == 3) : 0;
        
    }
}