using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SeatGuard.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "data_protection_keys",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    friendly_name = table.Column<string>(type: "text", nullable: true),
                    xml = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_data_protection_keys", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "events",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    venue = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    starts_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    is_sandbox = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_events", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "seats",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    event_id = table.Column<Guid>(type: "uuid", nullable: false),
                    row = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    number = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_seats", x => x.id);
                    table.ForeignKey(
                        name: "fk_seats_events_event_id",
                        column: x => x.event_id,
                        principalTable: "events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "stress_test_runs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    event_id = table.Column<Guid>(type: "uuid", nullable: false),
                    started_by = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    attempts = table.Column<int>(type: "integer", nullable: false),
                    seat_count = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    succeeded = table.Column<int>(type: "integer", nullable: false),
                    conflicts = table.Column<int>(type: "integer", nullable: false),
                    failed = table.Column<int>(type: "integer", nullable: false),
                    double_bookings = table.Column<int>(type: "integer", nullable: false),
                    duration_ms = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    completed_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_stress_test_runs", x => x.id);
                    table.ForeignKey(
                        name: "fk_stress_test_runs_events_event_id",
                        column: x => x.event_id,
                        principalTable: "events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reservations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    seat_id = table.Column<Guid>(type: "uuid", nullable: false),
                    event_id = table.Column<Guid>(type: "uuid", nullable: false),
                    holder_id = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    expires_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_reservations", x => x.id);
                    table.ForeignKey(
                        name: "fk_reservations_seats_seat_id",
                        column: x => x.seat_id,
                        principalTable: "seats",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "events",
                columns: new[] { "id", "created_at", "is_sandbox", "name", "starts_at", "venue" },
                values: new object[] { new Guid("5ea7c0de-0000-4000-8000-000000000001"), new DateTimeOffset(new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, "SeatGuard Live: Demo Night", new DateTimeOffset(new DateTime(2026, 12, 12, 19, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Avicii Arena, Stockholm" });

            migrationBuilder.InsertData(
                table: "seats",
                columns: new[] { "id", "event_id", "number", "row" },
                values: new object[,]
                {
                    { new Guid("5ea7c0de-0001-4000-8000-000000000001"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 1, "A" },
                    { new Guid("5ea7c0de-0001-4000-8000-000000000002"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 2, "A" },
                    { new Guid("5ea7c0de-0001-4000-8000-000000000003"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 3, "A" },
                    { new Guid("5ea7c0de-0001-4000-8000-000000000004"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 4, "A" },
                    { new Guid("5ea7c0de-0001-4000-8000-000000000005"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 5, "A" },
                    { new Guid("5ea7c0de-0001-4000-8000-000000000006"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 6, "A" },
                    { new Guid("5ea7c0de-0001-4000-8000-000000000007"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 7, "A" },
                    { new Guid("5ea7c0de-0001-4000-8000-000000000008"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 8, "A" },
                    { new Guid("5ea7c0de-0001-4000-8000-000000000009"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 9, "A" },
                    { new Guid("5ea7c0de-0001-4000-8000-00000000000a"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 10, "A" },
                    { new Guid("5ea7c0de-0001-4000-8000-00000000000b"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 11, "A" },
                    { new Guid("5ea7c0de-0001-4000-8000-00000000000c"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 12, "A" },
                    { new Guid("5ea7c0de-0001-4000-8000-00000000000d"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 13, "A" },
                    { new Guid("5ea7c0de-0001-4000-8000-00000000000e"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 14, "A" },
                    { new Guid("5ea7c0de-0001-4000-8000-00000000000f"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 15, "A" },
                    { new Guid("5ea7c0de-0001-4000-8000-000000000010"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 16, "A" },
                    { new Guid("5ea7c0de-0001-4000-8000-000000000011"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 17, "A" },
                    { new Guid("5ea7c0de-0001-4000-8000-000000000012"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 18, "A" },
                    { new Guid("5ea7c0de-0001-4000-8000-000000000013"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 19, "A" },
                    { new Guid("5ea7c0de-0001-4000-8000-000000000014"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 20, "A" },
                    { new Guid("5ea7c0de-0001-4000-8000-000100000001"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 1, "B" },
                    { new Guid("5ea7c0de-0001-4000-8000-000100000002"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 2, "B" },
                    { new Guid("5ea7c0de-0001-4000-8000-000100000003"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 3, "B" },
                    { new Guid("5ea7c0de-0001-4000-8000-000100000004"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 4, "B" },
                    { new Guid("5ea7c0de-0001-4000-8000-000100000005"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 5, "B" },
                    { new Guid("5ea7c0de-0001-4000-8000-000100000006"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 6, "B" },
                    { new Guid("5ea7c0de-0001-4000-8000-000100000007"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 7, "B" },
                    { new Guid("5ea7c0de-0001-4000-8000-000100000008"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 8, "B" },
                    { new Guid("5ea7c0de-0001-4000-8000-000100000009"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 9, "B" },
                    { new Guid("5ea7c0de-0001-4000-8000-00010000000a"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 10, "B" },
                    { new Guid("5ea7c0de-0001-4000-8000-00010000000b"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 11, "B" },
                    { new Guid("5ea7c0de-0001-4000-8000-00010000000c"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 12, "B" },
                    { new Guid("5ea7c0de-0001-4000-8000-00010000000d"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 13, "B" },
                    { new Guid("5ea7c0de-0001-4000-8000-00010000000e"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 14, "B" },
                    { new Guid("5ea7c0de-0001-4000-8000-00010000000f"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 15, "B" },
                    { new Guid("5ea7c0de-0001-4000-8000-000100000010"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 16, "B" },
                    { new Guid("5ea7c0de-0001-4000-8000-000100000011"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 17, "B" },
                    { new Guid("5ea7c0de-0001-4000-8000-000100000012"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 18, "B" },
                    { new Guid("5ea7c0de-0001-4000-8000-000100000013"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 19, "B" },
                    { new Guid("5ea7c0de-0001-4000-8000-000100000014"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 20, "B" },
                    { new Guid("5ea7c0de-0001-4000-8000-000200000001"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 1, "C" },
                    { new Guid("5ea7c0de-0001-4000-8000-000200000002"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 2, "C" },
                    { new Guid("5ea7c0de-0001-4000-8000-000200000003"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 3, "C" },
                    { new Guid("5ea7c0de-0001-4000-8000-000200000004"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 4, "C" },
                    { new Guid("5ea7c0de-0001-4000-8000-000200000005"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 5, "C" },
                    { new Guid("5ea7c0de-0001-4000-8000-000200000006"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 6, "C" },
                    { new Guid("5ea7c0de-0001-4000-8000-000200000007"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 7, "C" },
                    { new Guid("5ea7c0de-0001-4000-8000-000200000008"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 8, "C" },
                    { new Guid("5ea7c0de-0001-4000-8000-000200000009"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 9, "C" },
                    { new Guid("5ea7c0de-0001-4000-8000-00020000000a"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 10, "C" },
                    { new Guid("5ea7c0de-0001-4000-8000-00020000000b"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 11, "C" },
                    { new Guid("5ea7c0de-0001-4000-8000-00020000000c"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 12, "C" },
                    { new Guid("5ea7c0de-0001-4000-8000-00020000000d"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 13, "C" },
                    { new Guid("5ea7c0de-0001-4000-8000-00020000000e"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 14, "C" },
                    { new Guid("5ea7c0de-0001-4000-8000-00020000000f"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 15, "C" },
                    { new Guid("5ea7c0de-0001-4000-8000-000200000010"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 16, "C" },
                    { new Guid("5ea7c0de-0001-4000-8000-000200000011"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 17, "C" },
                    { new Guid("5ea7c0de-0001-4000-8000-000200000012"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 18, "C" },
                    { new Guid("5ea7c0de-0001-4000-8000-000200000013"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 19, "C" },
                    { new Guid("5ea7c0de-0001-4000-8000-000200000014"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 20, "C" },
                    { new Guid("5ea7c0de-0001-4000-8000-000300000001"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 1, "D" },
                    { new Guid("5ea7c0de-0001-4000-8000-000300000002"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 2, "D" },
                    { new Guid("5ea7c0de-0001-4000-8000-000300000003"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 3, "D" },
                    { new Guid("5ea7c0de-0001-4000-8000-000300000004"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 4, "D" },
                    { new Guid("5ea7c0de-0001-4000-8000-000300000005"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 5, "D" },
                    { new Guid("5ea7c0de-0001-4000-8000-000300000006"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 6, "D" },
                    { new Guid("5ea7c0de-0001-4000-8000-000300000007"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 7, "D" },
                    { new Guid("5ea7c0de-0001-4000-8000-000300000008"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 8, "D" },
                    { new Guid("5ea7c0de-0001-4000-8000-000300000009"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 9, "D" },
                    { new Guid("5ea7c0de-0001-4000-8000-00030000000a"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 10, "D" },
                    { new Guid("5ea7c0de-0001-4000-8000-00030000000b"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 11, "D" },
                    { new Guid("5ea7c0de-0001-4000-8000-00030000000c"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 12, "D" },
                    { new Guid("5ea7c0de-0001-4000-8000-00030000000d"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 13, "D" },
                    { new Guid("5ea7c0de-0001-4000-8000-00030000000e"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 14, "D" },
                    { new Guid("5ea7c0de-0001-4000-8000-00030000000f"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 15, "D" },
                    { new Guid("5ea7c0de-0001-4000-8000-000300000010"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 16, "D" },
                    { new Guid("5ea7c0de-0001-4000-8000-000300000011"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 17, "D" },
                    { new Guid("5ea7c0de-0001-4000-8000-000300000012"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 18, "D" },
                    { new Guid("5ea7c0de-0001-4000-8000-000300000013"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 19, "D" },
                    { new Guid("5ea7c0de-0001-4000-8000-000300000014"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 20, "D" },
                    { new Guid("5ea7c0de-0001-4000-8000-000400000001"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 1, "E" },
                    { new Guid("5ea7c0de-0001-4000-8000-000400000002"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 2, "E" },
                    { new Guid("5ea7c0de-0001-4000-8000-000400000003"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 3, "E" },
                    { new Guid("5ea7c0de-0001-4000-8000-000400000004"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 4, "E" },
                    { new Guid("5ea7c0de-0001-4000-8000-000400000005"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 5, "E" },
                    { new Guid("5ea7c0de-0001-4000-8000-000400000006"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 6, "E" },
                    { new Guid("5ea7c0de-0001-4000-8000-000400000007"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 7, "E" },
                    { new Guid("5ea7c0de-0001-4000-8000-000400000008"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 8, "E" },
                    { new Guid("5ea7c0de-0001-4000-8000-000400000009"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 9, "E" },
                    { new Guid("5ea7c0de-0001-4000-8000-00040000000a"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 10, "E" },
                    { new Guid("5ea7c0de-0001-4000-8000-00040000000b"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 11, "E" },
                    { new Guid("5ea7c0de-0001-4000-8000-00040000000c"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 12, "E" },
                    { new Guid("5ea7c0de-0001-4000-8000-00040000000d"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 13, "E" },
                    { new Guid("5ea7c0de-0001-4000-8000-00040000000e"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 14, "E" },
                    { new Guid("5ea7c0de-0001-4000-8000-00040000000f"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 15, "E" },
                    { new Guid("5ea7c0de-0001-4000-8000-000400000010"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 16, "E" },
                    { new Guid("5ea7c0de-0001-4000-8000-000400000011"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 17, "E" },
                    { new Guid("5ea7c0de-0001-4000-8000-000400000012"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 18, "E" },
                    { new Guid("5ea7c0de-0001-4000-8000-000400000013"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 19, "E" },
                    { new Guid("5ea7c0de-0001-4000-8000-000400000014"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 20, "E" },
                    { new Guid("5ea7c0de-0001-4000-8000-000500000001"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 1, "F" },
                    { new Guid("5ea7c0de-0001-4000-8000-000500000002"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 2, "F" },
                    { new Guid("5ea7c0de-0001-4000-8000-000500000003"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 3, "F" },
                    { new Guid("5ea7c0de-0001-4000-8000-000500000004"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 4, "F" },
                    { new Guid("5ea7c0de-0001-4000-8000-000500000005"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 5, "F" },
                    { new Guid("5ea7c0de-0001-4000-8000-000500000006"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 6, "F" },
                    { new Guid("5ea7c0de-0001-4000-8000-000500000007"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 7, "F" },
                    { new Guid("5ea7c0de-0001-4000-8000-000500000008"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 8, "F" },
                    { new Guid("5ea7c0de-0001-4000-8000-000500000009"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 9, "F" },
                    { new Guid("5ea7c0de-0001-4000-8000-00050000000a"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 10, "F" },
                    { new Guid("5ea7c0de-0001-4000-8000-00050000000b"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 11, "F" },
                    { new Guid("5ea7c0de-0001-4000-8000-00050000000c"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 12, "F" },
                    { new Guid("5ea7c0de-0001-4000-8000-00050000000d"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 13, "F" },
                    { new Guid("5ea7c0de-0001-4000-8000-00050000000e"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 14, "F" },
                    { new Guid("5ea7c0de-0001-4000-8000-00050000000f"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 15, "F" },
                    { new Guid("5ea7c0de-0001-4000-8000-000500000010"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 16, "F" },
                    { new Guid("5ea7c0de-0001-4000-8000-000500000011"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 17, "F" },
                    { new Guid("5ea7c0de-0001-4000-8000-000500000012"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 18, "F" },
                    { new Guid("5ea7c0de-0001-4000-8000-000500000013"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 19, "F" },
                    { new Guid("5ea7c0de-0001-4000-8000-000500000014"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 20, "F" },
                    { new Guid("5ea7c0de-0001-4000-8000-000600000001"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 1, "G" },
                    { new Guid("5ea7c0de-0001-4000-8000-000600000002"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 2, "G" },
                    { new Guid("5ea7c0de-0001-4000-8000-000600000003"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 3, "G" },
                    { new Guid("5ea7c0de-0001-4000-8000-000600000004"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 4, "G" },
                    { new Guid("5ea7c0de-0001-4000-8000-000600000005"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 5, "G" },
                    { new Guid("5ea7c0de-0001-4000-8000-000600000006"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 6, "G" },
                    { new Guid("5ea7c0de-0001-4000-8000-000600000007"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 7, "G" },
                    { new Guid("5ea7c0de-0001-4000-8000-000600000008"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 8, "G" },
                    { new Guid("5ea7c0de-0001-4000-8000-000600000009"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 9, "G" },
                    { new Guid("5ea7c0de-0001-4000-8000-00060000000a"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 10, "G" },
                    { new Guid("5ea7c0de-0001-4000-8000-00060000000b"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 11, "G" },
                    { new Guid("5ea7c0de-0001-4000-8000-00060000000c"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 12, "G" },
                    { new Guid("5ea7c0de-0001-4000-8000-00060000000d"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 13, "G" },
                    { new Guid("5ea7c0de-0001-4000-8000-00060000000e"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 14, "G" },
                    { new Guid("5ea7c0de-0001-4000-8000-00060000000f"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 15, "G" },
                    { new Guid("5ea7c0de-0001-4000-8000-000600000010"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 16, "G" },
                    { new Guid("5ea7c0de-0001-4000-8000-000600000011"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 17, "G" },
                    { new Guid("5ea7c0de-0001-4000-8000-000600000012"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 18, "G" },
                    { new Guid("5ea7c0de-0001-4000-8000-000600000013"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 19, "G" },
                    { new Guid("5ea7c0de-0001-4000-8000-000600000014"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 20, "G" },
                    { new Guid("5ea7c0de-0001-4000-8000-000700000001"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 1, "H" },
                    { new Guid("5ea7c0de-0001-4000-8000-000700000002"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 2, "H" },
                    { new Guid("5ea7c0de-0001-4000-8000-000700000003"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 3, "H" },
                    { new Guid("5ea7c0de-0001-4000-8000-000700000004"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 4, "H" },
                    { new Guid("5ea7c0de-0001-4000-8000-000700000005"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 5, "H" },
                    { new Guid("5ea7c0de-0001-4000-8000-000700000006"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 6, "H" },
                    { new Guid("5ea7c0de-0001-4000-8000-000700000007"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 7, "H" },
                    { new Guid("5ea7c0de-0001-4000-8000-000700000008"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 8, "H" },
                    { new Guid("5ea7c0de-0001-4000-8000-000700000009"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 9, "H" },
                    { new Guid("5ea7c0de-0001-4000-8000-00070000000a"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 10, "H" },
                    { new Guid("5ea7c0de-0001-4000-8000-00070000000b"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 11, "H" },
                    { new Guid("5ea7c0de-0001-4000-8000-00070000000c"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 12, "H" },
                    { new Guid("5ea7c0de-0001-4000-8000-00070000000d"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 13, "H" },
                    { new Guid("5ea7c0de-0001-4000-8000-00070000000e"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 14, "H" },
                    { new Guid("5ea7c0de-0001-4000-8000-00070000000f"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 15, "H" },
                    { new Guid("5ea7c0de-0001-4000-8000-000700000010"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 16, "H" },
                    { new Guid("5ea7c0de-0001-4000-8000-000700000011"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 17, "H" },
                    { new Guid("5ea7c0de-0001-4000-8000-000700000012"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 18, "H" },
                    { new Guid("5ea7c0de-0001-4000-8000-000700000013"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 19, "H" },
                    { new Guid("5ea7c0de-0001-4000-8000-000700000014"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 20, "H" },
                    { new Guid("5ea7c0de-0001-4000-8000-000800000001"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 1, "I" },
                    { new Guid("5ea7c0de-0001-4000-8000-000800000002"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 2, "I" },
                    { new Guid("5ea7c0de-0001-4000-8000-000800000003"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 3, "I" },
                    { new Guid("5ea7c0de-0001-4000-8000-000800000004"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 4, "I" },
                    { new Guid("5ea7c0de-0001-4000-8000-000800000005"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 5, "I" },
                    { new Guid("5ea7c0de-0001-4000-8000-000800000006"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 6, "I" },
                    { new Guid("5ea7c0de-0001-4000-8000-000800000007"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 7, "I" },
                    { new Guid("5ea7c0de-0001-4000-8000-000800000008"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 8, "I" },
                    { new Guid("5ea7c0de-0001-4000-8000-000800000009"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 9, "I" },
                    { new Guid("5ea7c0de-0001-4000-8000-00080000000a"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 10, "I" },
                    { new Guid("5ea7c0de-0001-4000-8000-00080000000b"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 11, "I" },
                    { new Guid("5ea7c0de-0001-4000-8000-00080000000c"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 12, "I" },
                    { new Guid("5ea7c0de-0001-4000-8000-00080000000d"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 13, "I" },
                    { new Guid("5ea7c0de-0001-4000-8000-00080000000e"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 14, "I" },
                    { new Guid("5ea7c0de-0001-4000-8000-00080000000f"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 15, "I" },
                    { new Guid("5ea7c0de-0001-4000-8000-000800000010"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 16, "I" },
                    { new Guid("5ea7c0de-0001-4000-8000-000800000011"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 17, "I" },
                    { new Guid("5ea7c0de-0001-4000-8000-000800000012"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 18, "I" },
                    { new Guid("5ea7c0de-0001-4000-8000-000800000013"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 19, "I" },
                    { new Guid("5ea7c0de-0001-4000-8000-000800000014"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 20, "I" },
                    { new Guid("5ea7c0de-0001-4000-8000-000900000001"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 1, "J" },
                    { new Guid("5ea7c0de-0001-4000-8000-000900000002"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 2, "J" },
                    { new Guid("5ea7c0de-0001-4000-8000-000900000003"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 3, "J" },
                    { new Guid("5ea7c0de-0001-4000-8000-000900000004"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 4, "J" },
                    { new Guid("5ea7c0de-0001-4000-8000-000900000005"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 5, "J" },
                    { new Guid("5ea7c0de-0001-4000-8000-000900000006"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 6, "J" },
                    { new Guid("5ea7c0de-0001-4000-8000-000900000007"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 7, "J" },
                    { new Guid("5ea7c0de-0001-4000-8000-000900000008"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 8, "J" },
                    { new Guid("5ea7c0de-0001-4000-8000-000900000009"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 9, "J" },
                    { new Guid("5ea7c0de-0001-4000-8000-00090000000a"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 10, "J" },
                    { new Guid("5ea7c0de-0001-4000-8000-00090000000b"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 11, "J" },
                    { new Guid("5ea7c0de-0001-4000-8000-00090000000c"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 12, "J" },
                    { new Guid("5ea7c0de-0001-4000-8000-00090000000d"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 13, "J" },
                    { new Guid("5ea7c0de-0001-4000-8000-00090000000e"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 14, "J" },
                    { new Guid("5ea7c0de-0001-4000-8000-00090000000f"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 15, "J" },
                    { new Guid("5ea7c0de-0001-4000-8000-000900000010"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 16, "J" },
                    { new Guid("5ea7c0de-0001-4000-8000-000900000011"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 17, "J" },
                    { new Guid("5ea7c0de-0001-4000-8000-000900000012"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 18, "J" },
                    { new Guid("5ea7c0de-0001-4000-8000-000900000013"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 19, "J" },
                    { new Guid("5ea7c0de-0001-4000-8000-000900000014"), new Guid("5ea7c0de-0000-4000-8000-000000000001"), 20, "J" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_events_is_sandbox_starts_at",
                table: "events",
                columns: new[] { "is_sandbox", "starts_at" });

            migrationBuilder.CreateIndex(
                name: "ix_reservations_event_id",
                table: "reservations",
                column: "event_id");

            migrationBuilder.CreateIndex(
                name: "ix_reservations_expires_at",
                table: "reservations",
                column: "expires_at");

            migrationBuilder.CreateIndex(
                name: "ix_reservations_holder_id",
                table: "reservations",
                column: "holder_id");

            migrationBuilder.CreateIndex(
                name: "ix_reservations_seat_id",
                table: "reservations",
                column: "seat_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_seats_event_id_row_number",
                table: "seats",
                columns: new[] { "event_id", "row", "number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_stress_test_runs_event_id",
                table: "stress_test_runs",
                column: "event_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "data_protection_keys");

            migrationBuilder.DropTable(
                name: "reservations");

            migrationBuilder.DropTable(
                name: "stress_test_runs");

            migrationBuilder.DropTable(
                name: "seats");

            migrationBuilder.DropTable(
                name: "events");
        }
    }
}
