using Microsoft.EntityFrameworkCore.Migrations;

namespace isDotServer.Migrations
{
    public partial class _2thMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GameSessions",
                table: "GameSessions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameSessions",
                table: "GameSessions",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_GameSessions_HostId",
                table: "GameSessions",
                column: "HostId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GameSessions",
                table: "GameSessions");

            migrationBuilder.DropIndex(
                name: "IX_GameSessions_HostId",
                table: "GameSessions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameSessions",
                table: "GameSessions",
                columns: new[] { "HostId", "GuestId" });
        }
    }
}
