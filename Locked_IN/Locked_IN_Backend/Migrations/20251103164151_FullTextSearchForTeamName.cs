using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Locked_IN_Backend.Migrations
{
    /// <inheritdoc />
    public partial class FullTextSearchForTeamName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "team",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.UpdateData(
                table: "team",
                keyColumn: "id",
                keyValue: 1,
                column: "description",
                value: "TestDescription1");

            migrationBuilder.UpdateData(
                table: "team",
                keyColumn: "id",
                keyValue: 2,
                column: "description",
                value: "TestDescription2");

            migrationBuilder.UpdateData(
                table: "team",
                keyColumn: "id",
                keyValue: 3,
                column: "description",
                value: "TestDescription3");

            migrationBuilder.UpdateData(
                table: "team",
                keyColumn: "id",
                keyValue: 4,
                column: "description",
                value: "TestDescription4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "description",
                table: "team",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.UpdateData(
                table: "team",
                keyColumn: "id",
                keyValue: 1,
                column: "description",
                value: 101);

            migrationBuilder.UpdateData(
                table: "team",
                keyColumn: "id",
                keyValue: 2,
                column: "description",
                value: 102);

            migrationBuilder.UpdateData(
                table: "team",
                keyColumn: "id",
                keyValue: 3,
                column: "description",
                value: 103);

            migrationBuilder.UpdateData(
                table: "team",
                keyColumn: "id",
                keyValue: 4,
                column: "description",
                value: 104);
        }
    }
}
