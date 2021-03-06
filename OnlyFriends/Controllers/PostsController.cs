﻿using OnlyFriends.Models;
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
        [Authorize(Roles = "User,Admin")]
        public ActionResult Index()
        {
            string CurrUserId = User.Identity.GetUserId();
            var myGroups = (from gr in db.GroupMembers
                           where gr.UserId == CurrUserId
                           select gr.GroupId).ToList();

            var myFriends = (from fr in db.UserRelations
                            where fr.User1Id == CurrUserId && (fr.Status == "Friend" || fr.Status == "Follows")
                            select fr.User2Id).ToList();

            
            var posts = db.Posts.Where(u => (u.UserId == CurrUserId || (u.GroupId != null && myGroups.Contains((int)u.GroupId)) || (u.GroupId == null && myFriends.Contains(u.UserId)))).OrderByDescending(a => a.Date).Include("User");

            // Search 
            var search = "";
            if (Request.Params.Get("search") != null)
            {
                search = Request.Params.Get("search").Trim();

                // Search through titles and contents of a post
                List<int> postsIds = db.Posts.Where(
                    post => post.Title.Contains(search)
                    || post.Content.Contains(search)
                    ).Select(p => p.PostId).ToList();

                posts = posts.Where(post => postsIds.Contains(post.PostId)).OrderByDescending(a => a.Date).Include("User"); ;
            }

            ViewBag.SearchString = search;
            ViewBag.Posts = posts;

            return View();
        }

        [Authorize(Roles = "User,Admin")]
        public ActionResult Show(int id)
        {
            Post post = db.Posts.Find(id);
            ViewBag.CurrentUser = new Tuple<string, bool>(User.Identity.GetUserId(), post.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"));
            ViewBag.IsLikedByCurrentUser = db.PostLikes.Find(id, User.Identity.GetUserId()) != null;
           
            return View(post);

        }

        [Authorize(Roles = "User,Admin")]
        public ActionResult New()
        {
            Post post = new Post();
            post.UserId = User.Identity.GetUserId();
            var curr = User.Identity.GetUserId();
            ViewBag.Curr = curr;

            return View(post);
        }

        [HttpPost]
        [Authorize(Roles = "User,Admin")]
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
                    TempData["message"] = "Your post has been added!";
                    /*if (post.GroupId != null)
                    {
                        return RedirectToAction("Show", "Groups", new { id = post.GroupId });
                        // return Redirect($"/Groups/Show/{post.GroupId}");
                    }
                    return RedirectToAction("Index")*/
                    return RedirectToAction((string)TempData["action"], (string)TempData["controller"], new { id = TempData["id"] });                    
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

        [Authorize(Roles = "User,Admin")]
        public ActionResult Edit(int id)
        {
            
            Post post = db.Posts.Find(id);
            if (post.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
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
        [Authorize(Roles = "User,Admin")]
        public ActionResult Edit(int id, Post requestPost)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Post post = db.Posts.Find(id);
                    if (post.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
                    {
                        if (TryUpdateModel(post))
                        {
                            post.Title = requestPost.Title;
                            post.Content = requestPost.Content;
                            post.Date = requestPost.Date;
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
        [Authorize(Roles = "User,Admin")]
        public ActionResult Delete(int id)
        {
            Post post = db.Posts.Find(id);
            if(post.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                if (post.UserId != User.Identity.GetUserId() && (User.IsInRole("Admin")))
                {

                    string subject = "Your post has been deleted.";
                    string body = "Hello, " + post.User.UserName + " ! <br /> Unfortunately, your post "+post.Title+" was deleted by OnlyFriends Admin: " + User.Identity.GetUserName() + "<br /> :(";

                    EmailSender emailsender = new EmailSender();
                    emailsender.SendEmailNotification(post.User.Email, subject, body);
                }
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

    
