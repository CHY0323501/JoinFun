using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using JoinFun.Models;

namespace JoinFun.ViewModel
{
    public class HistoryViewModel
    {
        public List<vw_HostHistory> vw_HostHistory { get; set; }
        public List<vw_PartHistory> vw_PartHistory { get; set; }
        public List<Photos_of_Activities> Photos_of_Activities { get; set; }
        public List<Activity_Class> Activity_Class { get; set; }
        public List<Activity_Details> Activity_Details { get; set; }
        public List<Member_Remarks> Member_Remarks { get; set; }

    }
}