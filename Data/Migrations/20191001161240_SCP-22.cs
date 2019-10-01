using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.SqlServer.Types;

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
                name: "AreaHierarchy",
                columns: table => new
                {
                    Node = table.Column<SqlHierarchyId>(nullable: false),
                    NodeLevel = table.Column<short>(nullable: true, computedColumnSql: "[Node].[GetLevel]()"),
                    AreaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AreaHierarchy", x => x.Node);
                    table.ForeignKey(
                        name: "FK_AreaHierarchy_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "AreaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AreaMaps",
                columns: table => new
                {
                    SourceAreaId = table.Column<int>(nullable: false),
                    DestinationAreaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AreaMaps", x => new { x.DestinationAreaId, x.SourceAreaId });
                    table.ForeignKey(
                        name: "FK_AreaMaps_Areas_DestinationAreaId",
                        column: x => x.DestinationAreaId,
                        principalTable: "Areas",
                        principalColumn: "AreaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AreaMaps_Areas_SourceAreaId",
                        column: x => x.SourceAreaId,
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
                    InsertedOn = table.Column<DateTime>(nullable: false),
                    InsertedBy = table.Column<string>(maxLength: 256, nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 256, nullable: false),
                    CartId = table.Column<string>(maxLength: 50, nullable: false),
                    OrderId = table.Column<string>(maxLength: 50, nullable: true),
                    Completed = table.Column<DateTime>(nullable: true),
                    Canceled = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<string>(maxLength: 256, nullable: false),
                    DeliveryType = table.Column<string>(maxLength: 50, nullable: false),
                    DestinationAreaId = table.Column<int>(nullable: false),
                    PreviousPrioritizedDeliveryId = table.Column<int>(nullable: true)
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
                    InsertedOn = table.Column<DateTime>(nullable: false),
                    InsertedBy = table.Column<string>(maxLength: 256, nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 256, nullable: false),
                    AethonJobId = table.Column<int>(nullable: false),
                    State = table.Column<string>(maxLength: 50, nullable: false),
                    DeliveryId = table.Column<int>(nullable: false),
                    Completed = table.Column<DateTime>(nullable: false),
                    Canceled = table.Column<DateTime>(nullable: false)
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
                    InsertedOn = table.Column<DateTime>(nullable: false),
                    InsertedBy = table.Column<string>(maxLength: 256, nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 256, nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    DeliveryId = table.Column<int>(nullable: true),
                    Reserved = table.Column<bool>(nullable: false),
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
                    InsertedOn = table.Column<DateTime>(nullable: false),
                    InsertedBy = table.Column<string>(maxLength: 256, nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 256, nullable: false),
                    AethonRunId = table.Column<int>(nullable: true),
                    State = table.Column<string>(maxLength: 50, nullable: false),
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
                    InsertedOn = table.Column<DateTime>(nullable: false),
                    InsertedBy = table.Column<string>(maxLength: 256, nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 256, nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_AreaDeliveryTypes_DeliveryType",
                table: "AreaDeliveryTypes",
                column: "DeliveryType");

            migrationBuilder.CreateIndex(
                name: "IX_AreaHierarchy_AreaId",
                table: "AreaHierarchy",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_AreaMaps_SourceAreaId",
                table: "AreaMaps",
                column: "SourceAreaId");

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
                name: "AreaHierarchy");

            migrationBuilder.DropTable(
                name: "AreaMaps");

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
