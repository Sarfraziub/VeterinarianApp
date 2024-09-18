using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VeterinarianApp.Migrations
{
    /// <inheritdoc />
    public partial class updatefiledname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "SurveryQuestions",
                newName: "SurveyQuestionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SurveyQuestionId",
                table: "SurveryQuestions",
                newName: "Id");
        }
    }
}
