namespace JoinFun.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Chat_Records
    {
        [Key]
        [StringLength(10)]
        public string chatSerial { get; set; }

        [Required]
        [StringLength(10)]
        public string FromMemId { get; set; }

        [Required]
        [StringLength(10)]
        public string ToMemId { get; set; }

        [Required]
        public string chatContent { get; set; }

        public DateTime Time { get; set; }

        public bool ReadYet { get; set; }

        public virtual Member Member { get; set; }
    }
}
