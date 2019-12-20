using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using JoinFun.Models;

namespace JoinFun.Controllers
{
    public class AdmAPIController : ApiController
    {
        JoinFunEntities db = new JoinFunEntities();



        // POST: api/AccountAPI
        public IHttpActionResult Get()
        {
           
            List<string> acc = new List<string>();
            foreach (var i in db.Administrator)
            {
                acc.Add(i.admAcc);
            }

            return Ok(acc);
        }
        
    }
}
