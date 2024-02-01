using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatRoomManagement.Infrastructure.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class AddFileAttach : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "File",
                table: "Chats",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "File",
                table: "Chats");
        }
    }
}
