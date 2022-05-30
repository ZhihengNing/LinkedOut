using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinkedOut.DB.Migrations
{
    public partial class modify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "app_file",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false, comment: "文件Id")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    url = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true, comment: "文件url", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "文件名称", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    file_type = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "文件类型（0是简历，1是动态）", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    associated_id = table.Column<int>(type: "int", nullable: true, comment: "与之关联的Id"),
                    create_time = table.Column<DateTime>(type: "datetime", nullable: false, comment: "创建时间"),
                    update_time = table.Column<DateTime>(type: "datetime", nullable: false, comment: "更新时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_file", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "application",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false, comment: "申请Id")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    job_id = table.Column<int>(type: "int", nullable: true, comment: "岗位Id"),
                    resume_id = table.Column<int>(type: "int", nullable: true, comment: "简历Id"),
                    user_id = table.Column<int>(type: "int", nullable: true, comment: "用户Id"),
                    resume_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "简历名称", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    create_time = table.Column<DateTime>(type: "datetime", nullable: false, comment: "创建时间"),
                    update_time = table.Column<DateTime>(type: "datetime", nullable: false, comment: "更新时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_application", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "comment",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false, comment: "评论Id")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    content = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "评论内容", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    unified_id = table.Column<int>(type: "int", nullable: true, comment: "用户Id"),
                    floor = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "楼层", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tweet_id = table.Column<int>(type: "int", nullable: true, comment: "动态Id"),
                    create_time = table.Column<DateTime>(type: "datetime", nullable: false, comment: "创建时间"),
                    update_time = table.Column<DateTime>(type: "datetime", nullable: false, comment: "更新时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comment", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "edu_experience",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false, comment: "主键")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    major = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "专业", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    degree = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "学位", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    college_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "学校名字", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    start_time = table.Column<DateTime>(type: "datetime", nullable: true, comment: "开始时间"),
                    end_time = table.Column<DateTime>(type: "datetime", nullable: true, comment: "结束时间"),
                    num_id = table.Column<int>(type: "int", nullable: true, comment: "用户教育经历Id"),
                    unified_id = table.Column<int>(type: "int", nullable: false, comment: "用户Id"),
                    create_time = table.Column<DateTime>(type: "datetime", nullable: false, comment: "创建时间"),
                    update_time = table.Column<DateTime>(type: "datetime", nullable: false, comment: "更新时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_edu_experience", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "enterprise_info",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false, comment: "主键")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    unified_id = table.Column<int>(type: "int", nullable: false, comment: "独一五二的Id"),
                    contact_way = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "联系方式", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "描述", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    create_time = table.Column<DateTime>(type: "datetime", nullable: false, comment: "创建时间"),
                    update_time = table.Column<DateTime>(type: "datetime", nullable: false, comment: "更新时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_enterprise_info", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "job_experience",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false, comment: "主键")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    description = table.Column<string>(type: "varchar(1024)", maxLength: 1024, nullable: true, comment: "简介", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    position_type = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "职位类型", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    enterprise_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "企业名字", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    start_time = table.Column<DateTime>(type: "datetime", nullable: true, comment: "开始时间"),
                    end_time = table.Column<DateTime>(type: "datetime", nullable: true, comment: "结束时间"),
                    num_id = table.Column<int>(type: "int", nullable: true, comment: "用户工作顺序经历Id"),
                    unified_id = table.Column<int>(type: "int", nullable: true, comment: "用户Id"),
                    create_time = table.Column<DateTime>(type: "datetime", nullable: false, comment: "创建时间"),
                    update_time = table.Column<DateTime>(type: "datetime", nullable: false, comment: "更新时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_experience", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "liked",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false, comment: "主键")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    tweet_id = table.Column<int>(type: "int", nullable: true, comment: "动态Id"),
                    unified_id = table.Column<int>(type: "int", nullable: true, comment: "用户Id"),
                    create_time = table.Column<DateTime>(type: "datetime", nullable: false, comment: "创建时间"),
                    update_time = table.Column<DateTime>(type: "datetime", nullable: false, comment: "更新时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_liked", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "position",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false, comment: "岗位Id")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    job_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "工作名称", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    position_type = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "岗位类型", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "岗位描述", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    reward = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "薪资", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    contact_way = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "联系方式", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    work_place = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "工作地点(-分隔省市eg.河北-石家庄）", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    limitation = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "岗位限制", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    enterprise_id = table.Column<int>(type: "int", nullable: true, comment: "企业Id"),
                    url = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "岗位图片", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    create_time = table.Column<DateTime>(type: "datetime", nullable: false, comment: "创建时间"),
                    update_time = table.Column<DateTime>(type: "datetime", nullable: false, comment: "更新时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_position", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "subscribed",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false, comment: "关注Id")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    first_user_id = table.Column<int>(type: "int", nullable: true),
                    second_user_id = table.Column<int>(type: "int", nullable: true),
                    create_time = table.Column<DateTime>(type: "datetime", nullable: false, comment: "创建时间"),
                    update_time = table.Column<DateTime>(type: "datetime", nullable: false, comment: "更新时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subscribed", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "tweet",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false, comment: "动态Id")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    content = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "动态内容", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    picture_url = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "照片文件数组", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    unified_id = table.Column<int>(type: "int", nullable: false, comment: "用户Id"),
                    like_num = table.Column<int>(type: "int", nullable: true, comment: "喜欢数"),
                    comment_num = table.Column<int>(type: "int", nullable: true, comment: "评论数"),
                    create_time = table.Column<DateTime>(type: "datetime", nullable: false, comment: "创建时间"),
                    update_time = table.Column<DateTime>(type: "datetime", nullable: false, comment: "更新时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tweet", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    unified_id = table.Column<int>(type: "int", nullable: false, comment: "唯一Id")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "姓名（不可以改变）", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "密码", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    user_type = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "用户类型", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "邮箱", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    avatar = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "头像", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    brief_info = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "简要介绍", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    true_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "真实名字", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    subscribe_num = table.Column<int>(type: "int", nullable: true, comment: "关注的数量"),
                    background = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "背景图", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    create_time = table.Column<DateTime>(type: "datetime", nullable: false, comment: "创建时间"),
                    update_time = table.Column<DateTime>(type: "datetime", nullable: false, comment: "更新时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.unified_id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "user_info",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false, comment: "主键")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    unified_id = table.Column<int>(type: "int", nullable: false, comment: "独一无二的Id"),
                    gender = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, comment: "性别", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    age = table.Column<int>(type: "int", nullable: true, comment: "年龄"),
                    id_card = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "身份证", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    phone_num = table.Column<int>(type: "int", nullable: true, comment: "电话号码"),
                    pre_position = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "职位偏好", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    live_place = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "居住地(-分隔省市eg.河北-石家庄)", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    create_time = table.Column<DateTime>(type: "datetime", nullable: false, comment: "创建时间"),
                    update_time = table.Column<DateTime>(type: "datetime", nullable: false, comment: "更新时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_info", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "app_file");

            migrationBuilder.DropTable(
                name: "application");

            migrationBuilder.DropTable(
                name: "comment");

            migrationBuilder.DropTable(
                name: "edu_experience");

            migrationBuilder.DropTable(
                name: "enterprise_info");

            migrationBuilder.DropTable(
                name: "job_experience");

            migrationBuilder.DropTable(
                name: "liked");

            migrationBuilder.DropTable(
                name: "position");

            migrationBuilder.DropTable(
                name: "subscribed");

            migrationBuilder.DropTable(
                name: "tweet");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "user_info");
        }
    }
}
