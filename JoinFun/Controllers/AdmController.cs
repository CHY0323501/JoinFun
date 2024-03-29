﻿using System;
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
using System.Collections;

namespace JoinFun.Controllers
{
    [LoginRule(Front = false)]
    public class AdmController : Controller
    {

        SqlConnection Conn = new SqlConnection("data source=MCSDD108212;initial catalog=JoinFun;persist security info=True;user id=joinfunadmin;password=joinfun123456;MultipleActiveResultSets=True;App=EntityFramework&quot;");
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
            //var AdmMIdEdit = db.Administrator.Where(m => m.admId == admId).FirstOrDefault();
            //if (admId == null)
            //{
            //    return RedirectToAction("Login", "Adm");
            //}
            //else
            //{
            //    if (Session["admid"].ToString() == admId)
            //    {
                    return View();
            //    }
            //    return RedirectToAction("Login", "Adm");
            //}
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

            return Content("<script>alert('新增管理員註冊成功');window.location='/Adm/Index';</script>");
            //return RedirectToAction("Index");

        }

        //修改管理員密碼、暱稱
        public ActionResult AdmPwdEdit(string admId)
        {



           
                var AdmMIdEdit = db.Administrator.Where(m => m.admId == admId).FirstOrDefault();
                //var AccountAdmEdit = db.Administrator.Find().admAcc;

                var AccountAdmEdit = (from a in db.Administrator
                                      where a.admId == admId
                                      select a.admAcc).FirstOrDefault();

                //Session["admid"] = "adm007";
                Session["AdmAccount"] = AccountAdmEdit;
                ViewBag.Nick = AdmMIdEdit.admNick;
                return View(AdmMIdEdit);
            //}
            //return RedirectToAction("Index", "Adm");

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

                    return Content("<script>alert('修改成功');window.location='/Adm/Index';</script>");
                }
                else
                {
                    ViewBag.AdmPwdEditErr = "舊密碼未填或有誤!";
                    return View(accPwd);
                }

            }
            return View(accPwd);
        }

        public ActionResult Index()
        {
            Post post = db.Post.OrderByDescending(m => m.postSerial).FirstOrDefault();


            return View(post);
        }
        //查看公告
        public ActionResult Post(string PostNo, int page = 1)
        {
            //Session["admid"] = "adm002";
            //if (Session["admid"] != null) {
                if (!String.IsNullOrEmpty(PostNo))
                    ViewBag.PostNo = PostNo;
                

                return View();
            //}
            
            
        }
        //新增公告
        public ActionResult PostCreate()
        {
            
            
                string session = Session["admid"].ToString();
                Post post = new Post();
                post.postTime = DateTime.Now;
                ViewBag.admNick = db.Administrator.Where(m => m.admId == session).FirstOrDefault().admNick;
                return View(post);
           

            
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
            
                Post post = db.Post.Where(m => m.postSerial == PostNo).FirstOrDefault();
                ViewBag.admId = new SelectList(db.Administrator, "admId", "admNick", post.admId);
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

            return RedirectToAction("Post", new { PostNo = post.postSerial });
        }

        //刪除公告
        public ActionResult PostDelete(string PostNo)
        {
            
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

        ////計算總頁數
        //private decimal getTotalPages(int TotalCount) {
        //    decimal TotalPages = TotalCount / pagesize;
        //    return Math.Ceiling(TotalPages);
        //}

        //查詢會員狀態
        public ActionResult Inquire(string searchString,int page=1)
        {
            var member = from a in db.Member select a;
             int pagesize = 10;
            int pagecurrent = page < 1 ? 1 : page;
            if (!string.IsNullOrEmpty(searchString))
            {
                var QueryMember = db.Member.Where(s => s.memId.Contains(searchString) || s.memNick.Contains(searchString)).ToList();

                var pagelist = QueryMember.ToPagedList(pagecurrent, pagesize);


                return View(pagelist);
            }
            else
            {
                var QueryMember2 = db.Member.ToList();
                
                var pagelist = QueryMember2.ToPagedList(pagecurrent, pagesize);

                return View(pagelist);
            }
        }
        //查看詳細違規紀錄
        public ActionResult InquireDetail(string memID="M000000003")
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

            MemberInquireVM detail = new MemberInquireVM()
            {
                Violation = AllVio.OrderByDescending(m => m.vioId).Where(m=>m.implement_admId!=null),
            };
            
            ViewBag.nick = db.Member.Where(m => m.memId == memID).Select(m => m.memNick).FirstOrDefault();

            return View(detail);
        }

        //編輯違規項目
        public ActionResult InquireEdit(string vioId,string memId)
        {
            MemberInquireVM edit = new MemberInquireVM()
            {
                Member=db.Member.ToList(),
                Violation=db.Violation.Where(m=>m.vioId==vioId),
                punishment=db.Punishment.ToList()
            };
            ViewBag.vioId = db.Violation.Where(m => m.vioId == vioId).Select(m => m.vioId).FirstOrDefault();
            ViewBag.oldpunid = db.Violation.Where(m => m.vioId == vioId).FirstOrDefault().punishId;
            ViewBag.sus = db.Member.Where(m => m.memId == memId).FirstOrDefault().Suspend;
            return View(edit);
        }

        [HttpPost]
        public ActionResult InquireEdit(string vioId,string memId,string punId,string oldvalue)
        {
            var editVio = db.Violation.Find(vioId);
            //var editmember = db.Member.Find(memId);
            var editmember = db.Member.Where(m=>m.memId== memId).FirstOrDefault();


            MessageCenter mail = new MessageCenter();
            List<string> mailList = new List<string> { editmember.Email };

            Session["admid"] = "adm004";

            if (oldvalue == "pmt0000001")
            {
                switch (punId)
                {
                    case "pmt0000002":
                        editVio.punishId = punId;
                        editVio.implement_admId = Session["admid"].ToString();
                        editVio.vioProcessTime = DateTime.Now;
                        db.SaveChanges();
                        if (editmember.numViolate < 3)
                        {
                            editmember.numViolate = Convert.ToInt16(editmember.numViolate + 1);
                            db.SaveChanges();
                            mail.SendEmail(mailList, "違規停權通知",
                                "親愛的Join Fun會員您好：　" +
                                "因您已違反Join Fun網站規定，本站依規定將此帳號" + db.Punishment.Where(m => m.punishId == punId).FirstOrDefault().punishName
                                + "，如有任何疑問請與本站客服人員聯絡． 感謝您對Join Fun的支持，Join Fun全體人員敬上．");
                        }
                        else
                        {
                            mail.SendEmail(mailList, "違規停權通知",
                                "親愛的Join Fun會員您好：　" +
                                "因您已違反Join Fun網站規定，且違規次數已達3次，本站依規定將此帳號永久停權，如您有任何疑問請與本站客服人員聯絡． 感謝您對Join Fun的支持，Join Fun全體人員敬上．");
                        }
                        break;
                    case "pmt0000003":
                    case "pmt0000004":
                        editVio.punishId = punId;
                        editVio.implement_admId = Session["admid"].ToString();
                        editVio.vioProcessTime = DateTime.Now;
                        editmember.Suspend = true;
                        db.SaveChanges();
                        if (editmember.numViolate < 3)
                        {
                            editmember.numViolate =Convert.ToInt16(editmember.numViolate + 1);
                            db.SaveChanges();
                            mail.SendEmail(mailList, "違規停權通知",
                                "親愛的Join Fun會員您好：　" +
                                "因您已違反Join Fun網站規定，本站依規定將此帳號" + db.Punishment.Where(m => m.punishId == punId).FirstOrDefault().punishName
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
                        editVio.punishId = punId;
                        editVio.implement_admId = Session["admid"].ToString();
                        editVio.vioProcessTime = DateTime.Now;
                        editmember.Suspend = true;
                        db.SaveChanges();
                        mail.SendEmail(mailList, "違規停權通知",
                               "親愛的Join Fun會員您好：　" +
                               "因您已嚴重違反Join Fun網站規定，本站依規定將此帳號永久停權，如您有任何疑問請與本站客服人員聯絡． 感謝您對Join Fun的支持，Join Fun全體人員敬上．");
                        break;
                }
            }
            else 
            {
                editmember.numViolate = Convert.ToInt16(editmember.numViolate-1);
                editmember.Suspend = false;
                switch (punId)
                {
                    case "pmt0000001":
                        editVio.punishId = punId;
                        editVio.implement_admId = Session["admid"].ToString();
                        editVio.vioProcessTime = DateTime.Now;
                        db.SaveChanges();
                        break;
                    case "pmt0000002":
                        editVio.punishId = punId;
                        editVio.implement_admId = Session["admid"].ToString();
                        editVio.vioProcessTime = DateTime.Now;
                        db.SaveChanges();
                        if (editmember.numViolate < 3)
                        {
                            editmember.numViolate = Convert.ToInt16(editmember.numViolate + 1);
                            db.SaveChanges();
                            mail.SendEmail(mailList, "違規停權通知",
                                "親愛的Join Fun會員您好：　" +
                                "因您已違反Join Fun網站規定，本站依規定將此帳號" + db.Punishment.Where(m => m.punishId == punId).FirstOrDefault().punishName
                                + "，如有任何疑問請與本站客服人員聯絡． 感謝您對Join Fun的支持，Join Fun全體人員敬上．");
                        }
                        else
                        {
                            mail.SendEmail(mailList, "違規停權通知",
                                "親愛的Join Fun會員您好：　" +
                                "因您已違反Join Fun網站規定，且違規次數已達3次，本站依規定將此帳號永久停權，如您有任何疑問請與本站客服人員聯絡． 感謝您對Join Fun的支持，Join Fun全體人員敬上．");
                        }
                        break;
                    case "pmt0000003":
                    case "pmt0000004":
                        editVio.punishId = punId;
                        editVio.implement_admId = Session["admid"].ToString();
                        editVio.vioProcessTime = DateTime.Now;
                        editmember.Suspend = true;
                        db.SaveChanges();
                        if (editmember.numViolate <= 3)
                        {
                            editmember.numViolate = Convert.ToInt16(editmember.numViolate + 1);
                            db.SaveChanges();
                            mail.SendEmail(mailList, "違規停權通知",
                                "親愛的Join Fun會員您好：　" +
                                "因您已違反Join Fun網站規定，本站依規定將此帳號" + db.Punishment.Where(m => m.punishId == punId).FirstOrDefault().punishName
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
                        editVio.punishId = punId;
                        editVio.implement_admId = Session["admid"].ToString();
                        editVio.vioProcessTime = DateTime.Now;
                        editmember.Suspend = true;
                        db.SaveChanges();
                        mail.SendEmail(mailList, "違規停權通知",
                               "親愛的Join Fun會員您好：　" +
                               "因您已嚴重違反Join Fun網站規定，本站依規定將此帳號永久停權，如您有任何疑問請與本站客服人員聯絡． 感謝您對Join Fun的支持，Join Fun全體人員敬上．");
                        break;
                }
            }
                db.SaveChanges();
            return RedirectToAction("Inquire","Adm");
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
            //List<Object> list = new List<object>();
            dynamic list = null;
            if (startDate != "" && endDate != "")
            {
                DateTime start = DateTime.Parse(startDate);
                DateTime end = DateTime.Parse(endDate);

                var violation = db.Violation.Where(m => m.vioReportTime >= start && m.vioReportTime <= end).ToList();
                list = getViolations(violation);
            }
            else if (startDate != "")
            {
                DateTime start = DateTime.Parse(startDate);
                var violation = db.Violation.Where(m => m.vioReportTime >= start).ToList();
                list = getViolations(violation);
            }
            else
            {
                DateTime end = DateTime.Parse(endDate);

                var violation = db.Violation.Where(m => m.vioReportTime <= end).ToList();
                list = getViolations(violation);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SortByAct(string startDate, string endDate)
        {
            List<VioList> list = new List<VioList>();
            var violation = db.Violation.ToList();
            if (startDate != "" && endDate != "")
            {
                DateTime start = DateTime.Parse(startDate);
                DateTime end = DateTime.Parse(endDate);
                var act = db.Join_Fun_Activities.Where(m => m.actTime >= start && m.actTime <= end).ToList();

                list = getViolations(violation, getList(act));
            }
            else if (startDate != "")
            {
                DateTime start = DateTime.Parse(startDate);
                var act = db.Join_Fun_Activities.Where(m => m.actTime >= start).ToList();
                list = getViolations(violation, getList(act));
            }
            else
            {
                DateTime end = DateTime.Parse(endDate);
                var act = db.Join_Fun_Activities.Where(m => m.actTime <= end).ToList();
                list = getViolations(violation, getList(act));
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SortById(string actID)
        {
            //List<Object> list = new List<object>();
            var violation = db.Violation.ToList();
            var act = db.Join_Fun_Activities.Where(a => a.actId == actID).ToList();
            return Json(getViolations(violation, getList(act)), JsonRequestBehavior.AllowGet);
        }

        public List<string> getList(List<Join_Fun_Activities> act)
        {
            List<string> id = new List<string>();
            foreach (var item in act)
            {
                id.Add(item.actId);
                if (db.Activity_Details.Any(a => a.actId == item.actId))
                {
                    var detail = db.Activity_Details.Where(a => a.actId == item.actId).ToList();
                    foreach (var i in detail)
                    {
                        if (db.Violation.Any(v => v.CorrespondingEventID == i.memId)&&!id.Contains(i.memId))
                            id.Add(db.Violation.Where(v => v.CorrespondingEventID == i.memId).FirstOrDefault().CorrespondingEventID);
                    }
                }
                if (db.Message_Board.Any(b => b.actId == item.actId))
                {
                    var board = db.Message_Board.Where(b => b.actId == item.actId).ToList();
                    foreach (var i in board)
                    {
                        if (db.Violation.Any(v => v.CorrespondingEventID == i.mboardSerial) && !id.Contains(i.mboardSerial))
                            id.Add(db.Violation.Where(v => v.CorrespondingEventID == i.mboardSerial).FirstOrDefault().CorrespondingEventID);
                    }
                }
                if (db.Member_Remarks.Any(r => r.actId == item.actId))
                {
                    var remark = db.Member_Remarks.Where(r => r.actId == item.actId).ToList();
                    foreach (var i in remark)
                    {
                        if (db.Violation.Any(v => v.CorrespondingEventID == i.remarkSerial) && !id.Contains(i.remarkSerial))
                            id.Add(db.Violation.Where(v => v.CorrespondingEventID == i.remarkSerial).FirstOrDefault().CorrespondingEventID);
                    }
                }
            }
            return id;
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
            ViewBag.Page = Page;
            return View(manage);
        }

        //依選擇日期範圍顯示已處理檢舉紀錄
        public ActionResult ShowByMonths(string type, int month)
        {
            var date = DateTime.Now.AddMonths(month);
            //List<Object> list = new List<object>();
            dynamic list = null;
            var violation = db.Violation.ToList();
            switch (type)
            {
                case "memList":
                    violation = violation.Where(m => m.CorrespondingEventID.StartsWith("M") && m.vioProcessTime != null && m.vioProcessTime >= date).ToList();
                    list = getViolations(violation);
                    break;
                case "remarkList":
                    violation = violation.Where(m => m.CorrespondingEventID.StartsWith("R") && m.vioProcessTime != null && m.vioProcessTime >= date).ToList();
                    list = getViolations(violation);
                    break;
                case "actList":
                    violation = violation.Where(m => m.CorrespondingEventID.StartsWith("A") && m.vioProcessTime != null && m.vioProcessTime >= date).ToList();
                    list = getViolations(violation);
                    break;
                case "commentList":
                    violation = violation.Where(m => m.CorrespondingEventID.StartsWith("B") && m.vioProcessTime != null && m.vioProcessTime >= date).ToList();
                    list = getViolations(violation);
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
            ViewBag.Page = Request["Page"];
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
                        punish.punishId = punishID;
                        punish.implement_admId = admID;
                        punish.vioProcessTime = DateTime.Now;
                        db.SaveChanges();
                        mail.SendEmail(mailList, "違規警告通知",
                                "<p> 親愛的Join Fun會員您好：</p><br/><p>　　因您已違反Join Fun網站規定，" +
                                "經查證後因違規情節輕微，本站依規定記警告一次，違規次數若超過三次將被停權，" +
                                "，敬請注意；如您有任何疑問，請與本站客服人員聯絡．感謝您對Join Fun的愛護與支持．</p><br /><br />" +
                                "<span>Join Fun全體人員敬上．</span>");
                        break;
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
                                "<p> 親愛的Join Fun會員您好：</p><br/><p>　　因您已違反Join Fun網站規定，本站依規定將此帳號"
                                + db.Punishment.Where(m => m.punishId == punishID).FirstOrDefault().punishName +
                                "；如有任何疑問，請與本站客服人員聯絡．感謝您對Join Fun的愛護與支持．</p><br /><br />" +
                                "<span>Join Fun全體人員敬上．</span>");
                        }
                        else
                        {
                            mail.SendEmail(mailList, "違規停權通知",
                                "<p> 親愛的Join Fun會員您好：</p><br/><p>　　因您已違反Join Fun網站規定，" +
                                "且違規次數已達3次，本站依規定將此帳號永久停權；如您有任何疑問，請與本站客服人員聯絡．感謝您對Join Fun的愛護與支持．</p><br /><br />" +
                                "<span>Join Fun全體人員敬上．</span>");
                        }
                        break;
                    case "pmt0000005":
                        punish.punishId = punishID;
                        punish.implement_admId = admID;
                        punish.vioProcessTime = DateTime.Now;
                        db.SaveChanges();
                        violateMem.Suspend = true;
                        db.SaveChanges();
                        mail.SendEmail(mailList, "違規停權通知",
                                "<p> 親愛的Join Fun會員您好：</p><br/><p>　　因您已違反Join Fun網站規定，" +
                                "本站依規定將此帳號永久停權；如您有任何疑問請與本站客服人員聯絡．感謝您對Join Fun的愛護與支持．</p><br /><br />" +
                                "<span>Join Fun全體人員敬上．</span>");
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
            ViewBag.Page = Page;
            return View(feedback);
        }

        public ActionResult FeedBackReply(string id)
        {
            Comment reply = db.Comment.Find(id);
            return View(reply);
        }

        [HttpPost]
        public ActionResult FeedBackReply(string id, string admId, string Page, string reportContent)
        {
            if (reportContent != null)
            {
                //db.sp_updateComment(id, content, admId, id, content);
                SqlParameter[] param = new SqlParameter[] {
                    new SqlParameter("@id", id),
                    new SqlParameter("@content", reportContent),
                    new SqlParameter("@admId", admId),
                    new SqlParameter("@instanceId", id),
                    new SqlParameter("@notiContent", reportContent)
                };
                db.Database.ExecuteSqlCommand("exec dbo.sp_updateComment @id, @content, @admId, @instanceId, @notiContent", param);
                db.SaveChanges();

                return RedirectToAction("FeedBack", new { Page = Page });
            }
            Comment reply = db.Comment.Find(id);
            ViewBag.Page = Page;
            return View(reply);
        }



        public ActionResult ActManage(int page = 1)
        {
            
            var actdetail = db.Join_Fun_Activities.OrderByDescending(m => m.actId).ToList();
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


            var actdetail = db.Join_Fun_Activities.OrderByDescending(m => m.actId).ToList();

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
        public IEnumerable getViolations(List<Violation> violation)
        {
            var list = (from v in violation
                        select new
                        {
                            id = v.vioId,
                            name = v.FromMemId != null ? (from m in db.Member where m.memId == v.FromMemId select m.memNick).FirstOrDefault() : (from a in db.Administrator where a.admId == v.FromAdmID select a.admNick).FirstOrDefault(),
                            type = v.CorrespondingEventID.StartsWith("M") ? "會員" : v.CorrespondingEventID.StartsWith("A") ? "揪團活動" : v.CorrespondingEventID.StartsWith("R") ? "會員評價" : "留言板",
                            typeId = v.CorrespondingEventID,
                            title = v.vioTitle,
                            vioTime = v.vioReportTime,
                            condition = v.vioProcessTime == null ? "未處理" : "已處理",
                            doneTime = v.vioProcessTime
                        }).ToList();
            return list.OrderByDescending(l => l.vioTime).ToList();
        }

        List<VioList> getViolations(List<Violation> violation, List<string> lid)
        {
            List<VioList> list = new List<VioList>();
            List<VioList> newList = new List<VioList>();
            foreach (var item in lid)
            {
                list = (from v in violation
                        where v.CorrespondingEventID == item
                        select new VioList
                        {
                            id = v.vioId,
                            name = v.FromMemId != null ? (from m in db.Member where m.memId == v.FromMemId select m.memNick).FirstOrDefault() : (from a in db.Administrator where a.admId == v.FromAdmID select a.admNick).FirstOrDefault(),
                            type = v.CorrespondingEventID.StartsWith("M") ? "會員" : v.CorrespondingEventID.StartsWith("A") ? "揪團活動" : v.CorrespondingEventID.StartsWith("R") ? "會員評價" : "留言板",
                            typeId = v.CorrespondingEventID,
                            title = v.vioTitle,
                            vioTime = v.vioReportTime,
                            condition = v.vioProcessTime == null ? "未處理" : "已處理",
                            doneTime = v.vioProcessTime
                        }

                 ).ToList();
                newList.AddRange(list);
            }
            return newList.OrderByDescending(n => n.vioTime).ToList();
        }

        class VioList
        {
            public string id { get; set; }
            public string name { get; set; }
            public string type { get; set; }
            public string typeId { get; set; }
            public string title { get; set; }
            public DateTime vioTime { get; set; }
            public string condition { get; set; }
            public Nullable<System.DateTime> doneTime { get; set; }
        }
    }
}