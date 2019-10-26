using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JoinFun.Models
{
    public class Meta_Acc_Pass
    {
        [Key]
        [StringLength(10)]
        public string Account { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [StringLength(10)]
        public string memId { get; set; }

        [Required]
        public string Salt { get; set; }

    }
}