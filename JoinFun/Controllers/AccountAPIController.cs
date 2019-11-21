using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using JoinFun.Models;

namespace JoinFun.Controllers
{
    public class AccountAPIController : ApiController
    {
        JoinFunEntities db = new JoinFunEntities();



        // POST: api/AccountAPI
        public IHttpActionResult Get()
        {
            //var acc= db.Acc_Pass.ToList();
            List<string> acc = new List<string>();
            foreach (var i in db.Acc_Pass)
            {
                acc.Add(i.Account);
            }

            return Ok(acc);
        }

    }
}
