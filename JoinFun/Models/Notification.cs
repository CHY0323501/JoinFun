namespace JoinFun.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Notification")]
    public partial class Notification
    {
        [Key]
        [StringLength(10)]
        public string NotiSerial { get; set; }

        [Required]
        [StringLength(10)]
        public string ToMemId { get; set; }

        [Required]
        public string NotifContent { get; set; }

        public bool readYet { get; set; }

        public virtual Member Member { get; set; }
    }
}
