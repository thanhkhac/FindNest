﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FindNest.Data.Models
{
    public class Room : BaseModel
    {
        [Key]       
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string? Name { get; set; }
        
        public virtual ICollection<RentPostRoom> RentPostRooms { get; set; }
    }
}