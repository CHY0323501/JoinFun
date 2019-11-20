using System;
using System.Collections.Generic;
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
        // GET: Activity
        public ActionResult Index(string hostId, string actId, string actClassId = "cls001")
        {
            if (actClassId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.actClassName = db.Activity_Class.Where(m => m.actClassId == actClassId).FirstOrDefault().actClassName;
            ViewBag.actClassId = actClassId;
            ViewBag.actId = actId;
            ActClass classList = new ActClass()
            {
                vwActivityList = db.vw_Activities.Where(m => m.actClassId == actClassId && m.keepAct == true).ToList(),
                ClassList = db.Activity_Class.ToList()
            };
            return View(classList);
        }

        public ActionResult Details(string actId, string actClassId)
        {
            if (actId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.actClassId = actClassId;
            ViewBag.actId = actId;

            var act = db.vw_Activities.Where(m => m.actId == actId).ToList();
            ViewBag.Picture = db.Photos_of_Activities.Where(m => m.actId == actId).ToList();
            return View(act);
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
        public ActionResult Create(Join_Fun_Activities act, FormCollection form, HttpPostedFileBase[] picture)
        {
            //呼叫Sql系統函數GetActId()取得新增的活動ID
            string actId = db.Database.SqlQuery<string>("Select dbo.GetActId()").FirstOrDefault();
            act.actId = actId;
            act.hostId = "M000000003";//Session["memId"].ToString();
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
                    photo.PhotoSerial = db.Database.SqlQuery<string>("Select dbo.GetPhotoId()").FirstOrDefault();
                    photo.actId = actId;
                    fileName = photo.PhotoSerial + photo.actId + ".jpg";
                    // 將檔案儲存到網站的Photos資料夾下
                    file.SaveAs(Server.MapPath("~/Photos/Activities/" + fileName)); //存入Photos資料夾
                    photo.actPics = "~/Photos/Activities/" + fileName;
                    db.Photos_of_Activities.Add(photo);
                    db.SaveChanges();
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
            //ViewBag.Drop = GetDropList();
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
            //ViewBag.Type = new SelectList(db.Type_of_Violate, "typeId", "vioClass");
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

        //建立"新增"或"編輯"有對應資料表Dropdown list
        private void GetSelectList()
        {
            //ViewBag.Activity_Class = new SelectList(db.Activity_Class, "actClassId", "actClassName");
            ViewBag.Age_Restriction = new SelectList(db.Age_Restriction, "serial", "age");
            ViewBag.Gender_Restriction = new SelectList(db.Gender_Restriction, "genderSerial", "gender");
            ViewBag.People_Restriction = new SelectList(db.People_Restriction, "peoSerial", "PeoRestriction");
            ViewBag.Budget_Restriction = new SelectList(db.Budget_Restriction, "BudgetNo", "Budget");
            ViewBag.Payment_Restriction = new SelectList(db.Payment_Restriction, "paymentSerial", "payment");
            ViewBag.County = new SelectList(db.County, "CountyNo", "CountyName");
            ViewBag.District = new SelectList(db.District, "DistrictSerial", "DistrictName");
        }
        private void GetSelectList(string actId)
        {
            var act = db.Join_Fun_Activities.Find(actId);
            ViewBag.Activity_Class = new SelectList(db.Activity_Class, "actClassId", "actClassName", act.actClassId);
            ViewBag.Age_Restriction = new SelectList(db.Age_Restriction, "serial", "age", act.ageRestrict);
            ViewBag.Gender_Restriction = new SelectList(db.Gender_Restriction, "genderSerial", "gender", act.gender);
            ViewBag.People_Restriction = new SelectList(db.People_Restriction, "peoSerial", "PeoRestriction", act.maxNumPeople);
            ViewBag.Budget_Restriction = new SelectList(db.Budget_Restriction, "BudgetNo", "Budget", act.maxBudget);
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


    }
}