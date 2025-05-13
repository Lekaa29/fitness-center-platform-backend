using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessCenterApi.Migrations
{
    /// <inheritdoc />
    public partial class AddMessageBooleanTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "Messages",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isPinned",
                table: "Messages",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "Conversations",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "isPinned",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "Conversations");
        }
    }
}
