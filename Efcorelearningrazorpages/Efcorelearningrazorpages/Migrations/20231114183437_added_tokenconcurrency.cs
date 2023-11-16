using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Efcorelearningrazorpages.Migrations
{
    /// <inheritdoc />
    public partial class added_tokenconcurrency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "token",
                table: "Departments",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "token",
                table: "Departments");
        }
    }
}
