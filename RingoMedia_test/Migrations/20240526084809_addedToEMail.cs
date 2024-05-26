using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingoMedia_test.Migrations
{
    /// <inheritdoc />
    public partial class addedToEMail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ToEmail",
                table: "Reminders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ToEmail",
                table: "Reminders");
        }
    }
}
