using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketGo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveOrderIdTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ID_Ticket",
                table: "Order");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ID_Ticket",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
