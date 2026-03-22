using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dyvenix.App1.App.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Key = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ExtAuthId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ExtClientId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BaseUrl = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Client_DeletedByUserId",
                table: "Client",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Client_Id",
                table: "Client",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Client_Key",
                table: "Client",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Client_ModifiedByUserId",
                table: "Client",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Client_Name",
                table: "Client",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Client");
        }
    }
}
