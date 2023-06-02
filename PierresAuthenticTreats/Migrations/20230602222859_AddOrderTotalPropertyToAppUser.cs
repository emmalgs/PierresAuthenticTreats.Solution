using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PierresAuthenticTreats.Migrations
{
    public partial class AddOrderTotalPropertyToAppUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "OrderTotal",
                table: "AspNetUsers",
                type: "float",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderTotal",
                table: "AspNetUsers");
        }
    }
}
