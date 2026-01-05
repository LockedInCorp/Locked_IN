using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Locked_IN_Backend.Migrations
{
    /// <inheritdoc />
    public partial class UserAsIdentityChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nickname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    avatar_url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    availability = table.Column<string>(type: "json", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_pk", x => x.Id);
                });

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
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<int>(type: "integer", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "friendship_user_2",
                        column: x => x.user_2_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
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
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "chat",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    last_message_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
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
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
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
                    last_read_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    unread_count = table.Column<int>(type: "integer", nullable: false),
                    role_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("chatparticipant_pk", x => x.chatparticipant_id);
                    table.ForeignKey(
                        name: "FK_chatparticipant_AspNetUsers_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
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
                    content = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    sent_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    edited_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    attachment_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
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
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "availability", "avatar_url", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "nickname", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { 1, 0, "{\"monday\": [\"18:00\", \"22:00\"], \"friday\": [\"19:00\", \"23:00\"]}", "https://example.com/avatars/john.png", "6f9098be-b650-4411-a000-5eb43a9552bb", "john.doe@example.com", false, false, null, "JohnDoe", null, null, "hashed_password_1", null, false, null, false, null },
                    { 2, 0, "{\"tuesday\": [\"17:00\", \"21:00\"], \"saturday\": [\"14:00\", \"18:00\"]}", null, "cdf5a155-7689-41bb-899d-082da450358d", "jane.smith@example.com", false, false, null, "JaneSmith", null, null, "hashed_password_2", null, false, null, false, null },
                    { 3, 0, "{\"wednesday\": [\"20:00\", \"24:00\"], \"sunday\": [\"16:00\", \"20:00\"]}", null, "7215511a-a6f6-4c13-9150-4adcd0386aa2", "mike.wilson@example.com", false, false, null, "MikeWilson", null, null, "hashed_password_3", null, false, null, false, null },
                    { 4, 0, "{\"thursday\": [\"19:00\", \"23:00\"], \"saturday\": [\"15:00\", \"19:00\"]}", null, "d88cb586-3c32-4568-8926-bc13615faee5", "sarah.johnson@example.com", false, false, null, "SarahJ", null, null, "hashed_password_4", null, false, null, false, null },
                    { 5, 0, "{}", null, "72731087-823f-47c7-914a-c94b877b812b", "test.user5@example.com", false, false, null, "TestUser5", null, null, "hashed_password_5", null, false, null, false, null },
                    { 6, 0, "{}", null, "cfdd42d5-0600-4945-a36f-17b0f22459f4", "test.user6@example.com", false, false, null, "TestUser6", null, null, "hashed_password_6", null, false, null, false, null }
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
                table: "role",
                columns: new[] { "id", "rolename" },
                values: new object[,]
                {
                    { 1, "Member" },
                    { 2, "Admin" },
                    { 3, "Moderator" }
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
                    { 4, new DateTime(2024, 10, 10, 18, 0, 0, 0, DateTimeKind.Utc), "TestDescription4", 2, 2, "https://pl.wikipedia.org/wiki/World_of_Warcraft#/media/Plik:WoW_icon.svg", true, false, 6, null, "Casual Gamers" },
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
                    { 1, true, new DateTime(2024, 9, 28, 10, 0, 0, 0, DateTimeKind.Utc), 1, 1, 1 },
                    { 2, false, new DateTime(2024, 10, 3, 14, 30, 0, 0, DateTimeKind.Utc), 2, 1, 2 },
                    { 3, true, new DateTime(2024, 10, 8, 16, 15, 0, 0, DateTimeKind.Utc), 1, 2, 3 },
                    { 4, false, new DateTime(2024, 10, 13, 11, 45, 0, 0, DateTimeKind.Utc), 3, 2, 4 },
                    { 5, true, new DateTime(2024, 10, 18, 9, 20, 0, 0, DateTimeKind.Utc), 1, 3, 1 },
                    { 6, true, new DateTime(2024, 10, 23, 18, 0, 0, 0, DateTimeKind.Utc), 1, 4, 2 },
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
                    { 1, 1, 1 },
                    { 2, 3, 1 },
                    { 3, 2, 2 },
                    { 4, 5, 2 },
                    { 5, 1, 3 },
                    { 6, 4, 3 },
                    { 7, 2, 4 },
                    { 8, 6, 4 },
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

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

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
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

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
                name: "AspNetRoles");

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
                name: "AspNetUsers");

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
