-- ================================================
-- 
-- This procedure will return details for an 
-- individual squad or all squads if no Id is given
-- 
-- ================================================


CREATE OR ALTER PROCEDURE Get_Squad
	@Id uniqueidentifier

AS
BEGIN

DECLARE @EXISTS bit = (SELECT COUNT(1) FROM Squads WHERE SquadPk = @Id)

IF @EXISTS = 1
	BEGIN
	SELECT SquadPK AS 'Id',
			   SquadName,
			   SquadDesc,
			   SquadLocation,
			   sm.UserFK AS 'SquadMasterId'
		FROM Squads s
		INNER JOIN SquadMembers sm ON sm.SquadFK = s.SquadPK
		WHERE sm.IsDeleted = 0
			AND s.IsDeleted = 0
			AND sm.IsSquadMaster = 1
			AND s.SquadPK = @Id
	END
ELSE
	BEGIN
		RETURN 0
	END
END
GO