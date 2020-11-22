using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnlyFriends.Models
{
    public class PostLike
    {
        [Key][Column(Order = 0)]
        public int PostId { get; set; }
        [Key][Column(Order = 1)]
        public string UserId { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}