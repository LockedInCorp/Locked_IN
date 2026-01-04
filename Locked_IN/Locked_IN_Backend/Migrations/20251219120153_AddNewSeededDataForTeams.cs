using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Locked_IN_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddNewSeededDataForTeams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "avatar_url",
                table: "User",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "id",
                keyValue: 1,
                column: "avatar_url",
                value: "https://example.com/avatars/john.png");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "id",
                keyValue: 2,
                column: "avatar_url",
                value: null);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "id",
                keyValue: 3,
                column: "avatar_url",
                value: null);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "id",
                keyValue: 4,
                column: "avatar_url",
                value: null);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "id",
                keyValue: 5,
                column: "avatar_url",
                value: null);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "id",
                keyValue: 6,
                column: "avatar_url",
                value: null);

            migrationBuilder.InsertData(
                table: "game_exp",
                columns: new[] { "id", "experience" },
                values: new object[,]
                {
                    { 1, "< 100 hours" },
                    { 2, "100-500 hours" },
                    { 3, "500-1000 hours" },
                    { 4, "1000+ hours" }
                });

            migrationBuilder.InsertData(
                table: "gameplay_pref",
                columns: new[] { "id", "preference" },
                values: new object[,]
                {
                    { 1, "Voice Chat Only" },
                    { 2, "Ping Only" },
                    { 3, "Any Communication" }
                });

            migrationBuilder.InsertData(
                table: "team",
                columns: new[] { "id", "creation_timespamp", "description", "experience_tag_id", "game_id", "icon_url", "isblitz", "isprivate", "max_player_count", "min_comp_score", "name" },
                values: new object[,]
                {
                    { 5, new DateTime(2024, 9, 15, 19, 30, 0, 0, DateTimeKind.Utc), "TestDescription5", 2, 1, "https://pl.wikipedia.org/wiki/World_of_Warcraft#/media/Plik:WoW_icon.svg", true, false, 5, 1200, "Aim Squad" },
                    { 6, new DateTime(2024, 8, 30, 8, 45, 0, 0, DateTimeKind.Utc), "TestDescription6", 1, 2, "https://pl.wikipedia.org/wiki/World_of_Warcraft#/media/Plik:WoW_icon.svg", false, true, 5, 900, "Nexus Five" },
                    { 7, new DateTime(2024, 10, 2, 21, 10, 0, 0, DateTimeKind.Utc), "TestDescription7", 3, 3, "https://pl.wikipedia.org/wiki/World_of_Warcraft#/media/Plik:WoW_icon.svg", false, false, 5, 1800, "VLR Strike" },
                    { 8, new DateTime(2024, 9, 5, 13, 5, 0, 0, DateTimeKind.Utc), "TestDescription8", 1, 2, "https://pl.wikipedia.org/wiki/World_of_Warcraft#/media/Plik:WoW_icon.svg", false, false, 5, null, "Chill Queue" },
                    { 9, new DateTime(2024, 11, 1, 9, 0, 0, 0, DateTimeKind.Utc), "TestDescription9", 4, 1, "https://pl.wikipedia.org/wiki/World_of_Warcraft#/media/Plik:WoW_icon.svg", true, true, 5, 1600, "Peak Hold" },
                    { 10, new DateTime(2024, 8, 20, 17, 25, 0, 0, DateTimeKind.Utc), "TestDescription10", 2, 3, "https://pl.wikipedia.org/wiki/World_of_Warcraft#/media/Plik:WoW_icon.svg", true, false, 5, 1100, "Spike Rush" }
                });

            migrationBuilder.InsertData(
                table: "team_member",
                columns: new[] { "id", "isleader", "jointimestamp", "member_status_id", "team_id", "user_id" },
                values: new object[,]
                {
                    { 7, true, new DateTime(2024, 9, 16, 10, 0, 0, 0, DateTimeKind.Utc), 1, 5, 3 },
                    { 8, true, new DateTime(2024, 9, 1, 12, 30, 0, 0, DateTimeKind.Utc), 1, 6, 4 },
                    { 9, true, new DateTime(2024, 10, 3, 20, 15, 0, 0, DateTimeKind.Utc), 1, 7, 5 },
                    { 10, true, new DateTime(2024, 9, 6, 9, 5, 0, 0, DateTimeKind.Utc), 1, 8, 6 },
                    { 11, true, new DateTime(2024, 11, 2, 10, 0, 0, 0, DateTimeKind.Utc), 1, 9, 1 },
                    { 12, true, new DateTime(2024, 8, 21, 18, 0, 0, 0, DateTimeKind.Utc), 1, 10, 2 }
                });

            migrationBuilder.InsertData(
                table: "team_preferencetag_relation",
                columns: new[] { "id", "preference_tag_id", "team_id" },
                values: new object[,]
                {
                    { 9, 1, 5 },
                    { 10, 6, 5 },
                    { 11, 2, 6 },
                    { 12, 3, 6 },
                    { 13, 4, 7 },
                    { 14, 1, 7 },
                    { 15, 5, 8 },
                    { 16, 2, 8 },
                    { 17, 3, 9 },
                    { 18, 6, 9 },
                    { 19, 4, 10 },
                    { 20, 1, 10 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "game_exp",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "game_exp",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "game_exp",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "game_exp",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "gameplay_pref",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "gameplay_pref",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "gameplay_pref",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "team_member",
                keyColumn: "id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "team_member",
                keyColumn: "id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "team_member",
                keyColumn: "id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "team_member",
                keyColumn: "id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "team_member",
                keyColumn: "id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "team_member",
                keyColumn: "id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "team_preferencetag_relation",
                keyColumn: "id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "team_preferencetag_relation",
                keyColumn: "id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "team_preferencetag_relation",
                keyColumn: "id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "team_preferencetag_relation",
                keyColumn: "id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "team_preferencetag_relation",
                keyColumn: "id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "team_preferencetag_relation",
                keyColumn: "id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "team_preferencetag_relation",
                keyColumn: "id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "team_preferencetag_relation",
                keyColumn: "id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "team_preferencetag_relation",
                keyColumn: "id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "team_preferencetag_relation",
                keyColumn: "id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "team_preferencetag_relation",
                keyColumn: "id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "team_preferencetag_relation",
                keyColumn: "id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "team",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "team",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "team",
                keyColumn: "id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "team",
                keyColumn: "id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "team",
                keyColumn: "id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "team",
                keyColumn: "id",
                keyValue: 10);

            migrationBuilder.DropColumn(
                name: "avatar_url",
                table: "User");
        }
    }
}
