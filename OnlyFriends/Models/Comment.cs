using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnlyFriends.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        [Required]
        public int PostId { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public int LikeCount { get; set; }

        public virtual Post Post { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<CommentLike> CommentLikes { get; set; }
    }
}