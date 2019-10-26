namespace JoinFun.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class dbJoinFun : DbContext
    {
        public dbJoinFun()
            : base("name=dbJoinFun")
        {
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
        public virtual DbSet<Dietary_Preference> Dietary_Preference { get; set; }
        public virtual DbSet<District> District { get; set; }
        public virtual DbSet<Fans> Fans { get; set; }
        public virtual DbSet<FollowUp> FollowUp { get; set; }
        public virtual DbSet<Friendship> Friendship { get; set; }
        public virtual DbSet<Gender_Restriction> Gender_Restriction { get; set; }
        public virtual DbSet<Habit> Habit { get; set; }
        public virtual DbSet<Hash_Tag> Hash_Tag { get; set; }
        public virtual DbSet<Join_Fun_Activities> Join_Fun_Activities { get; set; }
        public virtual DbSet<Member> Member { get; set; }
        public virtual DbSet<Member_Remarks> Member_Remarks { get; set; }
        public virtual DbSet<Message_Board> Message_Board { get; set; }
        public virtual DbSet<Notification> Notification { get; set; }
        public virtual DbSet<Payment_Restriction> Payment_Restriction { get; set; }
        public virtual DbSet<People_Restriction> People_Restriction { get; set; }
        public virtual DbSet<Photos_of_Activities> Photos_of_Activities { get; set; }
        public virtual DbSet<Post> Post { get; set; }
        public virtual DbSet<Punishment> Punishment { get; set; }
        public virtual DbSet<Social_Net_ID> Social_Net_ID { get; set; }
        public virtual DbSet<Type_of_Violate> Type_of_Violate { get; set; }
        public virtual DbSet<Violation> Violation { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Acc_Pass>()
                .Property(e => e.Account)
                .IsUnicode(false);

            modelBuilder.Entity<Acc_Pass>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<Acc_Pass>()
                .Property(e => e.memId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Acc_Pass>()
                .Property(e => e.Salt)
                .IsUnicode(false);

            modelBuilder.Entity<Activity_Class>()
                .Property(e => e.actClassId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Activity_Class>()
                .HasMany(e => e.Join_Fun_Activities)
                .WithRequired(e => e.Activity_Class)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Activity_Details>()
                .Property(e => e.memId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Activity_Details>()
                .Property(e => e.actId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Administrator>()
                .Property(e => e.admId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Administrator>()
                .Property(e => e.admAcc)
                .IsUnicode(false);

            modelBuilder.Entity<Administrator>()
                .Property(e => e.admPass)
                .IsUnicode(false);

            modelBuilder.Entity<Administrator>()
                .Property(e => e.admSalt)
                .IsUnicode(false);

            modelBuilder.Entity<Administrator>()
                .HasMany(e => e.Post)
                .WithRequired(e => e.Administrator)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Administrator>()
                .HasMany(e => e.Violation)
                .WithOptional(e => e.Administrator)
                .HasForeignKey(e => e.implement_admId);

            modelBuilder.Entity<Age_Restriction>()
                .HasMany(e => e.Join_Fun_Activities)
                .WithRequired(e => e.Age_Restriction)
                .HasForeignKey(e => e.ageRestrict)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Blacklist>()
                .Property(e => e.blockedMemId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Blacklist>()
                .Property(e => e.memId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Bookmark_Details>()
                .Property(e => e.memId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Bookmark_Details>()
                .Property(e => e.actId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Budget_Restriction>()
                .Property(e => e.Budget)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Budget_Restriction>()
                .HasMany(e => e.Join_Fun_Activities)
                .WithRequired(e => e.Budget_Restriction)
                .HasForeignKey(e => e.maxBudget)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Chat_Records>()
                .Property(e => e.chatSerial)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Chat_Records>()
                .Property(e => e.FromMemId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Chat_Records>()
                .Property(e => e.ToMemId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Comment>()
                .Property(e => e.commentId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Comment>()
                .Property(e => e.memId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Comment>()
                .Property(e => e.admId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<County>()
                .HasMany(e => e.District)
                .WithRequired(e => e.County)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<County>()
                .HasMany(e => e.Join_Fun_Activities)
                .WithRequired(e => e.County)
                .HasForeignKey(e => e.actCounty)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<County>()
                .HasMany(e => e.Member)
                .WithRequired(e => e.County)
                .HasForeignKey(e => e.memCounty)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Dietary_Preference>()
                .Property(e => e.dSerial)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Dietary_Preference>()
                .Property(e => e.memId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<District>()
                .HasMany(e => e.Join_Fun_Activities)
                .WithRequired(e => e.District)
                .HasForeignKey(e => e.actDistrict)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<District>()
                .HasMany(e => e.Member)
                .WithRequired(e => e.District)
                .HasForeignKey(e => e.memDistrict)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Fans>()
                .Property(e => e.fanMemId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Fans>()
                .Property(e => e.memId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<FollowUp>()
                .Property(e => e.FoMemId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<FollowUp>()
                .Property(e => e.memId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Friendship>()
                .Property(e => e.memId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Friendship>()
                .Property(e => e.friendMemId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Gender_Restriction>()
                .HasMany(e => e.Join_Fun_Activities)
                .WithRequired(e => e.Gender_Restriction)
                .HasForeignKey(e => e.gender)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Habit>()
                .Property(e => e.HabitSerial)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Habit>()
                .Property(e => e.memId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Hash_Tag>()
                .Property(e => e.hashSerial)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Hash_Tag>()
                .Property(e => e.actId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Join_Fun_Activities>()
                .Property(e => e.actId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Join_Fun_Activities>()
                .Property(e => e.actClassId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Join_Fun_Activities>()
                .Property(e => e.hostId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Join_Fun_Activities>()
                .HasMany(e => e.Activity_Details)
                .WithRequired(e => e.Join_Fun_Activities)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Join_Fun_Activities>()
                .HasMany(e => e.Bookmark_Details)
                .WithRequired(e => e.Join_Fun_Activities)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Join_Fun_Activities>()
                .HasMany(e => e.Hash_Tag)
                .WithRequired(e => e.Join_Fun_Activities)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Join_Fun_Activities>()
                .HasMany(e => e.Member_Remarks)
                .WithRequired(e => e.Join_Fun_Activities)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Join_Fun_Activities>()
                .HasMany(e => e.Message_Board)
                .WithRequired(e => e.Join_Fun_Activities)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Join_Fun_Activities>()
                .HasMany(e => e.Photos_of_Activities)
                .WithRequired(e => e.Join_Fun_Activities)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.memId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.cellPhone)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.Sex)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .HasMany(e => e.Acc_Pass)
                .WithRequired(e => e.Member)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Member>()
                .HasMany(e => e.Activity_Details)
                .WithRequired(e => e.Member)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Member>()
                .HasMany(e => e.Blacklist)
                .WithRequired(e => e.Member)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Member>()
                .HasMany(e => e.Bookmark_Details)
                .WithRequired(e => e.Member)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Member>()
                .HasMany(e => e.Chat_Records)
                .WithRequired(e => e.Member)
                .HasForeignKey(e => e.FromMemId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Member>()
                .HasMany(e => e.Comment)
                .WithRequired(e => e.Member)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Member>()
                .HasMany(e => e.Dietary_Preference)
                .WithRequired(e => e.Member)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Member>()
                .HasMany(e => e.Fans)
                .WithRequired(e => e.Member)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Member>()
                .HasMany(e => e.FollowUp)
                .WithRequired(e => e.Member)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Member>()
                .HasMany(e => e.Friendship)
                .WithRequired(e => e.Member)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Member>()
                .HasMany(e => e.Habit)
                .WithRequired(e => e.Member)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Member>()
                .HasMany(e => e.Member_Remarks)
                .WithRequired(e => e.Member)
                .HasForeignKey(e => e.FromMemId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Member>()
                .HasMany(e => e.Message_Board)
                .WithRequired(e => e.Member)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Member>()
                .HasMany(e => e.Notification)
                .WithRequired(e => e.Member)
                .HasForeignKey(e => e.ToMemId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Member>()
                .HasMany(e => e.Social_Net_ID)
                .WithRequired(e => e.Member)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Member>()
                .HasMany(e => e.Violation)
                .WithOptional(e => e.Member)
                .HasForeignKey(e => e.FromMemId);

            modelBuilder.Entity<Member_Remarks>()
                .Property(e => e.remarkSerial)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Member_Remarks>()
                .Property(e => e.actId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Member_Remarks>()
                .Property(e => e.FromMemId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Member_Remarks>()
                .Property(e => e.ToMemId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Message_Board>()
                .Property(e => e.mboardSerial)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Message_Board>()
                .Property(e => e.actId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Message_Board>()
                .Property(e => e.memId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Notification>()
                .Property(e => e.NotiSerial)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Notification>()
                .Property(e => e.ToMemId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Payment_Restriction>()
                .HasMany(e => e.Join_Fun_Activities)
                .WithRequired(e => e.Payment_Restriction)
                .HasForeignKey(e => e.paymentTerm)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<People_Restriction>()
                .HasMany(e => e.Join_Fun_Activities)
                .WithRequired(e => e.People_Restriction)
                .HasForeignKey(e => e.maxNumPeople)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Photos_of_Activities>()
                .Property(e => e.PhotoSerial)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Photos_of_Activities>()
                .Property(e => e.actId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Post>()
                .Property(e => e.postSerial)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Post>()
                .Property(e => e.admId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Punishment>()
                .Property(e => e.punishId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Social_Net_ID>()
                .Property(e => e.socialSerial)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Social_Net_ID>()
                .Property(e => e.memId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Social_Net_ID>()
                .Property(e => e.socialId)
                .IsUnicode(false);

            modelBuilder.Entity<Type_of_Violate>()
                .Property(e => e.typeId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Type_of_Violate>()
                .HasMany(e => e.Violation)
                .WithRequired(e => e.Type_of_Violate)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Violation>()
                .Property(e => e.vioId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Violation>()
                .Property(e => e.FromMemId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Violation>()
                .Property(e => e.FromAdmID)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Violation>()
                .Property(e => e.typeId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Violation>()
                .Property(e => e.CorrespondingEventID)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Violation>()
                .Property(e => e.implement_admId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Violation>()
                .Property(e => e.punishId)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}
