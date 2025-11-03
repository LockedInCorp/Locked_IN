using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Locked_IN_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddCreationTimestampToTeam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "creation_timespamp",
                table: "team",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "team",
                keyColumn: "id",
                keyValue: 1,
                column: "creation_timespamp",
                value: new DateTime(2024, 9, 28, 10, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "team",
                keyColumn: "id",
                keyValue: 2,
                column: "creation_timespamp",
                value: new DateTime(2024, 10, 1, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "team",
                keyColumn: "id",
                keyValue: 3,
                column: "creation_timespamp",
                value: new DateTime(2024, 10, 5, 15, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "team",
                keyColumn: "id",
                keyValue: 4,
                column: "creation_timespamp",
                value: new DateTime(2024, 10, 10, 18, 0, 0, 0, DateTimeKind.Utc));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "creation_timespamp",
                table: "team");
        }
    }
}
