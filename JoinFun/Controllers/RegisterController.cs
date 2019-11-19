using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JoinFun.Models;
using JoinFun.Utilities;

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
        public ActionResult Register(Member mem, string account, string password, string Introduction, string Habit, string Dietary_Preference)
        {
            

            //密碼雜湊 salt+hash
            string salt = Guid.NewGuid().ToString();
            byte[] passwordAndSaltBytes = System.Text.Encoding.UTF8.GetBytes(password + salt);
            byte[] hashBytes = new System.Security.Cryptography.SHA256Managed().ComputeHash(passwordAndSaltBytes);
            string hashString = Convert.ToBase64String(hashBytes);



            string getmmId = db.Database.SqlQuery<string>("select [dbo].[GetMemId]()").FirstOrDefault();
            //Guid emailId = new Guid();
            //emailId = db.Database.SqlQuery<string>("select [dbo].[newid]()").FirstOrDefault();
            Acc_Pass acc = new Acc_Pass();
            acc.memId = getmmId;
            acc.Account = account;
            acc.Password = hashString;
            acc.PasswordConfirm = hashString;
            mem.memCounty = Int16.Parse(Request["memCounty"]);
            mem.memDistrict = Int16.Parse(Request["memDistrict"]);
            //mem.email_ID = emailId;


            //型別轉換(string->char(1))
            string gender = Request["Sex"];
            if (gender == "男")
                gender = "M";
            else
                gender = "F";

           
            mem.Introduction = Introduction;
            mem.Habit =Habit;
            mem.Dietary_Preference =Dietary_Preference;
            mem.Sex = gender;
            mem.timeReg = DateTime.Now;
            mem.memId = getmmId;
            acc.Salt = salt;
            db.Acc_Pass.Add(acc);
            db.Member.Add(mem);
            db.SaveChanges();



            MessageCenter mes = new MessageCenter();
            List<string> mailList = new List<string>() { "ych4101861@gmail.com" };
            //mes.SendEmail(mailList, "JoinFun權益通知", "<a href='http://localhost:54129/Register/Approved?email_ID=" + mem.email_ID + "'>link</a></br>");

            


            return RedirectToAction("Index");
           
        }

        //修改密碼
        public ActionResult PwdEdit(string memId)
        {
            
            var accPwd = db.Acc_Pass.Where(m => m.memId == memId).FirstOrDefault();
            ViewBag.account= (from a in db.Acc_Pass
                             where a.memId == memId
                             select a.Account).FirstOrDefault();
            return View( accPwd);

        }

        [HttpPost] 
        public ActionResult PwdEdit(string memId, string OldPassword, string NewPassword)
        {
            var accPwd = db.Acc_Pass.Where(m => m.memId == memId).FirstOrDefault();
            ViewBag.account = (from a in db.Acc_Pass
                               where a.memId == memId
                               select a.Account).FirstOrDefault();




            if (accPwd.Password == OldPassword)
            {


                //密碼雜湊 salt+hash
                string salt = Guid.NewGuid().ToString();
                byte[] passwordAndSaltBytes = System.Text.Encoding.UTF8.GetBytes(NewPassword + salt);
                byte[] hashBytes = new System.Security.Cryptography.SHA256Managed().ComputeHash(passwordAndSaltBytes);
                string hashString = Convert.ToBase64String(hashBytes);

                accPwd.memId = memId;
                accPwd.Password = hashString;
                accPwd.PasswordConfirm = hashString;
                db.SaveChanges();


               

                return RedirectToAction("Index");
               

            }
               
                return View();
           

           

        }
        public ActionResult Approved()
        {
            //var accPwd = db.Member.Where(m => m.email_ID = email_ID).FirstOrDefault();

            return View();

        }








    }
}