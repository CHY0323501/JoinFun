using JoinFun.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JoinFun.Controllers
{
    public class LoginController : Controller
    {
        SqlConnection Conn = new SqlConnection("data source = MCSDD108212; initial catalog = JoinFun; integrated security = True; MultipleActiveResultSets=True;App=EntityFramework&quot;");
        JoinFunEntities db = new JoinFunEntities();
        // GET: Login
        //假想首頁
        public ActionResult Home()
        {
            return View();
        }
        //登入
        public ActionResult Login(int? c)
        {

            if (Session["memid"] != null) {
                if (String.IsNullOrEmpty(Session["memid"].ToString()))
                    return View();
                if (c == null)
                    return View();
            }
                
            
                return RedirectToAction("Index", "Activity");

        }

        [HttpPost]
        public ActionResult Login(string account, string pass)
        {


            //使用Linq查詢取得帳號(加密)
            var getAcc = db.Acc_Pass.Where(m => m.Account == account).FirstOrDefault();
            
            

            if (getAcc == null)
            {
                ViewBag.LoginERR = "您輸入的帳號或密碼錯誤";
                return View();
            }
            var a = db.Member.Find(getAcc.memId);


            //查詢會員帳號及密碼
            string sql = "select * from Acc_Pass where Account=@acc and Password=@pass";
            SqlCommand cmd = new SqlCommand(sql, Conn);
            //取得salt字串
            string salt = getAcc.Salt;
            //產生雜湊
            byte[] PasswordAndSaltBytes = System.Text.Encoding.UTF8.GetBytes(pass + salt);
            byte[] HashBytes = new System.Security.Cryptography.SHA256Managed().ComputeHash(PasswordAndSaltBytes);
            string HashString = Convert.ToBase64String(HashBytes);
            cmd.Parameters.AddWithValue("@acc", account);
            cmd.Parameters.AddWithValue("@pass", HashString);
            SqlDataReader reader;


            Conn.Open();
            reader = cmd.ExecuteReader();



            if (reader.Read())
            {
                if (a.Approved == true)
                {

                    Session["memid"] = reader["memId"].ToString();
                    Session["nick"] = a.memNick;
                    

                    return RedirectToAction("Index", "Activity");
                }
                else
                {
                    ViewBag.LoginERR = "帳號未啟用";
                    return View();
                }

            }

            Conn.Close();
            ViewBag.LoginERR = "您輸入的帳號或密碼錯誤";

            return View();

        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Activity");
        }
        
        public ActionResult FbLogin(string ID,string email) {
            var Smember=db.Social_Net_ID.Where(m => m.socialId == ID).FirstOrDefault();
            
            if (Smember == null) {
                
                //Session["SocialID"] = ID;
                //Session["email"] = email;
                return RedirectToAction("FBRegister",new { SocialID = ID , email = email });
            }
            else {
                var mem = db.Member.Where(m => m.memId == Smember.memId).FirstOrDefault();
                if (mem.Suspend == false)
                {

                    Session["memid"] = Smember.memId;
                    Session["nick"] = mem.memNick;


                    return RedirectToAction("Index", "Activity");
                }
            }
            return RedirectToAction("Login",new { c=1});
        }
        public ActionResult FBRegister(string SocialID,string email)
        {

            ViewBag.County = db.County.ToList();
            ViewBag.District = db.District.ToList();
            ViewBag.SocialID = SocialID;
            ViewBag.email = email;

            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FBRegister(Member mem, string Introduction, string Habit, string Dietary_Preference,string SocialID)
        {
            string getmmId = db.Database.SqlQuery<string>("select [dbo].[GetMemId]()").FirstOrDefault();

            mem.memCounty = Int16.Parse(Request["memCounty"]);
            mem.memDistrict = Int16.Parse(Request["memDistrict"]);


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
            db.Member.Add(mem);
            db.SaveChanges();

            Social_Net_ID social = new Social_Net_ID();
            string getSoId = db.Database.SqlQuery<string>("select [dbo].[GetSocialId]()").FirstOrDefault();
            social.socialSerial = getSoId;
            social.memId = getmmId;
            social.socialId = SocialID;
            db.Social_Net_ID.Add(social);
            db.SaveChanges();

            return RedirectToAction("FbLogin",new { ID= SocialID,email=mem.Email });

        }
    }
}
