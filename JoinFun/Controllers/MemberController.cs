using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
            Session["memid"] = "M000000003";
            //讀取特定會員相關資訊
            var mem = db.Member.Where(m => m.memId == memID).ToList();
            ViewBag.activities_count = db.Join_Fun_Activities.Where(m => m.hostId==memID).Count();
            ViewBag.mem_remarks_count = db.Member_Remarks.Where(m => m.ToMemId == memID).Count();
            return View(mem);
        }
        public ActionResult Edit(string memID) {
            //Session["memid"] = "M000000001";
            //讀取個人資訊
            var mem =db.Member.Where(m => m.memId == memID).FirstOrDefault();
            //下拉式選單用
            //ViewBag.county_drop = new SelectList(db.County, "CountyNo", "CountyName");
            //ViewBag.district_drop = new SelectList(db.District, "DistrictSerial", "DistrictName");
            ViewBag.county_drop = db.County.ToList();
            ViewBag.district_drop = db.District.ToList();
            return View(mem);
        }
        [HttpPost]
        public ActionResult Edit(string memId,DateTime Birthday, string Sex, string memNick, string Email, int memCounty, int memDistrict, string Introduction, string Habit, string Dietary_Preference)
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
            return RedirectToAction("Info", new { memID= memId });
        }
    }
}