using Microsoft.EntityFrameworkCore.Migrations;

namespace SimpleStore.Core.Migrations
{
    public partial class customerattributes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SelectedPaymentMethod",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsShippingAddress",
                table: "CustomerAddresses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SelectedPaymentMethod",
                table: "Carts",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SelectedPaymentMethod",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "IsShippingAddress",
                table: "CustomerAddresses");

            migrationBuilder.DropColumn(
                name: "SelectedPaymentMethod",
                table: "Carts");
        }
    }
}
