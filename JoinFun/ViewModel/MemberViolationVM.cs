using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JoinFun.Models;

namespace JoinFun.ViewModel
{
    public class MemberViolationVM

    {
        public List<Member> Member { get; set; }
        public List<Violation> Violation { get; set; }
        public List<Punishment> Punishment { get; set; }

    }
}