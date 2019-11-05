using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Lab1.DataAccess.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Parameters",
                columns: table => new
                {
                    ParameterId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Optimality = table.Column<int>(nullable: false),
                    Unit = table.Column<string>(maxLength: 20, nullable: true),
                    Scale = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parameters", x => x.ParameterId);
                });

            migrationBuilder.CreateTable(
                name: "Phones",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Competence = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParameterValues",
                columns: table => new
                {
                    ParameterValueId = table.Column<Guid>(nullable: false),
                    ParameterId = table.Column<Guid>(nullable: false),
                    Value = table.Column<double>(nullable: false),
                    ValueText = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParameterValues", x => x.ParameterValueId);
                    table.ForeignKey(
                        name: "FK_ParameterValues_Parameters_ParameterId",
                        column: x => x.ParameterId,
                        principalTable: "Parameters",
                        principalColumn: "ParameterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhoneSelections",
                columns: table => new
                {
                    PhoneSelectionId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    PhoneId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhoneSelections", x => x.PhoneSelectionId);
                    table.ForeignKey(
                        name: "FK_PhoneSelections_Phones_PhoneId",
                        column: x => x.PhoneId,
                        principalTable: "Phones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PhoneSelections_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhoneParameterValues",
                columns: table => new
                {
                    PhoneParameterValueId = table.Column<Guid>(nullable: false),
                    PhoneId = table.Column<Guid>(nullable: false),
                    ParameterValueId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhoneParameterValues", x => x.PhoneParameterValueId);
                    table.ForeignKey(
                        name: "FK_PhoneParameterValues_ParameterValues_ParameterValueId",
                        column: x => x.ParameterValueId,
                        principalTable: "ParameterValues",
                        principalColumn: "ParameterValueId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PhoneParameterValues_Phones_PhoneId",
                        column: x => x.PhoneId,
                        principalTable: "Phones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParameterValues_ParameterId",
                table: "ParameterValues",
                column: "ParameterId");

            migrationBuilder.CreateIndex(
                name: "IX_PhoneParameterValues_ParameterValueId",
                table: "PhoneParameterValues",
                column: "ParameterValueId");

            migrationBuilder.CreateIndex(
                name: "IX_PhoneParameterValues_PhoneId",
                table: "PhoneParameterValues",
                column: "PhoneId");

            migrationBuilder.CreateIndex(
                name: "IX_PhoneSelections_PhoneId",
                table: "PhoneSelections",
                column: "PhoneId");

            migrationBuilder.CreateIndex(
                name: "IX_PhoneSelections_UserId",
                table: "PhoneSelections",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PhoneParameterValues");

            migrationBuilder.DropTable(
                name: "PhoneSelections");

            migrationBuilder.DropTable(
                name: "ParameterValues");

            migrationBuilder.DropTable(
                name: "Phones");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Parameters");
        }
    }
}
