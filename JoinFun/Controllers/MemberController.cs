using System;
using System.Collections.Generic;
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
            
            Session["memid"] = "M000000001";
            //讀取特定會員相關資訊
            var mem = db.Member.Where(m => m.memId == memID).ToList();
            ViewBag.activities_count = db.Join_Fun_Activities.Where(m => m.hostId==memID).Count();
            ViewBag.mem_remarks_count = db.Member_Remarks.Where(m => m.ToMemId == memID).Count();
            return View(mem);
        }
        public ActionResult Edit(string memID = "M000000001") {
            Session["memid"] = "M000000002";
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
        public ActionResult Edit(string memId,Member member)
        {
            return View();
        }
    }
}