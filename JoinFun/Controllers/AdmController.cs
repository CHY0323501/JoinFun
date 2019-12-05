using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JoinFun.Models;
using X.PagedList;

namespace JoinFun.Controllers
{
    public class AdmController : Controller
    {
        SqlConnection Conn = new SqlConnection("data source = MCSDD108212; initial catalog = JoinFun; integrated security = True; MultipleActiveResultSets=True;App=EntityFramework&quot;");
        JoinFunEntities db = new JoinFunEntities();



        int pagesize = 10;
        //管理員登入

        public ActionResult Login() {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string account, string pass)
        {
            //使用Linq查詢取得帳號(加密用)
            var getAcc = db.Administrator.Where(m => m.admAcc == account).FirstOrDefault();
            if (getAcc == null)
            {
                ViewBag.admLoginERR = "您輸入的帳號或密碼錯誤";
                return View();
            }
            //查詢管理員帳號及密碼
            string sql = "select * from Administrator where admAcc=@acc and admPass=@pass";
            SqlCommand cmd = new SqlCommand(sql, Conn);
            SqlDataReader reader;



            //取得salt字串
            string salt = getAcc.admSalt;
            //產生雜湊
            byte[] PasswordAndSaltBytes = System.Text.Encoding.UTF8.GetBytes(pass + salt);
            byte[] HashBytes = new System.Security.Cryptography.SHA256Managed().ComputeHash(PasswordAndSaltBytes);
            string HashString = Convert.ToBase64String(HashBytes);
            cmd.Parameters.AddWithValue("@acc", account);
            cmd.Parameters.AddWithValue("@pass", HashString);

            //連線資料庫並傳回查詢結果
            Conn.Open();
            reader = cmd.ExecuteReader();

            //比對結果
            if (reader.Read()) {
                //保留登入狀態及管理員暱稱
                Session["admid"] = reader["admId"].ToString();
                Session["admNick"] = reader["admNick"].ToString();
                //中斷資料庫連線
                Conn.Close();
                ViewBag.admLoginERR = "";
                return RedirectToAction("Index");
            }
            Conn.Close();
            ViewBag.admLoginERR = "您輸入的帳號或密碼錯誤";
            return View();
        }



        //註冊管理員
        public ActionResult AdmRegister()
        {



            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdmRegister(string admId, string admAcc, string admPass, string admNick)
        {


            //密碼雜湊 salt+hash
            string salt = Guid.NewGuid().ToString();
            byte[] passwordAndSaltBytes = System.Text.Encoding.UTF8.GetBytes(admPass + salt);
            byte[] hashBytes = new System.Security.Cryptography.SHA256Managed().ComputeHash(passwordAndSaltBytes);
            string hashString = Convert.ToBase64String(hashBytes);



            //string getadmId = db.Database.SqlQuery<string>("select [dbo].[GetAdmId]()").FirstOrDefault();

            Administrator accAdm = new Administrator();
            accAdm.admId = admId;
            accAdm.admAcc = admAcc;
            accAdm.admPass = hashString;
            accAdm.admPasswordConfirm = hashString;
            accAdm.admNick = admNick;


            accAdm.admSalt = salt;
            db.Administrator.Add(accAdm);
            db.SaveChanges();


            return RedirectToAction("Index");

        }



        public ActionResult Index() {

            return View();
        }
        //查看公告
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
        //公告partial view
        //用於前後台觀看詳細公告、後台瀏覽所有公告
        [ChildActionOnly]
        public PartialViewResult _Post(string PostNo, int page = 1)
        {
            List<Post> post = db.Post.OrderByDescending(m => m.postSerial).ToList();
            ViewBag.PostCount = post.Count();
            if (!String.IsNullOrEmpty(PostNo))
            {
                var p = post.Where(m => m.postSerial == PostNo).ToList();
                ViewBag.PostCount = 1;
                return PartialView(p.ToPagedList(1, 1));
            }
            //公告分頁

            int pagecurrent = page < 1 ? 1 : page;
            var pagedlist = post.ToPagedList(pagecurrent, pagesize);

            return PartialView(pagedlist);
        }

        //編輯公告
        public ActionResult PostEdit(string PostNo) {
            Post post = db.Post.Where(m => m.postSerial == PostNo).FirstOrDefault();
            ViewBag.admId = new SelectList(db.Administrator, "admId", "admNick");
            return View(post);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PostEdit(Post post)
        {
            if (ModelState.IsValid) {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("Post", new { PostNo = post.postSerial });
        }

        //刪除公告
        public ActionResult PostDelete(string PostNo) {
            var post = db.Post.Find(PostNo);
            if (!String.IsNullOrEmpty(PostNo) && post != null) {
                db.Post.Remove(post);
                db.SaveChanges();
            }
            return RedirectToAction("Post");
        }

        //計算總頁數
        private decimal getTotalPages(int TotalCount) {
            decimal TotalPages = TotalCount / pagesize;
            return Math.Ceiling(TotalPages);
        }



        public ActionResult ActManage(int page = 1)
        {
            var actdetail = db.Join_Fun_Activities.ToList();

            int pagesize = 8;
            int pagecurrent = page < 1 ? 1 : page;
            var pagedlist = actdetail.ToPagedList(pagecurrent, pagesize);
            return View(pagedlist);



        }

        public void ActKeepChange(string actId)
        {

            var changeact = db.Join_Fun_Activities.Where(m => m.actId == actId).FirstOrDefault();
            bool keepact = db.Join_Fun_Activities.Where(m => m.actId == actId).FirstOrDefault().keepAct;
            changeact.keepAct = !keepact;
            db.SaveChanges();
            //return Json(true, JsonRequestBehavior.AllowGet);

        }
        //[HttpPost]
        //public ActionResult ActManage(string[] actId, bool[] keepAct, int page = 1)
        //{
        //    string id = "";
        //    for (var i = 0; i < actId.Length; i++) {
        //        id = actId[i];
        //        var changeact = db.Join_Fun_Activities.Where(m => m.actId == id).FirstOrDefault();
        //        changeact.keepAct = keepAct[i];
        //        db.SaveChanges();
        //    }


        //    var actdetail = db.Join_Fun_Activities.ToList();

        //    int pagesize = 8;
        //    int pagecurrent = page < 1 ? 1 : page;
        //    var pagedlist = actdetail.ToPagedList(pagecurrent, pagesize);
        //    return View(pagedlist);
        //}


        [HttpPost]
        public ActionResult ActManage(bool[] keepAct, string[] actId, int page = 1)
        {
            //db.Join_Fun_Activities.Where(m => m.actId == joinkeep.actId).FirstOrDefault().keepAct = joinkeep.keepAct;
            if(keepAct.Length > 0)
            {
                for(int i=0; i<keepAct.Length; i++)
                {
                    var id = actId[i];
                    var act = db.Join_Fun_Activities.Find(id);
                    act.keepAct = keepAct[i];
                    db.SaveChanges();
                }
            }
            //db.SaveChanges();
            //var result = db.Join_Fun_Activities.Where(m => m.actId == joinkeep.actId).FirstOrDefault().keepAct;

            var actdetail = db.Join_Fun_Activities.ToList();

            int pagesize = 8;
            int pagecurrent = page < 1 ? 1 : page;
            var pagedlist = actdetail.ToPagedList(pagecurrent, pagesize);
            return View(pagedlist);
        }


        public void DeleteAct(string actid) {
            var delact=db.Join_Fun_Activities.Where(m => m.actId == actid).FirstOrDefault();
            db.Join_Fun_Activities.Remove(delact);
            db.SaveChanges();


        }

        
    }
}