using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace FindNest.Data.Models
{
    public class User : IdentityUser
    {
        [StringLength(100)]
        public string? FullName { get; set; }
        
        public long Balance { get; set; }
    }
}