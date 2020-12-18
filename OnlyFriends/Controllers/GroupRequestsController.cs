using Microsoft.AspNet.Identity;
using OnlyFriends.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlyFriends.Controllers
{
    public class GroupRequestsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        // GET: GroupRequests
        [NonAction]
        public ActionResult Index()
        {
            return View();
        }

      /*  public ActionResult Show(int GroupId)
        {
            var group = db.Groups.Find(GroupId);
            var grouprequests = group.GroupRequests;
            ViewBag.Group = group;
            ViewBag.GroupRequests = grouprequests;
            ViewBag.CurrentUser = new Tuple<string, bool>(User.Identity.GetUserId(), group.UserId == User.Identity.GetUserId() || User.IsInRole("Editor") || User.IsInRole("Admin"));
            return View(group);
        }
*/

        [HttpPost]
        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult New(GroupRequest grouprequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.GroupRequests.Add(grouprequest);
                    db.SaveChanges();
                    return RedirectToAction("Show","Groups", new { id = grouprequest.GroupId  });
                }
                else
                {
                    return RedirectToAction("Show", "Groups", new { id = grouprequest.GroupId });
                }

            }
            catch (Exception e)
            {
                return RedirectToAction("Show", "Groups", new { id = grouprequest.GroupId });
            }

        }

        [Authorize(Roles = "User,Editor,Admin")]
        [HttpDelete]
        public ActionResult Delete(string SenderId, int GroupId)
        {
            GroupRequest ToDelete = db.GroupRequests.Find(SenderId, GroupId);
            db.GroupRequests.Remove(ToDelete);
            db.SaveChanges();
            return RedirectToAction("Show", "Groups", new { id = GroupId });
        }
        [HttpDelete]
        public ActionResult Accept(string SenderId, int GroupId)
        {
            GroupRequest ToDelete = db.GroupRequests.Find(SenderId, GroupId);
            db.GroupRequests.Remove(ToDelete);
            db.SaveChanges();
            
            GroupMember ToAdd = new GroupMember();
            ToAdd.GroupId = GroupId;
            ToAdd.UserId = SenderId;
            var GMC = new GroupMembersController();
            GMC.New(ToAdd);

            return RedirectToAction("Show", "Groups", new { id = GroupId });
        }
    }
}