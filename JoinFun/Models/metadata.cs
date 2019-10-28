using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JoinFun.Models
{
    public class MetaAcc_Pass
    {
        [DisplayName("會員帳號")]
        [Required(ErrorMessage = "請輸入會員帳號")]
        [StringLength(10, ErrorMessage = "最多輸入10字")]
        public string Account { get; set; }
        [DisplayName("會員密碼")]
        [Required(ErrorMessage = "請輸入會員密碼")]
        [StringLength(15, ErrorMessage = "最多輸入15字")]
        public string Password { get; set; }
        [DisplayName("會員編號")]
        [Required(ErrorMessage = "請輸入會員編號")]
        public string memId { get; set; }
        public string Salt { get; set; }
    }
    public  class MetaActivity_Class
    {
        [DisplayName("類別編號")]
        [Required(ErrorMessage = "請輸入活動類別編號")]
        public string actClassId { get; set; }
        [DisplayName("類別名稱")]
        [Required(ErrorMessage = "請輸入活動類別名稱")]
        [StringLength(20, ErrorMessage = "最多輸入20字")]
        public string actClassName { get; set; }
        [DisplayName("類別說明")]
        [Required(ErrorMessage = "請輸入活動類別說明")]
        [StringLength(50, ErrorMessage = "最多輸入50字")]
        public string actClassDescrip { get; set; }
        [DisplayName("類別照片")]
        [Required(ErrorMessage = "請選擇活動類別照片")]
        public byte[] Photos { get; set; }
    }
    public  class MetaActivity_Details
    {
        [DisplayName("會員編號")]
        [Required(ErrorMessage = "請輸入會員編號")]
        public string memId { get; set; }
        [DisplayName("揪團編號")]
        [Required(ErrorMessage = "請輸入揪團編號")]
        public string actId { get; set; }
        [DisplayName("審核狀態")]
        [Required(ErrorMessage = "請選擇審核狀態")]
        public bool appvStatus { get; set; }
        [DisplayName("審核日期")]
        public Nullable<System.DateTime> appvDate { get; set; }
    }
    public  class MetaAdministrator
    {
        [DisplayName("管理員編號")]
        [Required(ErrorMessage = "請輸入管理員編號")]
        public string admId { get; set; }
        [DisplayName("管理員帳號")]
        [Required(ErrorMessage = "請輸入管理員帳號")]
        [StringLength(10, ErrorMessage = "最多輸入10字")]
        public string admAcc { get; set; }
        [DisplayName("管理員密碼")]
        [Required(ErrorMessage = "請輸入管理員密碼")]
        [StringLength(15, ErrorMessage = "最多輸入15字")]
        public string admPass { get; set; }
        [DisplayName("管理員暱稱")]
        [Required(ErrorMessage = "請輸入管理員暱稱")]
        [StringLength(10, ErrorMessage = "最多輸入10字")]
        public string admNick { get; set; }
        public string admSalt { get; set; }
    }
    public  class MetaAge_Restriction
    {
        [DisplayName("年齡限制編號")]
        [Required(ErrorMessage = "請輸入年齡限制編號")]
        public short serial { get; set; }
        [DisplayName("年齡限制")]
        [Required(ErrorMessage = "請輸入年齡限制")]
        [StringLength(8, ErrorMessage = "最多輸入8字")]
        public string age { get; set; }
    }
    public  class MetaBlacklist
    {
        [DisplayName("會員編號")]
        [Required(ErrorMessage = "請輸入會員編號")]
        public string blockedMemId { get; set; }
        [DisplayName("黑名單對象會員編號")]
        [Required(ErrorMessage = "請輸入黑名單對象會員編號")]
        public string memId { get; set; }
    }
}