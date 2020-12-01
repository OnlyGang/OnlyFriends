using Microsoft.AspNet.Identity;
using OnlyFriends.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlyFriends.Controllers
{
    public class GroupsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Groups
        public ActionResult Index()
        {
            var groups = db.Groups.Include("User");
            ViewBag.Groups = groups;
            return View();
        }

        public ActionResult New()
        {
            Group group = new Group();
            group.UserId = User.Identity.GetUserId();
            var curr = User.Identity.GetUserId();
            ViewBag.Curr = curr;

            return View(group);
        }

        [HttpPost]
        public ActionResult New(Group group)
        {
            group.UserId = User.Identity.GetUserId();
            group.Date = DateTime.Now;
            try
            {
                if (ModelState.IsValid)
                {
                    db.Groups.Add(group);
                    db.SaveChanges();
                    TempData["message"] = "The group has been created!";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(group);
                }
            }
            catch (Exception e)
            {
                return View(group);
            }
        }

        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult Show(int id)
        {
            var group = db.Groups.Find(id);
            ViewBag.CurrentUser = new Tuple<string, bool>(User.Identity.GetUserId(), group.UserId == User.Identity.GetUserId() || User.IsInRole("Editor") || User.IsInRole("Admin"));
            ViewBag.IsMember = db.GroupMembers.Find(id, User.Identity.GetUserId()) != null;
            return View(group);

        }

        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult Edit(int id)
        {

            Group group = db.Groups.Find(id);
            if (group.UserId == User.Identity.GetUserId() || User.IsInRole("Editor") || User.IsInRole("Admin"))
            {
                return View(group);
            }
            else
            {
                TempData["message"] = "You don't have enough permissions to modify this group!";
                return RedirectToAction("Index");
            }
        }

        [HttpPut]
        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult Edit(int id, Group requestGroup)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Group group = db.Groups.Find(id);
                    if (group.UserId == User.Identity.GetUserId() || User.IsInRole("Editor") || User.IsInRole("Admin"))
                    {
                        if (TryUpdateModel(group))
                        {
                            group.Title = requestGroup.Title;
                            group.Description = requestGroup.Description;
                            group.Date = requestGroup.Date;
                            db.SaveChanges();
                            TempData["message"] = "The group has been successfully changed!";
                            return RedirectToAction("Show/" + id);
                        }

                        return View(requestGroup);
                    }
                    else
                    {
                        TempData["message"] = "You don't have enough permissions to modify this Group!";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return View(requestGroup);
                }
            }
            catch (Exception e)
            {
                return View(requestGroup);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult Delete(int id)
        {
            Group group = db.Groups.Find(id);
            if (group.UserId == User.Identity.GetUserId() || User.IsInRole("Editor") || User.IsInRole("Admin"))
            {
                db.Groups.Remove(group);
                db.SaveChanges();
                TempData["message"] = "The group was deleted";
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