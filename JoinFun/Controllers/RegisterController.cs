using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JoinFun.Models;

namespace JoinFun.Controllers
{
    public class RegisterController : Controller
    {
        JoinFunEntities db = new JoinFunEntities();
        // GET: RegNew
        public ActionResult Index()
        {

            return View(db.Member.ToList());
        }
        //註冊會員
        public ActionResult Register()
        {


            //ViewBag.County = db.County.ToList();
            //ViewBag.District = db.District.ToList();

            return View();

        }
        [HttpPost]
        public ActionResult Register(Member mem, string account, string password)
        {
                 

            if (ModelState.IsValid)
            {
                string getmmId = db.Database.SqlQuery<string>("select [dbo].[GetMemId]()").FirstOrDefault();
                Acc_Pass acc = new Acc_Pass();
                acc.memId = getmmId;
                acc.Account = account;
                acc.Password = password;
                db.Acc_Pass.Add(acc);
                db.Member.Add(mem);
                db.SaveChanges();


                return RedirectToAction("Index");
            }
            ViewBag.Acc = account;
            ViewBag.Pwd = password;
            return View();
        }
        //修改密碼
        public ActionResult PwdEdit()
        {


            return View(db.Member.ToList());
        }
    }
}