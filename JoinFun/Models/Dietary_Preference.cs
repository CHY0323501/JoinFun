namespace JoinFun.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Dietary_Preference
    {
        [Key]
        [StringLength(10)]
        public string dSerial { get; set; }

        [Required]
        [StringLength(10)]
        public string memId { get; set; }

        [Required]
        [StringLength(25)]
        public string dPreference { get; set; }

        public virtual Member Member { get; set; }
    }
}
