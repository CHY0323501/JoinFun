namespace JoinFun.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Friendship")]
    public partial class Friendship
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string memId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string friendMemId { get; set; }

        [Required]
        [StringLength(15)]
        public string friendNick { get; set; }

        public bool Approved { get; set; }

        public virtual Member Member { get; set; }
    }
}
