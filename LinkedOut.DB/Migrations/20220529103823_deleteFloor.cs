using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinkedOut.DB.Migrations
{
    public partial class deleteFloor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "floor",
                table: "comment");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "floor",
                table: "comment",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                comment: "楼层",
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
