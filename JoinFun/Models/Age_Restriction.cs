namespace JoinFun.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Age_Restriction
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Age_Restriction()
        {
            Join_Fun_Activities = new HashSet<Join_Fun_Activities>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short serial { get; set; }

        [Required]
        [StringLength(8)]
        public string age { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Join_Fun_Activities> Join_Fun_Activities { get; set; }
    }
}
