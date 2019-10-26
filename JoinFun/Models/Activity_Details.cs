namespace JoinFun.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Activity_Details
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string memId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string actId { get; set; }

        public bool appvStatus { get; set; }

        public DateTime? appvDate { get; set; }

        public virtual Join_Fun_Activities Join_Fun_Activities { get; set; }

        public virtual Member Member { get; set; }
    }
}
