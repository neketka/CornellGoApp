using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace BackendModel.Migrations
{
    public partial class AddMetricsModel0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageJPG",
                schema: "dbo",
                table: "Challenges");

            migrationBuilder.RenameColumn(
                name: "ImageJPG",
                schema: "dbo",
                table: "Suggestions",
                newName: "ImageUrl");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                schema: "dbo",
                table: "Challenges",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "SessonLogEntries",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SessonLogEntries",
                schema: "dbo");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                schema: "dbo",
                table: "Challenges");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                schema: "dbo",
                table: "Suggestions",
                newName: "ImageJPG");

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageJPG",
                schema: "dbo",
                table: "Challenges",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
