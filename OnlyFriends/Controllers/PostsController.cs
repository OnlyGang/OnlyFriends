using OnlyFriends.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace OnlyFriends.Controllers
{

    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Post
        [Authorize(Roles = "User,Editor,Admin")]
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
        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult Show(int id)
        {
            Post post = db.Posts.Find(id);
            ViewBag.CurrentUser = new Tuple<string, bool>(User.Identity.GetUserId(), post.UserId == User.Identity.GetUserId() || User.IsInRole("Editor") || User.IsInRole("Admin"));
            ViewBag.IsLikedByCurrentUser = (db.PostLikes.Find(id, User.Identity.GetUserId()) != null);
            return View(post);

        }


        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult New()
        {
            Post post = new Post();
            post.UserId = User.Identity.GetUserId();
            var curr = User.Identity.GetUserId();
            ViewBag.Curr = curr;

            return View(post);
        }

        [HttpPost]
        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult New(Post post)
        {
            post.UserId = User.Identity.GetUserId();
            post.Date = DateTime.Now;
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

        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult Edit(int id)
        {
            
            Post post = db.Posts.Find(id);
            if (post.UserId == User.Identity.GetUserId() || User.IsInRole("Editor") || User.IsInRole("Admin"))
            {
                return View(post);
            }
            else
            {
                TempData["message"] = "You don't have enough permissions to modify this post!";
                return RedirectToAction("Index");
            }
        }

        [HttpPut]
        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult Edit(int id, Post requestPost)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Post post = db.Posts.Find(id);
                    if (post.UserId == User.Identity.GetUserId() || User.IsInRole("Editor") || User.IsInRole("Admin"))
                    {
                        if (TryUpdateModel(post))
                        {
                            post.Title = requestPost.Title;
                            post.Content = requestPost.Content;
                            db.SaveChanges();
                            TempData["message"] = "The post has been successfully changed!";
                            return RedirectToAction("Show/" + id);
                        }

                        return View(requestPost);
                    }
                    else
                    {
                        TempData["message"] = "You don't have enough permissions to modify this post!";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return View(requestPost);
                }
            }
            catch (Exception e)
            {
                return View(requestPost);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult Delete(int id)
        {
            Post post = db.Posts.Find(id);
            if(post.UserId == User.Identity.GetUserId() || User.IsInRole("Editor") || User.IsInRole("Admin"))
            {
                db.Posts.Remove(post);
                db.SaveChanges();
                TempData["message"] = "The post was deleted";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "You don't have enough permissions to modify this post!";
                return RedirectToAction("Index");
            }

        }
        
    }
}

    
