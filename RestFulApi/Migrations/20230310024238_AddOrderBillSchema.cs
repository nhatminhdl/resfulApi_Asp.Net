using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RestFulApi.Migrations
{
    public partial class AddOrderBillSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderBill",
                columns: table => new
                {
                    OrderCode = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateOrder = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderStatus = table.Column<int>(type: "int", nullable: false),
                    Receiver = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderBill", x => x.OrderCode);
                });

            migrationBuilder.CreateTable(
                name: "DetailOrderBill",
                columns: table => new
                {
                    ProductCode = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderCode = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Discount = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailOrderBill", x => new { x.OrderCode, x.ProductCode });
                    table.ForeignKey(
                        name: "FK_DetailOrderBill_Order",
                        column: x => x.OrderCode,
                        principalTable: "OrderBill",
                        principalColumn: "OrderCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetailOrderBill_Product",
                        column: x => x.ProductCode,
                        principalTable: "Products",
                        principalColumn: "ProductCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetailOrderBill_ProductCode",
                table: "DetailOrderBill",
                column: "ProductCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetailOrderBill");

            migrationBuilder.DropTable(
                name: "OrderBill");
        }
    }
}
