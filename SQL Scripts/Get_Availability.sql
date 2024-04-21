-- ================================================
-- 
-- This procedure will return details for a 
-- singularly requested availability
-- 
-- ================================================


CREATE PROCEDURE Get_Availability
	@Id uniqueidentifier

AS
BEGIN

IF @Id IS NOT NULL
	BEGIN

		SELECT AvailabilityPK AS 'Id',
			   DayEnum,
			   StartTime,
			   EndTime
		FROM UserAvailability 
		WHERE @Id = AvailabilityPK AND IsDeleted = 0

	END
ELSE
	BEGIN
		RETURN 0
	END
END
GO
