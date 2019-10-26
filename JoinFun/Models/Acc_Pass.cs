namespace JoinFun.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Acc_Pass
    {
        [Key]
        [StringLength(10)]
        [Required]
        public string Account { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [StringLength(10)]
        public string memId { get; set; }

        [Required]
        public string Salt { get; set; }

        public virtual Member Member { get; set; }
    }
}
