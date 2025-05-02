using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessCenterApi.Migrations
{
    /// <inheritdoc />
    public partial class AddAttendance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    id_attendance = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    id_user = table.Column<int>(type: "INTEGER", nullable: false),
                    id_fitness_centar = table.Column<int>(type: "INTEGER", nullable: false),
                    timestamp = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.id_attendance);
                    table.ForeignKey(
                        name: "FK_Attendances_Fitness_Centar_id_fitness_centar",
                        column: x => x.id_fitness_centar,
                        principalTable: "Fitness_Centar",
                        principalColumn: "id_fitness_centar",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Attendances_User_id_user",
                        column: x => x.id_user,
                        principalTable: "User",
                        principalColumn: "id_user",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_id_fitness_centar",
                table: "Attendances",
                column: "id_fitness_centar");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_id_user",
                table: "Attendances",
                column: "id_user");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendances");
        }
    }
}
