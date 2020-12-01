using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlyFriends.Models
{
    public class Group
    {
        [Key]
        public int GroupId { get; set; }
        public string UserId { get; set; }
        [Required]
        public string Title { get; set; }
        public DateTime Date { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<GroupMember> GroupMembers { get; set; }
    }

}