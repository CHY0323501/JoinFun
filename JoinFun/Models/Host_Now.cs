//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace JoinFun.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Host_Now
    {
        public string actId { get; set; }
        public string actClassId { get; set; }
        public string hostId { get; set; }
        public string actTopic { get; set; }
        public System.DateTime actTime { get; set; }
        public System.DateTime actDeadline { get; set; }
        public string actDescription { get; set; }
        public short ageRestrict { get; set; }
        public short gender { get; set; }
        public short maxNumPeople { get; set; }
        public short maxBudget { get; set; }
        public short actCounty { get; set; }
        public short actDistrict { get; set; }
        public string actRoad { get; set; }
        public short paymentTerm { get; set; }
        public bool acceptDrop { get; set; }
        public int clickTimes { get; set; }
        public bool keepAct { get; set; }
        public string hashTag { get; set; }
        public string CountyName { get; set; }
        public string DistrictName { get; set; }
    }
}
