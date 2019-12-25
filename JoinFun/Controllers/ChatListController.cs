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
        [LoginRule( Front = true, isVisiter = false)]
        public ActionResult ChatHistory()
        {
            ////把memid參數拿掉，換成session
            string session = Session["memid"].ToString();

            //取得該會員所有的房號
            var getRoom = db.Chat_Records.Where(m => m.ToMemId == session || m.FromMemId == session).GroupBy(m => m.ChatRoom).Select(x=>x.Key).ToList();


            //找出每位會員的最新一筆聊天記錄
            var chatList = (from a in db.Chat_Records
                            where a.Time == (from r in db.Chat_Records
                                             where a.ChatRoom == r.ChatRoom
                                             select r.Time).Max()
                                             && getRoom.Contains(a.ChatRoom)
                                             
                            select a).ToList();
            //取得每位會員訊息未讀數
            List<int> ReadYetCount = new List<int>();
            foreach (var c in chatList.OrderByDescending(m => m.Time))
            {
                int count = db.Chat_Records.Where(m => m.ChatRoom == c.ChatRoom && m.ReadYet == false).Count();
                ReadYetCount.Add(count);
            }
            ViewBag.ReadYetCount = ReadYetCount;

            return View(chatList);
        }
        [LoginRule(Front = true, isVisiter = false)]
        public ActionResult Chat(string fromMemID)
        {
            string session = Session["memid"].ToString();
            //找出聊天室房號
            var findRecord = db.Chat_Records.Where(m => (m.ToMemId == session && m.FromMemId == fromMemID) || m.FromMemId == session && m.ToMemId == fromMemID).FirstOrDefault();
            int roomID;
            //判斷是否已有聊天記錄
            if (findRecord == null)
            {

                //判斷是否有空房可用
                //select* from chatroom as a left join Chat_Records as b on a.chatroom = b.chatroom where b.chatroom is null
                var EmptyRoom = (from a in db.ChatRoom
                                 join b in db.Chat_Records
                                 on a.ChatRoom1 equals b.ChatRoom
                                 into groupjoin
                                 from c in groupjoin.DefaultIfEmpty()
                                 where c.chatSerial == null
                                 select a).ToList();

                
                if (EmptyRoom.Count() > 0)
                {
                    //取得尚未取用的房號，(RoomUsed為false)
                    var emp = EmptyRoom.Take(1).FirstOrDefault();
                    //將之前已取用房號但仍未有聊天記錄的房間重新釋放
                    emp.RoomUsed = false;
                    roomID = emp.ChatRoom1;
                    //儲存房號
                    TempData["roomID"] = roomID;
                    //若房號已被取用，修改RoomUsed、取用日期欄位
                    db.ChatRoom.Find(roomID).RoomUsed = true;
                    db.ChatRoom.Find(roomID).CreateTime = DateTime.Now;

                    db.SaveChanges();

                }
                else
                {
                    //新建房間
                    DateTime now = DateTime.Now;
                    ChatRoom R = new ChatRoom()
                    {
                        CreateTime = now,
                        RoomUsed = false
                    };
                    db.ChatRoom.Add(R);
                    db.SaveChanges();
                }
            }
            else
            {
                //取得原房號
                roomID = (int)(findRecord.ChatRoom);
                //已讀所有對方寄送過來的未讀訊息
                string UpdateString = "Update Chat_Records set ReadYet=1 where chatroom=@chatroom and ToMemId=@ToMemId";
                db.Database.ExecuteSqlCommand(UpdateString, new SqlParameter("@chatroom", roomID),new SqlParameter("@ToMemId", session));
                //取得所有歷史記錄
                ViewBag.chat = db.Chat_Records.Where(m => m.ChatRoom == roomID).OrderByDescending(m => m.Time).ToList().OrderBy(m => m.Time);
                //儲存房號
                TempData["roomID"] = roomID;
            }

            //取得對方暱稱
            ViewBag.Nick = db.Member.Find(fromMemID).memNick;

            return View();
        }

    }
}