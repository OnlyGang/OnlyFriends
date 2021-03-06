﻿using Microsoft.AspNet.Identity;
using OnlyFriends.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlyFriends.Controllers
{
    public class FriendRequestsController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: FriendRequests
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public ActionResult New(FriendRequest friendrequest)
        {
            friendrequest.Date = DateTime.Now;
            try
            {
                if (ModelState.IsValid)
                {
                    db.FriendRequests.Add(friendrequest);
                    db.SaveChanges();
                    return RedirectToAction("Show", "Users", new { id = friendrequest.RecieverId });
                }
                else
                {                   
                    return RedirectToAction("Index", "Users");
                }
                
            }
            catch (Exception e)
            {
                TempData["ErrorMsg"] = e.Message;
                return RedirectToAction("Index", "Users");
            }

        }

        [HttpDelete]
        [Authorize(Roles = "User,Admin")]
        public ActionResult Accept(string SenderId, string RecieverId)
        {
            FriendRequest ToDelete = db.FriendRequests.Find(SenderId, RecieverId);
            db.FriendRequests.Remove(ToDelete);
            db.SaveChanges();
            UserRelation req = db.UserRelations.Find(SenderId, RecieverId);

            var URC = new UserRelationsController();
            if (req != null)
            {
                URC.Edit(req);
            }
            else
            {
                UserRelation newRel = new UserRelation();
                newRel.User1Id = SenderId;
                newRel.User2Id = RecieverId;
                newRel.Status = "Friend";
                URC.New(newRel);
            }
            return RedirectToAction((string)TempData["action"], (string)TempData["controller"], new { id = TempData["id"] });
        }
        

        [Authorize(Roles = "User,Admin")]
        [HttpDelete]
        public ActionResult Delete(string SenderId, string RecieverId)
        {
            FriendRequest ToDelete = db.FriendRequests.Find(SenderId, RecieverId);
            db.FriendRequests.Remove(ToDelete);
            db.SaveChanges();
            return RedirectToAction((string)TempData["action"], (string)TempData["controller"], new { id = TempData["id"] });
        }


        /*public ActionResult Show(string id)
        {
            var user = db.Users.Find(id);
            ViewBag.CurrentUser = new Tuple<string, bool>(User.Identity.GetUserId(), user.Id == User.Identity.GetUserId() || User.IsInRole("Admin"));
            return View(user);
        }*/
    }
}