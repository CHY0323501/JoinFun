namespace JoinFun.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Member")]
    public partial class Member
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Member()
        {
            Acc_Pass = new HashSet<Acc_Pass>();
            Activity_Details = new HashSet<Activity_Details>();
            Blacklist = new HashSet<Blacklist>();
            Bookmark_Details = new HashSet<Bookmark_Details>();
            Chat_Records = new HashSet<Chat_Records>();
            Comment = new HashSet<Comment>();
            Dietary_Preference = new HashSet<Dietary_Preference>();
            Fans = new HashSet<Fans>();
            FollowUp = new HashSet<FollowUp>();
            Friendship = new HashSet<Friendship>();
            Habit = new HashSet<Habit>();
            Member_Remarks = new HashSet<Member_Remarks>();
            Message_Board = new HashSet<Message_Board>();
            Notification = new HashSet<Notification>();
            Social_Net_ID = new HashSet<Social_Net_ID>();
            Violation = new HashSet<Violation>();
        }

        [Key]
        [StringLength(10)]
        public string memId { get; set; }

        [Required]
        [StringLength(15)]
        public string memNick { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(24)]
        public string cellPhone { get; set; }

        public short memCounty { get; set; }

        public short memDistrict { get; set; }

        public DateTime timeReg { get; set; }

        public DateTime Birthday { get; set; }

        public short numViolate { get; set; }

        public bool Suspend { get; set; }

        [Required]
        [StringLength(1)]
        public string Sex { get; set; }

        public bool Approved { get; set; }

        public string Introduction { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Acc_Pass> Acc_Pass { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Activity_Details> Activity_Details { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Blacklist> Blacklist { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Bookmark_Details> Bookmark_Details { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Chat_Records> Chat_Records { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comment { get; set; }

        public virtual County County { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Dietary_Preference> Dietary_Preference { get; set; }

        public virtual District District { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Fans> Fans { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FollowUp> FollowUp { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Friendship> Friendship { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Habit> Habit { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Member_Remarks> Member_Remarks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Message_Board> Message_Board { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Notification> Notification { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Social_Net_ID> Social_Net_ID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Violation> Violation { get; set; }
    }
}
