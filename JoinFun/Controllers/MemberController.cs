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
            //讀取特定會員相關資訊
            Session["memid"] = memID;
            var mem = db.Member.Where(m => m.memId == memID).ToList();
            ViewBag.activities_count = db.Join_Fun_Activities.Where(m => m.hostId==memID).Count();
            ViewBag.mem_remarks_count = db.Member_Remarks.Where(m => m.ToMemId == memID).Count();
            ViewBag.member_habit = db.Habit.Where(m => m.memId == memID).ToList();
            ViewBag.member_food = db.Dietary_Preference.Where(m => m.memId == memID).ToList();
            return View(mem);
        }
    }
}