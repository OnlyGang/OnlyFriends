using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnlyFriends.Models
{
    public class CommentLike
    {
        [Key]
        [Column(Order = 0)]
        public int CommentId { get; set; }
        [Key]
        [Column(Order = 1)]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Comment Comment { get; set; }
    }
}