using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DirectoryService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "department");

            migrationBuilder.CreateTable(
                name: "departments",
                schema: "department",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    depth = table.Column<int>(type: "integer", nullable: false),
                    department_identifier = table.Column<string>(type: "text", nullable: false),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true),
                    children_count = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    department_name = table.Column<string>(type: "text", nullable: false),
                    department_path = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_departments", x => x.id);
                    table.ForeignKey(
                        name: "FK_departments_departments_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "department",
                        principalTable: "departments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "locations",
                schema: "department",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    location_building = table.Column<string>(type: "text", nullable: false),
                    location_city = table.Column<string>(type: "text", nullable: false),
                    location_country = table.Column<string>(type: "text", nullable: false),
                    location_room_number = table.Column<int>(type: "integer", nullable: false),
                    location_street = table.Column<string>(type: "text", nullable: false),
                    location_name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    location_timezone = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_locations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "positions",
                schema: "department",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    position_description = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    position_name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_positions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "department_location",
                schema: "department",
                columns: table => new
                {
                    department_id = table.Column<Guid>(type: "uuid", nullable: false),
                    location_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_department_location", x => new { x.location_id, x.department_id });
                    table.ForeignKey(
                        name: "FK_department_location_departments_department_id",
                        column: x => x.department_id,
                        principalSchema: "department",
                        principalTable: "departments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_department_location_locations_location_id",
                        column: x => x.location_id,
                        principalSchema: "department",
                        principalTable: "locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "department_position",
                schema: "department",
                columns: table => new
                {
                    department_id = table.Column<Guid>(type: "uuid", nullable: false),
                    position_id = table.Column<Guid>(type: "uuid", nullable: false),
                    DepartmentPositionDepartmentId = table.Column<Guid>(type: "uuid", nullable: true),
                    DepartmentPositionPositionId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_department_position", x => new { x.position_id, x.department_id });
                    table.ForeignKey(
                        name: "FK_department_position_department_position_DepartmentPositionP~",
                        columns: x => new { x.DepartmentPositionPositionId, x.DepartmentPositionDepartmentId },
                        principalSchema: "department",
                        principalTable: "department_position",
                        principalColumns: new[] { "position_id", "department_id" });
                    table.ForeignKey(
                        name: "FK_department_position_departments_department_id",
                        column: x => x.department_id,
                        principalSchema: "department",
                        principalTable: "departments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_department_position_positions_position_id",
                        column: x => x.position_id,
                        principalSchema: "department",
                        principalTable: "positions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_department_location_department_id",
                schema: "department",
                table: "department_location",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "IX_department_position_department_id",
                schema: "department",
                table: "department_position",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "IX_department_position_DepartmentPositionPositionId_Department~",
                schema: "department",
                table: "department_position",
                columns: new[] { "DepartmentPositionPositionId", "DepartmentPositionDepartmentId" });

            migrationBuilder.CreateIndex(
                name: "IX_departments_ParentId",
                schema: "department",
                table: "departments",
                column: "ParentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "department_location",
                schema: "department");

            migrationBuilder.DropTable(
                name: "department_position",
                schema: "department");

            migrationBuilder.DropTable(
                name: "locations",
                schema: "department");

            migrationBuilder.DropTable(
                name: "departments",
                schema: "department");

            migrationBuilder.DropTable(
                name: "positions",
                schema: "department");
        }
    }
}
