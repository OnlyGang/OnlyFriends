using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnlyFriends.Models
{
    public class GroupRequest
    {   
        [Key]
        [Column(Order = 0)]
        [ForeignKey("User")]
        public string SenderId { get; set; }
        [Key]
        [Column(Order = 1)]
        public int GroupId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Group Group { get; set; }

    }
}