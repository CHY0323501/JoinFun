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
            //ViewBag.County = new SelectList(db.County, "CountyNo", "CountyName");
            //ViewBag.District = new SelectList(db.County, "DistrictSerial", "DistrictName");
            ViewBag.County = db.County.ToList();
            ViewBag.District = db.District.ToList();


            //SelectList selectList = new SelectList(this.GetCounty(), "CountyNo", "CountyName");
            //SelectList selectList2 = new SelectList(this.GetDistrict(), "DistrictSerial", "DistrictName");
            //ViewBag.SelectList = selectList;
            //ViewBag.SelectList2 = selectList2;
            return View();

        }

        //private IEnumerable<County> GetCounty()
        //{
        //    using (JoinFunEntities db = new JoinFunEntities())
        //    {
        //        var query = db.County.OrderBy(c => c.CountyNo);
        //        return query.ToList();
        //    }
        //}

        //private IEnumerable<District> GetDistrict()
        //{
        //    using (JoinFunEntities db = new JoinFunEntities())
        //    {
        //        var query = db.District.OrderBy(c => c.CountyNo);
        //        return query.ToList();
        //    }
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Member mem, string account, string password)
        {
            //string nick,string Email,int county,int district
            //Member mem = new Member();
            //, FormCollection form


            string salt = Guid.NewGuid().ToString();

            string getmmId = db.Database.SqlQuery<string>("select [dbo].[GetMemId]()").FirstOrDefault();
                Acc_Pass acc = new Acc_Pass();
                acc.memId = getmmId;
                acc.Account = account;
                acc.Password = password;
                mem.memCounty = Int16.Parse(Request["memCounty"]);
                mem.memDistrict = Int16.Parse(Request["memDistrict"]);
                mem.memId = getmmId;
                db.Acc_Pass.Add(acc);
                db.Member.Add(mem);
                db.SaveChanges();


                return RedirectToAction("Index");
        //ViewBag.Acc = account;
        //    ViewBag.Pwd = password;
        //    return View();
        }
        //修改密碼
        public ActionResult PwdEdit()
        {


            return View(db.Member.ToList());
        }









    }
}