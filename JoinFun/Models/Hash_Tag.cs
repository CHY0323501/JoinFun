namespace JoinFun.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Hash_Tag
    {
        [Key]
        [StringLength(10)]
        public string hashSerial { get; set; }

        [Required]
        [StringLength(10)]
        public string actId { get; set; }

        [Required]
        [StringLength(30)]
        public string hashContent { get; set; }

        public virtual Join_Fun_Activities Join_Fun_Activities { get; set; }
    }
}
