using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FindNest.Constants;
using Microsoft.EntityFrameworkCore;

namespace FindNest.Data.Models
{
    [Index(nameof(RentCategoryId))]
    [Index(nameof(CreatedAt))]
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
        
        public string? Description { get; set; }
        
        public bool IsHidden { get; set; }
        public virtual Region? Region { get; set; }
        public virtual RentCategory? RentCategory { get; set; }

        public virtual ICollection<RentPostRoom> RentPostRooms { get; set; }

        public virtual ICollection<Media> Mediae { get; set; }
        
        
        [NotMapped]
        public string? RegionAddress { get; set; }
        [NotMapped]
        public int BedRoomCount => RentPostRooms?.FirstOrDefault(x => x.RoomId == RoomConst.Bedroom)?.Quantity ?? 0;

        [NotMapped]
        public int BathRoomCount => RentPostRooms?.FirstOrDefault(x => x.RoomId == RoomConst.Bathroom)?.Quantity ?? 0;


    }
}