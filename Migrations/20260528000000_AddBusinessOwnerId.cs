using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MunchrBackendV2.Migrations
{
    /// <inheritdoc />
    public partial class AddBusinessOwnerId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Business",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Business");
        }
    }
}
