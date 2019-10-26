namespace JoinFun.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Gender_Restriction
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Gender_Restriction()
        {
            Join_Fun_Activities = new HashSet<Join_Fun_Activities>();
        }

        [Key]
        public short genderSerial { get; set; }

        [Required]
        [StringLength(10)]
        public string gender { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Join_Fun_Activities> Join_Fun_Activities { get; set; }
    }
}
