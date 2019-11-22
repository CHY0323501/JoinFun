using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JoinFun.Models;

namespace JoinFun.ViewModel
{
    public class MyActivitiesVM
    {
        public List<Host_Now> Host_Now { get; set; }
        public List<Part_Now> Part_Now { get; set; }
        public List<Photos_of_Activities> Photos_of_Activities { get; set; }
        public List<Activity_Class> Activity_Class { get; set; }
        public List<Activity_Details> Activity_Details { get; set; }
    }
}