namespace JoinFun.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Comment")]
    public partial class Comment
    {
        [StringLength(10)]
        public string commentId { get; set; }

        [Required]
        [StringLength(10)]
        public string memId { get; set; }

        [Required]
        [StringLength(30)]
        public string commentTitle { get; set; }

        [Column("Comment")]
        [Required]
        public string Comment1 { get; set; }

        public DateTime receivedTime { get; set; }

        public DateTime? reportTime { get; set; }

        public string reportContent { get; set; }

        [StringLength(6)]
        public string admId { get; set; }

        public virtual Administrator Administrator { get; set; }

        public virtual Member Member { get; set; }
    }
}
