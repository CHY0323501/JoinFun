using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JoinFun.Models;
//using websocket
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Web.WebSockets;
using System.Data.Entity.Infrastructure;

namespace JoinFun.Controllers
{
    public class ChatController : ApiController
    {
        public HttpResponseMessage Get(string fromMemID,string toMemID)
        {
            HttpContext.Current.AcceptWebSocketRequest(new ChatWebSocketHandler(fromMemID, toMemID));

            return Request.CreateResponse(HttpStatusCode.SwitchingProtocols);            
        }
        class ChatWebSocketHandler : WebSocketHandler
        {
            JoinFunEntities db = new JoinFunEntities();
            string _fromMemID;
            string _toMemID;
            int _roomID;

            //key為聊天室編號，value為WebSocketCollection
            static Dictionary<int, WebSocketCollection> _chatRooms = new Dictionary<int, WebSocketCollection>();
            public ChatWebSocketHandler(string fromMemID,string toMemID)
            {
                //string session = HttpContext.Current.Session["memid"].ToString();

                _fromMemID = fromMemID;
                _toMemID = toMemID;


                //若無聊天記錄，新增聊天房號，否則找出舊房號
                var checkRoomID = db.Chat_Records.Where(m => m.FromMemId == _toMemID && m.ToMemId == fromMemID || m.FromMemId == fromMemID && m.ToMemId == _toMemID).FirstOrDefault();
                if (checkRoomID != null)
                {
                    //找出舊房號
                    _roomID = (int)checkRoomID.ChatRoom;
                }
                else
                {
                    //新增聊天房號
                    _roomID = (from a in db.ChatRoom
                               select a.ChatRoom1).Max()+1;
                }
            }
            //覆寫OnOpen事件，鑄造新的ChatWebSocketHandler時觸發
            public override void OnOpen()
            {
                if (_chatRooms.ContainsKey(_roomID))
                {
                    //自己目前不在這個聊天對話中才加入
                    if (!_chatRooms[_roomID].Any(a => ((ChatWebSocketHandler)a)._fromMemID == _fromMemID))
                        _chatRooms[_roomID].Add(this);
                }
                else
                    _chatRooms[_roomID] = new WebSocketCollection() { this };
            }


            //覆寫OnMessage事件，前端send時觸發，被觸發後會回頭觸發前端的onmessage事件
            public override void OnMessage(string message)
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    var now = DateTime.Now;
                    try
                    {
                        //確認是否已有聊天記錄，若無記錄時新增聊天房
                        var checkRoomID = db.Chat_Records.Where(m => m.FromMemId == _toMemID && m.ToMemId == _fromMemID || m.FromMemId == _fromMemID && m.ToMemId == _toMemID).FirstOrDefault();
                        if (checkRoomID == null)
                        {
                            //新增聊天房
                            ChatRoom room = new ChatRoom()
                            {
                                LastChatTime = now
                            };
                            db.ChatRoom.Add(room);
                            db.SaveChanges();
                        }

                        //將對話記錄存入資料庫
                        Chat_Records chat = new Chat_Records()
                        {
                            chatSerial = db.Database.SqlQuery<string>("Select [dbo].[GetChatId]()").FirstOrDefault(),
                            FromMemId = _fromMemID,
                            ToMemId = _toMemID,
                            chatContent = message,
                            Time = now,
                            ReadYet = false,
                            ChatRoom = _roomID
                        };
                        db.Chat_Records.Add(chat);
                        db.SaveChanges();

                        transaction.Commit();
                    }
                    catch (DbUpdateException)
                    {
                        transaction.Rollback();
                    }
                }

                


                //推送對話給聊天對象
                string fromMemNick = db.Member.Find(_fromMemID).memNick;
                _chatRooms[_roomID].Broadcast(fromMemNick + " " + message+ " " + DateTime.Now);
            }

        }
    }
}