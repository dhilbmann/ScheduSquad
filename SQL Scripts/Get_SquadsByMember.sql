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

IF @Id IS NULL
	BEGIN
		RETURN 0;
	END
ELSE
	BEGIN
		SELECT SquadPK AS 'Id',
			   SquadName,
			   SquadDesc,
			   SquadLocation,
			   (Select UserFK From SquadMembers sm1 Where SquadFK = s.SquadPK AND sm1.IsSquadMaster = 1) as SquadMasterId
		FROM Squads s
		INNER JOIN SquadMembers sm ON sm.SquadFK = s.SquadPK
		WHERE s.IsDeleted = 0
			AND sm.UserFK = @Id

	END
END
GO