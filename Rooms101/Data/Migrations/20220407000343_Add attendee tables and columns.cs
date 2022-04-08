using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rooms101.Data.Migrations
{
    public partial class Addattendeetablesandcolumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BackgroundColour",
                table: "Rooms",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateMoment",
                table: "Meetings",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<bool>(
                name: "Cancelled",
                table: "Meetings",
                type: "bit",
                nullable: true,
                defaultValueSql: "(0)",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValueSql: "(0)");

            migrationBuilder.AddColumn<string>(
                name: "Owner",
                table: "Meetings",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MeetingAttendee",
                columns: table => new
                {
                    MeetingAttendeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MeetingId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Accepted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetingAttendee", x => x.MeetingAttendeeId);
                    table.ForeignKey(
                        name: "FK_MeetingAttendee_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MeetingAttendee_Meetings_MeetingId",
                        column: x => x.MeetingId,
                        principalTable: "Meetings",
                        principalColumn: "MeetingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MeetingAttendee_MeetingId",
                table: "MeetingAttendee",
                column: "MeetingId");

            migrationBuilder.CreateIndex(
                name: "IX_MeetingAttendee_UserId",
                table: "MeetingAttendee",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MeetingAttendee");

            migrationBuilder.DropColumn(
                name: "BackgroundColour",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Owner",
                table: "Meetings");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateMoment",
                table: "Meetings",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<bool>(
                name: "Cancelled",
                table: "Meetings",
                type: "bit",
                nullable: false,
                defaultValueSql: "(0)",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValueSql: "(0)");
        }
    }
}
