using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetoEscala.Migrations
{
    public partial class teste02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Ano",
                table: "Escala",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ano",
                table: "Escala");
        }
    }
}
