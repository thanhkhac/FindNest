using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FindNest.Constants;
using Microsoft.EntityFrameworkCore;

namespace FindNest.Data.Models
{
    [Index(nameof(RentCategoryId))]
    [Index(nameof(CreatedAt))]
    [Index(nameof(RegionId))]
    [Index(nameof(IsNegotiatedPrice), nameof(Price))]
    [Index(nameof(Area))]
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
        
        public double? Latitude { get; set; } //Vĩ độ
        
        public double? Longitude { get; set; } //Kinh độ
        
        public bool IsHidden { get; set; }
        
        public bool? IsApproved { get; set; }
        public virtual Region? Region { get; set; }
        public virtual RentCategory? RentCategory { get; set; }

        public virtual ICollection<RentPostRoom> RentPostRooms { get; set; }

        public virtual ICollection<Media> Mediae { get; set; }
        
        public ICollection<Like> Likes { get; set; }
        
        
        [NotMapped]
        public string? RegionAddress { get; set; }
        [NotMapped]
        public int BedRoomCount => RentPostRooms?.FirstOrDefault(x => x.RoomId == RoomConst.Bedroom)?.Quantity ?? 0;

        [NotMapped]
        public int BathRoomCount => RentPostRooms?.FirstOrDefault(x => x.RoomId == RoomConst.Bathroom)?.Quantity ?? 0;


    }
}