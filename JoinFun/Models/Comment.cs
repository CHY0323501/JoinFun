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
    
    public partial class Comment
    {
        public string commentId { get; set; }
        public string memId { get; set; }
        public string commentTitle { get; set; }
        public string Comment1 { get; set; }
        public System.DateTime receivedTime { get; set; }
        public Nullable<System.DateTime> reportTime { get; set; }
        public string reportContent { get; set; }
        public string admId { get; set; }
    
        public virtual Administrator Administrator { get; set; }
        public virtual Member Member { get; set; }
    }
}
