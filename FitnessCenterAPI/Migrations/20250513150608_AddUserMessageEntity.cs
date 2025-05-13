using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessCenterApi.Migrations
{
    /// <inheritdoc />
    public partial class AddUserMessageEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_User_IdRecipient",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_IdRecipient",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "IdRecipient",
                table: "Messages");

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "UserConversations",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "groupOwnerId",
                table: "Conversations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UserMessages",
                columns: table => new
                {
                    MessageId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    isRead = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMessages", x => new { x.MessageId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserMessages_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "IdMessage",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserMessages_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "id_user",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserMessages_UserId",
                table: "UserMessages",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserMessages");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "UserConversations");

            migrationBuilder.DropColumn(
                name: "groupOwnerId",
                table: "Conversations");

            migrationBuilder.AddColumn<int>(
                name: "IdRecipient",
                table: "Messages",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_IdRecipient",
                table: "Messages",
                column: "IdRecipient");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_User_IdRecipient",
                table: "Messages",
                column: "IdRecipient",
                principalTable: "User",
                principalColumn: "id_user",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
