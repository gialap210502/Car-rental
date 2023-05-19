using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Car_rental.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "category",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Deleted_Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "discount",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    percentage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    startDate = table.Column<int>(type: "int", nullable: false),
                    endDate = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_discount", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    citizen_identification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    driver_license = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    dob = table.Column<DateTime>(type: "datetime2", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    flag = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "bookings",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    startDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    endDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    totalAmount = table.Column<double>(type: "float", nullable: true),
                    userId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bookings", x => x.id);
                    table.ForeignKey(
                        name: "FK_bookings_user_userId",
                        column: x => x.userId,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Car",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    model = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    brand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    seat = table.Column<int>(type: "int", nullable: true),
                    color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    available = table.Column<int>(type: "int", nullable: true),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false),
                    discount_id = table.Column<int>(type: "int", nullable: false),
                    Discountid = table.Column<int>(type: "int", nullable: true),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    userid = table.Column<int>(type: "int", nullable: true),
                    category_id = table.Column<int>(type: "int", nullable: false),
                    categoryid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Car", x => x.id);
                    table.ForeignKey(
                        name: "FK_Car_category_categoryid",
                        column: x => x.categoryid,
                        principalTable: "category",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Car_discount_Discountid",
                        column: x => x.Discountid,
                        principalTable: "discount",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Car_user_userid",
                        column: x => x.userid,
                        principalTable: "user",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "userRole",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<int>(type: "int", nullable: false),
                    roleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userRole", x => x.id);
                    table.ForeignKey(
                        name: "FK_userRole_roles_roleId",
                        column: x => x.roleId,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_userRole_user_userId",
                        column: x => x.userId,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "payment",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    amount = table.Column<double>(type: "float", nullable: false),
                    paymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    paymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    carId = table.Column<int>(type: "int", nullable: false),
                    booking_id = table.Column<int>(type: "int", nullable: false),
                    bookingid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment", x => x.id);
                    table.ForeignKey(
                        name: "FK_payment_Car_carId",
                        column: x => x.carId,
                        principalTable: "Car",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_payment_bookings_bookingid",
                        column: x => x.bookingid,
                        principalTable: "bookings",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "rating",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    dateRating = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Star = table.Column<int>(type: "int", nullable: false),
                    comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    carId = table.Column<int>(type: "int", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rating", x => x.id);
                    table.ForeignKey(
                        name: "FK_rating_Car_carId",
                        column: x => x.carId,
                        principalTable: "Car",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_rating_user_userId",
                        column: x => x.userId,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_bookings_userId",
                table: "bookings",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Car_categoryid",
                table: "Car",
                column: "categoryid");

            migrationBuilder.CreateIndex(
                name: "IX_Car_Discountid",
                table: "Car",
                column: "Discountid");

            migrationBuilder.CreateIndex(
                name: "IX_Car_userid",
                table: "Car",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_payment_bookingid",
                table: "payment",
                column: "bookingid");

            migrationBuilder.CreateIndex(
                name: "IX_payment_carId",
                table: "payment",
                column: "carId");

            migrationBuilder.CreateIndex(
                name: "IX_rating_carId",
                table: "rating",
                column: "carId");

            migrationBuilder.CreateIndex(
                name: "IX_rating_userId",
                table: "rating",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_userRole_roleId",
                table: "userRole",
                column: "roleId");

            migrationBuilder.CreateIndex(
                name: "IX_userRole_userId",
                table: "userRole",
                column: "userId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "payment");

            migrationBuilder.DropTable(
                name: "rating");

            migrationBuilder.DropTable(
                name: "userRole");

            migrationBuilder.DropTable(
                name: "bookings");

            migrationBuilder.DropTable(
                name: "Car");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "category");

            migrationBuilder.DropTable(
                name: "discount");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
