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
        [Authorize(Roles = "User,Editor,Admin")]
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

        [Authorize(Roles = "User,Editor,Admin")]
        [HttpDelete]
        public ActionResult Delete(string SenderId, string RecieverId)
        {
            FriendRequest ToDelete = db.FriendRequests.Find(SenderId, RecieverId);
            db.FriendRequests.Remove(ToDelete);
            db.SaveChanges();
            return RedirectToAction("Show", "Users", new { id = RecieverId });
        }
    }
}