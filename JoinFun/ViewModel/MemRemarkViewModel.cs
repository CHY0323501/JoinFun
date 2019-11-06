using JoinFun.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JoinFun.ViewModel
{
    public class MemRemarkViewModel
    {
        public List<vw_Host_Remarks> vw_Host_Remarks { get; set; }
        public List<vw_Participant_Remarks> vw_Participant_Remarks { get; set; }
    }
}