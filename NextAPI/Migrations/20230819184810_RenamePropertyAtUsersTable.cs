using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NextAPI.Migrations
{
    public partial class RenamePropertyAtUsersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Userame",
                table: "Users",
                newName: "Username");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Users",
                newName: "Userame");
        }
    }
}
