namespace JoinFun.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Habit")]
    public partial class Habit
    {
        [Key]
        [StringLength(10)]
        public string HabitSerial { get; set; }

        [Required]
        [StringLength(10)]
        public string memId { get; set; }

        [Column("Habit")]
        [Required]
        [StringLength(25)]
        public string Habit1 { get; set; }

        public virtual Member Member { get; set; }
    }
}
