using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Locked_IN_Backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateChatEntitiesWithNewFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Update chat.name column length from 20 to 100
            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "chat",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            // Add last_message_at to chat table
            migrationBuilder.AddColumn<DateTime>(
                name: "last_message_at",
                table: "chat",
                type: "timestamp without time zone",
                nullable: true);

            // Add last_read_at and unread_count to chatparticipant table
            migrationBuilder.AddColumn<DateTime>(
                name: "last_read_at",
                table: "chatparticipant",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "unread_count",
                table: "chatparticipant",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            // Update message.content column length from 150 to 2000
            migrationBuilder.AlterColumn<string>(
                name: "content",
                table: "message",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150);

            // Add new fields to message table
            migrationBuilder.AddColumn<DateTime>(
                name: "edited_at",
                table: "message",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "message",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_at",
                table: "message",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "attachment_url",
                table: "message",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove new fields from message table
            migrationBuilder.DropColumn(
                name: "attachment_url",
                table: "message");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "message");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "message");

            migrationBuilder.DropColumn(
                name: "edited_at",
                table: "message");

            // Revert message.content column length back to 150
            migrationBuilder.AlterColumn<string>(
                name: "content",
                table: "message",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000);

            // Remove new fields from chatparticipant table
            migrationBuilder.DropColumn(
                name: "unread_count",
                table: "chatparticipant");

            migrationBuilder.DropColumn(
                name: "last_read_at",
                table: "chatparticipant");

            // Remove last_message_at from chat table
            migrationBuilder.DropColumn(
                name: "last_message_at",
                table: "chat");

            // Revert chat.name column length back to 20
            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "chat",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}
