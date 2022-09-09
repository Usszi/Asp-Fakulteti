using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FaksPrijave.Data.Migrations
{
    public partial class DodaniFakulteti : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fakultet",
                columns: table => new
                {
                    FakultetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FakultetIme = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SlobodnoMjesta = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fakultet", x => x.FakultetId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fakultet");
        }
    }
}
