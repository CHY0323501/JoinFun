using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _01JoinFun.Models
{
    public class MetaBookmarkDetails
    {

        [DisplayName("會員ID")]
        [Required(ErrorMessage = "會員ID為必填")]
        public string memId { get; set; }

        [DisplayName("活動ID")]
        [Required(ErrorMessage = "活動ID為必填")]        
        public string actId { get; set; }

        [DisplayName("收藏時間")]
        [Required(ErrorMessage = "收藏時間為必填")]
        [DataType(DataType.Date, ErrorMessage = "輸入時間有誤")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public System.DateTime BookMarkTime { get; set; }


    }

    public class MetaChatRecords
    {
        [DisplayName("聊天紀錄編號")]
        [Required(ErrorMessage = "聊天紀錄編號為必填")]       
        public string chatSerial { get; set; }

        [DisplayName("開啟對話者會員ID")]
        [Required(ErrorMessage = "開啟對話者會員ID為必填")]       
        public string FromMemId { get; set; }

        [DisplayName("聊天對象會員ID")]
        [Required(ErrorMessage = "聊天對象會員ID為必填")]        
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

    public class MetaBudgetRestriction
    {

        [DisplayName("預算")]
        [Required(ErrorMessage = "預算為必填")]
        [Range(double.Epsilon, double.MaxValue, ErrorMessage = "預算不可小於0")]
        public decimal Budget { get; set; }

    }

    public class MetaComment
    {
        [DisplayName("意見編號")]
        [Required(ErrorMessage = "意見編號為必填")]        
        public string commentId { get; set; }

        [DisplayName("會員ID")]
        [Required(ErrorMessage = "會員ID為必填")]        
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

        [DisplayName("管理員ID")]        
        public string admId { get; set; }


    }

    public class MetaDietaryPreference
    {
        [DisplayName("喜好編號")]
        [Required(ErrorMessage = "喜好編號為必填")]        
        public string dSerial { get; set; }

        [DisplayName("會員ID")]
        [Required(ErrorMessage = "會員ID為必填")]       
        public string memId { get; set; }

        [DisplayName("飲食喜好")]
        [Required(ErrorMessage = "飲食喜好為必填")]
        [StringLength(25, ErrorMessage = "意見標題最多25個字")]
        public string dPreference { get; set; }


    }

    public class MetaFans
    {
        [DisplayName("粉絲會員ID")]
        [Required(ErrorMessage = "粉絲會員ID為必填")]        
        public string fanMemId { get; set; }

        [DisplayName("被追蹤者會員ID")]
        [Required(ErrorMessage = "被追蹤者會員ID為必填")]        
        public string memId { get; set; }


    }

    public class MetaFollowUp
    {

        [DisplayName("被追蹤者會員ID")]
        [Required(ErrorMessage = "被追蹤者會員ID為必填")]        
        public string FoMemId { get; set; }

        [DisplayName("加入追蹤者會員ID")]
        [Required(ErrorMessage = "加入追蹤者會員ID為必填")]        
        public string memId { get; set; }


    }
}