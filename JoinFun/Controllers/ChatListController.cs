using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JoinFun.Models;

namespace JoinFun.Controllers
{
    public class ChatListController : Controller
    {
        JoinFunEntities db = new JoinFunEntities();
        [LoginRule(isVisiter = true, Front = true)]
        public ActionResult ChatHistory(string memID="M000000002")
        {

            //找出每位會員的最新一筆聊天記錄
            var chatList = (from a in db.Chat_Records
                            where a.Time == (from r in db.Chat_Records
                                             where a.FromMemId == r.FromMemId
                                             select r.Time).Max()
                                             && a.ToMemId == memID ||a.FromMemId==memID
                            select a).ToList();


            return View(chatList);
        }
        public ActionResult Chat(string fromMemID) {
            string session = Session["memid"].ToString();
            var chat = db.Chat_Records.Where(m=>(m.ToMemId== session && m.FromMemId==fromMemID)||m.FromMemId== session && m.ToMemId==fromMemID);

            return View();
        }
    }
}