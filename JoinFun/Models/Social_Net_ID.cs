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

    [MetadataType(typeof(MetaSocial_Net_ID))]
    public partial class Social_Net_ID
    {
        public string socialSerial { get; set; }
        public string memId { get; set; }
        public string socialId { get; set; }
    
        public virtual Member Member { get; set; }
    }
}
