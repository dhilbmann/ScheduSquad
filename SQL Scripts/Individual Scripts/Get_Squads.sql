-- ================================================
-- 
-- This procedure will return details for  
-- all squads 
-- 
-- ================================================


CREATE OR ALTER PROCEDURE Get_Squads

AS
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
END
GO