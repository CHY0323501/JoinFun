using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
//using model
using JoinFun.Models;
//using viewmodel
using JoinFun.ViewModel;
using JoinFun.Utilities;

namespace JoinFun.Controllers
{
    public class MemberController : Controller
    {
        JoinFunEntities db = new JoinFunEntities();
        SqlConnection Conn = new SqlConnection("data source = MCSDD108212; initial catalog = JoinFun; integrated security = True; MultipleActiveResultSets=True;App=EntityFramework&quot;");
        SqlCommand cmd = new SqlCommand();

        public ActionResult Info(string memID = "M000000002")
        {
            var member = db.Member.Where(m => m.memId == memID).FirstOrDefault();
            //當會員編號不存在時執行
            if (memID == null || member == null)
            {
                return RedirectToAction("Index", "Activity");
            }
            else
            {
                Session["memid"] = "M000000002";
                MemberViewModel Minfo = new MemberViewModel()
                {
                    Member = db.Member.Where(m => m.memId == memID).ToList(),
                    Blacklist = db.Blacklist.ToList(),
                    Bookmark_Details = db.Bookmark_Details.Where(m => m.memId == memID).ToList(),
                    Friendship = db.Friendship.Where(m => m.memId == memID).ToList(),
                    vw_FansNew = db.vw_FansNew.Where(m => m.memId == memID).ToList(),
                    vw_FollowUp = db.vw_FollowUp.Where(m => m.FoMemId == memID).ToList(),
                    vw_FriendShip = db.vw_FriendShip.Where(m => m.memId == memID && m.Approved == true).ToList(),
                    vw_Member_Remarks = db.vw_Member_Remarks.Where(m => m.ToMemId == memID).ToList(),
                    vw_HostHistory = db.vw_HostHistory.Where(m => m.hostId == memID).ToList(),
                    vw_PartHistory = db.vw_PartHistory.Where(m => m.memId == memID).ToList()
                };
                //MessageCenter mes = new MessageCenter();
                //List<string> mailList = new List<string>() {
                //    "bowbow19@gmail.com","ych4101861@gmail.com"
                //};
                //for (int i = 0; i < 100; i++)
                //{
                //    mes.SendEmail(mailList, "JoinFun緊急公告", "會員：吳芳飴女士，您已危害到工程師生存，請立刻停止攻擊，否則不排除提告！JoinFun營運團隊");
                //}

                return View(Minfo);
            }
        }
        public ActionResult Edit(string memID)
        {
            var member = db.Member.Where(m => m.memId == memID).FirstOrDefault();
            if (memID == null || member == null)
            {
                return RedirectToAction("Index", "Activity");
            }
            else
            {
                try
                {
                    if (Session["memid"].ToString() == memID)
                    {
                        //下拉式選單用
                        //ViewBag.county_drop = new SelectList(db.County, "CountyNo", "CountyName");
                        //ViewBag.district_drop = new SelectList(db.District, "DistrictSerial", "DistrictName");
                        ViewBag.county_drop = db.County.ToList();
                        ViewBag.district_drop = db.District.ToList();
                        return View(member);

                    }
                    //未登入時無法編輯並轉至首頁
                    return RedirectToAction("Index", "Activity");
                }
                catch
                {
                    return RedirectToAction("Index", "Activity");
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string memId, DateTime Birthday, string Sex, string memNick, string Email, int memCounty, int memDistrict, string Introduction, string Habit, string Dietary_Preference)
        {

            if (ModelState.IsValid && Session["memid"] != null)
            {
                var mem = db.Member.Find(memId);
                mem.Sex = Sex;
                mem.memNick = memNick;
                mem.Email = Email;
                mem.memCounty = (Int16)memCounty;
                mem.memDistrict = (Int16)memDistrict;
                mem.Birthday = Birthday;
                mem.Introduction = Introduction;
                mem.Habit = Habit;
                mem.Dietary_Preference = Dietary_Preference;
                db.SaveChanges();
            }
            //編輯完畢時回到個人資訊頁
            return RedirectToAction("Info", new { memID = Session["memid"] });
        }

        //會員評價
        public ActionResult Remarks(string memID)
        {
            var member = db.Member.Where(m => m.memId == memID).FirstOrDefault();
            if (memID == null || member == null )
            {
                return RedirectToAction("Index", "Activity");
            }
            else
            {
                MemRemarkViewModel MRemark = new MemRemarkViewModel()
                {
                    //判斷是否為主辦人評價
                    vw_Host_Remarks = db.vw_Host_Remarks.Where(m => m.ToMemId == memID).ToList(),
                    //判斷是否為參與者評價
                    vw_Participant_Remarks = db.vw_Participant_Remarks.Where(m => m.ToMemId == memID).ToList()
                };

                //查詢評價對象暱稱
                ViewBag.ToMemNick = (from m in db.Member
                                     where m.memId == memID
                                     select m.memNick).FirstOrDefault();

                return View(MRemark);
            }
        }
        //新增會員評價
        public ActionResult RemarkCreate(string actID, string FromMemID)
        {

            var member = db.Member.Where(m => m.memId == FromMemID).FirstOrDefault();
            if (FromMemID == null || member == null)
            {
                return RedirectToAction("Index", "Activity");
            }

            ViewBag.actTop = db.vw_HostHistory.Where(m => m.actId == actID).Select(m => m.actTopic).FirstOrDefault();
            ViewBag.actid = db.vw_HostHistory.Where(m => m.actId == actID).Select(m => m.actId).FirstOrDefault();


            GetMemList(actID, FromMemID);

            return View();

        }
        [HttpPost]
        public ActionResult RemarkCreate(string ToMemId,string actid,string remarkContent, short remarkStar, string FromMemId)
        {            
            
            //呼叫Sql函數GetActId()取得新增的活動ID

            string getremarkSerial = db.Database.SqlQuery<string>("Select [dbo].[GetRemarkId]()").FirstOrDefault();
            Member_Remarks aaa = new Member_Remarks();


            aaa.actId = db.vw_HostHistory.Where(m => m.actId == actid).Select(m => m.actId).FirstOrDefault();
            aaa.ToMemId = ToMemId;

            aaa.remarkSerial = getremarkSerial;
            aaa.FromMemId = FromMemId;
            aaa.remarkContent = remarkContent;
            aaa.remarkStar = remarkStar;
            aaa.remarkTime = DateTime.Now;
            db.Member_Remarks.Add(aaa);
            db.SaveChanges();

            return RedirectToAction("History");

        }
        //目前開團&參團清單
        public ActionResult MyActivities(string memID)
        {
            var member = db.Member.Where(m => m.memId == memID).FirstOrDefault();
            if (memID == null || member == null)
            {
                return RedirectToAction("Index", "Activity");
            }
            else
            {
                MyActivitiesVM MyActivity = new MyActivitiesVM()
                {
                    Host_Now = db.Host_Now.Where(m => m.hostId == memID).ToList(),
                    Part_Now = db.Part_Now.Where(m => m.memId == memID).ToList(),
                    Photos_of_Activities = db.Photos_of_Activities.ToList(),
                    Activity_Class = db.Activity_Class.ToList()
                };
                return View(MyActivity);
            }
        }


        //揪團歷史
        public ActionResult History(string memID)
        {
            var member = db.Member.Where(m => m.memId == memID).FirstOrDefault();
            if (memID == null || member == null)
            {
                return RedirectToAction("Index", "Activity");
            }
            else
            {
                HistoryViewModel History = new HistoryViewModel()
                {
                    vw_HostHistory = db.vw_HostHistory.Where(m => m.hostId == memID).ToList(),
                    vw_PartHistory = db.vw_PartHistory.Where(m => m.memId == memID).ToList(),
                    Photos_of_Activities = db.Photos_of_Activities.ToList(),
                    Activity_Class = db.Activity_Class.ToList(),
                    Activity_Details = db.Activity_Details.ToList(),
                    Member_Remarks = db.Member_Remarks.Where(m=>m.FromMemId== memID).ToList()
                };

                ViewBag.ToMemNick = (from m in db.Member
                                     where m.memId == memID
                                     select m.memNick).FirstOrDefault();
                return View(History);
            }
        }
        //好友管理

        public ActionResult FriendManagement(string memID)
        {
            var member = db.Member.Where(m => m.memId == memID).FirstOrDefault();
            if (memID == null || member == null )
            {
                return RedirectToAction("Index", "Activity");
            }
            else
            {
                try
                {
                    if (Session["memid"].ToString() == memID)
                    {
                        //好友相關資料
                        FriendManagementVW FDvw = new FriendManagementVW()
                        {
                            Member = db.Member.Where(m => m.memId == memID).ToList(),
                            vw_FriendShip = db.vw_FriendShip.Where(m => m.memId == memID).ToList()
                        };

                        ViewBag.FdManagementMemNick = db.Member.Where(m => m.memId == memID).FirstOrDefault().memNick;
                        return View(FDvw);
                    }
                    else {
                        return RedirectToAction("Index", "Activity");
                    }
                }
                catch {
                    return RedirectToAction("Index", "Activity");
                }
                
            }
        }
        //加入黑名單
        public ActionResult Block(string BlockedMemID, string memID)
        {
            db.Database.ExecuteSqlCommand("exec sp_blockMember @BlockedMemID,@memID", new SqlParameter("@BlockedMemID", BlockedMemID), new SqlParameter("@memID", memID));

            return RedirectToAction("Info", "Member", new { memID = BlockedMemID });
        }
        //黑名單清單
        public ActionResult BlockManage(string memID)
        {
            var member = db.Member.Where(m => m.memId == memID).FirstOrDefault();
            if (memID == null || member == null)
            {
                return RedirectToAction("Index", "Activity");
            }
            else
            {
                try
                {
                    if (Session["memid"].ToString() == memID)
                    {
                        BlockMemberVM BlockList = new BlockMemberVM()
                        {
                            Blacklist = db.Blacklist.Where(m => m.memId == memID).ToList(),
                            Member = db.Member.ToList(),
                        };
                        return View(BlockList);
                    }
                    //未登入時無法編輯並轉至首頁
                    return RedirectToAction("Index", "Activity");
                }
                catch
                {
                    return RedirectToAction("Index", "Activity");
                }
            }
        }
        //解除黑名單
        public ActionResult CancelBlock(string BlockedMemID, string memID)
        {
            if (Session["memid"].ToString().Any()) {
                var Block1 = db.Blacklist.Where(m => m.memId == memID && m.blockedMemId == BlockedMemID).FirstOrDefault();
                var Block2 = db.Blacklist.Where(m => m.memId == BlockedMemID && m.blockedMemId == memID).FirstOrDefault();
                if (Block1 != null)
                    db.Blacklist.Remove(Block1);
                if (Block2 != null)
                    db.Blacklist.Remove(Block2);

                db.SaveChanges();
                return RedirectToAction("BlockManage", "Member", new { memID = memID });
            }
            return RedirectToAction("Index", "Activity");
        }
        //會員搜尋
        public ActionResult Search(string search) {
            if (!String.IsNullOrEmpty(search)) {
                var SearchResult = from mem in db.Member
                                   where mem.memId == search || mem.memNick.Contains(search)
                                   select mem;
                ViewBag.SearchContent = search;
                return View(SearchResult);
            }
            return RedirectToAction("Index", "Activity");
        }

        //取得被評價會員的DropDownList清單方法
        public void GetMemList(string actID, string FromMemID)
        {
            var host = db.vw_Activities.Where(m => m.actId == actID).ToList();
            var members = db.Activity_Details.Where(m => m.actId == actID).ToList();
            var remarked = db.Member_Remarks.Where(m => m.FromMemId == FromMemID && m.actId == actID).ToList();
            //var member = db.Member.ToList();
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in host)
            {
                if(!remarked.Any(m => m.ToMemId == item.hostId))
                {
                    if (item.hostId != FromMemID)
                    {
                        list.Add(new SelectListItem() { Text = item.hostId + " " + item.memNick, Value = item.hostId });
                    }
                }
            }
            foreach (var item in members)
            {
                if (!remarked.Any(m => m.ToMemId == item.memId))
                {
                    if (item.memId != FromMemID)
                    {
                        list.Add(new SelectListItem() { Text = item.memId + " " + item.Member.memNick, Value = item.memId });
                    }
                }
            }
            ViewBag.MemList = new SelectList(list, "Value", "Text");
        }
    }
}