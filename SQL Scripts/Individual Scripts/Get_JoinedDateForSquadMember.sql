-- ================================================
-- 
-- This procedure will return the joined date 
-- for a member in a squad.
-- 
-- ================================================

CREATE OR ALTER PROCEDURE Get_JoinedDateForSquadMember
	@MemberId uniqueidentifier,
	@SquadId uniqueidentifier

AS
BEGIN

IF @MemberId IS NOT NULL AND @SquadId IS NOT NULL
	
	BEGIN
		Select JoinDate 
		From SquadMembers 
		Where SquadFK = @SquadId 
			AND UserFK = @MemberId 
			AND IsDeleted = 0
	END
ELSE
	BEGIN
		RETURN 0
	END
END
GO