using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dyvenix.App1.App.Api.Migrations
{
    /// <inheritdoc />
    public partial class ClientAuditAndSoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByUserId",
                table: "Client",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedUtc",
                table: "Client",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedByUserId",
                table: "Client",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedUtc",
                table: "Client",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedByUserId",
                table: "Client",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedUtc",
                table: "Client",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Client_DeletedByUserId",
                table: "Client",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Client_ModifiedByUserId",
                table: "Client",
                column: "ModifiedByUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Client_DeletedByUserId",
                table: "Client");

            migrationBuilder.DropIndex(
                name: "IX_Client_ModifiedByUserId",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "CreatedUtc",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "DeletedUtc",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "ModifiedByUserId",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "ModifiedUtc",
                table: "Client");
        }
    }
}
