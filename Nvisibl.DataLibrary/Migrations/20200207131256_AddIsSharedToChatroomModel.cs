using Microsoft.EntityFrameworkCore.Migrations;

namespace Nvisibl.DataLibrary.Migrations
{
    public partial class AddIsSharedToChatroomModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsShared",
                table: "Chatrooms",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsShared",
                table: "Chatrooms");
        }
    }
}
