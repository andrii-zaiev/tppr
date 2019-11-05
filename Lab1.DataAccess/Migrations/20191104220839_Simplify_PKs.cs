using Microsoft.EntityFrameworkCore.Migrations;

namespace Lab1.DataAccess.Migrations
{
    public partial class Simplify_PKs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhoneSelectionId",
                table: "PhoneSelections",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "PhoneParameterValueId",
                table: "PhoneParameterValues",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ParameterValueId",
                table: "ParameterValues",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ParameterId",
                table: "Parameters",
                newName: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "PhoneSelections",
                newName: "PhoneSelectionId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "PhoneParameterValues",
                newName: "PhoneParameterValueId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ParameterValues",
                newName: "ParameterValueId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Parameters",
                newName: "ParameterId");
        }
    }
}
