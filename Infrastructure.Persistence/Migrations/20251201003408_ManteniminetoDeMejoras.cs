using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ManteniminetoDeMejoras : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ImprovementId1",
                table: "PropertyImprovements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Improvements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyImprovements_ImprovementId1",
                table: "PropertyImprovements",
                column: "ImprovementId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyImprovements_Improvements_ImprovementId1",
                table: "PropertyImprovements",
                column: "ImprovementId1",
                principalTable: "Improvements",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyImprovements_Improvements_ImprovementId1",
                table: "PropertyImprovements");

            migrationBuilder.DropIndex(
                name: "IX_PropertyImprovements_ImprovementId1",
                table: "PropertyImprovements");

            migrationBuilder.DropColumn(
                name: "ImprovementId1",
                table: "PropertyImprovements");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Improvements");
        }
    }
}
