using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JoinFun.Models;
using X.PagedList;
using JoinFun.Utilities;

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
        public ActionResult Login(string account,string pass)
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
            SqlCommand cmd = new SqlCommand(sql,Conn);
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

        //管理員登出
        public ActionResult AdmLogout()
        {
            Session.Clear();
            return RedirectToAction("Login", "Adm");
        }

        //註冊管理員
        public ActionResult AdmRegister(string admId)
        {
            var AdmMIdEdit = db.Administrator.Where(m => m.admId == admId).FirstOrDefault();
            if (admId == null)
            {
                return RedirectToAction("Login", "Adm");
            }
            else
            {
                if (Session["admid"].ToString() == admId )
                {
                    return View();
                }
                return RedirectToAction("Login", "Adm");

            }
           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdmRegister( string admAcc, string admPass, string admNick)
        {
          //密碼雜湊 salt+hash
            string salt = Guid.NewGuid().ToString();
            byte[] passwordAndSaltBytes = System.Text.Encoding.UTF8.GetBytes(admPass + salt);
            byte[] hashBytes = new System.Security.Cryptography.SHA256Managed().ComputeHash(passwordAndSaltBytes);
            string hashString = Convert.ToBase64String(hashBytes);



            string getadmId = db.Database.SqlQuery<string>("select [dbo].[GetAdmId]()").FirstOrDefault();

            Administrator accAdm = new Administrator();
            accAdm.admId = getadmId;
            accAdm.admAcc = admAcc;
            accAdm.admPass= hashString;
            accAdm.admPasswordConfirm = hashString;
            accAdm.admNick = admNick;


            accAdm.admSalt = salt;
            db.Administrator.Add(accAdm);           
            db.SaveChanges();


            return RedirectToAction("Index");

        }

        //修改管理員密碼、暱稱
        public ActionResult AdmPwdEdit(string admId)
        {



            if (Session["admId"].ToString() == admId)
            {
                var AdmMIdEdit = db.Administrator.Where(m => m.admId == admId).FirstOrDefault();
            //var AccountAdmEdit = db.Administrator.Find().admAcc;

            var AccountAdmEdit = (from a in db.Administrator
                           where a.admId== admId
                                  select a.admAcc).FirstOrDefault();

                Session["admid"] = "adm007";
                Session["AdmAccount"] = AccountAdmEdit;
            return View();
        }
            return RedirectToAction("Index", "Adm");

    }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdmPwdEdit(string admId,string admNick, string OldPassword, string Password)
        {

            var accPwd = db.Administrator.Where(m => m.admId == admId).FirstOrDefault();
           
            //舊管理員密碼解密
            var oldsalt = db.Administrator.Where(m => m.admId == admId).FirstOrDefault().admSalt;          
            byte[] PasswordAndSaltBytes = System.Text.Encoding.UTF8.GetBytes(OldPassword + oldsalt);
            byte[] HashBytes = new System.Security.Cryptography.SHA256Managed().ComputeHash(PasswordAndSaltBytes);
            string HashString = Convert.ToBase64String(HashBytes);

            if (accPwd.admPass!= null)
            {
                if (accPwd.admPass == HashString)
                {

                    //新密碼雜湊 salt+hash
                    string salt = Guid.NewGuid().ToString();
                    byte[] passwordAndSaltBytes = System.Text.Encoding.UTF8.GetBytes(Password + salt);
                    byte[] hashBytes = new System.Security.Cryptography.SHA256Managed().ComputeHash(passwordAndSaltBytes);
                    string hashString = Convert.ToBase64String(hashBytes);


                    accPwd.admSalt = salt;
                    accPwd.admId = admId;
                    accPwd.admNick = admNick;
                    accPwd.admPass = hashString;
                    accPwd.admPasswordConfirm= hashString;

                    db.SaveChanges();


                    return RedirectToAction("Index", "Adm");


                }
                else
                {
                    ViewBag.AdmPwdEditErr = "舊密碼未填或有誤!";
                    return View();
                }

            }
            return View();
        }

        public ActionResult Index() {
            Post post = db.Post.OrderByDescending(m=>m.postSerial).FirstOrDefault();


            return View(post);
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
        //新增公告
        public ActionResult PostCreate() {

            Session["admid"] = "adm002";

            if (Session["admid"] != null) {
                string session = Session["admid"].ToString();
                Post post = new Post();
                post.postTime = DateTime.Now;
                ViewBag.admNick= db.Administrator.Where(m => m.admId == session).FirstOrDefault().admNick;
                return View(post);
            }
            
            return RedirectToAction("Post");
        }
        [HttpPost,ValidateAntiForgeryToken]
        public ActionResult PostCreate(Post post,HttpPostedFileBase postPics)
        {

            Session["admid"] = "adm002";

            if (post != null) {
                string getPostid = db.Database.SqlQuery<string>("Select [dbo].[GetPostId]()").FirstOrDefault();
                post.postSerial = getPostid;
                post.admId = Session["admid"].ToString();
                post.ShowInCarousel = Request["ShowInCarousel"] == "0" ? false : true;
                if (postPics != null)
                {
                    if (postPics.ContentLength > 0)
                    {               
                        
                        string fileName = getPostid+ Session["admid"].ToString() + ".jpg";
                       
                        postPics.SaveAs(Server.MapPath("~/Photos/Posts/" + fileName));
                        post.postPics = fileName;
                    }
                };
                try
                {
                    //重要公告通知
                    if (post.ShowInCarousel)
                    {

                        Common comm = new Common();

                        for (int i = 0; i < db.Member.Count(); i++)
                        {
                            comm.CreateNoti(false, post.postSerial, db.Member.ToList()[i].memId, post.postTitle, post.postContent);
                        }
                    }
                    //重要公告寄信
                    if (post.ShowInCarousel)
                    {

                        MessageCenter mes = new MessageCenter();

                        //取得所有會員信箱
                        List<string> getMemEmail = new List<string>();
                        for (int m = 0; m < db.Member.Count(); m++)
                        {
                            getMemEmail.Add(db.Member.ToList()[m].Email);
                        }
                        for (int i = 0; i < db.Member.Count(); i++)
                        {
                            mes.SendEmail(getMemEmail, post.postTitle, post.postContent);
                        }
                    }
                }
                catch {
                    db.Post.Add(post);
                    db.SaveChanges();
                }
                
                return RedirectToAction("Post");

            }

            //post.postTime = DateTime.Now;
            //ViewBag.admNick = db.Administrator.Where(m => m.admId == Session["admid"].ToString()).FirstOrDefault().admNick;
            return View(post);
            
            
        }

        //公告partial view
        [ChildActionOnly]
        public PartialViewResult _Post(string PostNo,int page=1)
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
                if(previous!=null)
                ViewBag.previous = previous.postSerial;
                
                ViewBag.PostCount = 1;
                return PartialView(p.ToPagedList(1,1));
            }
            //公告分頁
            
            int pagecurrent = page < 1 ? 1 : page;
            var pagedlist = post.ToPagedList(pagecurrent, pagesize);

            return PartialView(pagedlist);
        }

        //編輯公告
        public ActionResult PostEdit(string PostNo) {
            Post post= db.Post.Where(m=>m.postSerial==PostNo).FirstOrDefault();
            ViewBag.admId = new SelectList(db.Administrator, "admId", "admNick",post.admId);
            ViewBag.ShowInCarouselState = post.ShowInCarousel;
            return View(post);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PostEdit(Post post)
        {
            db.Entry(post).State = EntityState.Modified;
            post.ShowInCarousel = Request["ShowInCarousel"] == "0" ? false : true;

            db.SaveChanges();

            return RedirectToAction("Post",new { PostNo = post.postSerial });
        }

        //刪除公告
        public ActionResult PostDelete(string PostNo) {
            var post = db.Post.Find(PostNo);
            if (!String.IsNullOrEmpty(PostNo)&&post!=null) {
                //刪除原公告圖檔
                string filename = post.postPics;
                System.IO.File.Delete(Server.MapPath("~/Photos/Posts/") + filename);

                db.Post.Remove(post);
                db.SaveChanges();
            }
            return RedirectToAction("Post");
        }

        ////計算總頁數
        //private decimal getTotalPages(int TotalCount) {
        //    decimal TotalPages = TotalCount / pagesize;
        //    return Math.Ceiling(TotalPages);
        //}

        //查詢會員狀態
        public ActionResult Inquire(string memid)
        {
            List<Member> member = db.Member.OrderByDescending(m => m.memId).ToList();

<<<<<<< HEAD
            MemberViolationVM memVio = new MemberViolationVM()
            {
                Member = db.Member.ToList(),
                Violation = db.Violation.ToList(),
                Punishment=db.Punishment.ToList()
            };
            ViewBag.memNick = db.Member.Where(m => m.memId == memid).ToList();

            return View(memVio);
=======




            return View();
>>>>>>> parent of 5e9c8ad... 新增檢舉管理與意見回饋管理功能

        }



    }
}