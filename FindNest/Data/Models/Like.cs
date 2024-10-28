using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FindNest.Data.Models
{
    [PrimaryKey(nameof(UserId), nameof(RentPostId))]
    public class Like
    {
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey(nameof(RentPost))]
        public int RentPostId { get; set; }
        public virtual RentPost RentPost { get; set; }

        public DateTime LikedDate { get; set; } = DateTime.Now;
    }
}