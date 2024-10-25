using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FindNest.Data.Models
{
    [PrimaryKey(nameof(RoomId), nameof(RentPostId))]
    public class RentPostRoom
    {
        [Column(Order = 0)]
        [ForeignKey("Room")]
        public int RoomId { get; set; }
        public virtual Room Room { get; set; }

        [Column(Order = 1)]
        [ForeignKey("RentPost")]
        public int RentPostId { get; set; }
        public virtual RentPost RentPost { get; set; }
        
        public int Quantity { get; set; }
    }
}