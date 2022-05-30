using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinkedOut.DB.Migrations
{
    public partial class addResume : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "resume",
                table: "user_info",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: true,
                comment: "简历(用逗号分隔)",
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "resume",
                table: "user_info");
        }
    }
}
