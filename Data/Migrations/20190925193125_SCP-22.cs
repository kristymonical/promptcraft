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
                    DestinationAreaId = table.Column<int>(nullable: false)
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
                    AethonJobId = table.Column<int>(nullable: true),
                    State = table.Column<string>(maxLength: 50, nullable: false),
                    DeliveryId = table.Column<int>(nullable: false)
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

            migrationBuilder.Sql(@"
                CREATE OR ALTER PROCEDURE [dbo].[usp_getAvailableAncestorLocation]
                    @currentAreaName			NVARCHAR(50)
                    , @destinationAreaName		NVARCHAR(50)
                AS
                BEGIN
                    SET XACT_ABORT, NOCOUNT ON;

                    DECLARE
                        @currentAreaNode		HIERARCHYID
                        , @destinationAreaNode	HIERARCHYID;

                    BEGIN TRY
                        select
                            @currentAreaNode = ah.Node
                        from
                            dbo.Areas a with(nolock)
                            join dbo.AreaHierarchy ah with(nolock) on a.AreaId = ah.AreaId
                        where
                            1=1
                            and ah.NodeLevel = 1
                            and a.[Name] = @currentAreaName;

                        select
                            @destinationAreaNode = ah.Node
                        from
                            dbo.AreaHierarchy ah with(nolock)
                            join dbo.Areas a with(nolock) on a.AreaId = ah.AreaId
                        where
                            1=1
                            and a.Name = @destinationAreaName
                            and ah.Node.IsDescendantOf(@currentAreaNode) = 1
                            and ah.NodeLevel = 2
                        order by
                            ah.Node;

                        select top 1
                            l.*
                        from
                            dbo.AreaHierarchy ah with(nolock)
                            join dbo.Areas a with(nolock) on a.AreaId = ah.AreaId
                            join dbo.Locations l with(nolock) on a.AreaId = l.AreaId
                        where
                            1=1
                            and ah.Node <> @destinationAreaNode
                            and ah.Node.IsDescendantOf(@destinationAreaNode) = 1
                            and l.DeliveryId is null
                        order by
                            ah.Node;
                    END TRY
                    BEGIN CATCH
                        ;THROW
                    END CATCH
                END
                GO
            ");

            migrationBuilder.Sql(@"
                CREATE OR ALTER PROCEDURE [dbo].[usp_cart_move]
                    @AreaId             INT             = NULL
                    , @CartId           NVARCHAR(50)	= NULL
                    , @DeliveryId		INT				= -1
                    , @DeliveryType		NVARCHAR(50)    = NULL
                    , @DestinationId    INT				= -1
                    , @OrderId			NVARCHAR(50)	= NULL
                    , @Reserved			BIT				= 0
                    , @SourceId         INT				= -1
                    , @User             NVARCHAR(256)
                    , @NewDeliveryId    INT				= -1    OUTPUT
                AS
                BEGIN
                    SET XACT_ABORT, NOCOUNT ON;

                    DECLARE
                        @OutputTable TABLE(NewDeliveryId INT);
                    
                    DECLARE
                        @Completed		DATETIME2 = NULL
                        , @Delivery		INT
                        , @TranCount	INT;

                    BEGIN TRY
                        SELECT @TranCount = @@TRANCOUNT;

                        IF @TranCount = 0 BEGIN TRANSACTION;

                        IF (@DestinationId = -1 AND @SourceId = -1)
                            THROW 55555, N'A Valid @Destination and/or @SourceId Required', 1;

                        IF (@DestinationId <> -1 AND @Reserved = 0)
                            SELECT
                                @Completed = CASE
                                    WHEN l.AreaId = d.DestinationAreaId THEN GETDATE()
                                    ELSE NULL
                                END
                            FROM
                                dbo.Locations l
                                JOIN dbo.Deliveries d ON l.LocationId = @DestinationId AND d.DeliveryId = l.DeliveryId;

                        SET @Delivery = @DeliveryId;

                        IF @Delivery = -1
                        BEGIN
                            IF (@AreaId IS NULL OR @CartId IS NULL OR @DeliveryType IS NULL)
                                THROW 55555, N'Following Fields Required For New Delivery: @AreadId, @CartId, @DeliveryType, @User', 1;

                            INSERT INTO dbo.Deliveries
                                (CartId, OrderId, Completed, UserId, DeliveryType, DestinationAreaId)
                            OUTPUT
                                INSERTED.DeliveryId
                            INTO
                                @OutputTable
                            VALUES
                                (@CartId, @OrderId, @Completed, @User, @DeliveryType, @AreaId);

                            IF @@ROWCOUNT <> 1
                                THROW 55555, N'Failed to Create Delivery', 1;

                            SELECT @Delivery = NewDeliveryId FROM @OutputTable;

                            SET @NewDeliveryId = @Delivery;
                        END
                        ELSE
                        BEGIN
                            IF @Completed IS NOT NULL
                            BEGIN
                                UPDATE dbo.Deliveries
                                SET
                                    Completed = @Completed
                                    , ModifiedBy = @User
                                    , ModifiedOn = GETDATE()
                                WHERE
                                    DeliveryId = @Delivery;

                                IF @@ROWCOUNT <> 1
                                    THROW 55555, N'Failed to Update Delivery Completion', 1;
                            END
                        END

                        IF @SourceId <> -1
                        BEGIN
                            ;WITH locDel AS (
                                SELECT
                                    l.LocationId
                                    , d.Completed [previousDeliveryComplete]
                                FROM
                                    dbo.Locations l
                                    JOIN dbo.Deliveries d ON l.LocationId = @SourceId AND d.DeliveryId = l.DeliveryId					
                            )
                            UPDATE srcLoc
                            SET
                                DeliveryId = CASE
                                    WHEN DeliveryId = @DeliveryId THEN NULL
                                    ELSE @Delivery
                                END
                                , ModifiedBy = @User
                                , ModifiedOn = GETDATE()
                                , Reserved = 0
                            FROM
                                dbo.Locations srcLoc
                                JOIN locDel ON srcLoc.LocationId = locDel.LocationId
                            WHERE
                                srcLoc.LocationId = @SourceId
                                AND (srcLoc.DeliveryId = @Delivery OR srcLoc.DeliveryId IS NULL OR locDel.previousDeliveryComplete IS NOT NULL);

                            IF @@ROWCOUNT <> 1
                                THROW 55555, N'Failed to Update Source Cart Location', 1;
                        END
                        
                        IF @DestinationId <> -1
                        BEGIN

                            UPDATE dbo.Locations
                            SET
                                DeliveryId = @Delivery
                                , ModifiedBy = @User
                                , ModifiedOn = GETDATE()
                                , Reserved = @Reserved
                            WHERE
                                LocationId = @DestinationId
                                AND (DeliveryId IS NULL OR DeliveryId = @Delivery);

                            IF @@ROWCOUNT <> 1
                                THROW 55555, N'Failed to Update Destination Cart Location', 1;
                        END

                        IF @TranCount = 0 COMMIT TRANSACTION;
                    END TRY
                    BEGIN CATCH
                        IF XACT_STATE() <> 0 AND @TranCount = 0 
                            ROLLBACK TRANSACTION;
                        THROW;
                    END CATCH
                END
                GO
            ");

            migrationBuilder.Sql(@"
                ALTER TABLE [dbo].[Deliveries] ADD  CONSTRAINT [DF_Deliveries_InsertedBy]  DEFAULT (suser_sname()) FOR [InsertedBy]
                GO
                ALTER TABLE [dbo].[Deliveries] ADD  CONSTRAINT [DF_Deliveries_InsertedOn]  DEFAULT (getdate()) FOR [InsertedOn]
                GO
                ALTER TABLE [dbo].[Deliveries] ADD  CONSTRAINT [DF_Deliveries_ModifiedBy]  DEFAULT (suser_sname()) FOR [ModifiedBy]
                GO
                ALTER TABLE [dbo].[Deliveries] ADD  CONSTRAINT [DF_Deliveries_ModifiedOn]  DEFAULT (getdate()) FOR [ModifiedOn]
                GO

                INSERT [dbo].[AreaTypes] ([Value], [Description]) VALUES (N'cw', NULL)
                GO
                INSERT [dbo].[AreaTypes] ([Value], [Description]) VALUES (N'fpa', NULL)
                GO
                INSERT [dbo].[AreaTypes] ([Value], [Description]) VALUES (N'mal', NULL)
                GO
                INSERT [dbo].[AreaTypes] ([Value], [Description]) VALUES (N'smal', NULL)
                GO
                INSERT [dbo].[AreaTypes] ([Value], [Description]) VALUES (N'stg', NULL)
                GO
                SET IDENTITY_INSERT [dbo].[Pools] ON 
                GO
                INSERT [dbo].[Pools] ([PoolId], [Name]) VALUES (1, N'Pool 1')
                GO
                INSERT [dbo].[Pools] ([PoolId], [Name]) VALUES (2, N'Pool 2')
                GO
                INSERT [dbo].[Pools] ([PoolId], [Name]) VALUES (3, N'Pool 3')
                GO
                SET IDENTITY_INSERT [dbo].[Pools] OFF
                GO
                SET IDENTITY_INSERT [dbo].[Areas] ON 
                GO
                INSERT [dbo].[Areas] ([AreaId], [Name], [AreaType], [PoolId]) VALUES (1, N'101B-FPA', N'fpa', 1)
                GO
                INSERT [dbo].[Areas] ([AreaId], [Name], [AreaType], [PoolId]) VALUES (2, N'101C-CarWash', N'cw', 1)
                GO
                INSERT [dbo].[Areas] ([AreaId], [Name], [AreaType], [PoolId]) VALUES (3, N'1430-Staging', N'stg', 1)
                GO
                INSERT [dbo].[Areas] ([AreaId], [Name], [AreaType], [PoolId]) VALUES (4, N'1501-MAL-A', N'mal', 1)
                GO
                INSERT [dbo].[Areas] ([AreaId], [Name], [AreaType], [PoolId]) VALUES (5, N'2501-MAL-A', N'mal', 1)
                GO
                INSERT [dbo].[Areas] ([AreaId], [Name], [AreaType], [PoolId]) VALUES (6, N'1501-MAL-B', N'mal', 2)
                GO
                INSERT [dbo].[Areas] ([AreaId], [Name], [AreaType], [PoolId]) VALUES (7, N'2501-MAL-B', N'mal', 3)
                GO
                INSERT [dbo].[Areas] ([AreaId], [Name], [AreaType], [PoolId]) VALUES (8, N'15xx-Suite MALs', N'smal', 2)
                GO
                INSERT [dbo].[Areas] ([AreaId], [Name], [AreaType], [PoolId]) VALUES (9, N'25xx-Suite MALs', N'smal', 3)
                GO
                SET IDENTITY_INSERT [dbo].[Areas] OFF
                GO
                INSERT [dbo].[AreaHierarchy] ([Node], [AreaId]) VALUES (N'/1/', 1)
                GO
                INSERT [dbo].[AreaHierarchy] ([Node], [AreaId]) VALUES (N'/1/1/', 8)
                GO
                INSERT [dbo].[AreaHierarchy] ([Node], [AreaId]) VALUES (N'/1/1/1/', 4)
                GO
                INSERT [dbo].[AreaHierarchy] ([Node], [AreaId]) VALUES (N'/1/1/2/', 3)
                GO
                INSERT [dbo].[AreaHierarchy] ([Node], [AreaId]) VALUES (N'/1/2/', 9)
                GO
                INSERT [dbo].[AreaHierarchy] ([Node], [AreaId]) VALUES (N'/1/2/1/', 5)
                GO
                INSERT [dbo].[AreaHierarchy] ([Node], [AreaId]) VALUES (N'/1/2/2/', 3)
                GO
                INSERT [dbo].[AreaHierarchy] ([Node], [AreaId]) VALUES (N'/1/3/', 3)
                GO
                INSERT [dbo].[AreaHierarchy] ([Node], [AreaId]) VALUES (N'/2/', 2)
                GO
                INSERT [dbo].[AreaHierarchy] ([Node], [AreaId]) VALUES (N'/2/1/', 8)
                GO
                INSERT [dbo].[AreaHierarchy] ([Node], [AreaId]) VALUES (N'/2/1/1/', 4)
                GO
                INSERT [dbo].[AreaHierarchy] ([Node], [AreaId]) VALUES (N'/2/1/2/', 3)
                GO
                INSERT [dbo].[AreaHierarchy] ([Node], [AreaId]) VALUES (N'/2/2/', 9)
                GO
                INSERT [dbo].[AreaHierarchy] ([Node], [AreaId]) VALUES (N'/2/2/1/', 5)
                GO
                INSERT [dbo].[AreaHierarchy] ([Node], [AreaId]) VALUES (N'/2/2/2/', 3)
                GO
                INSERT [dbo].[AreaHierarchy] ([Node], [AreaId]) VALUES (N'/2/3/', 3)
                GO
                INSERT [dbo].[AreaHierarchy] ([Node], [AreaId]) VALUES (N'/3/', 8)
                GO
                INSERT [dbo].[AreaHierarchy] ([Node], [AreaId]) VALUES (N'/3/1/', 1)
                GO
                INSERT [dbo].[AreaHierarchy] ([Node], [AreaId]) VALUES (N'/3/1/1/', 6)
                GO
                INSERT [dbo].[AreaHierarchy] ([Node], [AreaId]) VALUES (N'/3/2/', 2)
                GO
                INSERT [dbo].[AreaHierarchy] ([Node], [AreaId]) VALUES (N'/3/2/1/', 6)
                GO
                INSERT [dbo].[AreaHierarchy] ([Node], [AreaId]) VALUES (N'/4/', 9)
                GO
                INSERT [dbo].[AreaHierarchy] ([Node], [AreaId]) VALUES (N'/4/1/', 1)
                GO
                INSERT [dbo].[AreaHierarchy] ([Node], [AreaId]) VALUES (N'/4/1/1/', 7)
                GO
                INSERT [dbo].[AreaHierarchy] ([Node], [AreaId]) VALUES (N'/4/2/', 2)
                GO
                INSERT [dbo].[AreaHierarchy] ([Node], [AreaId]) VALUES (N'/4/2/1/', 7)
                GO
                INSERT [dbo].[AreaMaps] ([SourceAreaId], [DestinationAreaId]) VALUES (1, 3)
                GO
                INSERT [dbo].[AreaMaps] ([SourceAreaId], [DestinationAreaId]) VALUES (1, 4)
                GO
                INSERT [dbo].[AreaMaps] ([SourceAreaId], [DestinationAreaId]) VALUES (1, 5)
                GO
                INSERT [dbo].[AreaMaps] ([SourceAreaId], [DestinationAreaId]) VALUES (3, 4)
                GO
                INSERT [dbo].[AreaMaps] ([SourceAreaId], [DestinationAreaId]) VALUES (3, 5)
                GO
                INSERT [dbo].[AreaMaps] ([SourceAreaId], [DestinationAreaId]) VALUES (4, 8)
                GO
                INSERT [dbo].[AreaMaps] ([SourceAreaId], [DestinationAreaId]) VALUES (5, 9)
                GO
                INSERT [dbo].[DeliveryTypes] ([Value], [Description]) VALUES (N'deliver', NULL)
                GO
                INSERT [dbo].[DeliveryTypes] ([Value], [Description]) VALUES (N'manual', NULL)
                GO
                INSERT [dbo].[DeliveryTypes] ([Value], [Description]) VALUES (N'return', NULL)
                GO
                INSERT [dbo].[DeliveryTypes] ([Value], [Description]) VALUES (N'stage', NULL)
                GO
                INSERT [dbo].[LocationTypes] ([Value], [Description]) VALUES (N'cw', NULL)
                GO
                INSERT [dbo].[LocationTypes] ([Value], [Description]) VALUES (N'fpa', NULL)
                GO
                INSERT [dbo].[LocationTypes] ([Value], [Description]) VALUES (N'mal', NULL)
                GO
                INSERT [dbo].[LocationTypes] ([Value], [Description]) VALUES (N'smal', NULL)
                GO
                INSERT [dbo].[LocationTypes] ([Value], [Description]) VALUES (N'stg', NULL)
                GO
                INSERT [dbo].[LocationTypes] ([Value], [Description]) VALUES (N'wait', NULL)
                GO
                SET IDENTITY_INSERT [dbo].[Locations] ON 
                GO
                INSERT [dbo].[Locations] ([LocationId], [Name], [DeliveryId], [Reserved], [InsertedBy], [InsertedOn], [ModifiedBy], [ModifiedOn], [LocationType], [AreaId]) VALUES (3, N'101B-FPA-001', NULL, 0, N'sa', CAST(N'2019-09-13T17:11:47.8666667' AS DateTime2), N'sa', CAST(N'2019-09-13T17:11:47.8666667' AS DateTime2), N'fpa', 1)
                GO
                INSERT [dbo].[Locations] ([LocationId], [Name], [DeliveryId], [Reserved], [InsertedBy], [InsertedOn], [ModifiedBy], [ModifiedOn], [LocationType], [AreaId]) VALUES (4, N'101B-FPA-002', NULL, 0, N'sa', CAST(N'2019-09-13T17:11:47.8666667' AS DateTime2), N'sa', CAST(N'2019-09-24T17:57:51.9666667' AS DateTime2), N'fpa', 1)
                GO
                INSERT [dbo].[Locations] ([LocationId], [Name], [DeliveryId], [Reserved], [InsertedBy], [InsertedOn], [ModifiedBy], [ModifiedOn], [LocationType], [AreaId]) VALUES (5, N'101C-CW-001', NULL, 0, N'sa', CAST(N'2019-09-13T17:11:47.8666667' AS DateTime2), N'sa', CAST(N'2019-09-13T17:11:47.8666667' AS DateTime2), N'cw', 2)
                GO
                INSERT [dbo].[Locations] ([LocationId], [Name], [DeliveryId], [Reserved], [InsertedBy], [InsertedOn], [ModifiedBy], [ModifiedOn], [LocationType], [AreaId]) VALUES (6, N'101C-CW-002', NULL, 0, N'sa', CAST(N'2019-09-13T17:11:47.8666667' AS DateTime2), N'sa', CAST(N'2019-09-13T17:11:47.8666667' AS DateTime2), N'cw', 2)
                GO
                INSERT [dbo].[Locations] ([LocationId], [Name], [DeliveryId], [Reserved], [InsertedBy], [InsertedOn], [ModifiedBy], [ModifiedOn], [LocationType], [AreaId]) VALUES (7, N'1430-STG-001', NULL, 0, N'sa', CAST(N'2019-09-13T17:11:47.8666667' AS DateTime2), N'sa', CAST(N'2019-09-13T17:11:47.8666667' AS DateTime2), N'stg', 3)
                GO
                INSERT [dbo].[Locations] ([LocationId], [Name], [DeliveryId], [Reserved], [InsertedBy], [InsertedOn], [ModifiedBy], [ModifiedOn], [LocationType], [AreaId]) VALUES (8, N'1430-STG-002', NULL, 0, N'sa', CAST(N'2019-09-13T17:11:47.8666667' AS DateTime2), N'sa', CAST(N'2019-09-13T17:11:47.8666667' AS DateTime2), N'stg', 3)
                GO
                INSERT [dbo].[Locations] ([LocationId], [Name], [DeliveryId], [Reserved], [InsertedBy], [InsertedOn], [ModifiedBy], [ModifiedOn], [LocationType], [AreaId]) VALUES (9, N'1501-MAL-A-001', NULL, 0, N'sa', CAST(N'2019-09-13T17:11:47.8666667' AS DateTime2), N'sa', CAST(N'2019-09-13T17:11:47.8666667' AS DateTime2), N'mal', 4)
                GO
                INSERT [dbo].[Locations] ([LocationId], [Name], [DeliveryId], [Reserved], [InsertedBy], [InsertedOn], [ModifiedBy], [ModifiedOn], [LocationType], [AreaId]) VALUES (10, N'1501-MAL-A-WAIT-001', NULL, 0, N'sa', CAST(N'2019-09-13T17:11:47.8666667' AS DateTime2), N'sa', CAST(N'2019-09-13T17:11:47.8666667' AS DateTime2), N'wait', 4)
                GO
                INSERT [dbo].[Locations] ([LocationId], [Name], [DeliveryId], [Reserved], [InsertedBy], [InsertedOn], [ModifiedBy], [ModifiedOn], [LocationType], [AreaId]) VALUES (11, N'2501-MAL-A-001', NULL, 0, N'sa', CAST(N'2019-09-13T17:11:47.8666667' AS DateTime2), N'sa', CAST(N'2019-09-13T17:11:47.8666667' AS DateTime2), N'mal', 5)
                GO
                INSERT [dbo].[Locations] ([LocationId], [Name], [DeliveryId], [Reserved], [InsertedBy], [InsertedOn], [ModifiedBy], [ModifiedOn], [LocationType], [AreaId]) VALUES (12, N'2501-MAL-A-WAIT-001', NULL, 0, N'sa', CAST(N'2019-09-13T17:11:47.8666667' AS DateTime2), N'sa', CAST(N'2019-09-13T17:11:47.8666667' AS DateTime2), N'wait', 5)
                GO
                INSERT [dbo].[Locations] ([LocationId], [Name], [DeliveryId], [Reserved], [InsertedBy], [InsertedOn], [ModifiedBy], [ModifiedOn], [LocationType], [AreaId]) VALUES (13, N'1501-MAL-B-001', NULL, 0, N'sa', CAST(N'2019-09-13T17:11:47.8666667' AS DateTime2), N'sa', CAST(N'2019-09-13T17:11:47.8666667' AS DateTime2), N'mal', 6)
                GO
                INSERT [dbo].[Locations] ([LocationId], [Name], [DeliveryId], [Reserved], [InsertedBy], [InsertedOn], [ModifiedBy], [ModifiedOn], [LocationType], [AreaId]) VALUES (14, N'1501-MAL-B-WAIT-001', NULL, 0, N'sa', CAST(N'2019-09-13T17:11:47.8666667' AS DateTime2), N'sa', CAST(N'2019-09-13T17:11:47.8666667' AS DateTime2), N'wait', 6)
                GO
                INSERT [dbo].[Locations] ([LocationId], [Name], [DeliveryId], [Reserved], [InsertedBy], [InsertedOn], [ModifiedBy], [ModifiedOn], [LocationType], [AreaId]) VALUES (15, N'2501-MAL-B-001', NULL, 0, N'sa', CAST(N'2019-09-13T17:11:47.8666667' AS DateTime2), N'sa', CAST(N'2019-09-13T17:11:47.8666667' AS DateTime2), N'mal', 7)
                GO
                INSERT [dbo].[Locations] ([LocationId], [Name], [DeliveryId], [Reserved], [InsertedBy], [InsertedOn], [ModifiedBy], [ModifiedOn], [LocationType], [AreaId]) VALUES (16, N'2501-MAL-B-WAIT-001', NULL, 0, N'sa', CAST(N'2019-09-13T17:11:47.8666667' AS DateTime2), N'sa', CAST(N'2019-09-13T17:11:47.8666667' AS DateTime2), N'wait', 7)
                GO
                INSERT [dbo].[Locations] ([LocationId], [Name], [DeliveryId], [Reserved], [InsertedBy], [InsertedOn], [ModifiedBy], [ModifiedOn], [LocationType], [AreaId]) VALUES (17, N'1528-MAL-001', NULL, 0, N'sa', CAST(N'2019-09-13T17:11:47.8666667' AS DateTime2), N'sa', CAST(N'2019-09-13T17:11:47.8666667' AS DateTime2), N'smal', 8)
                GO
                INSERT [dbo].[Locations] ([LocationId], [Name], [DeliveryId], [Reserved], [InsertedBy], [InsertedOn], [ModifiedBy], [ModifiedOn], [LocationType], [AreaId]) VALUES (18, N'1528-MAL-WAIT-001', NULL, 0, N'sa', CAST(N'2019-09-13T17:11:47.8666667' AS DateTime2), N'sa', CAST(N'2019-09-13T17:11:47.8666667' AS DateTime2), N'wait', 8)
                GO
                INSERT [dbo].[Locations] ([LocationId], [Name], [DeliveryId], [Reserved], [InsertedBy], [InsertedOn], [ModifiedBy], [ModifiedOn], [LocationType], [AreaId]) VALUES (19, N'2519-MAL-001', NULL, 0, N'sa', CAST(N'2019-09-13T17:11:47.8666667' AS DateTime2), N'sa', CAST(N'2019-09-13T17:11:47.8666667' AS DateTime2), N'smal', 9)
                GO
                INSERT [dbo].[Locations] ([LocationId], [Name], [DeliveryId], [Reserved], [InsertedBy], [InsertedOn], [ModifiedBy], [ModifiedOn], [LocationType], [AreaId]) VALUES (20, N'2519-MAL-WAIT-001', NULL, 0, N'sa', CAST(N'2019-09-13T17:11:47.8666667' AS DateTime2), N'sa', CAST(N'2019-09-13T17:11:47.8666667' AS DateTime2), N'wait', 9)
                GO
                SET IDENTITY_INSERT [dbo].[Locations] OFF
                GO
            ");
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
