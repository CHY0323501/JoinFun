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
        public ActionResult ChatHistory(string memID)
        {
            List<Chat_Records> chatList = db.Chat_Records.Where(m => m.ToMemId == memID).ToList();
            return View();
        }
    }
}