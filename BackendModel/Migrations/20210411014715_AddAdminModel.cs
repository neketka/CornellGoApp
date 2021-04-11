using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace BackendModel.Migrations
{
    public partial class AddAdminModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SessonLogEntries",
                schema: "dbo");

            migrationBuilder.AddColumn<string>(
                name: "CitationUrl",
                schema: "dbo",
                table: "Challenges",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LinkUrl",
                schema: "dbo",
                table: "Challenges",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LongDescription",
                schema: "dbo",
                table: "Challenges",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Admins",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    SignalRId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SessionLogEntries",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EntryType = table.Column<int>(type: "integer", nullable: false),
                    EntryData = table.Column<string>(type: "text", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UserSessonLogEntriesId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionLogEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SessionLogEntries_Users_UserSessonLogEntriesId",
                        column: x => x.UserSessonLogEntriesId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SessionLogEntries_UserSessonLogEntriesId",
                schema: "dbo",
                table: "SessionLogEntries",
                column: "UserSessonLogEntriesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SessionLogEntries",
                schema: "dbo");

            migrationBuilder.DropColumn(
                name: "CitationUrl",
                schema: "dbo",
                table: "Challenges");

            migrationBuilder.DropColumn(
                name: "LinkUrl",
                schema: "dbo",
                table: "Challenges");

            migrationBuilder.DropColumn(
                name: "LongDescription",
                schema: "dbo",
                table: "Challenges");

            migrationBuilder.CreateTable(
                name: "SessonLogEntries",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EntryData = table.Column<string>(type: "text", nullable: false),
                    EntryType = table.Column<int>(type: "integer", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UserSessonLogEntriesId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessonLogEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SessonLogEntries_Users_UserSessonLogEntriesId",
                        column: x => x.UserSessonLogEntriesId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SessonLogEntries_UserSessonLogEntriesId",
                schema: "dbo",
                table: "SessonLogEntries",
                column: "UserSessonLogEntriesId");
        }
    }
}
