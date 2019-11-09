using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
//using model
using JoinFun.Models;
//using viewmodel
using JoinFun.ViewModel;

namespace JoinFun.Controllers
{
    public class MemberController : Controller
    {
        JoinFunEntities db = new JoinFunEntities();
        SqlConnection Conn = new SqlConnection("data source = MCSDD108212; initial catalog = JoinFun; integrated security = True; MultipleActiveResultSets=True;App=EntityFramework&quot;");
        SqlCommand cmd = new SqlCommand();

        public ActionResult Info(string memID="M000000003")
        {
            var member = db.Member.Where(m => m.memId == memID).FirstOrDefault();
            //當會員編號不存在時執行
            if (memID == null|| member==null)
            {
                return RedirectToAction("Index", "Activity");
            }
            else {
                Session["memid"] = "M000000002";
                MemberViewModel Minfo = new MemberViewModel()
                {
                    Member = db.Member.Where(m => m.memId == memID).ToList(),
                    Bookmark_Details= db.Bookmark_Details.Where(m => m.memId == memID).ToList(),
                    Friendship = db.Friendship.Where(m => m.friendMemId == memID).ToList(),
                    vw_FansNew = db.vw_FansNew.Where(m => m.memId == memID).ToList(),
                    vw_FollowUp = db.vw_FollowUp.Where(m => m.FoMemId == memID).ToList(),
                    vw_FriendShip = db.vw_FriendShip.Where(m => m.memId == memID).ToList(),
                    vw_Member_Remarks = db.vw_Member_Remarks.Where(m => m.ToMemId == memID).ToList(),
                    vw_HostHistory = db.vw_HostHistory.Where(m => m.hostId == memID).ToList(),
                    vw_PartHistory = db.vw_PartHistory.Where(m => m.memId == memID).ToList()
                    
                };
                return View(Minfo);
            }
        }
        public ActionResult Edit(string memID) {
            var member = db.Member.Where(m => m.memId == memID).FirstOrDefault();
            if (memID == null || member == null)
            {
                return RedirectToAction("Index", "Activity");
            }
            else {
                try
                {
                    if ( Session["memid"].ToString() == memID)
                    {
                            //下拉式選單用
                            //ViewBag.county_drop = new SelectList(db.County, "CountyNo", "CountyName");
                            //ViewBag.district_drop = new SelectList(db.District, "DistrictSerial", "DistrictName");
                            ViewBag.county_drop = db.County.ToList();
                            ViewBag.district_drop = db.District.ToList();
                            return View(member);

                    }
                    //未登入時無法編輯並轉至首頁
                    return RedirectToAction("Index", "Activity");
                }
                catch {
                    return RedirectToAction("Index", "Activity");
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string memId, DateTime Birthday, string Sex, string memNick, string Email, int memCounty, int memDistrict, string Introduction, string Habit, string Dietary_Preference)
        {
            
            if (ModelState.IsValid&&Session["memid"]!=null)
            {
                var mem = db.Member.Find(memId);
                mem.Sex = Sex;
                mem.memNick = memNick;
                mem.Email = Email;
                mem.memCounty = (Int16)memCounty;
                mem.memDistrict = (Int16)memDistrict;
                mem.Birthday = Birthday;
                mem.Introduction = Introduction;
                mem.Habit = Habit;
                mem.Dietary_Preference = Dietary_Preference;
                db.SaveChanges();
            }
            //編輯完畢時回到個人資訊頁
            return RedirectToAction("Info", new { memID= Session["memid"] });
        }

       



        public ActionResult Remarks(string memID) {
            var member = db.Member.Where(m => m.memId == memID).FirstOrDefault();
            if (memID == null || member == null)
            {
                return RedirectToAction("Index", "Activity");
            }
            else {
                MemRemarkViewModel MRemark = new MemRemarkViewModel()
                {
                    //判斷是否為主辦人評價
                    vw_Host_Remarks = db.vw_Host_Remarks.Where(m => m.ToMemId == memID).ToList(),
                    //判斷是否為參與者評價
                    vw_Participant_Remarks = db.vw_Participant_Remarks.Where(m => m.ToMemId == memID).ToList()
                };

                //查詢評價對象暱稱
                ViewBag.ToMemNick = (from m in db.Member
                                     where m.memId == memID
                                     select m.memNick).FirstOrDefault();

                return View(MRemark);
            }
        }
        public ActionResult RemarkCreate()
        {
            

            return View();

        }
        [HttpPost]
        public ActionResult RemarkCreate(string remarkContent ,short remarkStar,DateTime remarkTime)
        {
            Member_Remarks aaa = new Member_Remarks();

            aaa.remarkContent = remarkContent;
            aaa.remarkStar = remarkStar;
            aaa.remarkTime = remarkTime;

            db.Member_Remarks.Add(aaa);
            db.SaveChanges();

            return RedirectToAction("Index");

        }
        //揪團歷史
        public ActionResult History(string memID) {
            var member = db.Member.Where(m => m.memId == memID).FirstOrDefault();
            if (memID == null || member == null)
            {
                return RedirectToAction("Index", "Activity");
            }
            else
            {
                HistoryViewModel History = new HistoryViewModel()
                {
                    vw_HostHistory = db.vw_HostHistory.Where(m => m.hostId == memID).ToList(),
                    vw_PartHistory = db.vw_PartHistory.Where(m => m.memId == memID).ToList(),
                    Photos_of_Activities = db.Photos_of_Activities.ToList(),
                    Activity_Class= db.Activity_Class.ToList()
                };

                ViewBag.ToMemNick = (from m in db.Member
                                     where m.memId == memID
                                     select m.memNick).FirstOrDefault();
                return View(History);
            }
        }

        public ActionResult FriendManagement(string memID) {
            //var friend = (from f in db.Friendship
            //             join m in db.Member on f.friendMemId equals m.memId
            //             where f.memId == memID
            //             select new { f.memId, f.friendMemId, f.friendNick, m.memNick, f.Approved, m.Sex }).ToList();

            FriendManagementVW friend = new FriendManagementVW()
            {
                Friendship=db.Friendship.Where(m=>m.memId== memID).ToList(),
                vw_FriendShip = db.vw_FriendShip.Where(m=>m.memId == memID).ToList()
            };

            return View(friend);
        }


        //取得活動照片用，若原活動無上傳照片時，抓取對應的類別圖片
        public FileContentResult GetActPhoto(string actId, string actClassId)
        {
            string photo = db.Photos_of_Activities.Where(m => m.actId == actId).FirstOrDefault().actPics;
            string AclassPhoto = db.Activity_Class.Where(m => m.actClassId == actClassId).FirstOrDefault().Photos;
            if (photo != null)
            {
                //將圖片轉成byte[] 傳給View
                string path = Server.MapPath(photo);
                byte[] image = System.IO.File.ReadAllBytes(path);
                return base.File(image, "image/jpeg");
            }
            string CLASSpath = Server.MapPath(AclassPhoto);
            byte[] CLASSimage = System.IO.File.ReadAllBytes(CLASSpath);
            return base.File(CLASSimage, "image/jpeg");
        }
    }
}