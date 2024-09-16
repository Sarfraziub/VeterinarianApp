using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VeterinarianApp.Migrations
{
    /// <inheritdoc />
    public partial class Domainupdatedrelationshipupdatedandnewfiledsadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FaceBook",
                table: "Veterinarians",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LicenseNo",
                table: "Veterinarians",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "VetHuntScore",
                table: "Veterinarians",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VeterinarianId",
                table: "Clinics",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "VeterinarianServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    VeterinarianId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VeterinarianServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VeterinarianServices_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VeterinarianServices_Veterinarians_VeterinarianId",
                        column: x => x.VeterinarianId,
                        principalTable: "Veterinarians",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clinics_VeterinarianId",
                table: "Clinics",
                column: "VeterinarianId",
                unique: true,
                filter: "[VeterinarianId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_VeterinarianServices_ServiceId",
                table: "VeterinarianServices",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_VeterinarianServices_VeterinarianId",
                table: "VeterinarianServices",
                column: "VeterinarianId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clinics_Veterinarians_VeterinarianId",
                table: "Clinics",
                column: "VeterinarianId",
                principalTable: "Veterinarians",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clinics_Veterinarians_VeterinarianId",
                table: "Clinics");

            migrationBuilder.DropTable(
                name: "VeterinarianServices");

            migrationBuilder.DropIndex(
                name: "IX_Clinics_VeterinarianId",
                table: "Clinics");

            migrationBuilder.DropColumn(
                name: "FaceBook",
                table: "Veterinarians");

            migrationBuilder.DropColumn(
                name: "LicenseNo",
                table: "Veterinarians");

            migrationBuilder.DropColumn(
                name: "VetHuntScore",
                table: "Veterinarians");

            migrationBuilder.DropColumn(
                name: "VeterinarianId",
                table: "Clinics");
        }
    }
}
