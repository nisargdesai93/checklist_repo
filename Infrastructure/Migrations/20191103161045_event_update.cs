using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class event_update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<short>(
                name: "AllDayEvent",
                table: "Event",
                nullable: false,
                oldClrType: typeof(short));

            migrationBuilder.AddColumn<long>(
                name: "PersonId",
                table: "Event",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "Event");

            migrationBuilder.AlterColumn<short>(
                name: "AllDayEvent",
                table: "Event",
                nullable: false,
                oldClrType: typeof(short));
        }
    }
}
