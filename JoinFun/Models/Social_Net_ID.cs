namespace JoinFun.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Social_Net_ID
    {
        [Key]
        [StringLength(10)]
        public string socialSerial { get; set; }

        [Required]
        [StringLength(10)]
        public string memId { get; set; }

        [Required]
        [StringLength(20)]
        public string socialId { get; set; }

        public virtual Member Member { get; set; }
    }
}
