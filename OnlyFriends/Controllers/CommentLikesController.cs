using OnlyFriends.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlyFriends.Controllers
{
    public class CommentLikesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CommentLikes
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public ActionResult New(CommentLike commlike)
        {
            try
            {
                db.CommentLikes.Add(commlike);
                db.Comments.Find(commlike.CommentId).LikeCount++;
                db.SaveChanges();
                return Redirect("/Posts/Show/" + commlike.Comment.PostId);
            }
            catch (Exception e)
            {
                return Redirect("/Posts/Show/" + commlike.Comment.PostId);
            }

        }

        [Authorize(Roles = "User,Admin")]
        [HttpDelete]
        public ActionResult Delete(int CommentId, string UserId)
        {
            CommentLike ToDelete = db.CommentLikes.Find(CommentId, UserId);
            db.CommentLikes.Remove(ToDelete);
            db.Comments.Find(CommentId).LikeCount--;
            db.SaveChanges();
            return Redirect("/Posts/Show/" + db.Comments.Find(CommentId).PostId);
        }
    }
}