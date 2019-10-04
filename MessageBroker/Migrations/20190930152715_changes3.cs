using Microsoft.EntityFrameworkCore.Migrations;

namespace MessageBroker.Migrations
{
    public partial class changes3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SocketId",
                table: "ClientRooms",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SocketId",
                table: "ClientRooms");
        }
    }
}
