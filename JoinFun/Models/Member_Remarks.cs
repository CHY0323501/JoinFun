namespace JoinFun.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Member_Remarks
    {
        [Key]
        [StringLength(10)]
        public string remarkSerial { get; set; }

        [Required]
        [StringLength(10)]
        public string actId { get; set; }

        [Required]
        [StringLength(10)]
        public string FromMemId { get; set; }

        [Required]
        [StringLength(10)]
        public string ToMemId { get; set; }

        public bool keepRemark { get; set; }

        public short remarkStar { get; set; }

        public string remarkContent { get; set; }

        public DateTime remarkTime { get; set; }

        public virtual Join_Fun_Activities Join_Fun_Activities { get; set; }

        public virtual Member Member { get; set; }
    }
}
