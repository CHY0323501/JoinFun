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
        public List<Member> MemberList { get; set; }
        public List<Member> members { get; set; }
        public List<Message_Board> MBoard { get; set; }

        public List<Photos_of_Activities> PhotoList { get; set; }
    }

    public class Finalchoose
    {
        public List<vw_Activities> vwActList { get; set; }
        public List<Age_Restriction> agelist { get; set; }
        public List<Budget_Restriction> budgetlist { get; set; }
        public List<Activity_Class> Actlist  { get; set; }
        public List<Payment_Restriction> paymentlist { get; set; }
        public List<People_Restriction> peoplelist { get; set; }
        public List<Gender_Restriction> genderlist { get; set; }
        public List<County> countylist { get; set; }
        public List<Join_Fun_Activities> joinfunlist { get; set; }
    }
}