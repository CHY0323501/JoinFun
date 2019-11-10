using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        //確認好友狀態
        public IHttpActionResult Get(string FriendID)
        {
            //var friendShip = (from i in db.Friendship
            //          where i.friendMemId == FriendID /*&& i.memId == HttpContext.Current.Session["memid"].ToString()*/
            //          select i).FirstOrDefault();

            var friendship = db.Friendship.Where(m => m.friendMemId == FriendID).ToList();

            return Ok(friendship);
        }

        //加入好友
        //需寫入Friendship
        public IHttpActionResult Post(string FriendID, string memID)
        {
            //寫入Friendship資料表
            Friendship friend1 = new Friendship();
            friend1.memId = memID;
            friend1.friendMemId = FriendID;

            Friendship friend2 = new Friendship();
            friend2.memId = FriendID;
            friend2.friendMemId = memID;

            db.Friendship.Add(friend1);
            db.Friendship.Add(friend2);
            
            db.SaveChanges();
            return Ok();
        }

        //確認好友、修改好友暱稱
        public IHttpActionResult Put(string FriendID, string memID,string newNick,bool checkFD)
        {
            var FDdata1 = db.Friendship.Where(m => m.memId == memID && m.friendMemId == FriendID).FirstOrDefault();
            var FDdata2 = db.Friendship.Where(m => m.memId == FriendID && m.friendMemId == memID).FirstOrDefault();

            //確認好友
            if (checkFD) {
                FDdata1.Approved = true;
                FDdata2.Approved = true;
            }
            else
                FDdata1.friendNick = newNick;

            db.SaveChanges();
            return Ok();
        }

        //刪除好友、取消邀請
        //需刪除Friendship、fans、followup三張資料表中的該會員資料
        public IHttpActionResult Delete(string FriendID, string memID)
        {
            //刪除Friendship資料表中資料
            var friend1 = db.Friendship.Where(m => m.memId == memID && m.friendMemId == FriendID).FirstOrDefault();
            var friend2 = db.Friendship.Where(m => m.memId == FriendID && m.friendMemId == memID).FirstOrDefault();

            db.Friendship.Remove(friend1);
            db.Friendship.Remove(friend2);

            //刪除fans資料表中資料
            var fan = db.Fans.Where(m => m.memId == FriendID && m.fanMemId == memID).FirstOrDefault();
            db.Fans.Remove(fan);

            //刪除followup資料表中資料
            var followUp = db.FollowUp.Where(m => m.memId == FriendID && m.FoMemId == memID).FirstOrDefault();
            db.FollowUp.Remove(followUp);


            db.SaveChanges();

            return Ok();
        }
    }
}
