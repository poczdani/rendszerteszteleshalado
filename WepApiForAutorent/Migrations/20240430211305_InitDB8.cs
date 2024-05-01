using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoRent.Migrations
{
    /// <inheritdoc />
    public partial class InitDB8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Passwor",
                table: "Users",
                newName: "Password");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Users",
                newName: "Passwor");
        }
    }
}
