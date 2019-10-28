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
            MemberViewModel mem = new MemberViewModel()
            {
                Member = db.Member.Where(m => m.memId == memID).ToList(),
                Acc_Pass = db.Acc_Pass.Where(m => m.memId == memID).ToList(),
                Blacklist = db.Blacklist.Where(m => m.memId == memID).ToList(),
                Bookmark_Details = db.Bookmark_Details.Where(m => m.memId == memID).ToList(),
                Dietary_Preference=db.Dietary_Preference.Where(m => m.memId == memID).ToList(),
                Fans=db.Fans.Where(m => m.memId == memID).ToList(),
                FollowUp=db.FollowUp.Where(m => m.memId == memID).ToList(),
                Friendship=db.Friendship.Where(m => m.memId == memID).ToList(),
                Habit=db.Habit.Where(m => m.memId == memID).ToList(),
                Member_Remarks=db.Member_Remarks.Where(m => m.FromMemId == memID).ToList()
            };
            return View(mem);
        }
    }
}