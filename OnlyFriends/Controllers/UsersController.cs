using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OnlyFriends.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlyFriends.Controllers
{
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Users
        public ActionResult Index()
        {
            var users = db.Users.OrderBy(u => u.UserName);
            /*var users = from user in db.Users
                        orderby user.UserName
                        select user;*/

            // Search 
            var search = "";
            if (Request.Params.Get("search") != null)
            {
                search = Request.Params.Get("search").Trim();

                // Search through titles and contents of a post
                List<string> userIds = db.Users.Where(
                    user => user.UserName.Contains(search) 
                    || user.Email.Contains(search)
                    ).Select(us => us.Id).ToList();

                users = users.Where(user => userIds.Contains(user.Id)).OrderBy(u => u.UserName);
            }

            ViewBag.SearchString = search;
            ViewBag.UsersList = users;
            ViewBag.CurrentUser = new Tuple<string, bool>(User.Identity.GetUserId(), User.IsInRole("Editor") || User.IsInRole("Admin"));
            return View();
        }

        public ActionResult MyPage()
        {
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            return View(user);
        }
        
        public ActionResult Show(string id)
        {
            // System.Diagnostics.Debug.WriteLine((id == null ? "null" : id));
            if (id == User.Identity.GetUserId())
                return RedirectToAction("MyPage");
            ApplicationUser user = db.Users.Find(id);
            string currentRole = user.Roles.FirstOrDefault().RoleId;
            ViewBag.CurrentUser = new Tuple<string, bool>(User.Identity.GetUserId(), User.IsInRole("Editor") || User.IsInRole("Admin"));
            var userRoleName = (from role in db.Roles
                                where role.Id == currentRole
                                select role.Name).First();

            ViewBag.Relation = db.UserRelations.Find(User.Identity.GetUserId(), id);

            ViewBag.RoleName = userRoleName;
            ViewBag.IsFriendRequestSent = (db.FriendRequests.Find(User.Identity.GetUserId(), id) != null);
            return View(user);
        }

        public ActionResult Edit(string id)
        {
            
            ApplicationUser user = db.Users.Find(id);
            user.AllRoles = GetAllRoles();
            var userRole = user.Roles.FirstOrDefault();
            ViewBag.userRole = userRole.RoleId;
            ViewBag.CurrentUser = new Tuple<string, bool>(User.Identity.GetUserId(), User.IsInRole("Editor") || User.IsInRole("Admin"));
            return View(user);
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllRoles()
        {
            var selectList = new List<SelectListItem>();
            var roles = from role in db.Roles select role;
            foreach (var role in roles)
            {
                selectList.Add(new SelectListItem
                {
                    Value = role.Id.ToString(),
                    Text = role.Name.ToString()
                });
            }
            return selectList;
        }

        [HttpPut]
        public ActionResult Edit(string id, ApplicationUser newData)
        {
            ApplicationUser user = db.Users.Find(id);
            user.AllRoles = GetAllRoles();
            var userRole = user.Roles.FirstOrDefault();
            ViewBag.userRole = userRole.RoleId;
            try
            {
                ApplicationDbContext context = new ApplicationDbContext();
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                if (TryUpdateModel(user))
                {
                    user.UserName = newData.UserName;
                    user.Email = newData.Email;
                    user.PhoneNumber = newData.PhoneNumber;
                    user.IsPrivate = newData.IsPrivate;
                    var roles = from role in db.Roles select role;
                    foreach (var role in roles)
                    {
                        UserManager.RemoveFromRole(id, role.Name);
                    }
                    var selectedRole =
                    db.Roles.Find(HttpContext.Request.Params.Get("newRole"));
                    UserManager.AddToRole(id, selectedRole.Name);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Response.Write(e.Message);
                newData.Id = id;
                return View(newData);
            }

        }


    }


}