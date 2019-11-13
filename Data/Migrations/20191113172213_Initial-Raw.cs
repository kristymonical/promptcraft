using Microsoft.EntityFrameworkCore.Migrations;

namespace SVT.Platform.Data.Migrations
{
    public partial class InitialRaw : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Deliveries After INSERT Trigger
            migrationBuilder.Sql(@"
                CREATE TRIGGER TRG_Deliveries_After_Insert
                    ON dbo.Deliveries
                    FOR INSERT
                    AS
                    BEGIN
                        SET NOCOUNT ON

                        UPDATE dbo.Deliveries
                            SET InsertedOn = GETUTCDATE(),
                                ModifiedOn = GETUTCDATE()
                        FROM dbo.Deliveries INNER JOIN inserted
                            ON dbo.Deliveries.DeliveryId = inserted.DeliveryId;
                    END");

            // Deliveries After UPDATE Trigger
            migrationBuilder.Sql(@"
                CREATE TRIGGER TRG_Deliveries_After_Update
                    ON dbo.Deliveries
                    FOR UPDATE
                    AS
                    BEGIN
                        SET NOCOUNT ON

                        IF ( (SELECT trigger_nestlevel() ) > 1 )
                            RETURN

                        UPDATE dbo.Deliveries
                            SET InsertedOn = deleted.InsertedOn,
                                ModifiedOn = GETUTCDATE()
                        FROM dbo.Deliveries INNER JOIN deleted
                            ON dbo.Deliveries.DeliveryId = deleted.DeliveryId;
                    END");

            // Itineraries After INSERT Trigger
            migrationBuilder.Sql(@"
                CREATE TRIGGER TRG_Itineraries_After_Insert
                    ON dbo.Itineraries
                    FOR INSERT
                    AS
                    BEGIN
                        SET NOCOUNT ON

                        UPDATE dbo.Itineraries
                            SET InsertedOn = GETUTCDATE(),
                                ModifiedOn = GETUTCDATE()
                        FROM dbo.Itineraries INNER JOIN inserted
                            ON dbo.Itineraries.ItineraryId = inserted.ItineraryId;
                    END
                GO");

            // Itineraries After UPDATE Trigger
            migrationBuilder.Sql(@"
                CREATE TRIGGER TRG_Itineraries_After_Update
                    ON dbo.Itineraries
                    FOR UPDATE
                    AS
                    BEGIN
                        SET NOCOUNT ON

                        IF ( (SELECT trigger_nestlevel() ) > 1 )
                            RETURN

                        UPDATE dbo.Itineraries
                            SET InsertedOn = deleted.InsertedOn,
                                ModifiedOn = GETUTCDATE()
                        FROM dbo.Itineraries INNER JOIN deleted
                            ON dbo.Itineraries.ItineraryId = deleted.ItineraryId;
                    END
                GO");

            // Jobs After INSERT Trigger
            migrationBuilder.Sql(@"
                CREATE TRIGGER TRG_Jobs_After_Insert
                    ON dbo.Jobs
                    FOR INSERT
                    AS
                    BEGIN
                        SET NOCOUNT ON

                        UPDATE dbo.Jobs
                            SET InsertedOn = GETUTCDATE(),
                                ModifiedOn = GETUTCDATE()
                        FROM dbo.Jobs INNER JOIN inserted
                            ON dbo.Jobs.JobId = inserted.JobId;
                    END
                GO");

            // Jobs After UPDATE Trigger
            migrationBuilder.Sql(@"
                CREATE TRIGGER TRG_Jobs_After_Update
                    ON dbo.Jobs
                    FOR UPDATE
                    AS
                    BEGIN
                        SET NOCOUNT ON

                        IF ( (SELECT trigger_nestlevel() ) > 1 )
                            RETURN

                        UPDATE dbo.Jobs
                            SET InsertedOn = deleted.InsertedOn,
                                ModifiedOn = GETUTCDATE()
                        FROM dbo.Jobs INNER JOIN deleted
                            ON dbo.Jobs.JobId = deleted.JobId;
                    END
                GO");

            // Locations After INSERT Trigger
            migrationBuilder.Sql(@"
                CREATE TRIGGER TRG_Locations_After_Insert
                    ON dbo.Locations
                    FOR INSERT
                    AS
                    BEGIN
                        SET NOCOUNT ON

                        UPDATE dbo.Locations
                            SET InsertedOn = GETUTCDATE(),
                                ModifiedOn = GETUTCDATE()
                        FROM dbo.Locations INNER JOIN inserted
                            ON dbo.Locations.LocationId = inserted.LocationId;
                    END
                GO");

            // Locations After UPDATE Trigger
            migrationBuilder.Sql(@"
                CREATE TRIGGER TRG_Locations_After_Update
                    ON dbo.Locations
                    FOR UPDATE
                    AS
                    BEGIN
                        SET NOCOUNT ON

                        IF ( (SELECT trigger_nestlevel() ) > 1 )
                                RETURN

                        UPDATE dbo.Locations
                            SET InsertedOn = deleted.InsertedOn,
                                    ModifiedOn = GETUTCDATE()
                        FROM dbo.Locations INNER JOIN deleted
                            ON dbo.Locations.LocationId = deleted.LocationId;
                    END
                GO");

            // Logs After INSERT Trigger
            migrationBuilder.Sql(@"
                CREATE TRIGGER TRG_Logs_After_Insert
                    ON dbo.Logs
                    FOR INSERT
                    AS
                    BEGIN
                        SET NOCOUNT ON

                        UPDATE dbo.Logs
                            SET InsertedOn = GETUTCDATE(),
                                ModifiedOn = GETUTCDATE()
                        FROM dbo.Logs INNER JOIN inserted
                            ON dbo.Logs.LogId = inserted.LogId;
                    END
                GO");

            // Logs After UPDATE Trigger
            migrationBuilder.Sql(@"
                CREATE TRIGGER TRG_Logs_After_Update
                    ON dbo.Logs
                    FOR UPDATE
                    AS
                    BEGIN
                        SET NOCOUNT ON

                        IF ( (SELECT trigger_nestlevel() ) > 1 )
                            RETURN

                        UPDATE dbo.Logs
                            SET InsertedOn = deleted.InsertedOn,
                                ModifiedOn = GETUTCDATE()
                        FROM dbo.Logs INNER JOIN deleted
                            ON dbo.Logs.LogId = deleted.LogId;
                    END
                GO");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
