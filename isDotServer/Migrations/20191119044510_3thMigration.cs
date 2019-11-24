using Microsoft.EntityFrameworkCore.Migrations;

namespace isDotServer.Migrations
{
    public partial class _3thMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WhosTurn",
                table: "GameSessions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WhosTurn",
                table: "GameSessions");
        }
    }
}
