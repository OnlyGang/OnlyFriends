using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlyFriends.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime Date { get; set; }
        [Required]
        public int PostId { get; set; }
        [Required]
        public string UserId { get; set; }
        public int LikesCount { get; set; }

        public virtual Post Post { get; set; }
    }
}