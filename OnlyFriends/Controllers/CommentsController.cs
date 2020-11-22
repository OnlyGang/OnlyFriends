using System;
using OnlyFriends.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace OnlyFriends.Controllers
{
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Comments
        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "User,Editor,Admin")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Comment comm = db.Comments.Find(id);
            db.Comments.Remove(comm);
            db.SaveChanges();
            return Redirect("/Posts/Show/" + comm.PostId);
        }

        [HttpPost]
        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult New(Comment comm)
        {
            comm.Date = DateTime.Now;
            comm.UserId = User.Identity.GetUserId();
            try
            {
                db.Comments.Add(comm);
                db.SaveChanges();
                return Redirect("/Posts/Show/" + comm.PostId);
            }

            catch(Exception e)
            {
                return Redirect("/Posts/Show/" + comm.PostId);
            }

        }
        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult Edit(int id)
        {
            Comment comm = db.Comments.Find(id);
            if (comm.UserId == User.Identity.GetUserId() || User.IsInRole("Editor") || User.IsInRole("Admin"))
            {
                return View(comm);
            }
            else
            {
                TempData["message"] = "You don't have enough permissions to modify this article!";
                return Redirect("/Posts/Show/" + comm.PostId);
            }
        }

        [HttpPut]
        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult Edit(int id, Comment requestComment)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Comment comm = db.Comments.Find(id);
                    if (comm.UserId == User.Identity.GetUserId() || User.IsInRole("Editor") || User.IsInRole("Admin"))
                    {
                        if (TryUpdateModel(comm))
                        {
                            comm.Content = requestComment.Content;
                            TempData["message"] = "The comment has been successfully changed!";
                            db.SaveChanges();
                            return Redirect("/Posts/Show/" + comm.PostId);
                        }
                        return View(requestComment);
                    }
                    else
                    {
                        TempData["message"] = "You don't have enough permissions to modify this comment!";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return View(requestComment);
                }
            }
            catch (Exception e)
            {
                return View(requestComment);
            }
        }
            
          
    }
}