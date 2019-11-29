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
using JoinFun.Utilities;

namespace JoinFun.Controllers
{
    public class ActCheckAPIController : ApiController
    {
        JoinFunEntities db = new JoinFunEntities();

        //確認審核狀態
        public IHttpActionResult Get(string memID, string actID)
        {
            var ActivityDetail = db.Activity_Details.Where(m => m.actId == actID && m.memId == memID).ToList();

            return Ok(ActivityDetail);
        }


        //確認參團
        //更改Activity_Detail的AppvStatus
        public IHttpActionResult Put(string memID, string actID)
        {
            //確認參團,修改Act_Detail審核
            var editAC = db.Activity_Details.Where(m => m.actId == actID && m.memId == memID).FirstOrDefault();
            editAC.appvStatus = true;
            editAC.appvDate = DateTime.Now;

            //{
            //    //確認後寄送通知
            //    Common com = new Common();
            //    com.CreateNoti(true, "", memID, "恭喜您已成功加入" + actID + "揪團活動。", " < br />Join Fun營運團隊");
            //}
            db.SaveChanges();
            return Ok();
        }


        //退出參團及取消參團需刪除Activity_Detail資料表中的該會員資料
        public IHttpActionResult Delete(string memID, string actID)
        {
            
                var DeleteAC = db.Activity_Details.Where(m => m.actId == actID && m.memId == memID).FirstOrDefault();
                            if (DeleteAC != null)

                    db.Activity_Details.Remove(DeleteAC);
            db.SaveChanges();

            return Ok();

        }
    }
}
