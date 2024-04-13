-- ================================================
-- 
-- This procedure will return details for an 
-- individual squad or all squads if no Id is given
-- 
-- ================================================


CREATE PROCEDURE Get_SquadsByMember
	@SquadMemberId uniqueidentifier

AS
BEGIN

IF @SquadMemberId IS NOT NULL
	BEGIN
		RETURN 0;
	END
ELSE
	BEGIN
		SELECT SquadPK AS 'Id',
			   SquadName,
			   SquadDesc,
			   SquadLocation
		FROM Squads s
		INNER JOIN SquadMembers sm ON sm.SquadFK = s.SquadPK
		WHERE s.IsDeleted = 0
			AND sm.UserFK = @SquadMemberId

	END
END
GO