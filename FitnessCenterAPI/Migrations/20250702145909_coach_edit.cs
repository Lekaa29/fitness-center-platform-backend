using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessCenterApi.Migrations
{
    /// <inheritdoc />
    public partial class coach_edit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coaches_Fitness_Centar_FitnessCentarIdFitnessCentar",
                table: "Coaches");

            migrationBuilder.DropIndex(
                name: "IX_Coaches_FitnessCentarIdFitnessCentar",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "FitnessCentarIdFitnessCentar",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "IdFitnessCentar",
                table: "Coaches");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FitnessCentarIdFitnessCentar",
                table: "Coaches",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdFitnessCentar",
                table: "Coaches",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Coaches_FitnessCentarIdFitnessCentar",
                table: "Coaches",
                column: "FitnessCentarIdFitnessCentar");

            migrationBuilder.AddForeignKey(
                name: "FK_Coaches_Fitness_Centar_FitnessCentarIdFitnessCentar",
                table: "Coaches",
                column: "FitnessCentarIdFitnessCentar",
                principalTable: "Fitness_Centar",
                principalColumn: "id_fitness_centar",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
