namespace JoinFun.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Post")]
    public partial class Post
    {
        [Key]
        [StringLength(10)]
        public string postSerial { get; set; }

        [Required]
        [StringLength(6)]
        public string admId { get; set; }

        [Required]
        [StringLength(30)]
        public string postTitle { get; set; }

        [Required]
        public string postContent { get; set; }

        public DateTime postTime { get; set; }

        [Column(TypeName = "image")]
        public byte[] postPics { get; set; }

        public virtual Administrator Administrator { get; set; }
    }
}
