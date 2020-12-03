using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnlyFriends.Models
{
    public class UserRelation
    {
        [Key]
        [Column(Order = 0)]
        public string User1Id { get; set; }
        public virtual ApplicationUser User1 { get; set; }

        [Key]
        [Column(Order = 1)]
        public string User2Id { get; set; }
        public virtual ApplicationUser User2 { get; set; }

        [Required]
        public DateTime Date { get; set; }
        [Required][DefaultValue("Friend")]
        public string Status { get; set; }
    }
}