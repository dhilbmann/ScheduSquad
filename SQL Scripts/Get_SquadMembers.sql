-- ================================================
-- 
-- This procedure will return a list of all 
-- members in a squad
-- 
-- ================================================


CREATE PROCEDURE Get_SquadMembers
	@SquadId uniqueidentifier

AS
BEGIN

IF @SquadId IS NOT NULL
	BEGIN
		SELECT SquadMemberPK AS 'Id',
			   u.UserPk,
			   u.FirstName,
			   u.LastName,
			   sm.IsSquadMaster,
			   sm.JoinDate
		FROM SquadMembers sm
		INNER JOIN Users u ON u.UserPk = sm.UserFK
		WHERE @SquadId = sm.SquadFK 
			AND sm.IsDeleted = 0
			AND u.IsDeleted = 0
	END
ELSE
	BEGIN
		RETURN 0
	END
END
GO