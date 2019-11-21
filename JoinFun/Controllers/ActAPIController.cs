using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using JoinFun.Models;

namespace JoinFun.Controllers
       
{
    public class ActAPIController : ApiController
    {
        JoinFunEntities db = new JoinFunEntities();

        //GET: api/ActAPI
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET: api/ActAPI/5
        public IHttpActionResult Get(string actId, string memID)
        {
           
            var MemJoin = db.Activity_Details.Where(m => m.actId == actId && m.memId == memID).FirstOrDefault();
            return Ok(MemJoin);
        }

        // POST: api/ActAPI
        public IHttpActionResult Post(string actId, string memID)
        {
            Activity_Details memact = new Activity_Details();
            memact.actId = actId;
            memact.memId = memID;
            db.Activity_Details.Add(memact);
            db.SaveChanges();
            return Ok();

        }



        // DELETE: api/ActAPI/5
        public IHttpActionResult Delete(string actId, string memID/*,bool IsAppv*/)
        {
            //{if (IsAppv)
            //    {

            var cancelJoin = db.Activity_Details.Where(m => m.actId == actId && m.memId == memID).FirstOrDefault();
            if (cancelJoin != null)
            
                db.Activity_Details.Remove(cancelJoin);
                db.SaveChanges();
                return Ok();

                //    }
                //}


            
        }
    }
}
