using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rooms101.Data.Migrations
{
    public partial class AddMeetingDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MeetingDescription",
                table: "Meetings",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MeetingDescription",
                table: "Meetings");
        }
    }
}
