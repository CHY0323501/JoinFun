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
        public ActionResult Login(string acc, string pwd)
        {

            string sql = "select * from tStudent where Account=@Account and Password=@Password";
            SqlCommand cmd = new SqlCommand(sql, Conn);
            cmd.Parameters.AddWithValue("@Account", acc);
            cmd.Parameters.AddWithValue("@Password", pwd);
            SqlDataReader rd;

            Conn.Open();
            rd = cmd.ExecuteReader();


            if (rd.Read())
            {
                Session["acc"] = rd["Account"].ToString();


                Conn.Close();
                return RedirectToAction("Home");
            }


            Conn.Close();
            ViewBag.LoginErr = "帳號或密碼有誤!!";
            return View();

        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}