using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JoinFun.Models;
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
            ////判斷url的page有無輸入正確頁數
            //int TotalCount = db.Post.ToList().Count();
            //if (page > getTotalPages(TotalCount))
            //    return RedirectToRoute(new { page = 1 });
            Session["admid"] = "adm002";

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
    }
}