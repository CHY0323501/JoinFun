namespace JoinFun.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Violation")]
    public partial class Violation
    {
        [Key]
        [StringLength(10)]
        public string vioId { get; set; }

        [StringLength(10)]
        public string FromMemId { get; set; }

        [StringLength(6)]
        public string FromAdmID { get; set; }

        [Required]
        [StringLength(8)]
        public string typeId { get; set; }

        [Required]
        [StringLength(10)]
        public string CorrespondingEventID { get; set; }

        [Required]
        [StringLength(20)]
        public string vioTitle { get; set; }

        [Required]
        public string vioContent { get; set; }

        public DateTime vioReportTime { get; set; }

        [StringLength(6)]
        public string implement_admId { get; set; }

        [StringLength(10)]
        public string punishId { get; set; }

        public DateTime? vioProcessTime { get; set; }

        public virtual Administrator Administrator { get; set; }

        public virtual Member Member { get; set; }

        public virtual Punishment Punishment { get; set; }

        public virtual Type_of_Violate Type_of_Violate { get; set; }
    }
}
