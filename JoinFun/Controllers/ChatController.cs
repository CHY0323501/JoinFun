﻿using System;
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
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace JoinFun.Controllers
{
    public class ChatController : ApiController
    {
        public HttpResponseMessage Get(string fromMemID, string toMemID, int roomID)
        {
            HttpContext.Current.AcceptWebSocketRequest(new ChatWebSocketHandler(fromMemID, toMemID, roomID));

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
            public ChatWebSocketHandler(string fromMemID, string toMemID, int roomID)
            {
                //string session = HttpContext.Current.Session["memid"].ToString();

                _fromMemID = fromMemID;
                _toMemID = toMemID;
                _roomID = roomID;

            }
            //覆寫OnOpen事件，鑄造新的ChatWebSocketHandler時觸發
            public override void OnOpen()
            {
                //判斷房間是否已在_chatRooms字典中，若沒有：新增一間房間；
                if (_chatRooms.ContainsKey(_roomID))
                {
                    _chatRooms[_roomID].Add(this);
                }
                else
                    _chatRooms[_roomID] = new WebSocketCollection() { this };
            }


            //覆寫OnMessage事件，前端send時觸發，被觸發後會回頭觸發前端的onmessage事件
            public override void OnMessage(string message)
            {
                DateTime now = DateTime.Now;
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

                //將訊息轉為json，儲存發送者、內容、時間
                Messageobj obj = new Messageobj {
                    fromMemID=this._fromMemID,
                    message=message,
                    time= now.ToString("MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture)
                };

                //推送對話給聊天對象及自己
                _chatRooms[_roomID].Broadcast(JsonConvert.SerializeObject(obj, Formatting.Indented));
            }

        }
    }
    public class Messageobj{
        public string fromMemID { get; set; }
        public string message { get; set; }
        public string time { get; set; }
    }
}