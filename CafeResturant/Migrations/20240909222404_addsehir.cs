using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CafeResturant.Migrations
{
    /// <inheritdoc />
    public partial class addsehir : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Sehir",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sehir",
                table: "AspNetUsers");
        }
    }
}
