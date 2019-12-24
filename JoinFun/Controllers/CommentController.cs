using JoinFun.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JoinFun.Controllers
{
    public class CommentController : Controller
    {
        JoinFunEntities db = new JoinFunEntities();
        //聯絡我們意見回饋
        public ActionResult Comment(string memId)
        {
            if (Session["memid"].ToString() == memId)
            {
                var aaa = db.Comment.Where(m => m.memId == memId).FirstOrDefault();




                Session["Account"] = aaa;
                return View();
            }
            return RedirectToAction("Index", "Activity");
            
        }
        [HttpPost]
        public ActionResult Comment(string memId, string commentTitle, string Comment1)
        {
            string getcommentId = db.Database.SqlQuery<string>("Select [dbo].[GetCommentId]()").FirstOrDefault();


            Comment com = new Comment();

            com.commentId = getcommentId;
            com.memId = memId;
            com.commentTitle = commentTitle;
            com.Comment1 = Comment1;
            com.receivedTime = DateTime.Now;
            db.Comment.Add(com);
            db.SaveChanges();

            return Content("<script>alert('意見回饋已提交，靜候我們的回應。祝您事事順心!!');window.location='/Activity/Index';</script>");
        }

    }
}