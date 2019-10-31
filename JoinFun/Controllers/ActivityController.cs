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
                ActivityList = db.vw_Activities.Where(m => m.actClassId == actClassId).ToList(),
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
            return View(act);
        }

        public ActionResult Create()
        {
            //if (Session["memid"] == null)
            //{
            //    return RedirectToAction("Index");
            //}

            ViewBag.Drop = GetDropList();
            GetSelectList();
            return View();
        }

        [HttpPost]
        public ActionResult Create(Join_Fun_Activities act, FormCollection form, Photos_of_Activities photo, HttpPostedFileBase picture)
        {
            //將Dropdown List的值取回 start
            string clsValue = form["Activity_Class"].ToString();
            short age = Int16.Parse(form["Age_Restriction"].ToString());
            short gender = Int16.Parse(form["Gender_Restriction"].ToString());
            short people = Int16.Parse(form["People_Restriction"].ToString());
            short budget = Int16.Parse(form["Budget_Restriction"].ToString());
            short payment = Int16.Parse(form["Payment_Restriction"].ToString());
            short county = Int16.Parse(form["County"].ToString());
            short district = Int16.Parse(form["District"].ToString());
            bool drop = Convert.ToBoolean(form["Drop"].ToString());
            //Dropdown List值 end
            //呼叫Sql系統函數GetActId()取得新增的活動ID
            string actId = db.Database.SqlQuery<string>("Select dbo.GetActId()").FirstOrDefault();

            act.actId = actId;
            act.hostId = Session["memId"].ToString();
            act.actClassId = clsValue;
            act.ageRestrict = age;
            act.gender = gender;
            act.maxNumPeople = people;
            act.maxBudget = budget;
            act.paymentTerm = payment;
            act.actCounty = county;
            act.actDistrict = district;
            act.acceptDrop = drop;

            if (picture != null)
            {
                //Stream fs = picture.InputStream;
                //BinaryReader br = new BinaryReader(fs);
                //byte[] bytes = br.ReadBytes((Int32)fs.Length);
                //photo.actPics = bytes;
                photo.actPics = new byte[picture.ContentLength];
                picture.InputStream.Read(photo.actPics, 0, picture.ContentLength);
            }
            else
            {
                GetSelectList();
                ViewBag.UploadError = "請選擇要上傳的圖片";
                return View();
            }

            photo.PhotoSerial = db.Database.SqlQuery<string>("Select dbo.GetPhotoId()").FirstOrDefault();
            photo.actId = actId;
            db.Join_Fun_Activities.Add(act);
            db.Photos_of_Activities.Add(photo);
            db.SaveChanges();
            return RedirectToAction("Index");

            //return RedirectToAction("Create");
        }

        public ActionResult Edit(string actId)
        {
            //if (Session["memId"] == null)
            //{
            //    return RedirectToAction("Index");
            //}
            var act = db.Join_Fun_Activities.Where(m => m.actId == actId).FirstOrDefault();

            GetSelectList();
            //Formmethod.Post需要傳actId參數,宣告ViewBag.actId存actId參數
            ViewBag.actId = actId;
            ViewBag.Drop = new SelectList(GetDropList(), "Value", "Text", act.acceptDrop);
            ViewBag.photo = db.Photos_of_Activities.Where(m => m.actId == actId).ToList();

            return View(act);
        }

        [HttpPost]
        public ActionResult Edit(string actId, FormCollection form, HttpPostedFileBase picture)
        {
            var act = db.Join_Fun_Activities.Where(m => m.actId == actId).FirstOrDefault();
            string clsValue = form["Activity_Class"].ToString();
            short age = Int16.Parse(form["Age_Restriction"].ToString());
            short people = Int16.Parse(form["People_Restriction"].ToString());
            short budget = Int16.Parse(form["Budget_Restriction"].ToString());
            short payment = Int16.Parse(form["Payment_Restriction"].ToString());
            short county = Int16.Parse(form["County"].ToString());
            short district = Int16.Parse(form["District"].ToString());
            bool drop = Convert.ToBoolean(form["Drop"].ToString());
            act.actClassId = clsValue;
            act.ageRestrict = age;
            act.maxNumPeople = people;
            act.maxBudget = budget;
            act.paymentTerm = payment;
            act.actCounty = county;
            act.actDistrict = district;
            act.acceptDrop = drop;
            ViewBag.actId = actId;

            var photo = db.Photos_of_Activities.Where(m => m.actId == actId).FirstOrDefault();
            //if (picture == null && photo.actPics != null)
            //{
            //    db.SaveChanges();
            //}
            if (picture != null)
            {
                photo.actPics = new byte[picture.ContentLength];
                picture.InputStream.Read(photo.actPics, 0, picture.ContentLength);
            }
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

        //建立"新增"或"編輯"有對應資料表Dropdown list
        private void GetSelectList()
        {
            ViewBag.Activity_Class = new SelectList(db.Activity_Class, "actClassId", "actClassName");
            ViewBag.Age_Restriction = new SelectList(db.Age_Restriction, "serial", "age");
            ViewBag.Gender_Restriction = new SelectList(db.Gender_Restriction, "genderSerial", "gender");
            ViewBag.People_Restriction = new SelectList(db.People_Restriction, "peoSerial", "PeoRestriction");
            ViewBag.Budget_Restriction = new SelectList(db.Budget_Restriction, "BudgetNo", "Budget");
            ViewBag.Payment_Restriction = new SelectList(db.Payment_Restriction, "paymentSerial", "payment");
            ViewBag.County = new SelectList(db.County, "CountyNo", "CountyName");
            ViewBag.District = new SelectList(db.District, "DistrictSerial", "DistrictName");
        }

        //建立"acceptDrop"的Dropdow list
        private List<SelectListItem> GetDropList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.AddRange(new[] {
                new SelectListItem() { Text = "是", Value = "True", Selected = true },
                new SelectListItem() { Text = "否", Value = "False", Selected = false },
            });
            return list;
        }

        //取得活動照片
        public FileContentResult GetPhoto(string actId)
        {
            var photo = db.Photos_of_Activities.Where(m => m.actId == actId).FirstOrDefault().actPics;
            if (photo != null)
            {
                return File(photo, "image/jpeg");
            }
            else
            {
                return null;
            }
        }

        public ActionResult AddPhoto()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddPhoto(string actId, Photos_of_Activities photo, HttpPostedFileBase picture)
        {
            if (picture != null)
            {
                photo.actPics = new byte[picture.ContentLength];
                picture.InputStream.Read(photo.actPics, 0, picture.ContentLength);
            }

            photo.PhotoSerial = db.Database.SqlQuery<string>("Select dbo.GetPhotoId()").FirstOrDefault();
            photo.actId = actId;
            db.Photos_of_Activities.Add(photo);
            db.SaveChanges();
            return View();
        }
    }
}