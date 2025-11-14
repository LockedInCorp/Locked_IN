using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Locked_IN_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddIconsToTeamTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "icon_url",
                table: "team",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "team",
                keyColumn: "id",
                keyValue: 1,
                column: "icon_url",
                value: "https://pl.wikipedia.org/wiki/World_of_Warcraft#/media/Plik:WoW_icon.svg");

            migrationBuilder.UpdateData(
                table: "team",
                keyColumn: "id",
                keyValue: 2,
                column: "icon_url",
                value: "https://pl.wikipedia.org/wiki/World_of_Warcraft#/media/Plik:WoW_icon.svg");

            migrationBuilder.UpdateData(
                table: "team",
                keyColumn: "id",
                keyValue: 3,
                column: "icon_url",
                value: "https://pl.wikipedia.org/wiki/World_of_Warcraft#/media/Plik:WoW_icon.svg");

            migrationBuilder.UpdateData(
                table: "team",
                keyColumn: "id",
                keyValue: 4,
                column: "icon_url",
                value: "https://pl.wikipedia.org/wiki/World_of_Warcraft#/media/Plik:WoW_icon.svg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "icon_url",
                table: "team");
        }
    }
}
