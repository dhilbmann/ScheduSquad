-- ================================================
-- 
-- This procedure will return a list of all 
-- availability for all members in a squad
-- 
-- ================================================


CREATE OR ALTER PROCEDURE Get_All_Availability_By_Id
	@Id uniqueidentifier --UserId if a certain user's availability is requested

AS
BEGIN

IF @Id IS NOT NULL
	BEGIN
		SELECT ua.AvailabilityPK as 'Id',
				ua.DayEnum,
				ua.StartTime,
				ua.EndTime
		FROM UserAvailability ua
		INNER JOIN Users u ON u.UserPk = ua.UserFK
		WHERE @Id = u.UserPk 
			AND u.IsDeleted = 0
			AND ua.IsDeleted = 0
		ORDER BY ua.DayEnum
	END
ELSE
	BEGIN
		RETURN 0
	END
END
GO