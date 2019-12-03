using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JoinFun.Models;

namespace JoinFun.ViewModel
{
    public class AdmView
    {
        public List<Post> postList { get; set; }
        public List<Violation> violateList { get; set; }
        public List<Join_Fun_Activities> actList { get; set; }
        public List<Member> memList { get; set; }
        public List<Punishment> punishList { get; set; }
        public List<Member_Remarks> remarkList { get; set; }
        public List<Message_Board> mboardList { get; set; }
    }
}