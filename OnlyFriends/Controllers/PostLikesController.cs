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