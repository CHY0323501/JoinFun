namespace JoinFun.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Blacklist")]
    public partial class Blacklist
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string blockedMemId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string memId { get; set; }

        public virtual Member Member { get; set; }
    }
}
