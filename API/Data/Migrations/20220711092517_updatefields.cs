using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatefields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isMain",
                table: "photos",
                newName: "IsMain");

            migrationBuilder.RenameColumn(
                name: "Public",
                table: "photos",
                newName: "PublicId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsMain",
                table: "photos",
                newName: "isMain");

            migrationBuilder.RenameColumn(
                name: "PublicId",
                table: "photos",
                newName: "Public");
        }
    }
}
