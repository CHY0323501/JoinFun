using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JoinFun.Models;
using JoinFun.ViewModel;

namespace JoinFun.Controllers
{
    public class ActivityController : Controller
    {
        JoinFunEntities db = new JoinFunEntities();
        Activity_Details m = new Activity_Details();
        
    
        // GET: Activity (原)
        //public ActionResult Index(string hostId, string actId, string actClassId = "cls001")
        //{
        //    if (actClassId == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ViewBag.actClassName = db.Activity_Class.Where(m => m.actClassId == actClassId).FirstOrDefault().actClassName;
        //    ViewBag.actClassId = actClassId;
        //    ViewBag.actId = actId;
        //    ActClass classList = new ActClass()
        //    {
        //        vwActivityList = db.vw_Activities.Where(m => m.actClassId == actClassId && m.keepAct == true).ToList(),
        //        ClassList = db.Activity_Class.ToList()
        //    };
        //    return View(classList);
        //}

        public ActionResult Index()
        {
            if (Session["memid"] == null) {
                Session["memid"] = "";
            }
            ViewBag.age = db.Age_Restriction.ToList();
            //ViewBag.joinTime = db.Activity_Details.Where(m => m.actId == actId && m.appvStatus == true).Count();
            Finalchoose fc = new Finalchoose()
            {
                vwActList = db.vw_Activities.Where(m => m.actDeadline != DateTime.Now).ToList(),
                joinfunlist = db.Join_Fun_Activities.ToList()
                
            };
            GetSelectList();
            //var vwAct = fc;
            return View(fc);
        }

      
        //
        public ActionResult GetActdetail(string actId, string MemID)
        {
            var onlyDetail=db.Activity_Details.Where(m => m.actId == actId && m.memId == MemID).FirstOrDefault();
            return Json(onlyDetail);
        }


