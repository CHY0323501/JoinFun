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
        public DateTime CheckMemberSuspend(string memID) {
            var activityVio = (from a in db.Join_Fun_Activities
                               join b in db.Violation
                               on a.actId equals b.CorrespondingEventID
                               where a.hostId == memID
                               select b).ToList();

            var RemarkVio = (from a in db.Member_Remarks
                             join b in db.Violation
                               on a.remarkSerial equals b.CorrespondingEventID
                             where a.FromMemId == memID
                             select b).ToList();

            var BoardVio = (from a in db.Message_Board
                            join b in db.Violation
                             on a.mboardSerial equals b.CorrespondingEventID
                            where a.memId == memID
                            select b).ToList();

            var MemberVio = db.Violation.Where(m => m.CorrespondingEventID == memID).ToList();

            //將會員、留言板、評價、揪團違規查詢結果合併
            var AllVio = activityVio.Union(RemarkVio).Union(BoardVio).Union(MemberVio);

            var Result_stop5 = AllVio.Where(m => m.punishId== "pmt0000004").OrderByDescending(m=>m.vioProcessTime).Take(1).FirstOrDefault();
            var Result_stop3 = AllVio.Where(m => m.punishId== "pmt0000003").OrderByDescending(m=>m.vioProcessTime).Take(1).FirstOrDefault();

            if (Result_stop5 != null && Result_stop3 != null) {
                if (Result_stop5.vioProcessTime.Value.AddDays(5) > Result_stop3.vioProcessTime.Value.AddDays(3))
                    return (DateTime)Result_stop5.vioProcessTime.Value.AddDays(5);
                return (DateTime)Result_stop3.vioProcessTime.Value.AddDays(3);
            }
            else if (Result_stop5 != null && Result_stop3 == null)
                return (DateTime)Result_stop5.vioProcessTime.Value.AddDays(5);
            else if (Result_stop5 == null && Result_stop3 != null)
                return (DateTime)Result_stop3.vioProcessTime.Value.AddDays(3);

            //若查無停權五天及停權三天紀錄時，回傳DateTime.MinValue;
            return DateTime.MinValue;
        }
    }
}