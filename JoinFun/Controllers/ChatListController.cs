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
            Session["memid"] = "M000000002";
            //找出每位會員的最新一筆聊天記錄
            var chatList = (from a in db.Chat_Records
                            where a.Time == (from r in db.Chat_Records
                                             where a.ChatRoom == r.ChatRoom
                                             select r.Time).Max()
                                             && a.ToMemId == memID ||a.FromMemId==memID
                            select a).ToList();

            List<int> ReadYetCount = new List<int>();
            foreach (var c in chatList.OrderByDescending(m=>m.Time))
            {
                int count = db.Chat_Records.Where(m => m.ChatRoom == c.ChatRoom&&m.ReadYet==false).Count();
                ReadYetCount.Add(count);
            }
            ViewBag.ReadYetCount = ReadYetCount;
            return View(chatList);
        }
        public ActionResult Chat(string fromMemID) {
            string session = Session["memid"].ToString();
            var chat = db.Chat_Records.Where(m=>(m.ToMemId== session && m.FromMemId==fromMemID)||m.FromMemId== session && m.ToMemId==fromMemID);

            return View();
        }
    }
}