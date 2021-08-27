using Microsoft.EntityFrameworkCore.Migrations;

namespace SimpleStore.Core.Migrations
{
    public partial class shippingaddShippingMethodName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShippingMethod",
                table: "Shipments",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShippingMethod",
                table: "Shipments");
        }
    }
}
