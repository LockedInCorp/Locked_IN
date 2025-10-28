using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Locked_IN_Backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTimestampToTimestamptz : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "User",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<DateTime>(
                name: "jointimestamp",
                table: "team_member",
                type: "timestamptz",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "id", "availability", "email", "hashed_pass", "nickname" },
                values: new object[,]
                {
                    { 1, "{\"monday\": [\"18:00\", \"22:00\"], \"friday\": [\"19:00\", \"23:00\"]}", "john.doe@example.com", "hashed_password_1", "JohnDoe" },
                    { 2, "{\"tuesday\": [\"17:00\", \"21:00\"], \"saturday\": [\"14:00\", \"18:00\"]}", "jane.smith@example.com", "hashed_password_2", "JaneSmith" },
                    { 3, "{\"wednesday\": [\"20:00\", \"24:00\"], \"sunday\": [\"16:00\", \"20:00\"]}", "mike.wilson@example.com", "hashed_password_3", "MikeWilson" },
                    { 4, "{\"thursday\": [\"19:00\", \"23:00\"], \"saturday\": [\"15:00\", \"19:00\"]}", "sarah.johnson@example.com", "hashed_password_4", "SarahJ" }
                });

            migrationBuilder.InsertData(
                table: "experience_tag",
                columns: new[] { "id", "experiencelevel" },
                values: new object[,]
                {
                    { 1, "Beginner" },
                    { 2, "Intermediate" },
                    { 3, "Advanced" },
                    { 4, "Professional" }
                });

            migrationBuilder.InsertData(
                table: "game",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "Counter-Strike 2" },
                    { 2, "League of Legends" },
                    { 3, "Valorant" }
                });

            migrationBuilder.InsertData(
                table: "member_status",
                columns: new[] { "id", "statusname" },
                values: new object[,]
                {
                    { 1, "Active" },
                    { 2, "Pending" },
                    { 3, "Inactive" }
                });

            migrationBuilder.InsertData(
                table: "preference_tag",
                columns: new[] { "id", "preferencename" },
                values: new object[,]
                {
                    { 1, "Competitive" },
                    { 2, "Casual" },
                    { 3, "Communication" },
                    { 4, "Strategy Focus" },
                    { 5, "Fun First" },
                    { 6, "Skill Development" }
                });

            migrationBuilder.InsertData(
                table: "team",
                columns: new[] { "id", "description", "experience_tag_id", "game_id", "isblitz", "isprivate", "max_player_count", "min_comp_score", "name" },
                values: new object[,]
                {
                    { 1, 101, 3, 1, false, false, 5, 1500, "CS2 Legends" },
                    { 2, 102, 1, 2, true, true, 5, 800, "LoL Rookies" },
                    { 3, 103, 4, 3, false, false, 5, 2000, "Valorant Pros" },
                    { 4, 104, 2, 2, true, false, 6, null, "Casual Gamers" }
                });

            migrationBuilder.InsertData(
                table: "team_member",
                columns: new[] { "id", "isleader", "jointimestamp", "member_status_id", "team_id", "user_id" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2024, 9, 28, 10, 0, 0, 0, DateTimeKind.Utc), 1, 1, 1 },
                    { 2, false, new DateTime(2024, 10, 3, 14, 30, 0, 0, DateTimeKind.Utc), 1, 1, 2 },
                    { 3, true, new DateTime(2024, 10, 8, 16, 15, 0, 0, DateTimeKind.Utc), 1, 2, 3 },
                    { 4, false, new DateTime(2024, 10, 13, 11, 45, 0, 0, DateTimeKind.Utc), 2, 2, 4 },
                    { 5, true, new DateTime(2024, 10, 18, 9, 20, 0, 0, DateTimeKind.Utc), 1, 3, 1 },
                    { 6, true, new DateTime(2024, 10, 23, 18, 0, 0, 0, DateTimeKind.Utc), 1, 4, 2 }
                });

            migrationBuilder.InsertData(
                table: "team_preferencetag_relation",
                columns: new[] { "id", "preference_tag_id", "team_id" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 3, 1 },
                    { 3, 2, 2 },
                    { 4, 5, 2 },
                    { 5, 1, 3 },
                    { 6, 4, 3 },
                    { 7, 2, 4 },
                    { 8, 6, 4 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "member_status",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "team_member",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "team_member",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "team_member",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "team_member",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "team_member",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "team_member",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "team_preferencetag_relation",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "team_preferencetag_relation",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "team_preferencetag_relation",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "team_preferencetag_relation",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "team_preferencetag_relation",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "team_preferencetag_relation",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "team_preferencetag_relation",
                keyColumn: "id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "team_preferencetag_relation",
                keyColumn: "id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "member_status",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "member_status",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "preference_tag",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "preference_tag",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "preference_tag",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "preference_tag",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "preference_tag",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "preference_tag",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "team",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "team",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "team",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "team",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "experience_tag",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "experience_tag",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "experience_tag",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "experience_tag",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "game",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "game",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "game",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "User",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<DateTime>(
                name: "jointimestamp",
                table: "team_member",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamptz");
        }
    }
}
