﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnlyFriends.Models
{
    public class FriendRequest
    {
        [Key]
        [Column(Order = 0)]
        public string SenderId { get; set; }
        public virtual ApplicationUser Sender { get; set; }

        [Key]
        [Column(Order = 1)]
        public string RecieverId { get; set; }
        public virtual ApplicationUser Reciever { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}