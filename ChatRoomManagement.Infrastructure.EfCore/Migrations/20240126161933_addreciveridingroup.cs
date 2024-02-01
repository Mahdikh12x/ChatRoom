using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatRoomManagement.Infrastructure.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class addreciveridingroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ReciverId",
                table: "Groups",
                type: "bigint",
                nullable: true,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReciverId",
                table: "Groups");
        }
    }
}
