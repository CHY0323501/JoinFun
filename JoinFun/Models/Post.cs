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
    
    public partial class Post
    {
        public string postSerial { get; set; }
        public string admId { get; set; }
        public string postTitle { get; set; }
        public string postContent { get; set; }
        public System.DateTime postTime { get; set; }
        public string postPics { get; set; }
        public bool ShowInCarousel { get; set; }
    
        public virtual Administrator Administrator { get; set; }
    }
}
