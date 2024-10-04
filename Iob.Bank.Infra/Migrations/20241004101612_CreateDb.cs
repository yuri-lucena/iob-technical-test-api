using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Iob.Bank.Infra.Migrations
{
    /// <inheritdoc />
    public partial class CreateDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tb_operation_type",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    created_by = table.Column<long>(type: "bigint", nullable: false),
                    modified_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    modified_by = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_operation_type", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tb_user_type",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    created_by = table.Column<long>(type: "bigint", nullable: false),
                    modified_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    modified_by = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_user_type", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tb_user",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    identifier = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    birthday = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    phone_number = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    address = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    user_type_id = table.Column<long>(type: "bigint", nullable: false),
                    active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    created_by = table.Column<long>(type: "bigint", nullable: false),
                    modified_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    modified_by = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_user", x => x.id);
                    table.ForeignKey(
                        name: "FK_tb_user_tb_user_type_user_type_id",
                        column: x => x.user_type_id,
                        principalTable: "tb_user_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tb_bank_account",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    balance = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    opening_date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    last_transaction_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    created_by = table.Column<long>(type: "bigint", nullable: false),
                    modified_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    modified_by = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_bank_account", x => x.id);
                    table.ForeignKey(
                        name: "FK_tb_bank_account_tb_user_user_id",
                        column: x => x.user_id,
                        principalTable: "tb_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tb_bank_launch",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    value = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    origin_account_id = table.Column<long>(type: "bigint", nullable: true),
                    destination_account_id = table.Column<long>(type: "bigint", nullable: true),
                    operation_type_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    created_by = table.Column<long>(type: "bigint", nullable: false),
                    modified_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    modified_by = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_bank_launch", x => x.id);
                    table.ForeignKey(
                        name: "FK_tb_bank_launch_tb_bank_account_destination_account_id",
                        column: x => x.destination_account_id,
                        principalTable: "tb_bank_account",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tb_bank_launch_tb_bank_account_origin_account_id",
                        column: x => x.origin_account_id,
                        principalTable: "tb_bank_account",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tb_bank_launch_tb_operation_type_operation_type_id",
                        column: x => x.operation_type_id,
                        principalTable: "tb_operation_type",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "tb_operation_type",
                columns: new[] { "id", "created_at", "created_by", "modified_at", "modified_by", "name" },
                values: new object[,]
                {
                    { 1L, new DateTime(2024, 10, 4, 7, 16, 12, 29, DateTimeKind.Local).AddTicks(5246), 1L, null, null, "Credit" },
                    { 2L, new DateTime(2024, 10, 4, 7, 16, 12, 29, DateTimeKind.Local).AddTicks(5265), 1L, null, null, "Debit" },
                    { 3L, new DateTime(2024, 10, 4, 7, 16, 12, 29, DateTimeKind.Local).AddTicks(5266), 1L, null, null, "Transfer" }
                });

            migrationBuilder.InsertData(
                table: "tb_user_type",
                columns: new[] { "id", "created_at", "created_by", "description", "is_active", "modified_at", "modified_by", "type" },
                values: new object[,]
                {
                    { 1L, new DateTime(2024, 10, 4, 7, 16, 12, 29, DateTimeKind.Local).AddTicks(6624), 1L, "System administrator", true, null, null, "Administrator" },
                    { 2L, new DateTime(2024, 10, 4, 7, 16, 12, 29, DateTimeKind.Local).AddTicks(6626), 1L, "Customer", true, null, null, "Customer" }
                });

            migrationBuilder.InsertData(
                table: "tb_user",
                columns: new[] { "id", "active", "address", "birthday", "created_at", "created_by", "email", "identifier", "modified_at", "modified_by", "name", "password", "phone_number", "user_type_id" },
                values: new object[,]
                {
                    { 1L, true, null, new DateTime(2002, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 10, 4, 7, 16, 12, 29, DateTimeKind.Local).AddTicks(6086), 0L, "yuri@iob.com", "12312312323", null, null, "Yuri", "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3", null, 1L },
                    { 2L, true, null, new DateTime(2003, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 10, 4, 7, 16, 12, 29, DateTimeKind.Local).AddTicks(6096), 0L, "tata@iob.com", "12312312323", null, null, "Tata", "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3", null, 1L }
                });

            migrationBuilder.InsertData(
                table: "tb_bank_account",
                columns: new[] { "id", "active", "balance", "created_at", "created_by", "last_transaction_date", "modified_at", "modified_by", "opening_date", "type", "user_id" },
                values: new object[,]
                {
                    { 1L, true, 0m, new DateTime(2024, 10, 4, 7, 16, 12, 26, DateTimeKind.Local).AddTicks(2910), 1L, null, null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Conta Corrente", 1L },
                    { 2L, true, 1000m, new DateTime(2024, 10, 4, 7, 16, 12, 26, DateTimeKind.Local).AddTicks(2933), 1L, null, null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Conta Corrente", 2L }
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_bank_account_user_id",
                table: "tb_bank_account",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_tb_bank_launch_destination_account_id",
                table: "tb_bank_launch",
                column: "destination_account_id");

            migrationBuilder.CreateIndex(
                name: "IX_tb_bank_launch_operation_type_id",
                table: "tb_bank_launch",
                column: "operation_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_tb_bank_launch_origin_account_id",
                table: "tb_bank_launch",
                column: "origin_account_id");

            migrationBuilder.CreateIndex(
                name: "IX_tb_user_user_type_id",
                table: "tb_user",
                column: "user_type_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_bank_launch");

            migrationBuilder.DropTable(
                name: "tb_bank_account");

            migrationBuilder.DropTable(
                name: "tb_operation_type");

            migrationBuilder.DropTable(
                name: "tb_user");

            migrationBuilder.DropTable(
                name: "tb_user_type");
        }
    }
}
