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
            

            //密碼雜湊 salt+hash
            string salt = Guid.NewGuid().ToString();
            byte[] passwordAndSaltBytes = System.Text.Encoding.UTF8.GetBytes(password + salt);
            byte[] hashBytes = new System.Security.Cryptography.SHA256Managed().ComputeHash(passwordAndSaltBytes);
            string hashString = Convert.ToBase64String(hashBytes);
            


                string getmmId = db.Database.SqlQuery<string>("select [dbo].[GetMemId]()").FirstOrDefault();
                Acc_Pass acc = new Acc_Pass();
                acc.memId = getmmId;
                acc.Account = account;
                acc.Password = hashString;
                mem.memCounty = Int16.Parse(Request["memCounty"]);
                mem.memDistrict = Int16.Parse(Request["memDistrict"]);

            //型別vu/6w94轉換(string->char(1))
            string gender = Request["Sex"];
            if (gender == "男")
                gender = "M";
            else
                gender = "F";

            //mem.Birthday = DateTime.Parse(Request["Birthday"]).ToString();           
            //mem.Birthday = Convert.ToString(mem.Birthday.ToDateTime("yyyy/MM/dd"));

                mem.Sex = gender;
                mem.timeReg = DateTime.Now;
                mem.memId = getmmId;
                acc.Salt = salt;
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