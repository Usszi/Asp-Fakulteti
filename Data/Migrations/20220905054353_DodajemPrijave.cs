using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FaksPrijave.Data.Migrations
{
    public partial class DodajemPrijave : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Prijava",
                columns: table => new
                {
                    PrijavaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FakultetId = table.Column<int>(type: "int", nullable: false),
                    BodoviPrijemni = table.Column<int>(type: "int", nullable: true),
                    BodoviSrednja = table.Column<int>(type: "int", nullable: true),
                    BodoviNatjecanja = table.Column<int>(type: "int", nullable: true),
                    UkupnoBodova = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prijava", x => x.PrijavaId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prijava");
        }
    }
}