        public ActionResult AddActdetail(string actId,string MemID)
        {
            m.actId = actId;
            m.memId = MemID;
            m.appvDate = DateTime.Now;
            db.Activity_Details.Add(m);
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public void ChangeAppv(string actId, string MemID)
        {
            GetActdetail(actId, MemID);


        }

        public ActionResult Delete(string actId, string MemID)
        {
            m.actId = actId;
            m.memId = MemID;
            db.Activity_Details.Remove(m);
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }


        //public void ChangeKeep(string actId, string MemID)
        //{
        //    var deadtime = db.Join_Fun_Activities.Where(m => m.actDeadline == DateTime.Now).ToList();
        //    foreach (var time in deadtime)
        //    {
        //        time.keepAct = false;
        //    }

        //}

        public ActionResult Details(string actId, string memID, string actClassId)
        {
            if (actId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.memID = memID;
            ViewBag.actClassId = actClassId;
            ViewBag.actId = actId;
            //ViewBag.memID = memID;
            ViewBag.joinTime = db.Activity_Details.Where(m => m.actId == actId && m.appvStatus == true).Count();
            ActClass ACT = new ActClass()
            {
                vwActivityList = db.vw_Activities.Where(m => m.actId == actId).ToList(),
                ActivityList = db.Join_Fun_Activities.Where(m => m.actId == actId).ToList(),
                //MemberList = db.Member.Where(m=>m.memId==memID).ToList(),
                members = db.Member.ToList(),
                MBoard = db.Message_Board.Where(m => m.actId == actId && m.keepMboard == true).ToList(),
                ActDetails=db.Activity_Details.Where(m => m.actId == actId && m.memId == memID).ToList()

            };
           
            ViewBag.Picture = db.Photos_of_Activities.Where(m => m.actId == actId).ToList();
            ViewBag.allpic = db.Photos_of_Activities.ToList();
            ViewBag.defaultPic = db.Activity_Class.Where(m => m.actClassId == actClassId).FirstOrDefault().Photos;
            return View(ACT);
        }

      
        //留言action
        [HttpPost]
        public ActionResult AddComment(string id, string memID, string comment)
        {
            Message_Board board = new Message_Board();
            string serial = db.Database.SqlQuery<string>("Select dbo.GetMBoardSerial()").FirstOrDefault();
            board.actId = id;
            board.mboardSerial = serial;
            board.memId = memID;
            board.boardMessage = comment;
            board.messageTime = DateTime.Now;
            board.keepMboard = true;
            db.Message_Board.Add(board);
            db.SaveChanges();

            //發出通知訊息給被標記的會員
            var member = db.Member.ToList();
            foreach (var item in member)
            {
                if (comment.StartsWith("@" + item.memNick))
                {
                    Notification message = new Notification();
                    message.NotiSerial = db.Database.SqlQuery<string>("Select dbo.GetNoteId()").FirstOrDefault();
                    message.InstanceId = serial;
                    message.ToMemId = item.memId;
                    message.NotiTitle = "留言板訊息";
                    message.NotifContent = comment;
                    message.timeReceived = DateTime.Now;
                    message.keepNotice = true;
                    db.Notification.Add(message);
                    db.SaveChanges();
                }
            }
            //new { mid = serial }
            return Json(true, JsonRequestBehavior.AllowGet);
        }


        public ActionResult FinalChoose(string actClassId,int? ageRestrict, int? gender, int? maxNumPeople, int? maxBudge, int? paymentTerm, int? actCounty)
        {
            GetSelectList();
            //var vwActivities = fc1.vwActList.Where(m => m.keepAct == true).ToList();
            var vwActivities = db.vw_Activities.Where(m => m.keepAct == true).ToList();

            if (actClassId != null)
            {
                vwActivities = vwActivities.Where(m => m.actClassId == actClassId).ToList();
            }
            if (ageRestrict != null)
            {
                vwActivities = vwActivities.Where(m => m.ageRestrict == ageRestrict).ToList();
            }
            if (gender != null)
            {
                vwActivities = vwActivities.Where(m => m.genderSerial == gender).ToList();
            }
            if (maxNumPeople != null)
            {
                vwActivities = vwActivities.Where(m => m.peoSerial == maxNumPeople).ToList();
            }
            if (maxBudge != null)
            {
                vwActivities = vwActivities.Where(m => m.BudgetNo == maxBudge).ToList();
            }
            if (paymentTerm != null)
            {
                vwActivities = vwActivities.Where(m => m.paymentSerial == paymentTerm).ToList();
            }
            if (actCounty != null)
            {
                vwActivities = vwActivities.Where(m => m.CountyNo == actCounty).ToList();
            }
          
           
            Finalchoose fc1 = new Finalchoose()
            {
                vwActList = vwActivities.ToList(),
                joinfunlist = db.Join_Fun_Activities.ToList()
            };
          
            return View(fc1);


        }

        //關鍵字搜尋
        public ActionResult Search(string keyword)
        {
            GetSelectList();
            if (keyword == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
          
            var searchResult = db.vw_Activities.Where(m => m.actTopic.Contains(keyword) || m.actDescription.Contains(keyword) || m.CountyName.Contains(keyword) || m.DistrictName.Contains(keyword) || m.actRoad.Contains(keyword) || m.hashTag.Contains(keyword) || m.memNick.Contains(keyword)).ToList();
            Finalchoose fc2 = new Finalchoose()
            {
                vwActList = searchResult.ToList(),
                joinfunlist = db.Join_Fun_Activities.ToList()
            };


            return View(fc2);
        }




        public ActionResult Create()
        {
            //if (Session["memid"] == null)
            //{
            //    return RedirectToAction("Index");
            //}
            //ViewBag.Drop = GetDropList();
            GetSelectList();
            return View();
        }

        [HttpPost]
        public ActionResult Create(Join_Fun_Activities act, HttpPostedFileBase[] picture)
        {
            //呼叫Sql系統函數GetActId()取得新增的活動ID
            string actId = db.Database.SqlQuery<string>("Select dbo.GetActId()").FirstOrDefault();
            act.actId = actId;
            act.hostId = "M000000003";//Session["memId"].ToString();
            //將Dropdown List的值取回 ---start--- 
            act.maxBudget = Int16.Parse(Request["Budget_Restrict"]);
            //將Dropdown List的值取回 ---end--- 
            act.actTime = DateTime.Parse(Request["actTime"]);
            act.actDeadline = DateTime.Parse(Request["actDeadline"]);
            act.keepAct = true;
            db.Join_Fun_Activities.Add(act);
            db.SaveChanges();
            string fileName = "";
            if (picture[0] != null)
            {
                for (int i = 0; i < picture.Length; i++)
                {
                    HttpPostedFileBase file = picture[i];
                    Photos_of_Activities photo = new Photos_of_Activities();
                    if (file != null)
                    {
                        photo.PhotoSerial = db.Database.SqlQuery<string>("Select dbo.GetPhotoId()").FirstOrDefault();
                        photo.actId = actId;
                        fileName = photo.PhotoSerial + photo.actId + ".jpg";
                        // 將檔案儲存到網站的Photos資料夾下
                        file.SaveAs(Server.MapPath("~/Photos/Activities/" + fileName)); //存入Photos資料夾
                        photo.actPics = "~/Photos/Activities/" + fileName;
                        db.Photos_of_Activities.Add(photo);
                        db.SaveChanges();
                    }
                    else
                    {
                        GetSelectList();
                        ViewBag.UploadError = "請選擇要上傳的圖片";
                        return View();
                    }
                }
            }
            else
            {
                Photos_of_Activities photo = new Photos_of_Activities();
                photo.PhotoSerial = db.Database.SqlQuery<string>("Select dbo.GetPhotoId()").FirstOrDefault();
                photo.actId = actId;
                switch (act.actClassId)
                {
                    case "cls001":
                        photo.actPics = "~/Photos/ClassIMG/活動.jpg";
                        break;
                    case "cls002":
                        photo.actPics = "~/Photos/ClassIMG/休閒.jpg";
                        break;
                    case "cls003":
                        photo.actPics = "~/Photos/ClassIMG/商務.jpg";
                        break;
                }
                db.Photos_of_Activities.Add(photo);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Edit(string actId)
        {
            //if (Session["memId"] == null)
            //{
            //    return RedirectToAction("Index");
            //}
            Join_Fun_Activities act = db.Join_Fun_Activities.Find(actId);
            if (act == null)
            {
                return HttpNotFound();
            }
            GetSelectList(actId);
            if(act.acceptDrop == true)
            {
                ViewBag.Drop = "是";
            }
            else
            {
                ViewBag.Drop = "否";
            }

            ViewBag.photo = db.Photos_of_Activities.Where(m => m.actId == actId).ToList();

            return View(act);
        }

        [HttpPost]
        public ActionResult Edit(string actId, FormCollection form, HttpPostedFileBase picture)
        {
            var act = db.Join_Fun_Activities.Where(m => m.actId == actId).FirstOrDefault();
            //string actTopic = form["actTopic"].ToString();
            //string actDescription = form["actDescription"].ToString();
            //short payment = Int16.Parse(form["Payment_Restriction"].ToString());
            //act.actTopic = actTopic;
            //act.actDescription = actDescription;
            //act.paymentTerm = payment;

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(string actId)
        {
            var act = db.Join_Fun_Activities.Where(m => m.actId == actId).FirstOrDefault();
            act.keepAct = false;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Report(string id, string reporterID)
        { 
            if (db.Member.Any(m => m.memId == id))
            {
                ViewBag.Type = "會員";
                ViewBag.TypeID = "vcls0001";
            }
            else if (db.Join_Fun_Activities.Any(m => m.actId == id))
            {
                ViewBag.Type = "揪團活動";
                ViewBag.TypeID = "vcls0002";
            }
            else if (db.Member_Remarks.Any(m => m.remarkSerial == id))
            {
                ViewBag.Type = "會員評價";
                ViewBag.TypeID = "vcls0003";
            }
            else
            {
                ViewBag.Type = "留言板";
                ViewBag.TypeID = "vcls0004";
            }

            ViewBag.ID = id;
            if (reporterID.StartsWith("M"))
            {
                ViewBag.MemID = reporterID;
            }
            else
            {
                ViewBag.AdmID = reporterID;
            }


            return View();
        }

        [HttpPost]
        public ActionResult Report(Violation violate)
        {
            violate.vioId = db.Database.SqlQuery<string>("Select dbo.GetVioId()").FirstOrDefault();
            violate.vioReportTime = DateTime.Now;
            db.Violation.Add(violate);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult Messages(string memID)
        {
            var message = db.Notification.Where(m => m.ToMemId == memID).ToList();
            return View(message);
        }

        [HttpPost]
        public ActionResult GetMStatus(string id)
        {
            int count = db.Notification.Count(m => m.ToMemId == id && m.readYet == false);
            return Json(count, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateRStatus(string serial, bool isRead)
        {
            var message = db.Notification.Find(serial);
            message.readYet = isRead;
            db.SaveChanges();

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RemoveMessage(string serial, bool keep)
        {
            var message = db.Notification.Find(serial);
            message.keepNotice = keep;
            db.SaveChanges();

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteMessage(string serial)
        {
            var message = db.Notification.Find(serial);
            db.Notification.Remove(message);
            db.SaveChanges();

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MContent(string serial)
        {
            if (serial != null && Session["memid"] != null)
            {
                var message = db.Notification.Find(serial);
                message.readYet = true;
                db.SaveChanges();
                return View(message);
            }
            return RedirectToAction("Messages");
        }

        //建立"新增"或"編輯"有對應資料表Dropdown list
        private void GetSelectList()
        {
            var budget = db.Budget_Restriction.ToList();
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in budget)
            {
                list.Add(new SelectListItem() { Text = item.Budget.ToString("NT$#"), Value = item.BudgetNo.ToString() });
            }
            ViewBag.Activity_Class = new SelectList(db.Activity_Class, "actClassId", "actClassName");
            ViewBag.Age_Restriction = new SelectList(db.Age_Restriction, "serial", "age");
            ViewBag.Gender_Restriction = new SelectList(db.Gender_Restriction, "genderSerial", "gender");
            ViewBag.People_Restriction = new SelectList(db.People_Restriction, "peoSerial", "PeoRestriction");
            ViewBag.Budget_Restriction = new SelectList(db.Budget_Restriction, "BudgetNo", "Budget");
            ViewBag.Budget_Restrict = list;
            ViewBag.Payment_Restriction = new SelectList(db.Payment_Restriction, "paymentSerial", "payment");
            ViewBag.County = new SelectList(db.County, "CountyNo", "CountyName");
            ViewBag.District = new SelectList(db.District, "DistrictSerial", "DistrictName");
            ViewBag.ActClass = new SelectList(db.Activity_Class, "actClassId", "actClassName");
        }
        private void GetSelectList(string actId)
        {
            var act = db.Join_Fun_Activities.Find(actId);
            var budget = db.Budget_Restriction.ToList();
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in budget)
            {
                if (item.BudgetNo == act.maxBudget)
                    list.Add(new SelectListItem() { Text = item.Budget.ToString("NT$#"), Value = item.BudgetNo.ToString(), Selected = true });
                else
                    list.Add(new SelectListItem() { Text = item.Budget.ToString("NT$#"), Value = item.BudgetNo.ToString() });
            }
            ViewBag.Activity_Class = new SelectList(db.Activity_Class, "actClassId", "actClassName", act.actClassId);
            ViewBag.Age_Restriction = new SelectList(db.Age_Restriction, "serial", "age", act.ageRestrict);
            ViewBag.Gender_Restriction = new SelectList(db.Gender_Restriction, "genderSerial", "gender", act.gender);
            ViewBag.People_Restriction = new SelectList(db.People_Restriction, "peoSerial", "PeoRestriction", act.maxNumPeople);
            ViewBag.Budget_Restriction = new SelectList(db.Budget_Restriction, "BudgetNo", "Budget", act.maxBudget);
            ViewBag.Budget_Restrict = list;
            ViewBag.Payment_Restriction = new SelectList(db.Payment_Restriction, "paymentSerial", "payment", act.paymentTerm);
            ViewBag.County = new SelectList(db.County, "CountyNo", "CountyName", act.actCounty);
            ViewBag.District = new SelectList(db.District, "DistrictSerial", "DistrictName", act.actDistrict);
        }

        //建立"acceptDrop"的Dropdow list
        //private List<SelectListItem> GetDropList()
        //{
        //    List<SelectListItem> list = new List<SelectListItem>();
        //    list.AddRange(new[] {
        //        new SelectListItem() { Text = "是", Value = "True", Selected = true },
        //        new SelectListItem() { Text = "否", Value = "False", Selected = false },
        //    });
        //    return list;
        //}

        //傳回活動ID取得圖片,給model無法直接關聯Photo_of_Activities使用
        public FileContentResult GetActPhoto(string actId)
        {
            if (db.Photos_of_Activities.Any(m => m.actId == actId))

            {
                string photo = db.Photos_of_Activities.Where(m => m.actId == actId).FirstOrDefault().actPics;
                if (photo != null)
                {
                    //將圖片轉成byte[] 傳給View
                    string path = Server.MapPath(photo);
                    byte[] image = System.IO.File.ReadAllBytes(path);
                    return base.File(image, "image/jpeg");
                }

            }
            return null;
        }

        //傳預設類別圖片
        public FileContentResult GetIndexPhoto(string actId, string actClassId)
        {
            bool flag = true;
            int count = 0;
            bool IsSameactId = db.Photos_of_Activities.Any(m => m.actId == actId);
            if (IsSameactId)
            {
                string photo = db.Photos_of_Activities.Where(m => m.actId == actId).FirstOrDefault().actPics;
                string path = Server.MapPath(photo);
                byte[] image = System.IO.File.ReadAllBytes(path);
                return base.File(image, "image/jpeg");
            }
            string classphoto = db.Activity_Class.Where(m => m.actClassId == actClassId).FirstOrDefault().Photos;
                string path2 = Server.MapPath(classphoto);
                byte[] image2 = System.IO.File.ReadAllBytes(path2);
                return base.File(image2, "image2/jpg");
        }


    }
}