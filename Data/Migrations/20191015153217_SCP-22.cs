using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SVT.Platform.Data.Migrations
{
    public partial class SCP22 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AreaTypes",
                columns: table => new
                {
                    Value = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AreaTypes", x => x.Value);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryTypes",
                columns: table => new
                {
                    Value = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryTypes", x => x.Value);
                });

            migrationBuilder.CreateTable(
                name: "DevLogs",
                columns: table => new
                {
                    DevLogId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsertedOn = table.Column<DateTime>(nullable: false),
                    InsertedBy = table.Column<string>(maxLength: 256, nullable: false),
                    Serialized = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DevLogs", x => x.DevLogId);
                });

            migrationBuilder.CreateTable(
                name: "LocationTypes",
                columns: table => new
                {
                    Value = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationTypes", x => x.Value);
                });

            migrationBuilder.CreateTable(
                name: "Pools",
                columns: table => new
                {
                    PoolId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pools", x => x.PoolId);
                });

            migrationBuilder.CreateTable(
                name: "UserLogs",
                columns: table => new
                {
                    UserLogId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsertedOn = table.Column<DateTime>(nullable: false),
                    InsertedBy = table.Column<string>(maxLength: 256, nullable: false),
                    Serialized = table.Column<string>(nullable: false),
                    vUserId = table.Column<string>(maxLength: 256, nullable: true, computedColumnSql: "CONVERT([nvarchar](256),json_value([Serialized],N'$.UserId'))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogs", x => x.UserLogId);
                });

            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    AreaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    PoolId = table.Column<int>(nullable: false),
                    AreaType = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.AreaId);
                    table.ForeignKey(
                        name: "FK_Areas_AreaTypes_AreaType",
                        column: x => x.AreaType,
                        principalTable: "AreaTypes",
                        principalColumn: "Value",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Areas_Pools_PoolId",
                        column: x => x.PoolId,
                        principalTable: "Pools",
                        principalColumn: "PoolId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AreaDeliveryTypes",
                columns: table => new
                {
                    AreaId = table.Column<int>(nullable: false),
                    DeliveryType = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AreaDeliveryTypes", x => new { x.AreaId, x.DeliveryType });
                    table.ForeignKey(
                        name: "FK_AreaDeliveryTypes_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "AreaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AreaDeliveryTypes_DeliveryTypes_DeliveryType",
                        column: x => x.DeliveryType,
                        principalTable: "DeliveryTypes",
                        principalColumn: "Value",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AreaMaps",
                columns: table => new
                {
                    PreviousAreaId = table.Column<int>(nullable: false),
                    NextAreaId = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AreaMaps", x => new { x.NextAreaId, x.PreviousAreaId });
                    table.ForeignKey(
                        name: "FK_AreaMaps_Areas_NextAreaId",
                        column: x => x.NextAreaId,
                        principalTable: "Areas",
                        principalColumn: "AreaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AreaMaps_Areas_PreviousAreaId",
                        column: x => x.PreviousAreaId,
                        principalTable: "Areas",
                        principalColumn: "AreaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AreaOverflow",
                columns: table => new
                {
                    DestinationAreaId = table.Column<int>(nullable: false),
                    OverflowAreaId = table.Column<int>(nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AreaOverflow", x => new { x.DestinationAreaId, x.OverflowAreaId });
                    table.ForeignKey(
                        name: "FK_AreaOverflow_Areas_DestinationAreaId",
                        column: x => x.DestinationAreaId,
                        principalTable: "Areas",
                        principalColumn: "AreaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AreaOverflow_Areas_OverflowAreaId",
                        column: x => x.OverflowAreaId,
                        principalTable: "Areas",
                        principalColumn: "AreaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Deliveries",
                columns: table => new
                {
                    DeliveryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsertedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    InsertedBy = table.Column<string>(maxLength: 256, nullable: true, defaultValueSql: "suser_sname()"),
                    ModifiedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    ModifiedBy = table.Column<string>(maxLength: 256, nullable: true, defaultValueSql: "suser_sname()"),
                    CartId = table.Column<string>(maxLength: 50, nullable: false),
                    OrderId = table.Column<string>(maxLength: 50, nullable: true),
                    Completed = table.Column<DateTime>(nullable: true),
                    Canceled = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<string>(maxLength: 256, nullable: false),
                    DeliveryType = table.Column<string>(maxLength: 50, nullable: false),
                    DestinationAreaId = table.Column<int>(nullable: false),
                    PreviousPrioritizedDeliveryId = table.Column<int>(nullable: true),
                    Queued = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deliveries", x => x.DeliveryId);
                    table.ForeignKey(
                        name: "FK_Deliveries_DeliveryTypes_DeliveryType",
                        column: x => x.DeliveryType,
                        principalTable: "DeliveryTypes",
                        principalColumn: "Value",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Deliveries_Areas_DestinationAreaId",
                        column: x => x.DestinationAreaId,
                        principalTable: "Areas",
                        principalColumn: "AreaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Deliveries_Deliveries_PreviousPrioritizedDeliveryId",
                        column: x => x.PreviousPrioritizedDeliveryId,
                        principalTable: "Deliveries",
                        principalColumn: "DeliveryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    JobId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsertedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    InsertedBy = table.Column<string>(maxLength: 256, nullable: true, defaultValueSql: "suser_sname()"),
                    ModifiedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    ModifiedBy = table.Column<string>(maxLength: 256, nullable: true, defaultValueSql: "suser_sname()"),
                    AethonJobId = table.Column<int>(nullable: false),
                    DeliveryId = table.Column<int>(nullable: false),
                    Completed = table.Column<DateTime>(nullable: true),
                    Canceled = table.Column<DateTime>(nullable: true),
                    Expired = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.JobId);
                    table.ForeignKey(
                        name: "FK_Jobs_Deliveries_DeliveryId",
                        column: x => x.DeliveryId,
                        principalTable: "Deliveries",
                        principalColumn: "DeliveryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    LocationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsertedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    InsertedBy = table.Column<string>(maxLength: 256, nullable: true, defaultValueSql: "suser_sname()"),
                    ModifiedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    ModifiedBy = table.Column<string>(maxLength: 256, nullable: true, defaultValueSql: "suser_sname()"),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    DeliveryId = table.Column<int>(nullable: true),
                    Reserved = table.Column<bool>(nullable: false, defaultValue: false),
                    LocationType = table.Column<string>(maxLength: 50, nullable: false),
                    AreaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.LocationId);
                    table.ForeignKey(
                        name: "FK_Locations_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "AreaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Locations_Deliveries_DeliveryId",
                        column: x => x.DeliveryId,
                        principalTable: "Deliveries",
                        principalColumn: "DeliveryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Locations_LocationTypes_LocationType",
                        column: x => x.LocationType,
                        principalTable: "LocationTypes",
                        principalColumn: "Value",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Itineraries",
                columns: table => new
                {
                    ItineraryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsertedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    InsertedBy = table.Column<string>(maxLength: 256, nullable: true, defaultValueSql: "suser_sname()"),
                    ModifiedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    ModifiedBy = table.Column<string>(maxLength: 256, nullable: true, defaultValueSql: "suser_sname()"),
                    AethonRunId = table.Column<int>(nullable: false),
                    Completed = table.Column<DateTime>(nullable: true),
                    TimedOut = table.Column<DateTime>(nullable: true),
                    JobId = table.Column<int>(nullable: false),
                    LocationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Itineraries", x => x.ItineraryId);
                    table.ForeignKey(
                        name: "FK_Itineraries_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "JobId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Itineraries_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ScheduledDelivery",
                columns: table => new
                {
                    ScheduledDeliveryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsertedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    InsertedBy = table.Column<string>(maxLength: 256, nullable: true, defaultValueSql: "suser_sname()"),
                    ModifiedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    ModifiedBy = table.Column<string>(maxLength: 256, nullable: true, defaultValueSql: "suser_sname()"),
                    TTL = table.Column<DateTime>(nullable: false),
                    Completed = table.Column<DateTime>(nullable: false),
                    Canceled = table.Column<DateTime>(nullable: false),
                    DeliveryId = table.Column<int>(nullable: false),
                    DestinationLocationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduledDelivery", x => x.ScheduledDeliveryId);
                    table.ForeignKey(
                        name: "FK_ScheduledDelivery_Deliveries_DeliveryId",
                        column: x => x.DeliveryId,
                        principalTable: "Deliveries",
                        principalColumn: "DeliveryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScheduledDelivery_Locations_DestinationLocationId",
                        column: x => x.DestinationLocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AreaTypes",
                columns: new[] { "Value", "Description" },
                values: new object[,]
                {
                    { "cw", null },
                    { "fpa", null },
                    { "mal", null },
                    { "smal", null },
                    { "stg", null }
                });

            migrationBuilder.InsertData(
                table: "DeliveryTypes",
                columns: new[] { "Value", "Description" },
                values: new object[,]
                {
                    { "return", null },
                    { "stage", null },
                    { "deliver", null },
                    { "manual", null }
                });

            migrationBuilder.InsertData(
                table: "LocationTypes",
                columns: new[] { "Value", "Description" },
                values: new object[,]
                {
                    { "cw", null },
                    { "fpa", null },
                    { "mal", null },
                    { "smal", null },
                    { "stg", null },
                    { "wait", null }
                });

            migrationBuilder.InsertData(
                table: "Pools",
                columns: new[] { "PoolId", "Name" },
                values: new object[,]
                {
                    { 2, "Pool 2" },
                    { 1, "Pool 1" },
                    { 3, "Pool 3" }
                });

            migrationBuilder.InsertData(
                table: "Areas",
                columns: new[] { "AreaId", "AreaType", "Name", "PoolId" },
                values: new object[,]
                {
                    { 1, "fpa", "101B-FPA", 1 },
                    { 2, "cw", "101C-CarWash", 1 },
                    { 3, "stg", "1430-Staging", 1 },
                    { 4, "mal", "1501-MAL-A", 1 },
                    { 5, "mal", "2501-MAL-A", 1 },
                    { 6, "mal", "1501-MAL-B", 2 },
                    { 8, "smal", "15xx-Suite MALs", 2 },
                    { 7, "mal", "2501-MAL-B", 3 },
                    { 9, "smal", "25xx-Suite MALs", 3 }
                });

            migrationBuilder.InsertData(
                table: "AreaMaps",
                columns: new[] { "NextAreaId", "PreviousAreaId", "Active" },
                values: new object[,]
                {
                    { 8, 6, true },
                    { 5, 1, true },
                    { 4, 3, true },
                    { 4, 2, true },
                    { 4, 1, true },
                    { 6, 4, true },
                    { 5, 2, true },
                    { 3, 2, true },
                    { 3, 1, true },
                    { 5, 3, true },
                    { 7, 5, true },
                    { 9, 7, true }
                });

            migrationBuilder.InsertData(
                table: "AreaOverflow",
                columns: new[] { "DestinationAreaId", "OverflowAreaId", "Active", "Priority" },
                values: new object[,]
                {
                    { 1, 3, true, 1 },
                    { 2, 3, true, 1 },
                    { 5, 3, true, 1 },
                    { 4, 3, true, 1 }
                });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "AreaId", "DeliveryId", "LocationType", "Name" },
                values: new object[] { 42, 9, null, "wait", "2519-MAL-WAIT-001" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "AreaId", "DeliveryId", "LocationType", "Name" },
                values: new object[] { 25, 5, null, "mal", "2501-MAL-A-001" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "AreaId", "DeliveryId", "LocationType", "Name" },
                values: new object[] { 26, 5, null, "wait", "2501-MA-A-WAIT-001" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "AreaId", "DeliveryId", "LocationType", "Name" },
                values: new object[] { 27, 5, null, "wait", "2501-MA-A-WAIT-002" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "AreaId", "DeliveryId", "LocationType", "Name" },
                values: new object[] { 41, 9, null, "smal", "2519-MAL-001" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "AreaId", "DeliveryId", "LocationType", "Name" },
                values: new object[] { 36, 7, null, "wait", "2501-MAL-B-WAIT-003" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "AreaId", "DeliveryId", "LocationType", "Name" },
                values: new object[] { 28, 5, null, "wait", "2501-MA-A-WAIT-003" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "AreaId", "DeliveryId", "LocationType", "Name" },
                values: new object[] { 35, 7, null, "wait", "2501-MAL-B-WAIT-002" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "AreaId", "DeliveryId", "LocationType", "Name" },
                values: new object[] { 29, 6, null, "mal", "1501-MAL-B-001" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "AreaId", "DeliveryId", "LocationType", "Name" },
                values: new object[] { 30, 6, null, "wait", "1501-MAL-B-WAIT-001" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "AreaId", "DeliveryId", "LocationType", "Name" },
                values: new object[] { 31, 6, null, "wait", "1501-MAL-B-WAIT-002" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "AreaId", "DeliveryId", "LocationType", "Name" },
                values: new object[] { 32, 6, null, "wait", "1501-MAL-B-WAIT-003" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "AreaId", "DeliveryId", "LocationType", "Name" },
                values: new object[] { 34, 7, null, "wait", "2501-MAL-B-WAIT-001" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "AreaId", "DeliveryId", "LocationType", "Name" },
                values: new object[] { 37, 8, null, "smal", "1528-MAL-001" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "AreaId", "DeliveryId", "LocationType", "Name" },
                values: new object[] { 38, 8, null, "wait", "1528-MAL-WAIT-001" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "AreaId", "DeliveryId", "LocationType", "Name" },
                values: new object[] { 39, 8, null, "wait", "1528-MAL-WAIT-002" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "AreaId", "DeliveryId", "LocationType", "Name" },
                values: new object[] { 33, 7, null, "mal", "2501-MAL-B-001" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "AreaId", "DeliveryId", "LocationType", "Name" },
                values: new object[] { 40, 8, null, "wait", "1528-MAL-WAIT-003" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "AreaId", "DeliveryId", "LocationType", "Name" },
                values: new object[] { 1, 1, null, "fpa", "101B-FPA-001" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "AreaId", "DeliveryId", "LocationType", "Name" },
                values: new object[] { 22, 4, null, "wait", "1501-MAL-A-WAIT-001" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "AreaId", "DeliveryId", "LocationType", "Name" },
                values: new object[] { 23, 4, null, "wait", "1501-MAL-A-WAIT-002" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "AreaId", "DeliveryId", "LocationType", "Name" },
                values: new object[] { 2, 1, null, "fpa", "101B-FPA-002" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "AreaId", "DeliveryId", "LocationType", "Name" },
                values: new object[] { 3, 1, null, "fpa", "101B-FPA-003" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "AreaId", "DeliveryId", "LocationType", "Name" },
                values: new object[] { 4, 1, null, "fpa", "101B-FPA-004" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "AreaId", "DeliveryId", "LocationType", "Name" },
                values: new object[] { 5, 1, null, "fpa", "101B-FPA-005" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "AreaId", "DeliveryId", "LocationType", "Name" },
                values: new object[] { 6, 2, null, "cw", "101C-CW-001" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "AreaId", "DeliveryId", "LocationType", "Name" },
                values: new object[] { 7, 2, null, "cw", "101C-CW-002" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "AreaId", "DeliveryId", "LocationType", "Name" },
                values: new object[] { 8, 2, null, "cw", "101C-CW-003" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "AreaId", "DeliveryId", "LocationType", "Name" },
                values: new object[] { 9, 2, null, "cw", "101C-CW-004" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "AreaId", "DeliveryId", "LocationType", "Name" },
                values: new object[] { 10, 2, null, "cw", "101C-CW-005" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "AreaId", "DeliveryId", "LocationType", "Name" },
                values: new object[] { 11, 3, null, "stg", "1430-STG-001" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "AreaId", "DeliveryId", "LocationType", "Name" },
                values: new object[] { 12, 3, null, "stg", "1430-STG-002" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "AreaId", "DeliveryId", "LocationType", "Name" },
                values: new object[] { 13, 3, null, "stg", "1430-STG-003" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "AreaId", "DeliveryId", "LocationType", "Name" },
                values: new object[] { 14, 3, null, "stg", "1430-STG-004" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "AreaId", "DeliveryId", "LocationType", "Name" },
                values: new object[] { 15, 3, null, "stg", "1430-STG-005" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "AreaId", "DeliveryId", "LocationType", "Name" },
                values: new object[] { 16, 3, null, "stg", "1430-STG-006" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "AreaId", "DeliveryId", "LocationType", "Name" },
                values: new object[] { 17, 3, null, "stg", "1430-STG-007" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "AreaId", "DeliveryId", "LocationType", "Name" },
                values: new object[] { 18, 3, null, "stg", "1430-STG-008" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "AreaId", "DeliveryId", "LocationType", "Name" },
                values: new object[] { 19, 3, null, "stg", "1430-STG-009" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "AreaId", "DeliveryId", "LocationType", "Name" },
                values: new object[] { 20, 3, null, "stg", "1430-STG-010" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "AreaId", "DeliveryId", "LocationType", "Name" },
                values: new object[] { 21, 4, null, "mal", "1501-MAL-A-001" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "AreaId", "DeliveryId", "LocationType", "Name" },
                values: new object[] { 43, 9, null, "wait", "2519-MAL-WAIT-002" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "AreaId", "DeliveryId", "LocationType", "Name" },
                values: new object[] { 24, 4, null, "wait", "1501-MAL-A-WAIT-003" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "AreaId", "DeliveryId", "LocationType", "Name" },
                values: new object[] { 44, 9, null, "wait", "2519-MAL-WAIT-003" });

            migrationBuilder.CreateIndex(
                name: "IX_AreaDeliveryTypes_DeliveryType",
                table: "AreaDeliveryTypes",
                column: "DeliveryType");

            migrationBuilder.CreateIndex(
                name: "IX_AreaMaps_PreviousAreaId",
                table: "AreaMaps",
                column: "PreviousAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_AreaOverflow_OverflowAreaId",
                table: "AreaOverflow",
                column: "OverflowAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Areas_AreaType",
                table: "Areas",
                column: "AreaType");

            migrationBuilder.CreateIndex(
                name: "IX_Areas_PoolId",
                table: "Areas",
                column: "PoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_DeliveryType",
                table: "Deliveries",
                column: "DeliveryType");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_DestinationAreaId",
                table: "Deliveries",
                column: "DestinationAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_PreviousPrioritizedDeliveryId",
                table: "Deliveries",
                column: "PreviousPrioritizedDeliveryId",
                unique: true,
                filter: "[PreviousPrioritizedDeliveryId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Itineraries_JobId",
                table: "Itineraries",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_Itineraries_LocationId",
                table: "Itineraries",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_DeliveryId",
                table: "Jobs",
                column: "DeliveryId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_AreaId",
                table: "Locations",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_DeliveryId",
                table: "Locations",
                column: "DeliveryId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_LocationType",
                table: "Locations",
                column: "LocationType");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledDelivery_DeliveryId",
                table: "ScheduledDelivery",
                column: "DeliveryId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledDelivery_DestinationLocationId",
                table: "ScheduledDelivery",
                column: "DestinationLocationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AreaDeliveryTypes");

            migrationBuilder.DropTable(
                name: "AreaMaps");

            migrationBuilder.DropTable(
                name: "AreaOverflow");

            migrationBuilder.DropTable(
                name: "DevLogs");

            migrationBuilder.DropTable(
                name: "Itineraries");

            migrationBuilder.DropTable(
                name: "ScheduledDelivery");

            migrationBuilder.DropTable(
                name: "UserLogs");

            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Deliveries");

            migrationBuilder.DropTable(
                name: "LocationTypes");

            migrationBuilder.DropTable(
                name: "DeliveryTypes");

            migrationBuilder.DropTable(
                name: "Areas");

            migrationBuilder.DropTable(
                name: "AreaTypes");

            migrationBuilder.DropTable(
                name: "Pools");
        }
    }
}
