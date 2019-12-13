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


            mem.email_ID = Guid.NewGuid().ToString("N");


            //型別轉換(string->char(1))
            string gender = Request["Sex"];
            if (gender == "男")
                gender = "M";
            else
                gender = "F";


            mem.Introduction = Introduction;
            mem.Habit = Habit;
            mem.Dietary_Preference = Dietary_Preference;
            mem.Sex = gender;
            mem.timeReg = DateTime.Now;
            mem.memId = getmmId;
            acc.Salt = salt;
            db.Acc_Pass.Add(acc);
            db.Member.Add(mem);
            db.SaveChanges();



            MessageCenter mes = new MessageCenter();
            List<string> mailList = new List<string>() { mem.Email };
            mes.SendEmail(mailList, "JoinFun驗證信通知", "<img src='https://i.ibb.co/dcBqtJk/img.png' > <h3>親愛的" + acc.Account + "會員:</h3></br><h3>您在JoinFun的帳號已建立,請點擊下方連結以完成帳號啟用!</h3></br><a href='http://localhost:54129/Register/Approved?email_ID=" + mem.email_ID + "'>信箱驗證請連結</a></br>");


            return RedirectToAction("CheckEmail", "Register", new { account = account });

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
        public ActionResult PwdEdit(string memId )
        {
            if (Session["memid"].ToString() == memId)
            {
                var accPwd = db.Acc_Pass.Where(m => m.memId == memId).FirstOrDefault();
                //var accAccount = db.Acc_Pass.Where(m => m.Account == Account).FirstOrDefault().Account;


                //ViewBag.account = (from a in db.Acc_Pass
                //                   where a.memId == memId
                //                   select a.Account).FirstOrDefault();

                var account = (from a in db.Acc_Pass
                               where a.memId == memId
                               select a.Account).FirstOrDefault();


                Session["Account"] = account;
                return View();
            }
            return RedirectToAction("Login", "Login");

        }

        [HttpPost]
        public ActionResult PwdEdit(string memId, string OldPassword, string Password)
        {

            var accPwd = db.Acc_Pass.Where(m => m.memId == memId).FirstOrDefault();
            //var accPwd = db.Acc_Pass.Find();

            var oldsalt = db.Acc_Pass.Where(m => m.memId == memId).FirstOrDefault().Salt;
            //var oldsalt = db.Acc_Pass.Find();
            //string old = oldsalt.Salt;

            //ViewBag.account = (from a in db.Acc_Pass
            //                   where a.memId == memId
            //                   select a.Account).FirstOrDefault();


            byte[] PasswordAndSaltBytes = System.Text.Encoding.UTF8.GetBytes(OldPassword + oldsalt);
            byte[] HashBytes = new System.Security.Cryptography.SHA256Managed().ComputeHash(PasswordAndSaltBytes);
            string HashString = Convert.ToBase64String(HashBytes);

            if (accPwd.Password != null)
            {
               if (accPwd.Password == HashString)
               {

                //密碼雜湊 salt+hash
                string salt = Guid.NewGuid().ToString();
                byte[] passwordAndSaltBytes = System.Text.Encoding.UTF8.GetBytes(Password + salt);
                byte[] hashBytes = new System.Security.Cryptography.SHA256Managed().ComputeHash(passwordAndSaltBytes);
                string hashString = Convert.ToBase64String(hashBytes);

                   
                accPwd.Salt = salt;
                accPwd.memId = memId;
                accPwd.Password = hashString;
                accPwd.PasswordConfirm = hashString;

                db.SaveChanges();

                    return Content("<script>alert('修改成功');window.location='/Register/Index';</script>");
                    //return RedirectToAction("Index", "Activity");


               }
               else
               {
                ViewBag.PwdEditErr = "舊密碼未填或有誤!";
                return View();
               }
               
            }
            return View();
        }

        //忘記密碼(填信箱頁面)
        public ActionResult ForgetPwd()
        {

            return View();
        }
        [HttpPost]
        public ActionResult ForgetPwd(string account, string email)
        {

            var acc = db.Acc_Pass.Where(m => m.Account == account).FirstOrDefault();

            var getmemid = db.Acc_Pass.Where(m => m.Account == account).FirstOrDefault();
            if (getmemid != null) {
                string a = getmemid.memId;
                var memEmail = db.Member.Where(m => m.memId == a).FirstOrDefault().Email;
            }
            
            if (acc != null)
            {
                if (acc.Account == account && db.Member.Where(m => m.memId == db.Acc_Pass.Where(c => c.Account == account).FirstOrDefault().memId).FirstOrDefault().Email == email)
                {

                    string getEmailID = db.Member.Where(M=>M.memId== db.Acc_Pass.Where(a => a.Account == account).FirstOrDefault().memId).FirstOrDefault().email_ID;

                    MessageCenter mes = new MessageCenter();
                    List<string> mailList = new List<string>() { db.Member.Where(m => m.memId == db.Acc_Pass.Where(b => b.Account == account).FirstOrDefault().memId).FirstOrDefault().Email };
                    mes.SendEmail(mailList, "JoinFun權益通知", "<img src='https://i.ibb.co/dcBqtJk/img.png' style='width:25 %'><h2>親愛的會員:</h2></br><h2>請點擊下面連結來重置您的密碼!</h2></br><a href='http://localhost:54129/Register/RemakePwd?email_ID=" + getEmailID + "'><h2>重置密碼請點擊</h2></a></br>");

                    ViewBag.ForgetPwd = "信件已發送";
                    return View();

                }



            }
            else
            {
                ViewBag.ForgetPwdErr = "您輸入的帳號或信箱錯誤";
                return View();
            }

            return View();





        }
        //重置密碼
        public ActionResult RemakePwd(string email_ID)
        {
            ViewBag.emailid = email_ID;
            var mem = db.Member.Where(m => m.email_ID == email_ID).FirstOrDefault();

            string e = mem.email_ID;
            if (email_ID == e)
                return View();
            return RedirectToAction("Login", "Login");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemakePwd(string email_ID, string Password)
        {
            var memid = db.Member.Where(m => m.email_ID == email_ID).FirstOrDefault().memId;

            var accPwd = db.Acc_Pass.Where(m => m.memId == memid).FirstOrDefault();


            string salt = Guid.NewGuid().ToString();
            byte[] passwordAndSaltBytes = System.Text.Encoding.UTF8.GetBytes(Password + salt);
            byte[] hashBytes = new System.Security.Cryptography.SHA256Managed().ComputeHash(passwordAndSaltBytes);
            string hashString = Convert.ToBase64String(hashBytes);

            accPwd.Salt = salt;
            accPwd.Password = hashString;
            accPwd.PasswordConfirm = hashString;
            db.SaveChanges();
            return Content("<script>alert('修改成功');window.location='/Login/Login';</script>");
            //return RedirectToAction("Login", "Login");
        }




        
    }
}