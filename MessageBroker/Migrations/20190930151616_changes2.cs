using Microsoft.EntityFrameworkCore.Migrations;

namespace MessageBroker.Migrations
{
    public partial class changes2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SocketId",
                table: "ClientRooms",
                newName: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ClientRooms",
                newName: "SocketId");
        }
    }
}
