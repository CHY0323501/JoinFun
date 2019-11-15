using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JoinFun.Models;

namespace JoinFun.ViewModel
{
    public class BlockMemberVM
    {
        public List<Blacklist> Blacklist { get; set; }
        public List<Member> Member { get; set; }
    }
}