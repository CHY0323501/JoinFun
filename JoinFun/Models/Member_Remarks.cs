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
    using System.ComponentModel.DataAnnotations;


    [MetadataType(typeof(MetaMember_Remarks))]
    public partial class Member_Remarks
    {
        public string remarkSerial { get; set; }
        public string actId { get; set; }
        public string FromMemId { get; set; }
        public string ToMemId { get; set; }
        public bool keepRemark { get; set; }
        public short remarkStar { get; set; }
        public string remarkContent { get; set; }
        public System.DateTime remarkTime { get; set; }
    
        public virtual Join_Fun_Activities Join_Fun_Activities { get; set; }
        public virtual Member Member { get; set; }
    }
}
