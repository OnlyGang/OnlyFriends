using System;
using OnlyFriends.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlyFriends.Controllers
{
    public class CommentsController : Controller
    {
        private OnlyFriends.Models.ApplicationDbContext db = new OnlyFriends.Models.ApplicationDbContext();

        // GET: Comments
        public ActionResult Index()
        {
            return View();
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Comment comm = db.Comments.Find(id);
            db.Comments.Remove(comm);
            db.SaveChanges();
            return Redirect("/Articles/Show/" + comm.ArticleId);
        }

        [HttpPost]
        public ActionResult New(Comment comm)
        {
            comm.Date = DateTime.Now;
            try
            {
                db.Comments.Add(comm);
                db.SaveChanges();
                return Redirect("/Articles/Show/" + comm.ArticleId);
            }

            catch(Exception e)
            {
                return Redirect("/Articles/Show/" + comm.ArticleId);
            }

        }

        public ActionResult Edit(int id)
        {
            Comment comm = db.Comments.Find(id);
            ViewBag.Comment = comm;
            return View();
        }

        [HttpPut]
        public ActionResult Edit(int id, Comment requestComment)
        {
            try
            {
                Comment comm = db.Comments.Find(id);
                if (TryUpdateModel(comm))
                {
                    comm.Content = requestComment.Content;
                    db.SaveChanges();
                }
                return Redirect("/Articles/Show/" + comm.ArticleId);
            }
            catch (Exception e)
            {
                return View();
            }

        }
            
          
    }
}