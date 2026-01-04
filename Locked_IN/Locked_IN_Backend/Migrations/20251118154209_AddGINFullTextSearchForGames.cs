using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Locked_IN_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddGINFullTextSearchForGames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "experience_tag",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    experiencelevel = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("experience_tag_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "friendship_status",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    status_name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("friendship_status_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "game",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("game_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "game_exp",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    experience = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("game_exp_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "gameplay_pref",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    preference = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("gameplay_pref_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "member_status",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    statusname = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("member_status_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "preference_tag",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    preferencename = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("preference_tag_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    rolename = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("role_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    nickname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    hashed_pass = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    availability = table.Column<string>(type: "json", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "team",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    min_comp_score = table.Column<int>(type: "integer", nullable: true),
                    max_player_count = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    game_id = table.Column<int>(type: "integer", nullable: false),
                    isprivate = table.Column<bool>(type: "boolean", nullable: false),
                    isblitz = table.Column<bool>(type: "boolean", nullable: false),
                    experience_tag_id = table.Column<int>(type: "integer", nullable: false),
                    creation_timespamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    icon_url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("team_pk", x => x.id);
                    table.ForeignKey(
                        name: "team_experience_tag",
                        column: x => x.experience_tag_id,
                        principalTable: "experience_tag",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "team_game",
                        column: x => x.game_id,
                        principalTable: "game",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "friendship",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    request_timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    user_2_id = table.Column<int>(type: "integer", nullable: false),
                    status_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("friendship_pk", x => x.id);
                    table.ForeignKey(
                        name: "friendship_status",
                        column: x => x.status_id,
                        principalTable: "friendship_status",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "friendship_user_1",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "friendship_user_2",
                        column: x => x.user_2_id,
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "game_profile",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    game_id = table.Column<int>(type: "integer", nullable: false),
                    rank = table.Column<int>(type: "integer", nullable: true),
                    isfavorite = table.Column<bool>(type: "boolean", nullable: false),
                    experience_tag_id = table.Column<int>(type: "integer", nullable: false),
                    game_exp_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("game_profile_pk", x => x.id);
                    table.ForeignKey(
                        name: "game_profile_experience_tag",
                        column: x => x.experience_tag_id,
                        principalTable: "experience_tag",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "game_profile_game_exp",
                        column: x => x.game_exp_id,
                        principalTable: "game_exp",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "user_game_game",
                        column: x => x.game_id,
                        principalTable: "game",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "user_game_user",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "chat",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    team_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("chat_pk", x => x.id);
                    table.ForeignKey(
                        name: "FK_chat_team_team_id",
                        column: x => x.team_id,
                        principalTable: "team",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "team_member",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    isleader = table.Column<bool>(type: "boolean", nullable: false),
                    jointimestamp = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    team_id = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    member_status_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("team_member_pk", x => x.id);
                    table.ForeignKey(
                        name: "team_member_status",
                        column: x => x.member_status_id,
                        principalTable: "member_status",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "team_member_team",
                        column: x => x.team_id,
                        principalTable: "team",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "team_member_user",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "team_preferencetag_relation",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    team_id = table.Column<int>(type: "integer", nullable: false),
                    preference_tag_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("team_preferencetag_relation_pk", x => x.id);
                    table.ForeignKey(
                        name: "team_preferencetag_relation_preference_tag",
                        column: x => x.preference_tag_id,
                        principalTable: "preference_tag",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "team_preferencetag_relation_team",
                        column: x => x.team_id,
                        principalTable: "team",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "game_profile_pref",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    gameplay_pref_id = table.Column<int>(type: "integer", nullable: false),
                    game_profile_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("game_profile_pref_pk", x => x.id);
                    table.ForeignKey(
                        name: "game_profile_pref_game_profile",
                        column: x => x.game_profile_id,
                        principalTable: "game_profile",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "game_profile_pref_gameplay_pref",
                        column: x => x.gameplay_pref_id,
                        principalTable: "gameplay_pref",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "gameprofile_preferencetag_relation",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    game_profile_id = table.Column<int>(type: "integer", nullable: false),
                    preference_tag_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("gameprofile_preferencetag_relation_pk", x => x.id);
                    table.ForeignKey(
                        name: "gameprofile_preferencetag_relation_game_profile",
                        column: x => x.game_profile_id,
                        principalTable: "game_profile",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "gameprofile_preferencetag_relation_preference_tag",
                        column: x => x.preference_tag_id,
                        principalTable: "preference_tag",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "chatparticipant",
                columns: table => new
                {
                    chatparticipant_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    chat_id = table.Column<int>(type: "integer", nullable: false),
                    joined_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    role_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("chatparticipant_pk", x => x.chatparticipant_id);
                    table.ForeignKey(
                        name: "FK_chatparticipant_User_user_id",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_chatparticipant_chat_chat_id",
                        column: x => x.chat_id,
                        principalTable: "chat",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_chatparticipant_role_role_id",
                        column: x => x.role_id,
                        principalTable: "role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "message",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    content = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    sent_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    chatparticipant_chatparticipant_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("message_pk", x => x.id);
                    table.ForeignKey(
                        name: "message_chatparticipant",
                        column: x => x.chatparticipant_chatparticipant_id,
                        principalTable: "chatparticipant",
                        principalColumn: "chatparticipant_id");
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "id", "availability", "email", "hashed_pass", "nickname" },
                values: new object[,]
                {
                    { 1, "{\"monday\": [\"18:00\", \"22:00\"], \"friday\": [\"19:00\", \"23:00\"]}", "john.doe@example.com", "hashed_password_1", "JohnDoe" },
                    { 2, "{\"tuesday\": [\"17:00\", \"21:00\"], \"saturday\": [\"14:00\", \"18:00\"]}", "jane.smith@example.com", "hashed_password_2", "JaneSmith" },
                    { 3, "{\"wednesday\": [\"20:00\", \"24:00\"], \"sunday\": [\"16:00\", \"20:00\"]}", "mike.wilson@example.com", "hashed_password_3", "MikeWilson" },
                    { 4, "{\"thursday\": [\"19:00\", \"23:00\"], \"saturday\": [\"15:00\", \"19:00\"]}", "sarah.johnson@example.com", "hashed_password_4", "SarahJ" },
                    { 5, "{}", "test.user5@example.com", "hashed_password_5", "TestUser5" },
                    { 6, "{}", "test.user6@example.com", "hashed_password_6", "TestUser6" }
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
                table: "friendship_status",
                columns: new[] { "id", "status_name" },
                values: new object[,]
                {
                    { 1, "Pending" },
                    { 2, "Accepted" },
                    { 3, "Blocked" }
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
                    { 1, "Leader" },
                    { 2, "Member" },
                    { 3, "Pending" },
                    { 4, "Rejected" }
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
                table: "friendship",
                columns: new[] { "id", "request_timestamp", "status_id", "user_2_id", "user_id" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, 2, 1 },
                    { 2, new DateTime(2024, 10, 5, 0, 0, 0, 0, DateTimeKind.Utc), 1, 1, 3 }
                });

            migrationBuilder.InsertData(
                table: "team",
                columns: new[] { "id", "creation_timespamp", "description", "experience_tag_id", "game_id", "icon_url", "isblitz", "isprivate", "max_player_count", "min_comp_score", "name" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 9, 28, 10, 0, 0, 0, DateTimeKind.Utc), "TestDescription1", 3, 1, "https://pl.wikipedia.org/wiki/World_of_Warcraft#/media/Plik:WoW_icon.svg", false, false, 5, 1500, "CS2 Legends" },
                    { 2, new DateTime(2024, 10, 1, 12, 0, 0, 0, DateTimeKind.Utc), "TestDescription2", 1, 2, "https://pl.wikipedia.org/wiki/World_of_Warcraft#/media/Plik:WoW_icon.svg", true, true, 5, 800, "LoL Rookies" },
                    { 3, new DateTime(2024, 10, 5, 15, 0, 0, 0, DateTimeKind.Utc), "TestDescription3", 4, 3, "https://pl.wikipedia.org/wiki/World_of_Warcraft#/media/Plik:WoW_icon.svg", false, false, 5, 2000, "Valorant Pros" },
                    { 4, new DateTime(2024, 10, 10, 18, 0, 0, 0, DateTimeKind.Utc), "TestDescription4", 2, 2, "https://pl.wikipedia.org/wiki/World_of_Warcraft#/media/Plik:WoW_icon.svg", true, false, 6, null, "Casual Gamers" }
                });

            migrationBuilder.InsertData(
                table: "team_member",
                columns: new[] { "id", "isleader", "jointimestamp", "member_status_id", "team_id", "user_id" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2024, 9, 28, 10, 0, 0, 0, DateTimeKind.Utc), 1, 1, 1 },
                    { 2, false, new DateTime(2024, 10, 3, 14, 30, 0, 0, DateTimeKind.Utc), 2, 1, 2 },
                    { 3, true, new DateTime(2024, 10, 8, 16, 15, 0, 0, DateTimeKind.Utc), 1, 2, 3 },
                    { 4, false, new DateTime(2024, 10, 13, 11, 45, 0, 0, DateTimeKind.Utc), 3, 2, 4 },
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

            migrationBuilder.CreateIndex(
                name: "IX_chat_team_id",
                table: "chat",
                column: "team_id");

            migrationBuilder.CreateIndex(
                name: "IX_chatparticipant_chat_id",
                table: "chatparticipant",
                column: "chat_id");

            migrationBuilder.CreateIndex(
                name: "IX_chatparticipant_role_id",
                table: "chatparticipant",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_chatparticipant_user_id",
                table: "chatparticipant",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_friendship_status_id",
                table: "friendship",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_friendship_user_2_id",
                table: "friendship",
                column: "user_2_id");

            migrationBuilder.CreateIndex(
                name: "IX_friendship_user_id",
                table: "friendship",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_game_profile_experience_tag_id",
                table: "game_profile",
                column: "experience_tag_id");

            migrationBuilder.CreateIndex(
                name: "IX_game_profile_game_exp_id",
                table: "game_profile",
                column: "game_exp_id");

            migrationBuilder.CreateIndex(
                name: "IX_game_profile_game_id",
                table: "game_profile",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "IX_game_profile_user_id",
                table: "game_profile",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_game_profile_pref_game_profile_id",
                table: "game_profile_pref",
                column: "game_profile_id");

            migrationBuilder.CreateIndex(
                name: "IX_game_profile_pref_gameplay_pref_id",
                table: "game_profile_pref",
                column: "gameplay_pref_id");

            migrationBuilder.CreateIndex(
                name: "IX_gameprofile_preferencetag_relation_game_profile_id",
                table: "gameprofile_preferencetag_relation",
                column: "game_profile_id");

            migrationBuilder.CreateIndex(
                name: "IX_gameprofile_preferencetag_relation_preference_tag_id",
                table: "gameprofile_preferencetag_relation",
                column: "preference_tag_id");

            migrationBuilder.CreateIndex(
                name: "IX_message_chatparticipant_chatparticipant_id",
                table: "message",
                column: "chatparticipant_chatparticipant_id");

            migrationBuilder.CreateIndex(
                name: "IX_team_experience_tag_id",
                table: "team",
                column: "experience_tag_id");

            migrationBuilder.CreateIndex(
                name: "IX_team_game_id",
                table: "team",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "IX_team_member_member_status_id",
                table: "team_member",
                column: "member_status_id");

            migrationBuilder.CreateIndex(
                name: "IX_team_member_team_id",
                table: "team_member",
                column: "team_id");

            migrationBuilder.CreateIndex(
                name: "IX_team_member_user_id",
                table: "team_member",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_team_preferencetag_relation_preference_tag_id",
                table: "team_preferencetag_relation",
                column: "preference_tag_id");

            migrationBuilder.CreateIndex(
                name: "IX_team_preferencetag_relation_team_id",
                table: "team_preferencetag_relation",
                column: "team_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "friendship");

            migrationBuilder.DropTable(
                name: "game_profile_pref");

            migrationBuilder.DropTable(
                name: "gameprofile_preferencetag_relation");

            migrationBuilder.DropTable(
                name: "message");

            migrationBuilder.DropTable(
                name: "team_member");

            migrationBuilder.DropTable(
                name: "team_preferencetag_relation");

            migrationBuilder.DropTable(
                name: "friendship_status");

            migrationBuilder.DropTable(
                name: "gameplay_pref");

            migrationBuilder.DropTable(
                name: "game_profile");

            migrationBuilder.DropTable(
                name: "chatparticipant");

            migrationBuilder.DropTable(
                name: "member_status");

            migrationBuilder.DropTable(
                name: "preference_tag");

            migrationBuilder.DropTable(
                name: "game_exp");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "chat");

            migrationBuilder.DropTable(
                name: "role");

            migrationBuilder.DropTable(
                name: "team");

            migrationBuilder.DropTable(
                name: "experience_tag");

            migrationBuilder.DropTable(
                name: "game");
        }
    }
}
