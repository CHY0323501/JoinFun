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

            //key為發送對象，value為WebSocketCollection
            static Dictionary<string, WebSocketCollection> _chatRooms = new Dictionary<string, WebSocketCollection>();
            public ChatWebSocketHandler(string fromMemID,string toMemID)
            {
                _fromMemID = fromMemID;
                _toMemID = toMemID;
            }


            //覆寫OnOpen事件，鑄造新的ChatWebSocketHandler時觸發
            public override void OnOpen()
            {
                if (_chatRooms.ContainsKey(_toMemID))
                {
                    //自己目前不在這個聊天對話中才加入
                    if (!_chatRooms[_toMemID].Any(a => ((ChatWebSocketHandler)a)._fromMemID == _fromMemID))
                        _chatRooms[_toMemID].Add(this);
                }
                else
                    _chatRooms[_toMemID] = new WebSocketCollection() { this };
            }


            //覆寫OnMessage事件，前端send時觸發，被觸發後會回頭觸發前端的onmessage事件
            public override void OnMessage(string message)
            {

                //將對話記錄存入資料庫
                Chat_Records chat = new Chat_Records()
                {
                    chatSerial= db.Database.SqlQuery<string>("Select [dbo].[GetChatId]()").FirstOrDefault(),
                    FromMemId = _fromMemID,
                    ToMemId=_toMemID,
                    chatContent=message,
                    Time=DateTime.Now,
                    ReadYet=false
                };
                db.Chat_Records.Add(chat);
                db.SaveChanges();


                //推送對話給聊天對象
                string fromMemNick = db.Member.Find(_fromMemID).memNick;
                _chatRooms[_toMemID].Broadcast(fromMemNick + " " + message+ " " + DateTime.Now);
            }

        }
    }
}