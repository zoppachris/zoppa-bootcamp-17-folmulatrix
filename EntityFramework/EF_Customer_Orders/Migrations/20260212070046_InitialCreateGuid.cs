using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EF_Customer_Orders.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AGENTS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AGENT_CODE = table.Column<string>(type: "TEXT", maxLength: 6, nullable: false),
                    AGENT_NAME = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    WORKING_AREA = table.Column<string>(type: "TEXT", maxLength: 35, nullable: false),
                    COMMISSION = table.Column<decimal>(type: "TEXT", nullable: false),
                    PHONE_NO = table.Column<string>(type: "TEXT", maxLength: 15, nullable: true),
                    COUNTRY = table.Column<string>(type: "TEXT", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AGENTS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CUSTOMER",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CUST_CODE = table.Column<string>(type: "TEXT", maxLength: 6, nullable: false),
                    CUST_NAME = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    CUST_CITY = table.Column<string>(type: "TEXT", maxLength: 35, nullable: false),
                    WORKING_AREA = table.Column<string>(type: "TEXT", maxLength: 35, nullable: false),
                    CUST_COUNTRY = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    GRADE = table.Column<int>(type: "INTEGER", nullable: true),
                    OPENING_AMT = table.Column<decimal>(type: "TEXT", nullable: true),
                    RECEIVE_AMT = table.Column<decimal>(type: "TEXT", nullable: true),
                    PAYMENT_AMT = table.Column<decimal>(type: "TEXT", nullable: true),
                    OUTSTANDING_AMT = table.Column<decimal>(type: "TEXT", nullable: true),
                    PHONE_NO = table.Column<string>(type: "TEXT", maxLength: 17, nullable: true),
                    AgentId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CUSTOMER", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CUSTOMER_AGENTS_AgentId",
                        column: x => x.AgentId,
                        principalTable: "AGENTS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ORDERS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ORD_NUM = table.Column<int>(type: "INTEGER", nullable: false),
                    ORD_AMOUNT = table.Column<decimal>(type: "TEXT", nullable: true),
                    ADVANCE_AMOUNT = table.Column<decimal>(type: "TEXT", nullable: true),
                    ORD_DATE = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CustomerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AgentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ORD_DESCRIPTION = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORDERS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ORDERS_AGENTS_AgentId",
                        column: x => x.AgentId,
                        principalTable: "AGENTS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ORDERS_CUSTOMER_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "CUSTOMER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AGENTS_AGENT_CODE",
                table: "AGENTS",
                column: "AGENT_CODE",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CUSTOMER_AgentId",
                table: "CUSTOMER",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_CUSTOMER_CUST_CODE",
                table: "CUSTOMER",
                column: "CUST_CODE",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ORDERS_AgentId",
                table: "ORDERS",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_ORDERS_CustomerId",
                table: "ORDERS",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ORDERS_ORD_NUM",
                table: "ORDERS",
                column: "ORD_NUM",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ORDERS");

            migrationBuilder.DropTable(
                name: "CUSTOMER");

            migrationBuilder.DropTable(
                name: "AGENTS");
        }
    }
}
