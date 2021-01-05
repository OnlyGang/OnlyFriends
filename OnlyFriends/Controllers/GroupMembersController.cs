using Microsoft.AspNet.Identity;
using OnlyFriends.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlyFriends.Controllers
{
    public class GroupMembersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        [NonAction]
        public ActionResult Index()
        {
            return View();
        }
        
        // GET: GroupMembers
        public ActionResult Show(int GroupId)
        {
            var group = db.Groups.Find(GroupId);
            ViewBag.CurrentUser = new Tuple<string, bool>(User.Identity.GetUserId(), group.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"));

            return View(group);
        }

        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public ActionResult New(GroupMember groupmember)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.GroupMembers.Add(groupmember);
                    db.SaveChanges();
                    return Redirect("/Groups/Show/" + groupmember.GroupId);
                }
                else
                {
                    return Redirect("/Groups/Show/" + groupmember.GroupId);
                }
                    
            }
            catch (Exception e)
            {
                return Redirect("/Groups/Show/" + groupmember.GroupId);
            }

        }

        [Authorize(Roles = "User,Admin")]
        [HttpDelete]
        public ActionResult Delete(int GroupId, string UserId)
        {
            GroupMember ToDelete = db.GroupMembers.Find(GroupId, UserId);
            db.GroupMembers.Remove(ToDelete);
            db.SaveChanges();
            string controller = (string)TempData["ReturnTo"];
            return Redirect($"/{controller}/ShowRelations/" + GroupId);
        }
    }

}