using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketGo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Discount",
                columns: table => new
                {
                    ID_Discount = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name_Discount = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Information = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Percent_Discount = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discount", x => x.ID_Discount);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    ID_Role = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.ID_Role);
                });

            migrationBuilder.CreateTable(
                name: "TrainRoute",
                columns: table => new
                {
                    ID_TrainRoute = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Point_Start = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Point_End = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainRoute", x => x.ID_TrainRoute);
                });

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    ID_Account = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Sex = table.Column<bool>(type: "bit", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "date", nullable: true),
                    ID_Role = table.Column<int>(type: "int", nullable: false),
                    IsEmailConfirmed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.ID_Account);
                    table.ForeignKey(
                        name: "FK_Account_Role",
                        column: x => x.ID_Role,
                        principalTable: "Role",
                        principalColumn: "ID_Role");
                });

            migrationBuilder.CreateTable(
                name: "Train",
                columns: table => new
                {
                    ID_Train = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name_Train = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Date_Start = table.Column<DateTime>(type: "datetime", nullable: true),
                    ID_TrainRoute = table.Column<int>(type: "int", nullable: false),
                    CoefficientTrain = table.Column<decimal>(type: "decimal(14,4)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Train", x => x.ID_Train);
                    table.ForeignKey(
                        name: "FK_Train_TrainRoute",
                        column: x => x.ID_TrainRoute,
                        principalTable: "TrainRoute",
                        principalColumn: "ID_TrainRoute");
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    ID_Order = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Unit_Price = table.Column<double>(type: "float", nullable: true),
                    Date_Order = table.Column<DateTime>(type: "datetime", nullable: true),
                    ID_Ticket = table.Column<int>(type: "int", nullable: false),
                    ID_Discount = table.Column<int>(type: "int", nullable: true),
                    NameCus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ID_Cus = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.ID_Order);
                    table.ForeignKey(
                        name: "FK_Order_Account",
                        column: x => x.ID_Cus,
                        principalTable: "Account",
                        principalColumn: "ID_Account");
                    table.ForeignKey(
                        name: "FK_Order_Discount",
                        column: x => x.ID_Discount,
                        principalTable: "Discount",
                        principalColumn: "ID_Discount");
                });

            migrationBuilder.CreateTable(
                name: "Tokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tokens_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "ID_Account",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Coach",
                columns: table => new
                {
                    ID_Coach = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name_Coach = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Seats_Quantity = table.Column<int>(type: "int", nullable: true),
                    BasicPrice = table.Column<double>(type: "float", nullable: true),
                    ID_Train = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coach", x => x.ID_Coach);
                    table.ForeignKey(
                        name: "FK_Coach_Train",
                        column: x => x.ID_Train,
                        principalTable: "Train",
                        principalColumn: "ID_Train");
                });

            migrationBuilder.CreateTable(
                name: "Seat",
                columns: table => new
                {
                    ID_Seat = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name_Seat = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    State = table.Column<bool>(type: "bit", nullable: false),
                    ID_Coach = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seat", x => x.ID_Seat);
                    table.ForeignKey(
                        name: "FK_Seat_Coach",
                        column: x => x.ID_Coach,
                        principalTable: "Coach",
                        principalColumn: "ID_Coach",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ticket",
                columns: table => new
                {
                    ID_Ticket = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true),
                    ID_Seat = table.Column<int>(type: "int", nullable: false),
                    ID_Train = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ticket", x => x.ID_Ticket);
                    table.ForeignKey(
                        name: "FK_Ticket_Seat",
                        column: x => x.ID_Seat,
                        principalTable: "Seat",
                        principalColumn: "ID_Seat");
                    table.ForeignKey(
                        name: "FK_Ticket_Train",
                        column: x => x.ID_Train,
                        principalTable: "Train",
                        principalColumn: "ID_Train");
                });

            migrationBuilder.CreateTable(
                name: "Order_Ticket",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Order = table.Column<int>(type: "int", nullable: false),
                    ID_Ticket = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order_Ticket", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Order_Ticket_Order",
                        column: x => x.ID_Order,
                        principalTable: "Order",
                        principalColumn: "ID_Order");
                    table.ForeignKey(
                        name: "FK_Order_Ticket_Ticket",
                        column: x => x.ID_Ticket,
                        principalTable: "Ticket",
                        principalColumn: "ID_Ticket");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_ID_Role",
                table: "Account",
                column: "ID_Role");

            migrationBuilder.CreateIndex(
                name: "IX_Coach_ID_Train",
                table: "Coach",
                column: "ID_Train");

            migrationBuilder.CreateIndex(
                name: "IX_Order_ID_Cus",
                table: "Order",
                column: "ID_Cus");

            migrationBuilder.CreateIndex(
                name: "IX_Order_ID_Discount",
                table: "Order",
                column: "ID_Discount");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Ticket_ID_Order",
                table: "Order_Ticket",
                column: "ID_Order");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Ticket_ID_Ticket",
                table: "Order_Ticket",
                column: "ID_Ticket");

            migrationBuilder.CreateIndex(
                name: "IX_Seat_ID_Coach",
                table: "Seat",
                column: "ID_Coach");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_ID_Seat",
                table: "Ticket",
                column: "ID_Seat");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_ID_Train",
                table: "Ticket",
                column: "ID_Train");

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_AccountId",
                table: "Tokens",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Train_ID_TrainRoute",
                table: "Train",
                column: "ID_TrainRoute");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Order_Ticket");

            migrationBuilder.DropTable(
                name: "Tokens");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Ticket");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Discount");

            migrationBuilder.DropTable(
                name: "Seat");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Coach");

            migrationBuilder.DropTable(
                name: "Train");

            migrationBuilder.DropTable(
                name: "TrainRoute");
        }
    }
}
