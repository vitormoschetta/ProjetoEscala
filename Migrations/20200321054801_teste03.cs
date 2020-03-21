using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetoEscala.Migrations
{
    public partial class teste03 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EscalaId",
                table: "Aviso",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Aviso_EscalaId",
                table: "Aviso",
                column: "EscalaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Aviso_Escala_EscalaId",
                table: "Aviso",
                column: "EscalaId",
                principalTable: "Escala",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Aviso_Escala_EscalaId",
                table: "Aviso");

            migrationBuilder.DropIndex(
                name: "IX_Aviso_EscalaId",
                table: "Aviso");

            migrationBuilder.DropColumn(
                name: "EscalaId",
                table: "Aviso");
        }
    }
}
