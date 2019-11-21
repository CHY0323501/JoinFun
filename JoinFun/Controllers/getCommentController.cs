using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using JoinFun.Models;

namespace JoinFun.Controllers
{
    public class getCommentController : ApiController
    {
        JoinFunEntities db = new JoinFunEntities();
        public IHttpActionResult Get(string id)
        {
            var comment = db.Message_Board.Where(m => m.mboardSerial == id).ToList();
            List<string> result = new List<string>();
            foreach (var item in comment)
            {
                result.Add(item.messageTime.ToString());
                result.Add(item.boardMessage);
            }
            return Ok(result);
        }
    }
}
