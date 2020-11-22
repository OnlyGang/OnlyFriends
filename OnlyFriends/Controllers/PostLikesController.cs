using OnlyFriends.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppContext = OnlyFriends.Models.ApplicationDbContext;
using Microsoft.AspNet.Identity;

namespace OnlyFriends.Controllers
{

    public class PostLikes : Controller
    {
        private AppContext db = new OnlyFriends.Models.ApplicationDbContext();
        // GET: PostsLikedB
        [Authorize(Roles = "User,Editor,Admin")]
        [HttpPost]
        public ActionResult AddLike(PostLike postlike) 
        {

            try
            {
                db.PostLikes.Add(postlike);
                db.SaveChanges();
                return Redirect("/Posts/Show/" + postlike.PostId);
            }

            catch (Exception e)
            {
                return Redirect("/Posts/Show/" + postlike.PostId);
            }
        }
    }
}