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

    public class MetaBudget_Restriction
    {
        [DisplayName("預算")]
        public decimal Budget { get; set; }
    }

    public class MetaCounty
    {
        [DisplayName("縣/市")]
        public string CountyName { get; set; }
    }

    public class MetaDistrict
    {
        [DisplayName("鄉/鎮/市/區")]
        public string DistrictName { get; set; }
    }

    public class MetaJoin_Fun_Activities
    {
        [DisplayName("活動ID")]
        public string actId { get; set; }
        [DisplayName("主辦人會員ID")]
        public string hostId { get; set; }
        [DisplayName("活動主題")]
        [Required(ErrorMessage = "請輸入活動主題")]
        public string actTopic { get; set; }
        [DisplayName("活動舉辦時間")]
        [Required(ErrorMessage = "請輸入活動時間")]
        [DataType(DataType.Date)]
        public System.DateTime actTime { get; set; }
        [DisplayName("活動說明")]
        [Required(ErrorMessage = "請說明活動")]
        public string actDescription { get; set; }
        [DisplayName("年齡限制")]
        public string ageRestrict { get; set; }
        [DisplayName("性別限制")]
        public string gender { get; set; }
        [DisplayName("人數限制")]
        public short maxNumPeople { get; set; }
        [DisplayName("預算")]
        public decimal maxBudget { get; set; }
        [DisplayName("縣/市")]
        public short actCounty { get; set; }
        [DisplayName("鄉/鎮/市/區")]
        public short actDistrict { get; set; }
        [DisplayName("路名")]
        public string actRoad { get; set; }
        [DisplayName("付款方式")]
        public string paymentTerm { get; set; }
        [DisplayName("是否保留此揪團活動")]
        public bool keepAct { get; set; }
        [DisplayName("是否可退出")]
        public bool acceptDrop { get; set; }
        [DisplayName("點擊次數")]
        public int clickTimes { get; set; }
        [DisplayName("報名截止時間")]
        [DataType(DataType.Date)]
        public System.DateTime actDeadline { get; set; }
        [DisplayName("活動類別ID")]
        public string actClassId { get; set; }
    }


    public class MetaMember
    {
        [Key]
        [StringLength(10)]
        [DisplayName("會員ID")]
        public string memId { get; set; }

        [Required(ErrorMessage = "請輸入暱稱")]
        [StringLength(15, ErrorMessage = "暱稱不可超過15碼")]
        [DisplayName("暱稱")]
        public string memNick { get; set; }

        [Required(ErrorMessage = "請輸入您的郵件信箱")]
        [StringLength(50)]
        [DisplayName("電子郵件信箱")]
        public string Email { get; set; }

        [StringLength(24)]
        [DisplayName("手機號碼")]
        [Required(ErrorMessage = "請輸入手機號碼")]
        public string cellPhone { get; set; }

        [DisplayName("縣/市")]
        public short memCounty { get; set; }

        [DisplayName("鄉/鎮/市/區")]
        public short memDistrict { get; set; }

        [DisplayName("註冊時間")]
        public DateTime timeReg { get; set; }

        [DisplayName("生日")]
        [Required(ErrorMessage = "請輸入生日")]
        public DateTime Birthday { get; set; }

        [DisplayName("違規次數")]
        public short numViolate { get; set; }

        [DisplayName("停權")]
        public bool Suspend { get; set; }

        [Required]
        [StringLength(1, ErrorMessage = "請輸入\"男\"或\"女\"")]
        [DisplayName("性別")]
        public string Sex { get; set; }

        [DisplayName("是否已審核")]
        public bool Approved { get; set; }

        [DisplayName("個人簡介")]
        public string Introduction { get; set; }
    }

    public class MetaPayment_Restriction
    {
        [DisplayName("付款方式")]
        public string payment { get; set; }
    }

    public class MetaPeople_Restriction
    {
        [DisplayName("人數上限")]
        public int PeoRestriction { get; set; }
    }
    public class MetaPhotos_of_Activities
    {
        [DisplayName("活動照片")]
        public byte[] actPics { get; set; }
    }

    public class Metavw_Activities
    {
        [DisplayName("活動ID")]
        public string actId { get; set; }
        [DisplayName("主辦人會員ID")]
        public string hostId { get; set; }
        [DisplayName("主辦人暱稱")]
        public string memNick { get; set; }
        [DisplayName("活動主題")]
        [Required(ErrorMessage = "請輸入活動主題")]
        public string actTopic { get; set; }
        [DisplayName("活動舉辦時間")]
        [Required(ErrorMessage = "請輸入活動時間")]
        [DataType(DataType.Date)]
        public System.DateTime actTime { get; set; }
        [DisplayName("活動說明")]
        [Required(ErrorMessage = "請說明活動")]
        public string actDescription { get; set; }
        [DisplayName("年齡")]
        public string age { get; set; }
        [DisplayName("性別限制")]
        public string gender { get; set; }
        [DisplayName("人數上限")]
        public int PeoRestriction { get; set; }
        [DisplayName("預算")]
        public decimal Budget { get; set; }
        [DisplayName("縣/市")]
        public string CountyName { get; set; }
        [DisplayName("鄉/鎮/市/區")]
        public string DistrictName { get; set; }
        [DisplayName("路名")]
        public string actRoad { get; set; }
        [DisplayName("付款方式")]
        public string payment { get; set; }
        [DisplayName("是否保留此揪團活動")]
        public bool keepAct { get; set; }
        [DisplayName("是否可退出")]
        public bool acceptDrop { get; set; }
        [DisplayName("點擊次數")]
        public int clickTimes { get; set; }
        [DisplayName("報名截止時間")]
        [DataType(DataType.Date)]
        public System.DateTime actDeadline { get; set; }
        [DisplayName("活動類別ID")]
        public string actClassId { get; set; }
        [DisplayName("活動類別")]
        public string actClassName { get; set; }

        [DisplayName("活動照片")]
        public byte[] actPics { get; set; }
    }
}