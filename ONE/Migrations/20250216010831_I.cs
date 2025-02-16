using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ONE.Migrations
{
    /// <inheritdoc />
    public partial class I : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostImage",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<byte[]>(
                name: "PictureSource",
                table: "AspNetUsers",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PictureSource",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "PostImage",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
