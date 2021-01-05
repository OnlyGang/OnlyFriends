using OnlyFriends.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlyFriends.Controllers
{
    public class UserRelationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: UserRelations
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public ActionResult New(UserRelation userrelation)
        {
            userrelation.Date = DateTime.Now;
            var userrelation2 = new UserRelation();
            userrelation2.Date = userrelation.Date;
            userrelation2.Status = userrelation.Status;

            switch (userrelation.Status)
            {
                case "Friend":
                    userrelation2.Status = "Friend";
                    break;
                case "Follows":
                    userrelation2.Status = "IsFollowed";
                    break;
                case "Blocked":
                    userrelation2.Status = "IsBlocked";
                    break;
            } 

            userrelation2.User1Id = userrelation.User2Id;
            userrelation2.User2Id = userrelation.User1Id;
            try
            {
                if (ModelState.IsValid)
                {
                    db.UserRelations.Add(userrelation);
                    db.UserRelations.Add(userrelation2);
                    db.SaveChanges();
                    return RedirectToAction("Show", "Users", new { id = userrelation.User2Id });
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

        [Authorize(Roles = "User,Admin")]
        [HttpPut]
        public ActionResult Edit(UserRelation request)
        {
            string User1Id = request.User1Id;
            string User2Id = request.User2Id;

            try
            {
                if (ModelState.IsValid)
                {
                    UserRelation userrelation = db.UserRelations.Find(User1Id, User2Id);
                    UserRelation userrelation2 = db.UserRelations.Find(User2Id, User1Id);

                    if (TryUpdateModel(userrelation) /*&& TryUpdateModel(userrelation2)*/)
                    {
                        userrelation.Status = request.Status;

                        switch (userrelation.Status)
                        {
                            case "Friend":
                                userrelation2.Status = "Friend";
                                break;
                            case "Follows":
                                if (userrelation2.Status == "Follows")
                                {
                                    userrelation.Status = userrelation2.Status = "Friend";
                                    break;
                                }
                                userrelation2.Status = "IsFollowed";
                                break;
                            case "Blocked":
                                userrelation2.Status = "IsBlocked";
                                break;
                        }

                        userrelation.Date = DateTime.Now;
                        userrelation2.Date = DateTime.Now;
                        db.SaveChanges();
                        TempData["message"] = "The status has been successfully changed!";
                        return RedirectToAction("Show", "Users", new { id = User2Id });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Users");
                    }
                }
                else
                {
                    TempData["ErrorMsg"] = "Error boss!";
                    return RedirectToAction("Index", "Users");
                }
            }
            catch (Exception e)
            {
                TempData["ErrorMsg"] = e.Message;
                return RedirectToAction("Index", "Users");
            }

        }

        [Authorize(Roles = "User,Admin")]
        [HttpDelete]
        public ActionResult Delete(string User1Id, string User2Id)
        {
            UserRelation ToDelete1 = db.UserRelations.Find(User1Id, User2Id);
            UserRelation ToDelete2 = db.UserRelations.Find(User2Id, User1Id);
            db.UserRelations.Remove(ToDelete1);
            db.UserRelations.Remove(ToDelete2);
            db.SaveChanges();
            // return RedirectToAction("Show", "Users", new { id = User2Id });
            return RedirectToAction((string) TempData["action"], (string) TempData["controller"], new { id = (string) TempData["id"] });
        }
    }
}