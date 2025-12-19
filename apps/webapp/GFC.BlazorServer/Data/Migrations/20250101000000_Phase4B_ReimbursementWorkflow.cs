using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GFC.BlazorServer.Data.Migrations;

public partial class Phase4B_ReimbursementWorkflow : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Create ReimbursementCategories table
        migrationBuilder.CreateTable(
            name: "ReimbursementCategories",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                IsActive = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ReimbursementCategories", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_ReimbursementCategories_IsActive",
            table: "ReimbursementCategories",
            column: "IsActive");

        // Seed categories
        migrationBuilder.InsertData(
            table: "ReimbursementCategories",
            columns: new[] { "Id", "Name", "IsActive" },
            values: new object[,]
            {
                { 1, "Costco", true },
                { 2, "BJ's", true },
                { 3, "Supplies", true },
                { 4, "Misc", true }
            });

        // Create ReimbursementRequests table
        migrationBuilder.CreateTable(
            name: "ReimbursementRequests",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                RequestorMemberId = table.Column<int>(type: "int", nullable: false),
                RequestDate = table.Column<DateTime>(type: "date", nullable: false),
                Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                TotalAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                EditedFlag = table.Column<bool>(type: "bit", nullable: false),
                ApprovedByMemberId = table.Column<int>(type: "int", nullable: true),
                ApprovedDateUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                RejectedByMemberId = table.Column<int>(type: "int", nullable: true),
                RejectedDateUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                RejectReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                PaidByMemberId = table.Column<int>(type: "int", nullable: true),
                PaidDateUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ReimbursementRequests", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_ReimbursementRequests_RequestorMemberId",
            table: "ReimbursementRequests",
            column: "RequestorMemberId");

        migrationBuilder.CreateIndex(
            name: "IX_ReimbursementRequests_RequestDate",
            table: "ReimbursementRequests",
            column: "RequestDate");

        migrationBuilder.CreateIndex(
            name: "IX_ReimbursementRequests_Status",
            table: "ReimbursementRequests",
            column: "Status");

        // Create ReimbursementItems table
        migrationBuilder.CreateTable(
            name: "ReimbursementItems",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                RequestId = table.Column<int>(type: "int", nullable: false),
                ExpenseDate = table.Column<DateTime>(type: "date", nullable: false),
                Amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                CategoryId = table.Column<int>(type: "int", nullable: false),
                Vendor = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ReimbursementItems", x => x.Id);
                table.ForeignKey(
                    name: "FK_ReimbursementItems_ReimbursementRequests_RequestId",
                    column: x => x.RequestId,
                    principalTable: "ReimbursementRequests",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_ReimbursementItems_ReimbursementCategories_CategoryId",
                    column: x => x.CategoryId,
                    principalTable: "ReimbursementCategories",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_ReimbursementItems_RequestId",
            table: "ReimbursementItems",
            column: "RequestId");

        migrationBuilder.CreateIndex(
            name: "IX_ReimbursementItems_CategoryId",
            table: "ReimbursementItems",
            column: "CategoryId");

        // Create ReceiptFiles table
        migrationBuilder.CreateTable(
            name: "ReceiptFiles",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                RequestItemId = table.Column<int>(type: "int", nullable: false),
                FileName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                RelativePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                UploadedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                UploadedByMemberId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ReceiptFiles", x => x.Id);
                table.ForeignKey(
                    name: "FK_ReceiptFiles_ReimbursementItems_RequestItemId",
                    column: x => x.RequestItemId,
                    principalTable: "ReimbursementItems",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_ReceiptFiles_RequestItemId",
            table: "ReceiptFiles",
            column: "RequestItemId");

        migrationBuilder.CreateIndex(
            name: "IX_ReceiptFiles_UploadedByMemberId",
            table: "ReceiptFiles",
            column: "UploadedByMemberId");

        // Create ReimbursementChangeLogs table
        migrationBuilder.CreateTable(
            name: "ReimbursementChangeLogs",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                RequestId = table.Column<int>(type: "int", nullable: false),
                ChangedByMemberId = table.Column<int>(type: "int", nullable: false),
                ChangeUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                FieldName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                OldValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                NewValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ReimbursementChangeLogs", x => x.Id);
                table.ForeignKey(
                    name: "FK_ReimbursementChangeLogs_ReimbursementRequests_RequestId",
                    column: x => x.RequestId,
                    principalTable: "ReimbursementRequests",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_ReimbursementChangeLogs_RequestId",
            table: "ReimbursementChangeLogs",
            column: "RequestId");

        migrationBuilder.CreateIndex(
            name: "IX_ReimbursementChangeLogs_ChangedByMemberId",
            table: "ReimbursementChangeLogs",
            column: "ChangedByMemberId");

        migrationBuilder.CreateIndex(
            name: "IX_ReimbursementChangeLogs_ChangeUtc",
            table: "ReimbursementChangeLogs",
            column: "ChangeUtc");

        // Create ReimbursementSettings table
        migrationBuilder.CreateTable(
            name: "ReimbursementSettings",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ReceiptRequired = table.Column<bool>(type: "bit", nullable: false),
                NotificationRecipients = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ReimbursementSettings", x => x.Id);
            });

        // Seed default settings
        migrationBuilder.InsertData(
            table: "ReimbursementSettings",
            columns: new[] { "Id", "ReceiptRequired", "NotificationRecipients" },
            values: new object[] { 1, false, null });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "ReimbursementChangeLogs");

        migrationBuilder.DropTable(
            name: "ReimbursementSettings");

        migrationBuilder.DropTable(
            name: "ReceiptFiles");

        migrationBuilder.DropTable(
            name: "ReimbursementItems");

        migrationBuilder.DropTable(
            name: "ReimbursementRequests");

        migrationBuilder.DropTable(
            name: "ReimbursementCategories");
    }
}
