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
    
    public partial class Violation
    {
        public string vioId { get; set; }
        public string FromMemId { get; set; }
        public string FromAdmID { get; set; }
        public string typeId { get; set; }
        public string CorrespondingEventID { get; set; }
        public string vioTitle { get; set; }
        public string vioContent { get; set; }
        public System.DateTime vioReportTime { get; set; }
        public string implement_admId { get; set; }
        public string punishId { get; set; }
        public Nullable<System.DateTime> vioProcessTime { get; set; }
    
        public virtual Administrator Administrator { get; set; }
        public virtual Member Member { get; set; }
        public virtual Punishment Punishment { get; set; }
        public virtual Type_of_Violate Type_of_Violate { get; set; }
    }
}
