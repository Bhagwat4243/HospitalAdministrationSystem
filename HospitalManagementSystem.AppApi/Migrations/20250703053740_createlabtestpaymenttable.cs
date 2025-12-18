using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalManagementSystem.AppApi.Migrations
{
    /// <inheritdoc />
    public partial class createlabtestpaymenttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LabTestPayment_Tbl",
                columns: table => new
                {
                    PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    PaymentStatus = table.Column<int>(type: "int", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabTestPayment_Tbl", x => x.PaymentId);
                });

            migrationBuilder.CreateTable(
                name: "LabTestPaymentMapping_Tbl",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientLabTestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabTestPaymentMapping_Tbl", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LabTestPaymentMapping_Tbl_LabTestPayment_Tbl_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "LabTestPayment_Tbl",
                        principalColumn: "PaymentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LabTestPaymentMapping_Tbl_PatientLabTest_Tbl_PatientLabTestId",
                        column: x => x.PatientLabTestId,
                        principalTable: "PatientLabTest_Tbl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LabTestPaymentMapping_Tbl_PatientLabTestId",
                table: "LabTestPaymentMapping_Tbl",
                column: "PatientLabTestId");

            migrationBuilder.CreateIndex(
                name: "IX_LabTestPaymentMapping_Tbl_PaymentId",
                table: "LabTestPaymentMapping_Tbl",
                column: "PaymentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LabTestPaymentMapping_Tbl");

            migrationBuilder.DropTable(
                name: "LabTestPayment_Tbl");
        }
    }
}
