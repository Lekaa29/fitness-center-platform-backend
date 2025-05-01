using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessCenterApi.Migrations
{
    /// <inheritdoc />
    public partial class AddArticlesEventsCoaches : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    IdArticle = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdFitnessCentar = table.Column<int>(type: "INTEGER", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: false),
                    PictureLink = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.IdArticle);
                    table.ForeignKey(
                        name: "FK_Articles_Fitness_Centar_IdFitnessCentar",
                        column: x => x.IdFitnessCentar,
                        principalTable: "Fitness_Centar",
                        principalColumn: "id_fitness_centar",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Coaches",
                columns: table => new
                {
                    IdCoach = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdUser = table.Column<int>(type: "INTEGER", nullable: false),
                    Experience = table.Column<string>(type: "TEXT", nullable: false),
                    PictureLink = table.Column<string>(type: "TEXT", nullable: false),
                    VideoLink = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coaches", x => x.IdCoach);
                    table.ForeignKey(
                        name: "FK_Coaches_User_IdUser",
                        column: x => x.IdUser,
                        principalTable: "User",
                        principalColumn: "id_user",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    IdEvent = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdFitnessCentar = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PictureUrl = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    MaxParticipants = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.IdEvent);
                    table.ForeignKey(
                        name: "FK_Events_Fitness_Centar_IdFitnessCentar",
                        column: x => x.IdFitnessCentar,
                        principalTable: "Fitness_Centar",
                        principalColumn: "id_fitness_centar",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FitnessCenterUsers",
                columns: table => new
                {
                    IdUser = table.Column<int>(type: "INTEGER", nullable: false),
                    IdFitnessCentar = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FitnessCenterUsers", x => new { x.IdUser, x.IdFitnessCentar });
                    table.ForeignKey(
                        name: "FK_FitnessCenterUsers_Fitness_Centar_IdFitnessCentar",
                        column: x => x.IdFitnessCentar,
                        principalTable: "Fitness_Centar",
                        principalColumn: "id_fitness_centar",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FitnessCenterUsers_User_IdUser",
                        column: x => x.IdUser,
                        principalTable: "User",
                        principalColumn: "id_user",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CoachPrograms",
                columns: table => new
                {
                    IdCoachProgram = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdCoach = table.Column<int>(type: "INTEGER", nullable: false),
                    IdFitnessCentar = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoachPrograms", x => x.IdCoachProgram);
                    table.ForeignKey(
                        name: "FK_CoachPrograms_Coaches_IdCoach",
                        column: x => x.IdCoach,
                        principalTable: "Coaches",
                        principalColumn: "IdCoach",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoachPrograms_Fitness_Centar_IdFitnessCentar",
                        column: x => x.IdFitnessCentar,
                        principalTable: "Fitness_Centar",
                        principalColumn: "id_fitness_centar",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventParticipants",
                columns: table => new
                {
                    IdEvent = table.Column<int>(type: "INTEGER", nullable: false),
                    IdUser = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventParticipants", x => new { x.IdEvent, x.IdUser });
                    table.ForeignKey(
                        name: "FK_EventParticipants_Events_IdEvent",
                        column: x => x.IdEvent,
                        principalTable: "Events",
                        principalColumn: "IdEvent",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventParticipants_User_IdUser",
                        column: x => x.IdUser,
                        principalTable: "User",
                        principalColumn: "id_user",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Articles_IdFitnessCentar",
                table: "Articles",
                column: "IdFitnessCentar");

            migrationBuilder.CreateIndex(
                name: "IX_Coaches_IdUser",
                table: "Coaches",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_CoachPrograms_IdCoach",
                table: "CoachPrograms",
                column: "IdCoach");

            migrationBuilder.CreateIndex(
                name: "IX_CoachPrograms_IdFitnessCentar",
                table: "CoachPrograms",
                column: "IdFitnessCentar");

            migrationBuilder.CreateIndex(
                name: "IX_EventParticipants_IdUser",
                table: "EventParticipants",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_Events_IdFitnessCentar",
                table: "Events",
                column: "IdFitnessCentar");

            migrationBuilder.CreateIndex(
                name: "IX_FitnessCenterUsers_IdFitnessCentar",
                table: "FitnessCenterUsers",
                column: "IdFitnessCentar");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "CoachPrograms");

            migrationBuilder.DropTable(
                name: "EventParticipants");

            migrationBuilder.DropTable(
                name: "FitnessCenterUsers");

            migrationBuilder.DropTable(
                name: "Coaches");

            migrationBuilder.DropTable(
                name: "Events");
        }
    }
}
