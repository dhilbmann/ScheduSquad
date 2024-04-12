-- ================================================
-- 
-- This procedure will return a list of all 
-- availability for all members in a squad
-- 
-- ================================================


CREATE PROCEDURE Get_Availability_for_SquadMembers
	@SquadId uniqueidentifier

AS
BEGIN

IF @SquadId IS NOT NULL
	BEGIN
		SELECT u.UserPk AS 'Id',
			   u.UserPk,
			   ua.DayEnum,
			   ua.StartTime,
			   ua.EndTime
		FROM SquadMembers sm
		INNER JOIN Users u ON u.UserPk = sm.UserFK
		INNER JOIN UserAvailability ua ON ua.UserFK = u.UserPk
		WHERE @SquadId = sm.SquadFK 
			AND sm.IsDeleted = 0
			AND u.IsDeleted = 0
			AND ua.IsDeleted = 0
	END
ELSE
	BEGIN
		RETURN 0
	END
END
GO