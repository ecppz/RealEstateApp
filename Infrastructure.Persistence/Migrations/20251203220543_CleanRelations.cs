using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CleanRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 🔹 PropertyTypeId1
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_PropertyTypes_PropertyTypeId1",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_PropertyTypeId1",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "PropertyTypeId1",
                table: "Properties");

            // 🔹 SaleTypeId1
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_SaleTypes_SaleTypeId1",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_SaleTypeId1",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "SaleTypeId1",
                table: "Properties");

            // 🔹 ImprovementId1
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyImprovements_Improvements_ImprovementId1",
                table: "PropertyImprovements");

            migrationBuilder.DropIndex(
                name: "IX_PropertyImprovements_ImprovementId1",
                table: "PropertyImprovements");

            migrationBuilder.DropColumn(
                name: "ImprovementId1",
                table: "PropertyImprovements");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // 🔹 Primero elimina el FK y el índice de PropertyTypeId1
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_PropertyTypes_PropertyTypeId1",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_PropertyTypeId1",
                table: "Properties");

            // 🔹 Ahora sí elimina la columna
            migrationBuilder.DropColumn(
                name: "PropertyTypeId1",
                table: "Properties");

            // 🔹 Repite lo mismo para SaleTypeId1
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_SaleTypes_SaleTypeId1",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_SaleTypeId1",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "SaleTypeId1",
                table: "Properties");

            // 🔹 Y para ImprovementId1
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyImprovements_Improvements_ImprovementId1",
                table: "PropertyImprovements");

            migrationBuilder.DropIndex(
                name: "IX_PropertyImprovements_ImprovementId1",
                table: "PropertyImprovements");

            migrationBuilder.DropColumn(
                name: "ImprovementId1",
                table: "PropertyImprovements");
        }
    }
}
