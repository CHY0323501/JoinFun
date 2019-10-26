namespace JoinFun.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Photos_of_Activities
    {
        [Key]
        [StringLength(10)]
        public string PhotoSerial { get; set; }

        [Required]
        [StringLength(10)]
        public string actId { get; set; }

        [Column(TypeName = "image")]
        [Required]
        public byte[] actPics { get; set; }

        public virtual Join_Fun_Activities Join_Fun_Activities { get; set; }
    }
}
