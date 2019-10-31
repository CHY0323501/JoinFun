using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JoinFun.Models;

namespace JoinFun.ViewModel
{
    public class MemberViewModel
    {
        public List<Member> Member { get; set; }
        public List<Acc_Pass> Acc_Pass { get; set; }
        public List<Blacklist> Blacklist { get; set; }
        public List<Bookmark_Details> Bookmark_Details { get; set; }
        public List<Fans> Fans { get; set; }
        public List<FollowUp> FollowUp { get; set; }
        public List<Friendship> Friendship { get; set; }
        public List<Member_Remarks> Member_Remarks { get; set; }
    }
}