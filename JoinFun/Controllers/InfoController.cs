using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JoinFun.Models;
using Newtonsoft.Json;
using X.PagedList;

namespace JoinFun.Views.Info
{
    public class InfoController : Controller
    {
        JoinFunEntities db = new JoinFunEntities();

        int pagesize = 10;

        
        //一般會員查看公告
        public ActionResult Post(string PostNo, int page = 1)
        {
            if (!String.IsNullOrEmpty(PostNo))
                ViewBag.PostNo = PostNo;
            
            return View();
        }
        [ChildActionOnly]
        public PartialViewResult _Post(string PostNo, int page = 1)
        {
            List<Post> post = db.Post.OrderByDescending(m => m.postSerial).ToList();
            ViewBag.PostCount = post.Count();
            if (!String.IsNullOrEmpty(PostNo))
            {
                var p = post.Where(m => m.postSerial == PostNo).ToList();

                //取得上、下一則公告編號
                Post next = post.SkipWhile(m => m.postSerial != PostNo).Skip(1).FirstOrDefault();
                Post previous = post.TakeWhile(m => m.postSerial != PostNo).LastOrDefault();
                if (next != null)
                    ViewBag.next = next.postSerial;
                if (previous != null)
                    ViewBag.previous = previous.postSerial;

                ViewBag.PostCount = 1;
                return PartialView(p.ToPagedList(1, 1));
            }
            //公告分頁

            int pagecurrent = page < 1 ? 1 : page;
            var pagedlist = post.ToPagedList(pagecurrent, pagesize);

            return PartialView(pagedlist);
        }


        //聯絡我們意見回饋
        public ActionResult Comment(string memid)
        {
          
               var aaa = db.Comment.Where(m => m.memId == memid).FirstOrDefault();


             
            return View();
        }

        [HttpPost]
        public ActionResult Comment(string memid,string commentTitle,string Comment)
        {
            string getcommentId = db.Database.SqlQuery<string>("Select [dbo].[GetCommentId]()").FirstOrDefault();


            Comment com = new Comment();

            com.commentId = getcommentId;
            com.memId= memid;
            com.commentTitle = commentTitle;
            com.Comment1 = Comment;
            com.receivedTime = DateTime.Now;
            db.Comment.Add(com);
            db.SaveChanges();

            return View();
        }

    }
}