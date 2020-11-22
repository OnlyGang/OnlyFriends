using System;
using OnlyFriends.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace OnlyFriends.Controllers
{
    
    public class PostLikesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: PostLikes
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult New(PostLike postlike)
        {
            try
            {
                db.PostLikes.Add(postlike);
                db.Posts.Find(postlike.PostId).LikeCount++;
                db.SaveChanges();
                return Redirect("/Posts/Show/" + postlike.PostId);
            }
            catch (Exception e)
            {
                return Redirect("/Posts/Show/" + postlike.PostId);
            }

        }

        [Authorize(Roles = "User,Editor,Admin")]
        [HttpDelete]
        public ActionResult Delete(int PostId, string UserId)
        {
            PostLike ToDelete = db.PostLikes.Find(PostId, UserId);
            db.PostLikes.Remove(ToDelete);
            db.Posts.Find(PostId).LikeCount--;
            db.SaveChanges();
            return Redirect("/Posts/Show/" + PostId);
        }
    }
}