using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dyvenix.App1.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Patient_PatientId",
                table: "Invoice");

            migrationBuilder.RenameColumn(
                name: "PersonId",
                table: "Invoice",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoice_PersonId",
                table: "Invoice",
                newName: "IX_Invoice_CategoryId");

            migrationBuilder.AlterColumn<Guid>(
                name: "PatientId",
                table: "Invoice",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Category_Id",
                table: "Category",
                column: "Id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Category_CategoryId",
                table: "Invoice",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Patient_PatientId",
                table: "Invoice",
                column: "PatientId",
                principalTable: "Patient",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Category_CategoryId",
                table: "Invoice");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Patient_PatientId",
                table: "Invoice");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Invoice",
                newName: "PersonId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoice_CategoryId",
                table: "Invoice",
                newName: "IX_Invoice_PersonId");

            migrationBuilder.AlterColumn<Guid>(
                name: "PatientId",
                table: "Invoice",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Patient_PatientId",
                table: "Invoice",
                column: "PatientId",
                principalTable: "Patient",
                principalColumn: "Id");
        }
    }
}
