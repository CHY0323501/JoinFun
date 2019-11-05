using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JoinFun.Models;

namespace JoinFun.ViewModel
{
    public class ActClass
    {
        public List<vw_Activities> vwActivityList { get; set; }
        public List<Activity_Class> ClassList { get; set; }
        public List<Join_Fun_Activities> ActivityList { get; set; }

        public List<Photos_of_Activities> PhotoList { get; set; }
    }
}