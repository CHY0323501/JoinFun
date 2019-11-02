using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public ActionResult Info(string memID="M000000001")
        {
            var member = db.Member.Where(m => m.memId == memID).FirstOrDefault();
            //當會員編號不存在時執行
            if (memID == null|| member==null)
            {
                return RedirectToAction("Index", "Activity");
            }
            else {
                Session["memid"] = "M000000003";
                MemberViewModel Minfo = new MemberViewModel()
                {
                    Member = db.Member.Where(m => m.memId == memID).ToList(),
                    Acc_Pass= db.Acc_Pass.Where(m => m.memId == memID).ToList(),
                    Blacklist= db.Blacklist.Where(m => m.memId == memID).ToList(),
                    Bookmark_Details= db.Bookmark_Details.Where(m => m.memId == memID).ToList(),
                    Fans= db.Fans.Where(m => m.memId == memID).ToList(),
                    FollowUp= db.FollowUp.Where(m => m.FoMemId == memID).ToList(),
                    Friendship= db.Friendship.Where(m => m.memId == memID).ToList(),
                    Member_Remarks= db.Member_Remarks.Where(m => m.ToMemId == memID).ToList(),
                    Join_Fun_Activities = db.Join_Fun_Activities.Where(m => m.hostId == memID).ToList()
                };


                //讀取特定會員相關資訊
                //var mem = db.Member.Where(m => m.memId == memID).ToList();
                //viewbag.activities_count = db.join_fun_activities.where(m => m.hostid == memid).count();
                //ViewBag.mem_remarks_count = db.Member_Remarks.Where(m => m.ToMemId == memID).Count();
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
                if (Session["memid"].ToString() == memID)
                {
                    //讀取個人資訊
                    var mem = db.Member.Where(m => m.memId == memID).FirstOrDefault();
                    //下拉式選單用
                    //ViewBag.county_drop = new SelectList(db.County, "CountyNo", "CountyName");
                    //ViewBag.district_drop = new SelectList(db.District, "DistrictSerial", "DistrictName");
                    ViewBag.county_drop = db.County.ToList();
                    ViewBag.district_drop = db.District.ToList();
                    return View(mem);
                }
                //未登入時無法編輯並轉至首頁
                return RedirectToAction("Index", "Activity");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string memId, DateTime Birthday, string Sex, string memNick, string Email, int memCounty, int memDistrict, string Introduction, string Habit, string Dietary_Preference)
        {
            if (ModelState.IsValid)
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
    }
}