using Microsoft.EntityFrameworkCore.Migrations;

namespace my_books.Migrations
{
    public partial class BookStatusEnum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Exels",
                table: "Exels");

            migrationBuilder.RenameTable(
                name: "Exels",
                newName: "BookExel");

            migrationBuilder.AddColumn<int>(
                name: "BookStatus",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookExel",
                table: "BookExel",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BookExel",
                table: "BookExel");

            migrationBuilder.DropColumn(
                name: "BookStatus",
                table: "Books");

            migrationBuilder.RenameTable(
                name: "BookExel",
                newName: "Exels");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Exels",
                table: "Exels",
                column: "Id");
        }
    }
}
