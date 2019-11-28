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
        public IHttpActionResult Get(string memid,string actid)
        {
            var ActivityDetail = db.Activity_Details.Where(m => m.actId == actid&&m.memId==memid).ToList();

            return Ok(ActivityDetail);
        }

 
        //確認參團
        //更改Activity_Detail
        public IHttpActionResult Put(string actid,string memid)
        {
            //確認參團,修改Act_Detail審核
            var editAC = db.Activity_Details.Where(m => m.actId == actid && m.memId == memid).FirstOrDefault();
            editAC.appvStatus = true;

            //確認後寄送通知
            Common com = new Common();
            var M1 = db.Member.Find(memid).memNick;
            com.CreateNoti(true, "", memid, "恭喜您已成功加入" + actid + "揪團活動。"," < br />Join Fun營運團隊");
            db.SaveChanges();
            return Ok();
        }

        //刪除Activity_Detail資料列
        public IHttpActionResult Delete(string actid, string memid)
        {
            var DeleteAC = db.Activity_Details.Where(m => m.actId == actid && m.memId == memid).FirstOrDefault();
            if (DeleteAC != null)
            db.Activity_Details.Remove(DeleteAC);

            db.SaveChanges();
            return Ok();

        }
    }
}
