using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BillingInvoicingPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_PaymentEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentDate",
                table: "Payments",
                column: "PaymentDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Payments_PaymentDate",
                table: "Payments");
        }
    }
}
