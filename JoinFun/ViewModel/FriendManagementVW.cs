﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JoinFun.Models;

namespace JoinFun.ViewModel
{
    public class FriendManagementVW
    {
        public List<Member> Member { get; set; }
        public List<vw_FriendShip> vw_FriendShip { get; set; }
    }
}