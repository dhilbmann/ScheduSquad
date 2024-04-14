-- ================================================
-- 
-- This procedure will return details for  
-- all squads a member is a part of
-- 
-- ================================================


CREATE PROCEDURE Get_SquadsByMember
	@Id uniqueidentifier

AS
BEGIN

IF @Id IS NOT NULL
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
			AND sm.UserFK = @Id

	END
END
GO