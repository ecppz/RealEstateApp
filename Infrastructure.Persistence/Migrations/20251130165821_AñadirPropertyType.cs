using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AñadirPropertyType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PropertyTypeId1",
                table: "Properties",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Properties_PropertyTypeId1",
                table: "Properties",
                column: "PropertyTypeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_PropertyTypes_PropertyTypeId1",
                table: "Properties",
                column: "PropertyTypeId1",
                principalTable: "PropertyTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_PropertyTypes_PropertyTypeId1",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_PropertyTypeId1",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "PropertyTypeId1",
                table: "Properties");
        }
    }
}
