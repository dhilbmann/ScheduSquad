-- ================================================
-- 
-- This procedure will return details for an 
-- individual squad or all squads if no Id is given
-- 
-- ================================================


CREATE PROCEDURE Get_Squads
	@Id uniqueidentifier

AS
BEGIN

IF @Id IS NOT NULL
	BEGIN

		SELECT SquadPK AS 'Id',
			   SquadName,
			   SquadDesc,
			   SquadLocation
		FROM Squads 
		WHERE @Id = SquadPK

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