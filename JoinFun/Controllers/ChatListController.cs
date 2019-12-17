using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
            ////把memid參數拿掉，換成session


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
        public ActionResult Chat(string fromMemID= "M000000001") {
            Session["memid"] = "M000000002";
            string session = Session["memid"].ToString();
            //找出聊天室房號
            int roomID = (int)(db.Chat_Records.Where(m => (m.ToMemId == session && m.FromMemId == fromMemID) || m.FromMemId == session && m.ToMemId == fromMemID).FirstOrDefault().ChatRoom);

            //已讀所有未讀訊息
            string UpdateString = "Update Chat_Records set ReadYet=1 where chatroom=@chatroom";
            db.Database.ExecuteSqlCommand(UpdateString, new SqlParameter("@chatroom", roomID));


            //取得所有歷史記錄
            var chat = db.Chat_Records.Where(m => m.ChatRoom == roomID).OrderByDescending(m=>m.Time).ToList().OrderBy(m=>m.Time);


            ViewBag.Nick = db.Member.Find(fromMemID).memNick;

            return View(chat);
        }
    }
}