using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mYPMS.Migrations
{
    public partial class CreateIdentitySchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tPaymentMethods",
                columns: table => new
                {
                    xId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    xPaymentMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tPaymentMethods", x => x.xId);
                });

            migrationBuilder.CreateTable(
                name: "tSettings",
                columns: table => new
                {
                    xId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    xName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    xValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    xRelationship = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tSettings", x => x.xId);
                });

            migrationBuilder.CreateTable(
                name: "tTariffs",
                columns: table => new
                {
                    xId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    xTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    xEntryPrice = table.Column<int>(type: "int", nullable: true),
                    xDayHourPrice = table.Column<int>(type: "int", nullable: true),
                    xNightHourPrice = table.Column<int>(type: "int", nullable: true),
                    xWholeDayPrice = table.Column<int>(type: "int", nullable: true),
                    xNightStartHour = table.Column<int>(type: "int", nullable: true),
                    xNightEndHour = table.Column<int>(type: "int", nullable: true),
                    xDescription = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tTariffs", x => x.xId);
                });

            migrationBuilder.CreateTable(
                name: "tUsers",
                columns: table => new
                {
                    xId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    xUsername = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    xPassword = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    xName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    xAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    xPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    xMobile = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    xDecription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    xRules = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tUsers", x => x.xId);
                });

            migrationBuilder.CreateTable(
                name: "vParkeds",
                columns: table => new
                {
                    xId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    xTagId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    xEnterDateTime = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    xExitDateTime = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    xEnterOperatorName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    xExitOperatorName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    xLicencePlate = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    xProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    xStatusCode = table.Column<int>(type: "int", nullable: true),
                    xGate = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    xPayment = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    xPaymentMethod = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    xParkingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tParkings",
                columns: table => new
                {
                    xId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    xCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    xTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    xAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    xPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    xCordination = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    xCapacity = table.Column<int>(type: "int", nullable: true),
                    xReservedCapacity = table.Column<int>(type: "int", nullable: true),
                    xPassword = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    xContractor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    xDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    xStatusCode = table.Column<int>(type: "int", nullable: true),
                    xSettings = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    xTariffId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tParkings", x => x.xId);
                    table.ForeignKey(
                        name: "FK_tParkings_tTariffs_xTariffId",
                        column: x => x.xTariffId,
                        principalTable: "tTariffs",
                        principalColumn: "xId");
                });

            migrationBuilder.CreateTable(
                name: "tTagGroup",
                columns: table => new
                {
                    xId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    xName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    xTariffId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tTagGroup", x => x.xId);
                    table.ForeignKey(
                        name: "FK_tTagGroup_tTariffs_xTariffId",
                        column: x => x.xTariffId,
                        principalTable: "tTariffs",
                        principalColumn: "xId");
                });

            migrationBuilder.CreateTable(
                name: "tGates",
                columns: table => new
                {
                    xId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    xName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    xDirection = table.Column<int>(type: "int", nullable: true),
                    xVideoInUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    xVideoOutUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    xImageInUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    xImageOutUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    xBarrierInUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    xBarrierOutUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    xDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    xParkingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tGates", x => x.xId);
                    table.ForeignKey(
                        name: "FK_tGates_tParkings_xParkingId",
                        column: x => x.xParkingId,
                        principalTable: "tParkings",
                        principalColumn: "xId");
                });

            migrationBuilder.CreateTable(
                name: "tTagList",
                columns: table => new
                {
                    xId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    xExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    xComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    xTagGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tTagList", x => x.xId);
                    table.ForeignKey(
                        name: "FK_tTagList_tTagGroup_xTagGroupId",
                        column: x => x.xTagGroupId,
                        principalTable: "tTagGroup",
                        principalColumn: "xId");
                });

            migrationBuilder.CreateTable(
                name: "tTraffics",
                columns: table => new
                {
                    xId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    xParkingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    xTagId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    xEntryDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    xDepartureDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    xEntryOperatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    xDepartureOperatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    xLicencePlateEn = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    xLicencePlateEx = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    xProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    xStatusCode = table.Column<int>(type: "int", nullable: true),
                    xEntryGateId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    xDepartureGateId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    xPaid = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    xPaidMethodId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    xTariffId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tTraffics", x => x.xId);
                    table.ForeignKey(
                        name: "FK_tTraffics_tGates_xDepartureGateId",
                        column: x => x.xDepartureGateId,
                        principalTable: "tGates",
                        principalColumn: "xId");
                    table.ForeignKey(
                        name: "FK_tTraffics_tGates_xEntryGateId",
                        column: x => x.xEntryGateId,
                        principalTable: "tGates",
                        principalColumn: "xId");
                    table.ForeignKey(
                        name: "FK_tTraffics_tParkings_xParkingId",
                        column: x => x.xParkingId,
                        principalTable: "tParkings",
                        principalColumn: "xId");
                    table.ForeignKey(
                        name: "FK_tTraffics_tPaymentMethods_xPaidMethodId",
                        column: x => x.xPaidMethodId,
                        principalTable: "tPaymentMethods",
                        principalColumn: "xId");
                    table.ForeignKey(
                        name: "FK_tTraffics_tTariffs_xTariffId",
                        column: x => x.xTariffId,
                        principalTable: "tTariffs",
                        principalColumn: "xId");
                    table.ForeignKey(
                        name: "FK_tTraffics_tUsers_xDepartureOperatorId",
                        column: x => x.xDepartureOperatorId,
                        principalTable: "tUsers",
                        principalColumn: "xId");
                    table.ForeignKey(
                        name: "FK_tTraffics_tUsers_xEntryOperatorId",
                        column: x => x.xEntryOperatorId,
                        principalTable: "tUsers",
                        principalColumn: "xId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_tGates_xParkingId",
                table: "tGates",
                column: "xParkingId");

            migrationBuilder.CreateIndex(
                name: "IX_tParkings_xTariffId",
                table: "tParkings",
                column: "xTariffId");

            migrationBuilder.CreateIndex(
                name: "IX_tTagGroup_xTariffId",
                table: "tTagGroup",
                column: "xTariffId");

            migrationBuilder.CreateIndex(
                name: "IX_tTagList_xTagGroupId",
                table: "tTagList",
                column: "xTagGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_tTraffics_xDepartureGateId",
                table: "tTraffics",
                column: "xDepartureGateId");

            migrationBuilder.CreateIndex(
                name: "IX_tTraffics_xDepartureOperatorId",
                table: "tTraffics",
                column: "xDepartureOperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_tTraffics_xEntryGateId",
                table: "tTraffics",
                column: "xEntryGateId");

            migrationBuilder.CreateIndex(
                name: "IX_tTraffics_xEntryOperatorId",
                table: "tTraffics",
                column: "xEntryOperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_tTraffics_xPaidMethodId",
                table: "tTraffics",
                column: "xPaidMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_tTraffics_xParkingId",
                table: "tTraffics",
                column: "xParkingId");

            migrationBuilder.CreateIndex(
                name: "IX_tTraffics_xTariffId",
                table: "tTraffics",
                column: "xTariffId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "tSettings");

            migrationBuilder.DropTable(
                name: "tTagList");

            migrationBuilder.DropTable(
                name: "tTraffics");

            migrationBuilder.DropTable(
                name: "vParkeds");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "tTagGroup");

            migrationBuilder.DropTable(
                name: "tGates");

            migrationBuilder.DropTable(
                name: "tPaymentMethods");

            migrationBuilder.DropTable(
                name: "tUsers");

            migrationBuilder.DropTable(
                name: "tParkings");

            migrationBuilder.DropTable(
                name: "tTariffs");
        }
    }
}
