using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using JoinFun.Models;

namespace JoinFun.Controllers
{
    public class BlockMemberController : ApiController
    {
        JoinFunEntities db = new JoinFunEntities();
        //確認是否為黑名單狀態
        public IHttpActionResult Get(string memID)
        {
            var Blacklist = db.Blacklist.Where(m => m.memId == memID).ToList();
            return Ok(Blacklist);
        }
        //加入黑名單並刪除追蹤、粉絲及好友狀態
        public IHttpActionResult Post(string BlockedMemID, string memID)
        {
            db.Database.ExecuteSqlCommand("exec sp_blockMember @BlockedMemID,@memID", new SqlParameter("@BlockedMemID", BlockedMemID), new SqlParameter("@memID", memID));

            return Ok();
        }
    }
}
