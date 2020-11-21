using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OnlyFriends.Models
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }
       
        [Required]
        public string Title { get; set; }
        public string Content { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int LikeCount { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<PostsLikedBy> UsersWhoLike { get; set; }
    }


}