-- ================================================
-- 
-- This procedure will update details on the
-- specified availability
-- 
-- ================================================


CREATE PROCEDURE Update_Availability
	@Id uniqueidentifier,
	@DayEnum int,
	@StartTime time,
	@EndTime time

AS
BEGIN

DECLARE @Exists bit = (SELECT COUNT(1) from UserAvailability WHERE AvailabilityPK = @Id)

IF @Exists = 0
	BEGIN
	RETURN 0
	END
ELSE
	BEGIN
		UPDATE UserAvailability 
		SET 
			DayEnum = @DayEnum, 
			StartTime = @StartTime,
			EndTime = @EndTime
		WHERE AvailabilityPK = @Id
	RETURN 1
	END
END
GO