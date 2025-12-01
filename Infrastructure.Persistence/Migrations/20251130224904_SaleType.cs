using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SaleType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SaleTypeId1",
                table: "Properties",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Properties_SaleTypeId1",
                table: "Properties",
                column: "SaleTypeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_SaleTypes_SaleTypeId1",
                table: "Properties",
                column: "SaleTypeId1",
                principalTable: "SaleTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_SaleTypes_SaleTypeId1",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_SaleTypeId1",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "SaleTypeId1",
                table: "Properties");
        }
    }
}
