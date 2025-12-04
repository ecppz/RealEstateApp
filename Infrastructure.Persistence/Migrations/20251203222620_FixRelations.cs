using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_PropertyTypes_PropertyTypeId1",
                table: "Properties");

            migrationBuilder.DropForeignKey(
                name: "FK_Properties_SaleTypes_SaleTypeId1",
                table: "Properties");

            migrationBuilder.DropForeignKey(
                name: "FK_PropertyImprovements_Improvements_ImprovementId1",
                table: "PropertyImprovements");

            migrationBuilder.DropIndex(
                name: "IX_PropertyImprovements_ImprovementId1",
                table: "PropertyImprovements");

            migrationBuilder.DropIndex(
                name: "IX_Properties_PropertyTypeId1",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_SaleTypeId1",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "ImprovementId1",
                table: "PropertyImprovements");

            migrationBuilder.DropColumn(
                name: "PropertyTypeId1",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "SaleTypeId1",
                table: "Properties");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ImprovementId1",
                table: "PropertyImprovements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PropertyTypeId1",
                table: "Properties",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SaleTypeId1",
                table: "Properties",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PropertyImprovements_ImprovementId1",
                table: "PropertyImprovements",
                column: "ImprovementId1");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_PropertyTypeId1",
                table: "Properties",
                column: "PropertyTypeId1");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_SaleTypeId1",
                table: "Properties",
                column: "SaleTypeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_PropertyTypes_PropertyTypeId1",
                table: "Properties",
                column: "PropertyTypeId1",
                principalTable: "PropertyTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_SaleTypes_SaleTypeId1",
                table: "Properties",
                column: "SaleTypeId1",
                principalTable: "SaleTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyImprovements_Improvements_ImprovementId1",
                table: "PropertyImprovements",
                column: "ImprovementId1",
                principalTable: "Improvements",
                principalColumn: "Id");
        }
    }
}
