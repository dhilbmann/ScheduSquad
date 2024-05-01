-- ================================================
-- 
-- This procedure will return a list of all 
-- members not in a specific squad
-- 
-- ================================================

CREATE OR ALTER PROCEDURE Get_MembersNotInSquad
	@SquadId uniqueidentifier

AS
BEGIN

IF @SquadId IS NOT NULL
	
	BEGIN
	Select DISTINCT * from (
		SELECT u.UserPk AS 'Id',
			   u.FirstName,
			   u.LastName,
			   u.Email
		FROM Users u
		LEFT JOIN SquadMembers sm ON u.UserPK = sm.UserFK 
		AND sm.SquadFK = @SquadID
		WHERE sm.SquadMemberPK IS NULL
		UNION
		SELECT u.UserPk AS 'Id',
			   u.FirstName,
			   u.LastName,
			   u.Email
		FROM Users u
		JOIN SquadMembers sm ON u.UserPK = sm.UserFK 
		AND sm.SquadFK = @SquadID 
		WHERE sm.IsDeleted = 1
		) a
	END
ELSE
	BEGIN
		RETURN 0
	END
END
GO