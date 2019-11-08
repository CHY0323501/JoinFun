using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using JoinFun.Models;

namespace JoinFun.Controllers
{
    public class getClassDescriptionController : ApiController
    {
        JoinFunEntities db = new JoinFunEntities();
        public IHttpActionResult Get(string id)
        {
            var getDescrip = db.Activity_Class.Where(a => a.actClassId == id).ToList();
            if (getDescrip == null)
                return NotFound();

            List<string> result = new List<string>();

            for (int i = 0; i < getDescrip.Count; i++)
            {
                //result.Add(getDescrip[i].actClassId);
                result.Add(getDescrip[i].actClassDescrip);
            }
            return Ok(result); //回傳status狀態碼200，表示傳送資料成功

        }
    }
}
