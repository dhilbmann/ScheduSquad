-- ================================================
-- 
-- This procedure will return details for an 
-- individual squad or all squads if no Id is given
-- 
-- ================================================


CREATE PROCEDURE Get_Squads
	@SquadId uniqueidentifier

AS
BEGIN

IF @SquadId IS NOT NULL
	BEGIN

		SELECT SquadPK AS 'Id',
			   SquadName,
			   SquadDesc,
			   SquadLocation
		FROM Squads 
		WHERE @SquadId = SquadPK

	END
ELSE
	BEGIN
		SELECT SquadPK AS 'Id',
			   SquadName,
			   SquadDesc,
			   SquadLocation
		FROM Squads
		WHERE IsDeleted = 0
	END
END
GO