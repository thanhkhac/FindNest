using System.ComponentModel.DataAnnotations;
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
        
        public ICollection<Like> Likes { get; set; }
    }
}