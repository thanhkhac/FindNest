using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace FindNest.Data.Models
{
    public class User : IdentityUser
    {
        [StringLength(100)]
        public string? FullName { get; set; }
        
        public long Balance { get; set; }
        
        [StringLength(25)]
        public string? ContactPhoneNumber { get; set; }
        [StringLength(255)]
        public string? Avatar { get; set; }
        
        public bool IsBanned { get; set; }
        
        public ICollection<Like> Likes { get; set; }
        public ICollection<RentPost> RentPost { get; set; }
        
    }
}