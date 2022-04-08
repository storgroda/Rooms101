using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rooms101.Data.Migrations
{
    public partial class SetDefaultValuesforMeetingCancelledandCreateMomentTryAgainAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateMoment",
                table: "Meetings",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Cancelled",
                table: "Meetings",
                type: "bit",
                nullable: false,
                defaultValueSql: "(0)",
                oldClrType: typeof(bool),
                oldType: "bit");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateMoment",
                table: "Meetings",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<bool>(
                name: "Cancelled",
                table: "Meetings",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValueSql: "(0)");
        }
    }
}
