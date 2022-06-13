using LinkedOut.DB.Entity;
using Microsoft.EntityFrameworkCore;

namespace LinkedOut.DB;

public partial class LinkedOutContext : DbContext
{
    public LinkedOutContext()
    {
    }

    public LinkedOutContext(DbContextOptions<LinkedOutContext> options)
        : base(options)
    {
    }
        
    public async Task SaveChangesAsync()
    {
        var ee = ChangeTracker.Entries().ToList();
        foreach (var entry in ee)
        {
            var now = DateTime.Now;
            switch (entry.State)
            {
                case EntityState.Added:
                    Entry(entry.Entity).Property("CreateTime").CurrentValue = now;
                    Entry(entry.Entity).Property("UpdateTime").CurrentValue = now;
                    break;
                case EntityState.Modified:
                    Entry(entry.Entity).Property("UpdateTime").CurrentValue = now;
                    break;
                case EntityState.Detached:
                    break;
                case EntityState.Unchanged:
                    break;
                case EntityState.Deleted:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        await base.SaveChangesAsync();
    }

    public virtual DbSet<AppFile> AppFiles { get; set; } = null!;
    public virtual DbSet<Application> Applications { get; set; } = null!;
    public virtual DbSet<Comment> Comments { get; set; } = null!;
    public virtual DbSet<EduExperience> EduExperiences { get; set; } = null!;
    public virtual DbSet<EnterpriseInfo> EnterpriseInfos { get; set; } = null!;
    public virtual DbSet<JobExperience> JobExperiences { get; set; } = null!;
    public virtual DbSet<Liked> Likeds { get; set; } = null!;
    public virtual DbSet<Position> Positions { get; set; } = null!;
    public virtual DbSet<Subscribed> Subscribeds { get; set; } = null!;
    public virtual DbSet<Tweet> Tweets { get; set; } = null!;
    public virtual DbSet<User> Users { get; set; } = null!;
    public virtual DbSet<UserInfo> UserInfos { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
            optionsBuilder.UseMySql("server=175.24.202.178;port=3306;database=linkedOut;user=root;password=123456", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.27-mysql"));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<AppFile>(entity =>
        {
            entity.ToTable("app_file");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasComment("文件Id");

            entity.Property(e => e.AssociatedId)
                .HasColumnName("associated_id")
                .HasComment("与之关联的Id");

            entity.Property(e => e.CreateTime)
                .HasColumnType("datetime")
                .HasColumnName("create_time")
                .HasComment("创建时间");

            entity.Property(e => e.FileType)
                .HasColumnName("file_type")
                .HasComment("文件类型（0是简历，1是动态）");

            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name")
                .HasComment("文件名称");

            entity.Property(e => e.UpdateTime)
                .HasColumnType("datetime")
                .HasColumnName("update_time")
                .HasComment("更新时间");

            entity.Property(e => e.Url)
                .HasMaxLength(1000)
                .HasColumnName("url")
                .HasComment("文件url");
        });

        modelBuilder.Entity<Application>(entity =>
        {
            entity.ToTable("application");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasComment("申请Id");

            entity.Property(e => e.CreateTime)
                .HasColumnType("datetime")
                .HasColumnName("create_time")
                .HasComment("创建时间");

            entity.Property(e => e.JobId)
                .HasColumnName("job_id")
                .HasComment("岗位Id");

            entity.Property(e => e.ResumeId)
                .HasColumnName("resume_id")
                .HasComment("简历Id");

            entity.Property(e => e.ResumeName)
                .HasMaxLength(255)
                .HasColumnName("resume_name")
                .HasComment("简历名称");

            entity.Property(e => e.UpdateTime)
                .HasColumnType("datetime")
                .HasColumnName("update_time")
                .HasComment("更新时间");

            entity.Property(e => e.UserId)
                .HasColumnName("user_id")
                .HasComment("用户Id");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.ToTable("comment");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasComment("评论Id");

            entity.Property(e => e.Content)
                .HasMaxLength(255)
                .HasColumnName("content")
                .HasComment("评论内容");

            entity.Property(e => e.CreateTime)
                .HasColumnType("datetime")
                .HasColumnName("create_time")
                .HasComment("创建时间");

            entity.Property(e => e.TweetId)
                .HasColumnName("tweet_id")
                .HasComment("动态Id");

            entity.Property(e => e.UpdateTime)
                .HasColumnType("datetime")
                .HasColumnName("update_time")
                .HasComment("更新时间");

            entity.Property(e => e.UnifiedId)
                .HasColumnName("unified_id")
                .HasComment("用户Id");
        });

        modelBuilder.Entity<EduExperience>(entity =>
        {
            entity.ToTable("edu_experience");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasComment("主键");

            entity.Property(e => e.CollegeName)
                .HasMaxLength(255)
                .HasColumnName("college_name")
                .HasComment("学校名字");

            entity.Property(e => e.CreateTime)
                .HasColumnType("datetime")
                .HasColumnName("create_time")
                .HasComment("创建时间");

            entity.Property(e => e.Degree)
                .HasMaxLength(255)
                .HasColumnName("degree")
                .HasComment("学位");

            entity.Property(e => e.EndTime)
                .HasColumnType("datetime")
                .HasColumnName("end_time")
                .HasComment("结束时间");

            entity.Property(e => e.Major)
                .HasMaxLength(255)
                .HasColumnName("major")
                .HasComment("专业");

            entity.Property(e => e.StartTime)
                .HasColumnType("datetime")
                .HasColumnName("start_time")
                .HasComment("开始时间");

            entity.Property(e => e.UpdateTime)
                .HasColumnType("datetime")
                .HasColumnName("update_time")
                .HasComment("更新时间");

            entity.Property(e => e.UnifiedId)
                .HasColumnName("unified_id")
                .HasComment("用户Id");
        });

        modelBuilder.Entity<EnterpriseInfo>(entity =>
        {
            entity.ToTable("enterprise_info");
            
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasComment("主键");
            
            entity.Property(e => e.UnifiedId)
                .HasColumnName("unified_id")
                .HasComment("独一五二的Id");

            entity.Property(e => e.ContactWay)
                .HasMaxLength(255)
                .HasColumnName("contact_way")
                .HasComment("联系方式");

            entity.Property(e => e.CreateTime)
                .HasColumnType("datetime")
                .HasColumnName("create_time")
                .HasComment("创建时间");

            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description")
                .HasComment("描述");

            entity.Property(e => e.UpdateTime)
                .HasColumnType("datetime")
                .HasColumnName("update_time")
                .HasComment("更新时间");
        });

        modelBuilder.Entity<JobExperience>(entity =>
        {
            entity.ToTable("job_experience");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasComment("主键");

            entity.Property(e => e.CreateTime)
                .HasColumnType("datetime")
                .HasColumnName("create_time")
                .HasComment("创建时间");

            entity.Property(e => e.Description)
                .HasMaxLength(1024)
                .HasColumnName("description")
                .HasComment("简介");

            entity.Property(e => e.EndTime)
                .HasColumnType("datetime")
                .HasColumnName("end_time")
                .HasComment("结束时间");

            entity.Property(e => e.EnterpriseName)
                .HasMaxLength(255)
                .HasColumnName("enterprise_name")
                .HasComment("企业名字");
            
            entity.Property(e => e.PositionType)
                .HasMaxLength(255)
                .HasColumnName("position_type")
                .HasComment("职位类型");

            entity.Property(e => e.StartTime)
                .HasColumnType("datetime")
                .HasColumnName("start_time")
                .HasComment("开始时间");

            entity.Property(e => e.UpdateTime)
                .HasColumnType("datetime")
                .HasColumnName("update_time")
                .HasComment("更新时间");

            entity.Property(e => e.UnifiedId)
                .HasColumnName("unified_id")
                .HasComment("用户Id");
        });

        modelBuilder.Entity<Liked>(entity =>
        {
            entity.ToTable("liked");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasComment("主键");

            entity.Property(e => e.CreateTime)
                .HasColumnType("datetime")
                .HasColumnName("create_time")
                .HasComment("创建时间");

            entity.Property(e => e.TweetId)
                .HasColumnName("tweet_id")
                .HasComment("动态Id");

            entity.Property(e => e.UpdateTime)
                .HasColumnType("datetime")
                .HasColumnName("update_time")
                .HasComment("更新时间");

            entity.Property(e => e.UnifiedId)
                .HasColumnName("unified_id")
                .HasComment("用户Id");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.ToTable("position");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasComment("岗位Id");

            entity.Property(e => e.ContactWay)
                .HasMaxLength(255)
                .HasColumnName("contact_way")
                .HasComment("联系方式");

            entity.Property(e => e.CreateTime)
                .HasColumnType("datetime")
                .HasColumnName("create_time")
                .HasComment("创建时间");

            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description")
                .HasComment("岗位描述");

            entity.Property(e => e.EnterpriseId)
                .HasColumnName("enterprise_id")
                .HasComment("企业Id");

            entity.Property(e => e.JobName)
                .HasMaxLength(255)
                .HasColumnName("job_name")
                .HasComment("工作名称");

            entity.Property(e => e.Limitation)
                .HasMaxLength(255)
                .HasColumnName("limitation")
                .HasComment("岗位限制");

            entity.Property(e => e.PositionType)
                .HasMaxLength(255)
                .HasColumnName("position_type")
                .HasComment("岗位类型");

            entity.Property(e => e.Reward)
                .HasMaxLength(255)
                .HasColumnName("reward")
                .HasComment("薪资");

            entity.Property(e => e.UpdateTime)
                .HasColumnType("datetime")
                .HasColumnName("update_time")
                .HasComment("更新时间");

            entity.Property(e => e.Url)
                .HasMaxLength(255)
                .HasColumnName("url")
                .HasComment("岗位图片");

            entity.Property(e => e.WorkPlace)
                .HasMaxLength(255)
                .HasColumnName("work_place")
                .HasComment("工作地点(-分隔省市eg.河北-石家庄）");
        });

        modelBuilder.Entity<Subscribed>(entity =>
        {
            entity.ToTable("subscribed");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasComment("关注Id");

            entity.Property(e => e.CreateTime)
                .HasColumnType("datetime")
                .HasColumnName("create_time")
                .HasComment("创建时间");

            entity.Property(e => e.FirstUserId).HasColumnName("first_user_id");

            entity.Property(e => e.SecondUserId).HasColumnName("second_user_id");

            entity.Property(e => e.UpdateTime)
                .HasColumnType("datetime")
                .HasColumnName("update_time")
                .HasComment("更新时间");
        });

        modelBuilder.Entity<Tweet>(entity =>
        {
            entity.ToTable("tweet");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasComment("动态Id");

            entity.Property(e => e.CommentNum)
                .HasColumnName("comment_num")
                .HasComment("评论数");

            entity.Property(e => e.Content)
                .HasMaxLength(255)
                .HasColumnName("content")
                .HasComment("动态内容");

            entity.Property(e => e.CreateTime)
                .HasColumnType("datetime")
                .HasColumnName("create_time")
                .HasComment("创建时间");

            entity.Property(e => e.LikeNum)
                .HasColumnName("like_num")
                .HasComment("喜欢数");

            entity.Property(e => e.UpdateTime)
                .HasColumnType("datetime")
                .HasColumnName("update_time")
                .HasComment("更新时间");

            entity.Property(e => e.UnifiedId)
                .HasColumnName("unified_id")
                .HasComment("用户Id");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UnifiedId)
                .HasName("PRIMARY");

            entity.ToTable("user");

            entity.Property(e => e.UnifiedId)
                .HasColumnName("unified_id")
                .HasComment("唯一Id");

            entity.Property(e => e.Avatar)
                .HasMaxLength(255)
                .HasColumnName("avatar")
                .HasComment("头像");

            entity.Property(e => e.Background)
                .HasMaxLength(255)
                .HasColumnName("background")
                .HasComment("背景图");

            entity.Property(e => e.BriefInfo)
                .HasMaxLength(255)
                .HasColumnName("brief_info")
                .HasComment("简要介绍");

            entity.Property(e => e.CreateTime)
                .HasColumnType("datetime")
                .HasColumnName("create_time")
                .HasComment("创建时间");

            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email")
                .HasComment("邮箱");

            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .HasColumnName("name")
                .HasComment("姓名（不可以改变）");

            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password")
                .HasComment("密码");

            entity.Property(e => e.UserType)
                .HasMaxLength(255)
                .HasColumnName("user_type")
                .HasComment("用户类型");

            entity.Property(e => e.SubscribeNum)
                .HasColumnName("subscribe_num")
                .HasComment("关注的数量");

            entity.Property(e => e.TrueName)
                .HasMaxLength(255)
                .HasColumnName("true_name")
                .HasComment("真实名字");

            entity.Property(e => e.UpdateTime)
                .HasColumnType("datetime")
                .HasColumnName("update_time")
                .HasComment("更新时间");
        });

        modelBuilder.Entity<UserInfo>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.ToTable("user_info");

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasComment("主键");
            
            entity.Property(e => e.UnifiedId)
                .HasColumnName("unified_id")
                .HasComment("独一无二的Id");

            entity.Property(e => e.Age)
                .HasColumnName("age")
                .HasComment("年龄");

            entity.Property(e => e.CreateTime)
                .HasColumnType("datetime")
                .HasColumnName("create_time")
                .HasComment("创建时间");

            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .HasColumnName("gender")
                .HasComment("性别");

            entity.Property(e => e.IdCard)
                .HasMaxLength(255)
                .HasColumnName("id_card")
                .HasComment("身份证");

            entity.Property(e => e.LivePlace)
                .HasMaxLength(255)
                .HasColumnName("live_place")
                .HasComment("居住地(-分隔省市eg.河北-石家庄)");
            

            entity.Property(e => e.PhoneNum)
                .HasColumnName("phone_num")
                .HasComment("电话号码");

            entity.Property(e => e.PrePosition)
                .HasMaxLength(255)
                .HasColumnName("pre_position")
                .HasComment("职位偏好");

            entity.Property(e => e.UpdateTime)
                .HasColumnType("datetime")
                .HasColumnName("update_time")
                .HasComment("更新时间");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}