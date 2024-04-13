-- ================================================
-- 
-- This procedure will add a row of availability 
-- for an individual user
-- 
-- ================================================


CREATE PROCEDURE Add_Availability
	@Id uniqueidentifier,
	@UserId uniqueidentifier,
	@DayEnum int,
	@StartTime datetime2,
	@EndTime datetime2

AS
BEGIN

DECLARE @IdAlreadyExists bit = (SELECT COUNT(1)
							  FROM UserAvailability u
							  WHERE AvailabilityPK = @Id)
DECLARE @UserExists bit = (SELECT COUNT(1) 
							FROM Users 
							WHERE UserPk = @UserId)

-- Checks if the Availability ID already exists or if the user does not exist
IF @IdAlreadyExists = 1 OR @UserExists = 0
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
				@Id,
				@UserId,
				@DayEnum,
				@StartTime,
				@EndTime
		)
	END
END
GO