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
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string account, string pass)
        {
            //使用Linq查詢取得帳號(加密用)
            var getAcc = db.Acc_Pass.Where(m => m.Account == account).FirstOrDefault();
            if (getAcc == null)
            {
                ViewBag.LoginERR = "您輸入的帳號或密碼錯誤";
                return View();
            }
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
                
                Session["memid"] = reader["memId"].ToString();
               
                
                Conn.Close();
                
                return RedirectToAction("Index","Activity");
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

    }
}
