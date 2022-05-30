using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinkedOut.DB.Migrations
{
    public partial class deleteJobNum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "num_id",
                table: "job_experience");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "num_id",
                table: "job_experience",
                type: "int",
                nullable: true,
                comment: "用户工作顺序经历Id");
        }
    }
}
