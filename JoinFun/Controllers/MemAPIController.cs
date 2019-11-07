using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using JoinFun.Models;

namespace JoinFun.Controllers
{
    public class MemAPIController : ApiController
    {
        JoinFunEntities db = new JoinFunEntities();
        //Get確認好友狀態
        public IHttpActionResult Get(string FriendID)
        {
            //var friendShip = (from i in db.Friendship
            //          where i.friendMemId == FriendID /*&& i.memId == HttpContext.Current.Session["memid"].ToString()*/
            //          select i).FirstOrDefault();

            var friendship = db.Friendship.Where(m => m.friendMemId == FriendID).ToList();

            return Ok(friendship);
        }

        ////Post加入好友
        ////需寫入Friendship、fans、followup三張資料表
        //public IHttpActionResult Post(string memID)
        //{

        //    db.Add();
        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (db.學生.Count(m => m.學號 == stu.學號) > 0)
        //            return Conflict();
        //        else
        //            throw;      //拋出例外
        //    }


        //    return Ok();
        //}


        //// DELETE刪除好友
        /////需刪除Friendship、fans、followup三張資料表中的該會員資料
        //public IHttpActionResult Delete(string id)
        //{
        //    var stu = db.學生.Find(id);
        //    if (stu == null)
        //        return NotFound();

        //    db.學生.Remove(stu);
        //    db.SaveChanges();

        //    return Ok(stu);
        //}
    }
}
