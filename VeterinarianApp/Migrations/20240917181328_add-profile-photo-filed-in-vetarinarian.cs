using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VeterinarianApp.Migrations
{
    /// <inheritdoc />
    public partial class addprofilephotofiledinvetarinarian : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfilePhoto",
                table: "Veterinarians",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePhoto",
                table: "Veterinarians");
        }
    }
}
