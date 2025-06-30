using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalManagementSystem.AppApi.Migrations
{
    /// <inheritdoc />
    public partial class appointmentPaymentCreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ConsultantFee",
                table: "Doctor_Tbl",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "AppointmentPayment_Tbl",
                columns: table => new
                {
                    BilingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppointmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentStatus = table.Column<int>(type: "int", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentPayment_Tbl", x => x.BilingId);
                    table.ForeignKey(
                        name: "FK_AppointmentPayment_Tbl_Appointment_Tbl_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointment_Tbl",
                        principalColumn: "AppointmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentPayment_Tbl_AppointmentId",
                table: "AppointmentPayment_Tbl",
                column: "AppointmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppointmentPayment_Tbl");

            migrationBuilder.DropColumn(
                name: "ConsultantFee",
                table: "Doctor_Tbl");
        }
    }
}
