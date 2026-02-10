using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dyvenix.App1.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPractice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PracticeId",
                table: "Patient",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Practice",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Practice", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Patient_PracticeId",
                table: "Patient",
                column: "PracticeId");

            migrationBuilder.CreateIndex(
                name: "IX_Practice_Id",
                table: "Practice",
                column: "Id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Patient_Practice_PracticeId",
                table: "Patient",
                column: "PracticeId",
                principalTable: "Practice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patient_Practice_PracticeId",
                table: "Patient");

            migrationBuilder.DropTable(
                name: "Practice");

            migrationBuilder.DropIndex(
                name: "IX_Patient_PracticeId",
                table: "Patient");

            migrationBuilder.DropColumn(
                name: "PracticeId",
                table: "Patient");
        }
    }
}
