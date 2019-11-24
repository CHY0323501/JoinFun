﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace JoinFun.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class JoinFunEntities : DbContext
    {
        public JoinFunEntities()
            : base("name=JoinFunEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Acc_Pass> Acc_Pass { get; set; }
        public virtual DbSet<Activity_Class> Activity_Class { get; set; }
        public virtual DbSet<Activity_Details> Activity_Details { get; set; }
        public virtual DbSet<Administrator> Administrator { get; set; }
        public virtual DbSet<Age_Restriction> Age_Restriction { get; set; }
        public virtual DbSet<Blacklist> Blacklist { get; set; }
        public virtual DbSet<Bookmark_Details> Bookmark_Details { get; set; }
        public virtual DbSet<Budget_Restriction> Budget_Restriction { get; set; }
        public virtual DbSet<Chat_Records> Chat_Records { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<County> County { get; set; }
        public virtual DbSet<District> District { get; set; }
        public virtual DbSet<Fans> Fans { get; set; }
        public virtual DbSet<FollowUp> FollowUp { get; set; }
        public virtual DbSet<Friendship> Friendship { get; set; }
        public virtual DbSet<Gender_Restriction> Gender_Restriction { get; set; }
        public virtual DbSet<Join_Fun_Activities> Join_Fun_Activities { get; set; }
        public virtual DbSet<Member> Member { get; set; }
        public virtual DbSet<Member_Remarks> Member_Remarks { get; set; }
        public virtual DbSet<Message_Board> Message_Board { get; set; }
        public virtual DbSet<Payment_Restriction> Payment_Restriction { get; set; }
        public virtual DbSet<People_Restriction> People_Restriction { get; set; }
        public virtual DbSet<Photos_of_Activities> Photos_of_Activities { get; set; }
        public virtual DbSet<Post> Post { get; set; }
        public virtual DbSet<Punishment> Punishment { get; set; }
        public virtual DbSet<Social_Net_ID> Social_Net_ID { get; set; }
        public virtual DbSet<Type_of_Violate> Type_of_Violate { get; set; }
        public virtual DbSet<Violation> Violation { get; set; }
        public virtual DbSet<vw_Activities> vw_Activities { get; set; }
        public virtual DbSet<vw_FansNew> vw_FansNew { get; set; }
        public virtual DbSet<vw_FollowUp> vw_FollowUp { get; set; }
        public virtual DbSet<vw_FriendShip> vw_FriendShip { get; set; }
        public virtual DbSet<vw_Host_Remarks> vw_Host_Remarks { get; set; }
        public virtual DbSet<vw_Member_Remarks> vw_Member_Remarks { get; set; }
        public virtual DbSet<vw_Participant_Remarks> vw_Participant_Remarks { get; set; }
        public virtual DbSet<vw_HostHistory> vw_HostHistory { get; set; }
        public virtual DbSet<vw_PartHistory> vw_PartHistory { get; set; }
        public virtual DbSet<Host_Now> Host_Now { get; set; }
        public virtual DbSet<Part_Now> Part_Now { get; set; }
        public virtual DbSet<Notification> Notification { get; set; }
        public virtual DbSet<vw_Member_Join> vw_Member_Join { get; set; }
        public virtual DbSet<vw_MemJoinDetail> vw_MemJoinDetail { get; set; }
    }
}
