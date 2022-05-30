using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinkedOut.DB.Migrations
{
    public partial class deleteNum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "num_id",
                table: "edu_experience");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "num_id",
                table: "edu_experience",
                type: "int",
                nullable: true,
                comment: "用户教育经历Id");
        }
    }
}
