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
        [DisplayName("活動編號")]
        public string actId { get; set; }
        [DisplayName("主辦人會員編號")]
        public string hostId { get; set; }
        [DisplayName("活動主題")]
        [Required(ErrorMessage = "請輸入活動主題")]
        public string actTopic { get; set; }
        [DisplayName("活動舉辦時間")]
        [Required(ErrorMessage = "請輸入活動時間")]
        [DataType(DataType.Date)]
        public System.DateTime actTime { get; set; }
        [DisplayName("活動說明")]
        [Required(ErrorMessage = "請輸入活動說明")]
        public string actDescription { get; set; }
        [DisplayName("年齡限制")]
        public string ageRestrict { get; set; }
        [DisplayName("性別限制")]
        public string gender { get; set; }
        [DisplayName("人數限制")]
        public short maxNumPeople { get; set; }
        [DisplayName("預算上限")]
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
        [DisplayName("點閱次數")]
        public int clickTimes { get; set; }
        [DisplayName("報名截止時間")]
        [DataType(DataType.Date)]
        public System.DateTime actDeadline { get; set; }
        [DisplayName("活動類別編號")]
        public string actClassId { get; set; }
    }


    public class MetaMember
    {
        [Key]
        [StringLength(10)]
        [DisplayName("會員編號")]
        public string memId { get; set; }

        [Required(ErrorMessage = "請輸入暱稱")]
        [StringLength(15, ErrorMessage = "暱稱不可超過15碼")]
        [DisplayName("暱稱")]
        public string memNick { get; set; }

        [Required(ErrorMessage = "請輸入您的電子信箱")]
        [StringLength(50)]
        [DisplayName("電子信箱")]
        [DataType(DataType.EmailAddress,ErrorMessage ="請輸入正確的電子信箱")]
        public string Email { get; set; }

        [StringLength(24)]
        [DisplayName("手機號碼")]
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

        [DisplayName("是否停權")]
        public bool Suspend { get; set; }

        [Required]
        [StringLength(1, ErrorMessage = "請輸入\"男\"或\"女\"")]
        [DisplayName("性別")]
        public string Sex { get; set; }

        [DisplayName("是否已啟用帳號")]
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

    public class MetaBookmark_Details
    {

        [DisplayName("會員編號")]
        [Required(ErrorMessage = "會員編號為必填")]
        public string memId { get; set; }

        [DisplayName("活動編號")]
        [Required(ErrorMessage = "活動編號為必填")]
        public string actId { get; set; }

        [DisplayName("收藏時間")]
        [Required(ErrorMessage = "收藏時間為必填")]
        [DataType(DataType.Date, ErrorMessage = "輸入時間有誤")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public System.DateTime BookMarkTime { get; set; }
    }

    public class MetaChat_Records
    {
        [DisplayName("聊天紀錄編號")]
        [Required(ErrorMessage = "聊天紀錄編號為必填")]
        public string chatSerial { get; set; }

        [DisplayName("開啟對話者會員編號")]
        [Required(ErrorMessage = "開啟對話者會員編號為必填")]
        public string FromMemId { get; set; }

        [DisplayName("聊天對象會員編號")]
        [Required(ErrorMessage = "聊天對象會員編號為必填")]
        public string ToMemId { get; set; }

        [DisplayName("聊天內容")]
        [Required(ErrorMessage = "聊天內容為必填")]
        public string chatContent { get; set; }

        [DisplayName("對話時間")]
        [Required(ErrorMessage = "對話時間為必填")]
        [DataType(DataType.Date, ErrorMessage = "輸入時間有誤")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public System.DateTime Time { get; set; }

        [DisplayName("是否已讀")]
        [Required(ErrorMessage = "是否已讀為必填")]
        [DefaultValue(false)]
        public bool ReadYet { get; set; }


    }

    public class MetaBudget_Restriction
    {

        [DisplayName("預算限制")]
        [Required(ErrorMessage = "預算限制為必填")]
        [Range(double.Epsilon, double.MaxValue, ErrorMessage = "預算不可小於0")]
        public decimal Budget { get; set; }

    }

    public class MetaComment
    {
        [DisplayName("意見編號")]
        [Required(ErrorMessage = "意見編號為必填")]
        public string commentId { get; set; }

        [DisplayName("會員編號")]
        [Required(ErrorMessage = "會員編號為必填")]
        public string memId { get; set; }

        [DisplayName("意見標題")]
        [Required(ErrorMessage = "意見標題為必填")]
        [StringLength(30, ErrorMessage = "意見標題最多30個字")]
        public string commentTitle { get; set; }

        [DisplayName("意見內容")]
        [Required(ErrorMessage = "意見內容為必填")]
        public string Comment1 { get; set; }

        [DisplayName("意見提出時間")]
        [Required(ErrorMessage = "意見提出時間為必填")]
        [DataType(DataType.Date, ErrorMessage = "輸入時間有誤")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public System.DateTime receivedTime { get; set; }

        [DisplayName("意見回覆時間")]
        [DataType(DataType.Date, ErrorMessage = "輸入時間有誤")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> reportTime { get; set; }

        [DisplayName("意見回覆內容")]
        public string reportContent { get; set; }

        [DisplayName("管理員編號")]
        public string admId { get; set; }


    }

    public class MetaDietary_Preference
    {
        [DisplayName("飲食偏好編號")]
        [Required(ErrorMessage = "飲食偏好編號為必填")]
        public string dSerial { get; set; }

        [DisplayName("會員編號")]
        [Required(ErrorMessage = "會員編號為必填")]
        public string memId { get; set; }

        [DisplayName("飲食偏好")]
        [Required(ErrorMessage = "飲食偏好為必填")]
        [StringLength(25, ErrorMessage = "飲食偏好最多25個字")]
        public string dPreference { get; set; }


    }

    public class MetaFans
    {
        [DisplayName("粉絲會員編號")]
        [Required(ErrorMessage = "粉絲會員編號為必填")]
        public string fanMemId { get; set; }

        [DisplayName("被追蹤者會員編號")]
        [Required(ErrorMessage = "被追蹤者會員編號為必填")]
        public string memId { get; set; }


    }

    public class MetaFollowUp
    {

        [DisplayName("被追蹤者會員編號")]
        [Required(ErrorMessage = "被追蹤者會員編號為必填")]
        public string FoMemId { get; set; }

        [DisplayName("加入追蹤者會員編號")]
        [Required(ErrorMessage = "加入追蹤者會員編號為必填")]
        public string memId { get; set; }


    }


    public class MetaFriendship
    {
        [DisplayName("會員編號")]
        public string memId { get; set; }
        [DisplayName("好友會員編號")]
        public string friendMemId { get; set; }

        [DisplayName("好友暱稱")]
        [Required(ErrorMessage = "必填")]
        [StringLength(15, ErrorMessage = "最多輸入15個字")]

        public string friendNick { get; set; }
        [DisplayName("是否確認好友")]
        public bool Approved { get; set; }

    }

    public  class MetaGender_Restriction
    {


        [DisplayName("性別序號")]
        public short genderSerial { get; set; }

        [DisplayName("性別")]
        [Required(ErrorMessage = "請輸入性別")]
        [StringLength(10, ErrorMessage = "性別最多10個字")]
        public string gender { get; set; }

       }

    public class MetaHash_Tag
    {

        [DisplayName("HashTag編號")]
        public string hashSerial { get; set; }

        [DisplayName("活動編號")]
        public string actId { get; set; }

        [DisplayName("HashTag內容")]
        [StringLength(30, ErrorMessage = "最多輸入30個字")]
        [Required(ErrorMessage = "必填")]
        public string hashContent { get; set; }


    }

    public class MetaHabit
    {
        [DisplayName("興趣編號")]
        public string HabitSerial { get; set; }
        [DisplayName("會員編號")]
        public string memId { get; set; }
        [Required(ErrorMessage = "必填")]
        [DisplayName("個人興趣")]
        [StringLength(25, ErrorMessage = "最多輸入25個字")]
        public string Habit1 { get; set; }

    }


    public class MetaMember_Remarks
    {
        [DisplayName("評價編號")]
        public string remarkSerial { get; set; }
        [DisplayName("活動編號")]
        public string actId { get; set; }
        [DisplayName("評價者會員編號")]
        public string FromMemId { get; set; }
        [DisplayName("被評價者會員編號")]
        public string ToMemId { get; set; }
        [DisplayName("是否保留評價")]
        public bool keepRemark { get; set; }
        [DisplayName("評價星等")]
        [Required(ErrorMessage = "必填")]
        [Range(1, 5, ErrorMessage = "請填1~5分數字")]
        public short remarkStar { get; set; }
        [DisplayName("評價內容")]
        public string remarkContent { get; set; }
        [DisplayName("評價時間")]
        public System.DateTime remarkTime { get; set; }


    }

    public class MetaMessage_Board
    {
        [DisplayName("留言版編號")]
        public string mboardSerial { get; set; }
        [DisplayName("活動編號")]
        public string actId { get; set; }
        [DisplayName("會員編號")]
        public string memId { get; set; }

        [DisplayName("留言內容")]
        [Required(ErrorMessage = "必填")]
        public string boardMessage { get; set; }
        [DisplayName("留言時間")]

        public System.DateTime messageTime { get; set; }
        [DisplayName("是否保留留言")]

        public bool keepMboard { get; set; }


    }

    public class MetaNotification
    {
        [DisplayName("通知編號")]
        public string NotiSerial { get; set; }
        [DisplayName("接收通知者會員編號")]
        public string ToMemId { get; set; }
        [DisplayName("通知內容")]
        [Required(ErrorMessage = "必填")]
        public string NotifContent { get; set; }
        [DisplayName("是否已讀")]
        public bool readYet { get; set; }

    }

    public class MetaPeople_Restriction
    {

        [DisplayName("人數限制編號")]
        public short peoSerial { get; set; }
        [DisplayName("人數限制")]
        public int PeoRestriction { get; set; }

    }

    public  class MetaPhotos_of_Activities
    {

        [DisplayName("照片編號")]
        [Required(ErrorMessage = "請輸入照片編號")]
        public string PhotoSerial { get; set; }


        [DisplayName("活動編號")]
        [Required(ErrorMessage = "請輸入活動編號")]
        public string actId { get; set; }


        [DisplayName("照片")]
        [Required(ErrorMessage = "請上傳照片")]
        public byte[] actPics { get; set; }

    }


    public  class MetaPost
    {
        [DisplayName("公告編號")]
        [Required(ErrorMessage = "請輸入公告編號")]
        public string postSerial { get; set; }

        [DisplayName("發佈管理員編號")]
        [Required(ErrorMessage = "請輸入管理員編號")]
        public string admId { get; set; }

        [DisplayName("公告標題")]
        [Required(ErrorMessage = "請輸入公告標題")]
        [StringLength(30, ErrorMessage = "公告標題最多30個字")]
        public string postTitle { get; set; }

        [DisplayName("公告內容")]
        [Required(ErrorMessage = "請輸入公告內容")]
        public string postContent { get; set; }


        [DisplayName("公告時間")]
        [Required(ErrorMessage = "請輸入公告時間")]
        [DataType(DataType.DateTime,ErrorMessage ="輸入時間有誤")]
        [DisplayFormat(DataFormatString ="{0:yyyy/MM/dd}",ApplyFormatInEditMode =true)]
        public System.DateTime postTime { get; set; }

        [DisplayName("照片")]
        [Required(ErrorMessage = "請上傳圖片")]
        public byte[] postPics { get; set; }

    }



    public  class MetaPunishment
    {

        [DisplayName("處置編號")]
        [Required(ErrorMessage = "請輸入處置編號")]
        public string punishId { get; set; }

        [DisplayName("處置名稱")]
        [Required(ErrorMessage = "請輸入處置名稱")]
        [StringLength(10, ErrorMessage = "處置名稱最多10個字")]
        public string punishName { get; set; }

    }



    public  class MetaSocial_Net_ID
    {
        [DisplayName("社群帳號編號")]
        [Required(ErrorMessage = "請輸入社群帳號編號")]
        public string socialSerial { get; set; }

        [DisplayName("會員編號")]
        [Required(ErrorMessage = "請輸入會員編號")]
        [StringLength(10, ErrorMessage = "會員編號最多10個字")]
        public string memId { get; set; }

        [DisplayName("社群ID")]
        [Required(ErrorMessage = "請輸入社群ID")]
        [StringLength(50, ErrorMessage = "社群ID最多50個字")]
        public string socialId { get; set; }

    }


    public  class MetaType_of_Violate
    {
        [DisplayName("類別編號")]
        [Required(ErrorMessage = "請輸入類別編號")]
        public string typeId { get; set; }

        [DisplayName("違規類別名稱")]
        [Required(ErrorMessage = "請輸入違規類別名稱")]
        [StringLength(10, ErrorMessage = "違規類別名稱最多10個字")]
        public string vioClass { get; set; }
 
    }

    public class MetaViolation
    {

        [DisplayName("違規編號")]
        public string vioId { get; set; }

        [DisplayName("檢舉者會員編號")]
        public string FromMemId { get; set; }

        [DisplayName("檢舉者管理員編號")]
        public string FromAdmID { get; set; }

        [DisplayName("違規類別編號")]
        [Required(ErrorMessage = "請輸入違規類別編號")]
        [StringLength(8, ErrorMessage = "類別編號最多8碼")]
        public string typeId { get; set; }

        [DisplayName("對應事項編號")]
        [Required(ErrorMessage = "請輸入對應事項編號")]
        [StringLength(10, ErrorMessage = "編號最多10碼")]
        public string CorrespondingEventID { get; set; }

        [DisplayName("檢舉標題")]
        [Required(ErrorMessage = "請輸入檢舉標題")]
        [StringLength(20, ErrorMessage = "標題最多20個字")]
        public string vioTitle { get; set; }

        [DisplayName("檢舉內容")]
        [Required(ErrorMessage = "請輸入檢舉內容")]
        public string vioContent { get; set; }


        [DisplayName("檢舉時間")]
        [Required(ErrorMessage = "請輸入檢舉時間")]
        [DataType(DataType.DateTime, ErrorMessage = "輸入時間有誤")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public System.DateTime vioReportTime { get; set; }


        [DisplayName("處置管理員編號")]
        public string implement_admId { get; set; }


        [DisplayName("處置編號")]
        public string punishId { get; set; }


        [DisplayName("違規處置時間")]
        [Required(ErrorMessage = "請輸入處置時間")]
        [DataType(DataType.DateTime, ErrorMessage = "輸入時間有誤")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> vioProcessTime { get; set; }

    }

}