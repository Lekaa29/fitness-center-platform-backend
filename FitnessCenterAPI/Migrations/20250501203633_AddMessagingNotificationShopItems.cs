using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessCenterApi.Migrations
{
    /// <inheritdoc />
    public partial class AddMessagingNotificationShopItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    IdMessage = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdSender = table.Column<int>(type: "INTEGER", nullable: false),
                    IdRecipient = table.Column<int>(type: "INTEGER", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.IdMessage);
                    table.ForeignKey(
                        name: "FK_Messages_User_IdRecipient",
                        column: x => x.IdRecipient,
                        principalTable: "User",
                        principalColumn: "id_user",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_User_IdSender",
                        column: x => x.IdSender,
                        principalTable: "User",
                        principalColumn: "id_user",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    IdNotification = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdUser = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    IsRead = table.Column<bool>(type: "INTEGER", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.IdNotification);
                    table.ForeignKey(
                        name: "FK_Notifications_User_IdUser",
                        column: x => x.IdUser,
                        principalTable: "User",
                        principalColumn: "id_user",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShopItems",
                columns: table => new
                {
                    IdShopItem = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdFitnessCentar = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    PictureUrl = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    LoyaltyPrice = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopItems", x => x.IdShopItem);
                    table.ForeignKey(
                        name: "FK_ShopItems_Fitness_Centar_IdFitnessCentar",
                        column: x => x.IdFitnessCentar,
                        principalTable: "Fitness_Centar",
                        principalColumn: "id_fitness_centar",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserItems",
                columns: table => new
                {
                    IdUser = table.Column<int>(type: "INTEGER", nullable: false),
                    IdShopItem = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserItems", x => new { x.IdUser, x.IdShopItem });
                    table.ForeignKey(
                        name: "FK_UserItems_ShopItems_IdShopItem",
                        column: x => x.IdShopItem,
                        principalTable: "ShopItems",
                        principalColumn: "IdShopItem",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserItems_User_IdUser",
                        column: x => x.IdUser,
                        principalTable: "User",
                        principalColumn: "id_user",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_IdRecipient",
                table: "Messages",
                column: "IdRecipient");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_IdSender",
                table: "Messages",
                column: "IdSender");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_IdUser",
                table: "Notifications",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_ShopItems_IdFitnessCentar",
                table: "ShopItems",
                column: "IdFitnessCentar");

            migrationBuilder.CreateIndex(
                name: "IX_UserItems_IdShopItem",
                table: "UserItems",
                column: "IdShopItem");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "UserItems");

            migrationBuilder.DropTable(
                name: "ShopItems");
        }
    }
}
