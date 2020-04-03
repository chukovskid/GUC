using Microsoft.EntityFrameworkCore.Migrations;

namespace DatingApp.API.Migrations
{
    public partial class InitalCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Values", // sam si go davam i zima vrednosti od Models.Value
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false) // Core avtomatski znae deka e Id 
                        .Annotation("Sqlite:Autoincrement", true), // ova znaci deka ke dade nov broj
                    Name = table.Column<string>(nullable: true) // i za ova deka e Name
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Values", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder) // ova samo ke ja vrati tamelata so vrednost Values
        {
            migrationBuilder.DropTable(
                name: "Values"); 
        }
    }
}
