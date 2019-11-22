using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JoinFun.Models;

namespace JoinFun.Utilities
{
    public class Common
    {
        JoinFunEntities db = new JoinFunEntities();
        /// <summary>
        /// 新增通知中心資料
        /// </summary>
        /// <param name="isStation">是否為站內訊息</param>
        /// <param name="instanceID">對應事件編號(若為站內訊息時此欄位給空字串即可)</param>
        /// <param name="TomemId">通知對象</param>
        /// <param name="title">通知標題</param>
        /// <param name="Content">通知內容</param>
        public void CreateNoti(bool isStation, string instanceID, string TomemId, string title, string Content)
        {
            Notification Noti = new Notification();

            string getNo = db.Database.SqlQuery<string>("select [dbo].[GetNoteId]()").FirstOrDefault();
            Noti.NotiSerial = getNo;
            Noti.ToMemId = TomemId;
            Noti.NotiTitle = title;
            Noti.NotifContent = Content;
            Noti.timeReceived = DateTime.Now;
            Noti.readYet = false;
            Noti.keepNotice = true;
            //處理對應事件編號欄位
            if (isStation)
            {
                Noti.InstanceId = getNo;
            }
            else
            {
                Noti.InstanceId = instanceID;
            }

            db.Notification.Add(Noti);
            db.SaveChanges();
        }
    }
}