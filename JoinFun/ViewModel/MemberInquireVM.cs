using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JoinFun.Models;

namespace JoinFun.ViewModel
{
    public class MemberInquireVM

    {
        public List<Member> Member { get; set; }
        public IEnumerable<Violation> Violation { get; set; }
        public List<Punishment> Punishment { get; set; }
        public List<Member_Remarks> MemberRemark { get; set; }
        public List<Message_Board> MessageBoard { get; set; }
        public List<Join_Fun_Activities> Activity { get; set; }
        public List<Type_of_Violate> Type_of_Violate { get; set; }
        public List<Punishment> punishment { get; set; }

    }
}