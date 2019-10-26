namespace JoinFun.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Activity_Class
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Activity_Class()
        {
            Join_Fun_Activities = new HashSet<Join_Fun_Activities>();
        }

        [Key]
        [StringLength(6)]
        public string actClassId { get; set; }

        [Required]
        [StringLength(20)]
        public string actClassName { get; set; }

        [Required]
        [StringLength(50)]
        public string actClassDescrip { get; set; }

        [Column(TypeName = "image")]
        public byte[] Photos { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Join_Fun_Activities> Join_Fun_Activities { get; set; }
    }
}
