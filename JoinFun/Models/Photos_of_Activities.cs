﻿//------------------------------------------------------------------------------
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

    [MetadataType(typeof(MetadataPhotos_of_Activities))]
    public partial class Photos_of_Activities
    {
        public string PhotoSerial { get; set; }
        public string actId { get; set; }
        public byte[] actPics { get; set; }
    
        public virtual Join_Fun_Activities Join_Fun_Activities { get; set; }
    }
}
