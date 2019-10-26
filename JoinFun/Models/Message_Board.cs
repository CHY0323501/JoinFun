namespace JoinFun.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Message_Board
    {
        [Key]
        [StringLength(10)]
        public string mboardSerial { get; set; }

        [Required]
        [StringLength(10)]
        public string actId { get; set; }

        [Required]
        [StringLength(10)]
        public string memId { get; set; }

        [Required]
        public string boardMessage { get; set; }

        public DateTime messageTime { get; set; }

        public bool keepMboard { get; set; }

        public virtual Join_Fun_Activities Join_Fun_Activities { get; set; }

        public virtual Member Member { get; set; }
    }
}
