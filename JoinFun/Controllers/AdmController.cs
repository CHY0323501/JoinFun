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
using JoinFun.ViewModel;
using System.Web.UI.WebControls;
using System.Data.Entity.Infrastructure;

namespace JoinFun.Controllers
{
    public class AdmController : Controller
    {
        SqlConnection Conn = new SqlConnection("data source = MCSDD108212; initial catalog = JoinFun; integrated security = True; MultipleActiveResultSets=True;App=EntityFramework&quot;");
        JoinFunEntities db = new JoinFunEntities();

        int pagesize = 10;
        //管理員登入

        public ActionResult Login()
        {
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
            if (reader.Read())
            {
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
                if (Session["admid"].ToString() == admId)
                {
                    return View();
                }
                return RedirectToAction("Login", "Adm");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdmRegister(string admAcc, string admPass, string admNick)
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
            accAdm.admPass = hashString;
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
                                      where a.admId == admId
                                      select a.admAcc).FirstOrDefault();

                //Session["admid"] = "adm007";
                Session["AdmAccount"] = AccountAdmEdit;
                ViewBag.Nick = AdmMIdEdit.admNick;
                return View();
            }
            return RedirectToAction("Index", "Adm");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdmPwdEdit(string admId, string admNick, string OldPassword, string Password)
        {

            var accPwd = db.Administrator.Where(m => m.admId == admId).FirstOrDefault();

            //舊管理員密碼解密
            var oldsalt = db.Administrator.Where(m => m.admId == admId).FirstOrDefault().admSalt;
            byte[] PasswordAndSaltBytes = System.Text.Encoding.UTF8.GetBytes(OldPassword + oldsalt);
            byte[] HashBytes = new System.Security.Cryptography.SHA256Managed().ComputeHash(PasswordAndSaltBytes);
            string HashString = Convert.ToBase64String(HashBytes);

            if (accPwd.admPass != null)
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
                    accPwd.admPasswordConfirm = hashString;

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

        public ActionResult Index()
        {
            Post post = db.Post.OrderByDescending(m => m.postSerial).FirstOrDefault();


            return View(post);
        }
        //查看公告
        public ActionResult Post(string PostNo, int page = 1)
        {
            Session["admid"] = "adm002";
            if (Session["admid"] != null) {
                if (!String.IsNullOrEmpty(PostNo))
                    ViewBag.PostNo = PostNo;
                ////判斷url的page有無輸入正確頁數
                //int TotalCount = db.Post.ToList().Count();
                //if (page > getTotalPages(TotalCount))
                //    return RedirectToRoute(new { page = 1 });

                return View();
            }
            return RedirectToAction("Login","Adm");
            
        }
        //新增公告
        public ActionResult PostCreate()
        {
            Session["admid"] = "adm002";
            if (Session["admid"] != null)
            {
                string session = Session["admid"].ToString();
                Post post = new Post();
                post.postTime = DateTime.Now;
                ViewBag.admNick = db.Administrator.Where(m => m.admId == session).FirstOrDefault().admNick;
                return View(post);
            }

            return RedirectToAction("Login", "Adm");
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult PostCreate(Post post, HttpPostedFileBase postPics)
        {
            if (post != null)
            {
                string getPostid = db.Database.SqlQuery<string>("Select [dbo].[GetPostId]()").FirstOrDefault();
                post.postSerial = getPostid;
                post.admId = Session["admid"].ToString();
                post.ShowInCarousel = Request["ShowInCarousel"] == "0" ? false : true;
                if (postPics != null)
                {
                    if (postPics.ContentLength > 0)
                    {

                        string fileName = getPostid + Session["admid"].ToString() + ".jpg";

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
                        
                         mes.SendEmail(getMemEmail, post.postTitle, post.postContent);
                        
                    }
                }
                finally
                {
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

        public ActionResult PostEdit(string PostNo)
        {
            if (Session["admid"] != null) {
                Post post = db.Post.Where(m => m.postSerial == PostNo).FirstOrDefault();
                ViewBag.admId = new SelectList(db.Administrator, "admId", "admNick", post.admId);
                ViewBag.ShowInCarouselState = post.ShowInCarousel;
                return View(post);
            }
            return RedirectToAction("Login", "Adm");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PostEdit(Post post)
        {
            db.Entry(post).State = EntityState.Modified;
            post.ShowInCarousel = Request["ShowInCarousel"] == "0" ? false : true;

            db.SaveChanges();

            return RedirectToAction("Post", new { PostNo = post.postSerial });
        }

        //刪除公告
        public ActionResult PostDelete(string PostNo)
        {
            if (Session["admid"] != null) {
                var post = db.Post.Find(PostNo);
                if (!String.IsNullOrEmpty(PostNo) && post != null)
                {
                    //刪除原公告圖檔
                    string filename = post.postPics;
                    System.IO.File.Delete(Server.MapPath("~/Photos/Posts/") + filename);

                    db.Post.Remove(post);
                    db.SaveChanges();
                }
                return RedirectToAction("Post");
            }
            return RedirectToAction("Login", "Adm");
        }

        ////計算總頁數
        //private decimal getTotalPages(int TotalCount) {
        //    decimal TotalPages = TotalCount / pagesize;
        //    return Math.Ceiling(TotalPages);
        //}

        //查詢會員狀態
        public ActionResult Inquire(string searchString)
        {


            var member = from a in db.Member select a;

            if (!string.IsNullOrEmpty(searchString))
            {
                //member = member.Where(m => m.memId.Contains(searchString));
                MemberInquireVM model = new MemberInquireVM()
                {
                    Member = db.Member.Where(s => s.memId.Contains(searchString) || s.memNick.Contains(searchString)).ToList()
                };
                return View(model);
            }
            else
            {

                MemberInquireVM read = new MemberInquireVM()
                {
                    Member = db.Member.ToList(),
                    Violation = db.Violation.ToList(),
                    Punishment = db.Punishment.ToList()


                };
                return View(read);
            }



        }
        //修改違規紀錄

        //public ActionResult InquireEdit(string memID)
        //{
        //    var vio = from a in db.Violation select a;



        //    { 
        //    MemberInquireVM edit = new MemberInquireVM()
        //    {
        //        Member = db.Member.Where(m => m.memId == memID).ToList(),
        //        MemberRemark = db.Member_Remarks.Where(m => m.FromMemId == memID).ToList(),
        //        MessageBoard = db.Message_Board.Where(m => m.memId == memID).ToList(),
        //        Activity = db.Join_Fun_Activities.Where(m => m.hostId == memID).ToList(),
        //        Violation = db.Violation.Where(m => m.CorrespondingEventID == memID).ToList()
        //    };
        //    }
        //    ViewBag.nick = db.Member.Where(m => m.memId == memID).Select(m => m.memNick).FirstOrDefault();


        //    return View(edit);

        public ActionResult InquireEdit(string memID="M000000003")
        {
            var activityVio = (from a in db.Join_Fun_Activities
                                        join b in db.Violation
                                        on a.actId equals b.CorrespondingEventID
                                        where a.hostId == memID
                                        select b).ToList();

            var RemarkVio = (from a in db.Member_Remarks
                                      join b in db.Violation
                                        on a.remarkSerial equals b.CorrespondingEventID
                                      where a.FromMemId == memID
                                      select b).ToList();

            var BoardVio = (from a in db.Message_Board
                                     join b in db.Violation
                                      on a.mboardSerial equals b.CorrespondingEventID
                                     where a.memId == memID
                                     select b).ToList();

            var MemberVio = db.Violation.Where(m => m.CorrespondingEventID == memID).ToList();

            //將會員、留言板、評價、揪團違規查詢結果合併
            var AllVio = activityVio.Union(RemarkVio).Union(BoardVio).Union(MemberVio);



            MemberInquireVM edit = new MemberInquireVM()
            {
                Violation = AllVio.OrderByDescending(m => m.vioId).Where(m=>m.implement_admId!=null),
            };
            
            ViewBag.nick = db.Member.Where(m => m.memId == memID).Select(m => m.memNick).FirstOrDefault();

            return View(edit);


        }


        //所有違規管理(含已處理和未處理項目),預設僅顯示未處理項目,可搜尋選項則包含所有已處理和未處理項目
        public ActionResult AllViolations(string Page)
        {
            AdmView manage = new AdmView()
            {
                violateList = db.Violation.ToList(),
                memList = db.Member.ToList(),
                admin = db.Administrator.ToList()
            };
            return View(manage);
        }

        public ActionResult SortByReport(string startDate, string endDate)
        {
            List<Object> list = new List<object>();
            if (startDate != "" && endDate != "")
            {
                DateTime start = DateTime.Parse(startDate);
                DateTime end = DateTime.Parse(endDate);

                var violation = db.Violation.Where(m => m.vioReportTime >= start && m.vioReportTime <= end).ToList();
                getViolations(list, violation);
            }
            else if (startDate != "")
            {
                DateTime start = DateTime.Parse(startDate);
                var violation = db.Violation.Where(m => m.vioReportTime >= start).ToList();
                getViolations(list, violation);
            }
            else
            {
                DateTime end = DateTime.Parse(endDate);

                var violation = db.Violation.Where(m => m.vioReportTime <= end).ToList();
                getViolations(list, violation);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SortByAct(string startDate, string endDate)
        {
            List<Object> list = new List<object>();
            var violation = db.Violation.ToList();
            if (startDate != "" && endDate != "")
            {
                DateTime start = DateTime.Parse(startDate);
                DateTime end = DateTime.Parse(endDate);
                var act = db.Join_Fun_Activities.Where(m => m.actTime >= start && m.actTime <= end).ToList();
                foreach (var i in act)
                {
                    getViolations(list, violation.Where(m => m.CorrespondingEventID == i.actId).ToList());
                }
            }
            else if (startDate != "")
            {
                DateTime start = DateTime.Parse(startDate);
                var act = db.Join_Fun_Activities.Where(m => m.actTime >= start).ToList();
                foreach (var i in act)
                {
                    getViolations(list, violation.Where(m => m.CorrespondingEventID == i.actId).ToList());
                }
            }
            else
            {
                DateTime end = DateTime.Parse(endDate);
                var act = db.Join_Fun_Activities.Where(m => m.actTime <= end).ToList();
                foreach (var i in act)
                {
                    getViolations(list, violation.Where(m => m.CorrespondingEventID == i.actId).ToList());
                };
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SortById(string actID)
        {
            List<Object> list = new List<object>();
            var violation = db.Violation.Where(m => m.CorrespondingEventID == actID).ToList();
            getViolations(list, violation);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        //依分類顯示管理的違規項目
        public ActionResult ManageViolations(string Page)
        {
            AdmView manage = new AdmView()
            {
                violateList = db.Violation.Where(m => m.vioProcessTime == null).ToList(),
                memList = db.Member.ToList(),
                admin = db.Administrator.ToList()
            };
            ViewBag.ID = Page;
            return View(manage);
        }

        //依選擇日期範圍顯示已處理檢舉紀錄
        public ActionResult ShowByMonths(string type, int month)
        {
            var date = DateTime.Now.AddMonths(month);
            List<Object> list = new List<object>();
            var violation = db.Violation.ToList();
            switch (type)
            {
                case "memList":
                    violation = violation.Where(m => m.CorrespondingEventID.StartsWith("M") && m.vioProcessTime != null && m.vioProcessTime >= date).ToList();
                    getViolations(list, violation);
                    break;
                case "remarkList":
                    violation = violation.Where(m => m.CorrespondingEventID.StartsWith("R") && m.vioProcessTime != null && m.vioProcessTime >= date).ToList();
                    getViolations(list, violation);
                    break;
                case "actList":
                    violation = violation.Where(m => m.CorrespondingEventID.StartsWith("A") && m.vioProcessTime != null && m.vioProcessTime >= date).ToList();
                    getViolations(list, violation);
                    break;
                case "commentList":
                    violation = violation.Where(m => m.CorrespondingEventID.StartsWith("B") && m.vioProcessTime != null && m.vioProcessTime >= date).ToList();
                    getViolations(list, violation);
                    break;
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViolationContent(string vioID)
        {
            var evtID = db.Violation.Where(m => m.vioId == vioID).FirstOrDefault().CorrespondingEventID;
            var member = db.Member.ToList();
            var remark = db.Member_Remarks.ToList();
            var mboard = db.Message_Board.ToList();
            //找出違規會員,若無則回傳某一筆資料以避免空值
            if (db.Member.Any(m => m.memId == evtID))
                member = db.Member.Where(m => m.memId == evtID).ToList();
            member.Where(m => m.memId == "M000000001").ToList();
            //找出被檢舉的評價,若無則回傳某一筆資料以避免空值
            if (db.Member_Remarks.Any(m => m.remarkSerial == evtID))
                remark = db.Member_Remarks.Where(m => m.remarkSerial == evtID).ToList();
            remark.Where(m => m.remarkSerial == "R000000001").ToList();
            //找出被檢舉的不當留言,若無則回傳某一筆資料以避免空值
            if (db.Message_Board.Any(m => m.mboardSerial == evtID))
                mboard = db.Message_Board.Where(m => m.mboardSerial == evtID).ToList();
            mboard.Where(m => m.memId == "B000000001").ToList();

            AdmView violate = new AdmView()
            {
                violateList = db.Violation.Where(m => m.vioId == vioID).ToList(),
                punishList = db.Punishment.ToList(),
                memList = member,
                actList = db.Join_Fun_Activities.ToList(),
                remarkList = remark,
                mboardList = mboard
            };
            ViewBag.ID = Request["Page"];
            return View(violate);
        }

        [HttpPost]
        public ActionResult ViolationContent(string vioID, string punishID, string admID, string memID)
        {
            if (punishID != null)
            {
                var punish = db.Violation.Find(vioID);
                var violateMem = db.Member.Find(memID);
                MessageCenter mail = new MessageCenter();
                List<string> mailList = new List<string> { violateMem.Email };
                switch (punishID)
                {
                    case "pmt0000001":
                        punish.punishId = punishID;
                        punish.implement_admId = admID;
                        punish.vioProcessTime = DateTime.Now;
                        db.SaveChanges();
                        break;
                    case "pmt0000002":
                    case "pmt0000003":
                    case "pmt0000004":
                        punish.punishId = punishID;
                        punish.implement_admId = admID;
                        punish.vioProcessTime = DateTime.Now;
                        db.SaveChanges();
                        if (violateMem.numViolate < 3)
                        {
                            violateMem.numViolate = Convert.ToInt16(violateMem.numViolate + 1);
                            db.SaveChanges();
                            mail.SendEmail(mailList, "違規停權通知",
                                "親愛的Join Fun會員您好：　" +
                                "因您已違反Join Fun網站規定，本站依規定將此帳號" + db.Punishment.Where(m => m.punishId == punishID).FirstOrDefault().punishName
                                + "，如有任何疑問請與本站客服人員聯絡． 感謝您對Join Fun的支持，Join Fun全體人員敬上．");
                        }
                        else
                        {
                            mail.SendEmail(mailList, "違規停權通知",
                                "親愛的Join Fun會員您好：　" +
                                "因您已違反Join Fun網站規定，且違規次數已達3次，本站依規定將此帳號永久停權，如您有任何疑問請與本站客服人員聯絡． 感謝您對Join Fun的支持，Join Fun全體人員敬上．");
                        }
                        break;
                    case "pmt0000005":
                        punish.punishId = punishID;
                        punish.implement_admId = admID;
                        punish.vioProcessTime = DateTime.Now;
                        db.SaveChanges();
                        mail.SendEmail(mailList, "違規停權通知",
                                "親愛的Join Fun會員您好：　" +
                                "因您已嚴重違反Join Fun網站規定，本站依規定將此帳號永久停權，如您有任何疑問請與本站客服人員聯絡． 感謝您對Join Fun的支持，Join Fun全體人員敬上．");
                        break;
                }
                return RedirectToAction("AllViolations");
            }
            var evtID = db.Violation.Where(m => m.vioId == vioID).FirstOrDefault().CorrespondingEventID;
            var member = db.Member.ToList();
            var remark = db.Member_Remarks.ToList();
            var mboard = db.Message_Board.ToList();
            //以下為避免寫入資料庫失敗後return view後為空值的問題
            if (db.Member.Any(m => m.memId == evtID))
                member = db.Member.Where(m => m.memId == evtID).ToList();
            member.Where(m => m.memId == "M000000001").ToList();
            if (db.Member_Remarks.Any(m => m.remarkSerial == evtID))
                remark = db.Member_Remarks.Where(m => m.remarkSerial == evtID).ToList();
            remark.Where(m => m.remarkSerial == "R000000001").ToList();
            if (db.Message_Board.Any(m => m.mboardSerial == evtID))
                mboard = db.Message_Board.Where(m => m.mboardSerial == evtID).ToList();
            mboard.Where(m => m.memId == "B000000001").ToList();
            AdmView violate = new AdmView()
            {
                violateList = db.Violation.Where(m => m.vioId == vioID).ToList(),
                memList = member,
                actList = db.Join_Fun_Activities.ToList(),
                punishList = db.Punishment.ToList(),
                remarkList = remark,
                mboardList = mboard
            };
            return View(violate);
        }

        //隱藏有問題的活動
        [HttpPost]
        public ActionResult HideActivity(string actID)
        {
            var act = db.Join_Fun_Activities.Find(actID);
            act.keepAct = false;
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        //隱藏不當留言
        [HttpPost]
        public ActionResult HideComment(string commentID)
        {
            var comment = db.Message_Board.Find(commentID);
            comment.keepMboard = false;
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Feedback(string Page)
        {
            var feedback = db.Comment.ToList();
            ViewBag.ID = Page;
            return View(feedback);
        }

        public ActionResult FeedBackReply(string id)
        {
            Comment reply = db.Comment.Find(id);
            return View(reply);
        }

        [HttpPost]
        public ActionResult FeedBackReply(string id, string admId, string Page)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var reply = db.Comment.Find(id);
                    var replyId = reply.commentId;
                    var memId = reply.memId;
                    reply.reportTime = DateTime.Now;
                    reply.reportContent = Request["reportContent"];
                    reply.admId = admId;
                    db.SaveChanges();
                    Notification message = new Notification();
                    message.NotiSerial = db.Database.SqlQuery<string>("Select dbo.GetNoteId()").FirstOrDefault();
                    message.InstanceId = replyId;
                    message.ToMemId = memId;
                    message.NotiTitle = "Re: " + reply.commentTitle;
                    message.NotifContent = Request["reportContent"];
                    message.timeReceived = DateTime.Now;
                    message.keepNotice = true;
                    db.Notification.Add(message);
                    db.SaveChanges();
                    transaction.Commit();
                }
                catch (DbUpdateException)
                {
                    transaction.Rollback();
                }
            }
            return RedirectToAction("FeedBack", new { Page = Page });
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
        [HttpPost]
        public ActionResult ActManage(string[] actId, bool[] keepAct, int page = 1)
        {
            string id = "";
            for (var i = 0; i < actId.Length; i++)
            {
                id = actId[i];
                var changeact = db.Join_Fun_Activities.Where(m => m.actId == id).FirstOrDefault();
                changeact.keepAct = keepAct[i];
                db.SaveChanges();
            }


            var actdetail = db.Join_Fun_Activities.ToList();

            int pagesize = 8;
            int pagecurrent = page < 1 ? 1 : page;
            var pagedlist = actdetail.ToPagedList(pagecurrent, pagesize);
            return View(pagedlist);
        }

        public ActionResult key(string keyword,int page=1) {
            var result=db.Join_Fun_Activities.Where(m => m.actId.Contains(keyword) || m.actTopic.Contains(keyword));
            int pagesize = 8;
            int pagecurrent = page < 1 ? 1 : page;
            var pagedlist = result.ToPagedList(pagecurrent, pagesize);
            return View(pagedlist);
        }


        public void DeleteAct(string actid) {
            var delact=db.Join_Fun_Activities.Where(m => m.actId == actid).FirstOrDefault();
            db.Join_Fun_Activities.Remove(delact);
            db.SaveChanges();


        }

        //取得檢舉明細清單方法 for Json data
        public List<Object> getViolations(List<Object> list, List<Violation> violation)
        {
            var member = db.Member.ToList();
            var administrator = db.Administrator.ToList();
            dynamic data = null;
            foreach (var item in violation)
            {
                string name = "";
                if (item.FromAdmID != null)
                    name = administrator.Where(m => m.admId == item.FromAdmID).FirstOrDefault().admNick;
                else if (item.FromMemId != null)
                    name = member.Where(m => m.memId == item.FromMemId).FirstOrDefault().memNick;
                string type = "";
                if (item.CorrespondingEventID.StartsWith("M"))
                {
                    type = "會員";
                }
                else if (item.CorrespondingEventID.StartsWith("A"))
                {
                    type = "揪團活動";
                }
                else if (item.CorrespondingEventID.StartsWith("R"))
                {
                    type = "會員評價";
                }
                else
                {
                    type = "留言板";
                }
                string condition = "";
                if (item.vioProcessTime == null)
                    condition = "未處理";
                else if (item.vioProcessTime != null)
                    condition = "已處理";

                data = new
                {
                    id = item.vioId,
                    name,
                    type,
                    typeId = item.CorrespondingEventID,
                    title = item.vioTitle,
                    vioTime = item.vioReportTime,
                    condition,
                    doneTime = item.vioProcessTime
                };
                list.Add(data);
            }
            return list;
        }
    }
}