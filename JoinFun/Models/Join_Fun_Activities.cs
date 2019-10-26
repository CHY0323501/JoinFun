namespace JoinFun.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Join_Fun_Activities
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Join_Fun_Activities()
        {
            Activity_Details = new HashSet<Activity_Details>();
            Bookmark_Details = new HashSet<Bookmark_Details>();
            Hash_Tag = new HashSet<Hash_Tag>();
            Member_Remarks = new HashSet<Member_Remarks>();
            Message_Board = new HashSet<Message_Board>();
            Photos_of_Activities = new HashSet<Photos_of_Activities>();
        }

        [Key]
        [StringLength(10)]
        public string actId { get; set; }

        [Required]
        [StringLength(6)]
        public string actClassId { get; set; }

        [Required]
        [StringLength(10)]
        public string hostId { get; set; }

        [Required]
        [StringLength(30)]
        public string actTopic { get; set; }

        public DateTime actTime { get; set; }

        public DateTime actDeadline { get; set; }

        public string actDescription { get; set; }

        public short ageRestrict { get; set; }

        public short gender { get; set; }

        public short maxNumPeople { get; set; }

        public short maxBudget { get; set; }

        public short actCounty { get; set; }

        public short actDistrict { get; set; }

        [Required]
        [StringLength(40)]
        public string actRoad { get; set; }

        public short paymentTerm { get; set; }

        public bool acceptDrop { get; set; }

        public int clickTimes { get; set; }

        public bool keepAct { get; set; }

        public virtual Activity_Class Activity_Class { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Activity_Details> Activity_Details { get; set; }

        public virtual Age_Restriction Age_Restriction { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Bookmark_Details> Bookmark_Details { get; set; }

        public virtual Budget_Restriction Budget_Restriction { get; set; }

        public virtual County County { get; set; }

        public virtual District District { get; set; }

        public virtual Gender_Restriction Gender_Restriction { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Hash_Tag> Hash_Tag { get; set; }

        public virtual People_Restriction People_Restriction { get; set; }

        public virtual Payment_Restriction Payment_Restriction { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Member_Remarks> Member_Remarks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Message_Board> Message_Board { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Photos_of_Activities> Photos_of_Activities { get; set; }
    }
}
