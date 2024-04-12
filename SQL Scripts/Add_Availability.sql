-- ================================================
-- 
-- This procedure will add a row of availability 
-- for an individual user
-- 
-- ================================================


CREATE PROCEDURE Add_Availability
	@AvailabilityId uniqueidentifier,
	@UserId uniqueidentifier,
	@DayEnum int,
	@StartTime datetime2,
	@EndTime datetime2

AS
BEGIN

DECLARE @AlreadyExists bit = (SELECT @AvailabilityId 
							  FROM UserAvailability 
							  WHERE AvailabilityPK = @AvailabilityId)

IF @AlreadyExists = 1
	BEGIN
		RETURN 0;
	END
ELSE
	BEGIN
		INSERT INTO UserAvailability (AvailabilityPk, 
									  UserFK, 
									  DayEnum, 
									  StartTime,
									  EndTime)
		VALUES (
				@AvailabilityId,
				@UserId,
				@DayEnum,
				@StartTime,
				@EndTime
		)
	END
END
GO