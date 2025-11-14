using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INSTITUTO_C.Migrations
{
    /// <inheritdoc />
    public partial class materiacursadanombre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "MateriasCursadas",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MateriasCursadas_Nombre",
                table: "MateriasCursadas",
                column: "Nombre",
                unique: true,
                filter: "[Nombre] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MateriasCursadas_Nombre",
                table: "MateriasCursadas");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "MateriasCursadas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
