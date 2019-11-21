﻿using System;
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
           
            ViewBag.County = db.County.ToList();
            ViewBag.District = db.District.ToList();
         
            return View();

        }
      
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
           
            Acc_Pass acc = new Acc_Pass();
            acc.memId = getmmId;
            acc.Account = account;
            acc.Password = hashString;
            acc.PasswordConfirm = hashString;
            mem.memCounty = Int16.Parse(Request["memCounty"]);
            mem.memDistrict = Int16.Parse(Request["memDistrict"]);

            
            mem.email_ID=Guid.NewGuid().ToString("N");


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
            mes.SendEmail(mailList, "JoinFun權益通知", "<h3>親愛的" + acc.Account + "會員:</h3></br><h3>您在JoinFun的帳號已建立,請點擊下面信箱驗證請連結完成啟用操作!</h3></br><a href='http://localhost:54129/Register/Approved?email_ID=" + mem.email_ID + "'>信箱驗證請連結</a></br>");


            return RedirectToAction("CheckEmail", "Register",new { account = account });

        }
        //註冊完提醒確認密碼
        public ActionResult CheckEmail(string account)
        {
            var acc = db.Acc_Pass.Where(m => m.Account == account).FirstOrDefault();
            string getacc = acc.Account;
            ViewBag.account = getacc;

            return View();
        }
        //帳號啟用
        public ActionResult Approved(string email_ID)
        {
            var approved = db.Member.Where(m => m.email_ID == email_ID).FirstOrDefault();
            string x = approved.email_ID;
            if (email_ID == x)
            {

                if (approved.Approved == false)
                {
                    approved.Approved = true;
                    db.SaveChanges();
                }


            }
            return View();

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
            var oldsalt = db.Acc_Pass.Where(m => m.memId == memId).FirstOrDefault().Salt;

            //ViewBag.account = (from a in db.Acc_Pass
            //                   where a.memId == memId
            //                   select a.Account).FirstOrDefault();

            byte[] PasswordAndSaltBytes = System.Text.Encoding.UTF8.GetBytes(OldPassword + oldsalt);
            byte[] HashBytes = new System.Security.Cryptography.SHA256Managed().ComputeHash(PasswordAndSaltBytes);
            string HashString = Convert.ToBase64String(HashBytes);


            if (accPwd.Password == HashString)
            {

                //密碼雜湊 salt+hash
                string salt = Guid.NewGuid().ToString();
                byte[] passwordAndSaltBytes = System.Text.Encoding.UTF8.GetBytes(NewPassword + salt);
                byte[] hashBytes = new System.Security.Cryptography.SHA256Managed().ComputeHash(passwordAndSaltBytes);
                string hashString = Convert.ToBase64String(hashBytes);

                accPwd.Salt = salt;
                accPwd.memId = memId;
                accPwd.Password = hashString;
                accPwd.PasswordConfirm = hashString;
                db.SaveChanges();

                return RedirectToAction("Index", "Activity");


            }
               
                return View();
        }
      








    }
}