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
        [LoginRule(isVisiter = true, Front = true)]
        public ActionResult Chat(string fromMemID= "M000000005") {
            Session["memid"] = "M000000002";
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
                
                //將之前已取用房號但仍未有聊天記錄的房間重新釋放
                foreach (var i in EmptyRoom)
                {
                    db.ChatRoom.Find(i.ChatRoom1).RoomUsed = false;
                    db.SaveChanges();
                }

                bool getRoom = false;

                while (getRoom==false) {
                    if (EmptyRoom.Count() > 0)
                    {
                        //取得尚未取用的房號，(RoomUsed為false)
                        roomID=EmptyRoom.Where(m => m.RoomUsed == false).FirstOrDefault().ChatRoom1;
                        //儲存房號
                        ViewBag.roomID = roomID;
                        //若房號已被取用，修改RoomUsed欄位
                        db.ChatRoom.Find(roomID).RoomUsed = true;
                        db.SaveChanges();

                        //離開迴圈
                        getRoom = true;
                        
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
            }
            else {
                //取得原房號
                roomID = (int)(findRecord.ChatRoom);

                //已讀所有未讀訊息
                string UpdateString = "Update Chat_Records set ReadYet=1 where chatroom=@chatroom";
                db.Database.ExecuteSqlCommand(UpdateString, new SqlParameter("@chatroom", roomID));
                //取得所有歷史記錄
                ViewBag.chat = db.Chat_Records.Where(m => m.ChatRoom == roomID).OrderByDescending(m => m.Time).ToList().OrderBy(m => m.Time);
                //儲存房號
                ViewBag.roomID = roomID;
            }

            
            
            //取得對方暱稱
            ViewBag.Nick = db.Member.Find(fromMemID).memNick;

            return View();
        }
    }
}