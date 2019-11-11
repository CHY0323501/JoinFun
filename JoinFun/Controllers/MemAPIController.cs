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
        public IHttpActionResult Get(string memID)
        {
            //var friendShip = (from i in db.Friendship
            //          where i.friendMemId == FriendID /*&& i.memId == HttpContext.Current.Session["memid"].ToString()*/
            //          select i).FirstOrDefault();

            var friendship = db.Friendship.Where(m => m.memId == memID).ToList();

            return Ok(friendship);
        }

        //加入好友
        //需寫入Friendship
        public IHttpActionResult Post(string FriendID, string memID)
        {
            
            Friendship friend2 = new Friendship();
            friend2.memId = FriendID;
            friend2.friendMemId = memID;
                        
            db.Friendship.Add(friend2);

            db.SaveChanges();
            return Ok();
        }

        //確認好友、修改好友暱稱(checkFD來判別為確認好友或修改暱稱)
        public IHttpActionResult Put(string FriendID, string memID,string newNick,bool checkFD)
        {

            //確認好友
            if (checkFD) {
                //新增資料至friendship資料表
                Friendship friend1 = new Friendship();
                friend1.memId = memID;
                friend1.friendMemId = FriendID;
                db.Friendship.Add(friend1);
                db.SaveChanges();
                var FDdata1 = db.Friendship.Where(m => m.memId == memID && m.friendMemId == FriendID).FirstOrDefault();
                var FDdata2 = db.Friendship.Where(m => m.memId == FriendID && m.friendMemId == memID).FirstOrDefault();
                //修改FriendShip資料表中的approved欄位
                FDdata1.Approved = true;
                FDdata2.Approved = true;

                //若成為好友前尚未為粉絲或追蹤關係時，新增粉絲及追蹤關係
                Fans fan = new Fans();
                FollowUp follow = new FollowUp();

                var fan1 = db.Fans.Where(m => m.memId == FriendID && m.fanMemId == memID).FirstOrDefault();
                var fan2 = db.Fans.Where(m => m.memId == memID && m.fanMemId == FriendID).FirstOrDefault();
                var followUp1 = db.FollowUp.Where(m => m.memId == FriendID && m.FoMemId == memID).FirstOrDefault();
                var followUp2 = db.FollowUp.Where(m => m.memId == memID && m.FoMemId == FriendID).FirstOrDefault();
                if (fan1 == null && fan2 == null && followUp1 == null && followUp2 == null)
                {
                    //新增fans資料表中雙方資料
                    fan.fanMemId = memID;
                    fan.memId = FriendID;
                    db.Fans.Add(fan);
                    db.SaveChanges();

                    Fans fan3 = new Fans();
                    fan3.fanMemId = FriendID;
                    fan3.memId = memID;
                    db.Fans.Add(fan3);
                    db.SaveChanges();
                    //新增followup資料表中雙方資料
                    follow.FoMemId = memID;
                    follow.memId = FriendID;
                    db.FollowUp.Add(follow);
                    db.SaveChanges();

                    FollowUp follow3 = new FollowUp();
                    follow3.FoMemId = FriendID;
                    follow3.memId = memID;
                    db.FollowUp.Add(follow3);
                    db.SaveChanges();

                }
                //當A已追蹤B時，新增B方資料至fans及followup資料表中
                else if (fan1 == null && fan2 != null && followUp1 == null && followUp2 != null)
                {
                    //新增B方資料至fans資料表
                    Fans fan4 = new Fans();
                    fan4.fanMemId = memID;
                    fan4.memId = FriendID;
                    db.Fans.Add(fan4);
                    //新增B方資料至followup資料表
                    FollowUp follow4 = new FollowUp();
                    follow4.FoMemId = memID;
                    follow4.memId = FriendID;
                    db.FollowUp.Add(follow4);
                }
                //當B已追蹤A時，新增A方資料至fans及followup資料表中
                else if (fan1 != null && fan2 == null && followUp1 != null && followUp2 == null) {
                    //新增A方資料至fans資料表
                    Fans fan5 = new Fans();
                    fan5.fanMemId = FriendID;
                    fan5.memId = memID;
                    db.Fans.Add(fan5);

                    //新增A方資料至followup資料表
                    FollowUp follow5 = new FollowUp();
                    follow5.FoMemId = FriendID;
                    follow5.memId = memID;
                    db.FollowUp.Add(follow5);
                }
            }
            //已為好友關係，僅修改好友暱稱
            else {
                var FDdata1 = db.Friendship.Where(m => m.memId == memID && m.friendMemId == FriendID).FirstOrDefault();
                FDdata1.friendNick = newNick;
            }
            db.SaveChanges();
            return Ok();
        }

        //刪除好友、取消邀請
        //刪除好友需刪除Friendship、fans、followup三張資料表中的該會員資料
        //取消邀請需刪除Friendship資料表中的該會員資料
        public IHttpActionResult Delete(string FriendID, string memID,bool Cancel)
        {
            if (Cancel==false) {
                //刪除fans資料表中資料
                var fan = db.Fans.Where(m => m.memId == FriendID && m.fanMemId == memID).FirstOrDefault();
                var fan2 = db.Fans.Where(m => m.memId == memID && m.fanMemId == FriendID).FirstOrDefault();
                if(fan!=null)
                db.Fans.Remove(fan);
                if(fan2!=null)
                db.Fans.Remove(fan2);

                //刪除followup資料表中資料
                var followUp = db.FollowUp.Where(m => m.memId == FriendID && m.FoMemId == memID).FirstOrDefault();
                var followUp2 = db.FollowUp.Where(m => m.memId == memID && m.FoMemId == FriendID).FirstOrDefault();
                if(followUp!=null)
                db.FollowUp.Remove(followUp);
                if(followUp2!=null)
                db.FollowUp.Remove(followUp2);
            }
            //刪除Friendship資料表中資料
            var friend1 = db.Friendship.Where(m => m.memId == memID && m.friendMemId == FriendID).FirstOrDefault();
            var friend2 = db.Friendship.Where(m => m.memId == FriendID && m.friendMemId == memID).FirstOrDefault();
            if(friend1!=null)
            db.Friendship.Remove(friend1);
            if(friend2!=null)
            db.Friendship.Remove(friend2);

            db.SaveChanges();

            return Ok();
        }
    }
}
