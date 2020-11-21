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
    public class PostsController : Controller
    {
        private AppContext db = new OnlyFriends.Models.ApplicationDbContext();
        // GET: Post
        public ActionResult Index()
        {

            var posts = db.Posts.Include("User");
            ViewBag.Posts = posts;
            
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }

            return View();
        }

        public ActionResult Show(int id)
        {
            Post post = db.Posts.Find(id);
            return View(post);

        }


        public ActionResult New()
        {
            Post post = new Post();
            post.UserId = User.Identity.GetUserId();
            var curr = User.Identity.GetUserId();
            ViewBag.Curr = curr;

            return View(post);
        }

        [HttpPost]
        public ActionResult New(Post post)
        {

            post.Date = DateTime.Now;
            post.UserId = User.Identity.GetUserId();
            try
            {
                if (ModelState.IsValid)
                {
                    db.Posts.Add(post);
                    db.SaveChanges();
                    TempData["message"] = "Postarea a fost adaugata!";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(post);
                }
            }
            catch (Exception e)
            {
                return View(post);
            }
        }
    }

    

}