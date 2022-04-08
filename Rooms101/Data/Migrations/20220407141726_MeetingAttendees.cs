using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rooms101.Data.Migrations
{
    public partial class MeetingAttendees : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeetingAttendee_AspNetUsers_UserId",
                table: "MeetingAttendee");

            migrationBuilder.DropForeignKey(
                name: "FK_MeetingAttendee_Meetings_MeetingId",
                table: "MeetingAttendee");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MeetingAttendee",
                table: "MeetingAttendee");

            migrationBuilder.RenameTable(
                name: "MeetingAttendee",
                newName: "MeetingAttendees");

            migrationBuilder.RenameIndex(
                name: "IX_MeetingAttendee_UserId",
                table: "MeetingAttendees",
                newName: "IX_MeetingAttendees_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_MeetingAttendee_MeetingId",
                table: "MeetingAttendees",
                newName: "IX_MeetingAttendees_MeetingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MeetingAttendees",
                table: "MeetingAttendees",
                column: "MeetingAttendeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_MeetingAttendees_AspNetUsers_UserId",
                table: "MeetingAttendees",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MeetingAttendees_Meetings_MeetingId",
                table: "MeetingAttendees",
                column: "MeetingId",
                principalTable: "Meetings",
                principalColumn: "MeetingId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeetingAttendees_AspNetUsers_UserId",
                table: "MeetingAttendees");

            migrationBuilder.DropForeignKey(
                name: "FK_MeetingAttendees_Meetings_MeetingId",
                table: "MeetingAttendees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MeetingAttendees",
                table: "MeetingAttendees");

            migrationBuilder.RenameTable(
                name: "MeetingAttendees",
                newName: "MeetingAttendee");

            migrationBuilder.RenameIndex(
                name: "IX_MeetingAttendees_UserId",
                table: "MeetingAttendee",
                newName: "IX_MeetingAttendee_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_MeetingAttendees_MeetingId",
                table: "MeetingAttendee",
                newName: "IX_MeetingAttendee_MeetingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MeetingAttendee",
                table: "MeetingAttendee",
                column: "MeetingAttendeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_MeetingAttendee_AspNetUsers_UserId",
                table: "MeetingAttendee",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MeetingAttendee_Meetings_MeetingId",
                table: "MeetingAttendee",
                column: "MeetingId",
                principalTable: "Meetings",
                principalColumn: "MeetingId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
