using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using JoinFun.Models;
using Newtonsoft.Json;

namespace JoinFun.Controllers
{
    public class getDistrictController : ApiController
    {
        JoinFunEntities db = new JoinFunEntities();
        public IHttpActionResult Get(int countyNo)
        {
            var getDistrict = db.District.Where(a => a.CountyNo == countyNo).ToList();
            if (getDistrict == null)
                return NotFound();

            List<string> result = new List<string>();

            for (int i = 0; i < getDistrict.Count; i++)
            {
                result.Add(getDistrict[i].DistrictSerial.ToString());
                result.Add(getDistrict[i].DistrictName);
            }
            return Ok(result); //回傳status狀態碼200，表示傳送資料成功
            
        }
    }
}
