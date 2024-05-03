-- ================================================
-- 
-- This procedure will delete a member from 
-- the SquadMember table
-- 
-- ================================================


CREATE OR ALTER PROCEDURE Delete_Member_from_Squad
	@userId uniqueidentifier,
	@squadId uniqueidentifier

AS
BEGIN

DECLARE @Exists bit = (SELECT COUNT(1) from Users WHERE UserPk = @userId)

IF @Exists = 0
	BEGIN
	RETURN 0
	END
ELSE
	BEGIN
	UPDATE SquadMembers 
	SET IsDeleted = 1 
	WHERE SquadFK = @squadId 
		AND UserFK = @userId
	RETURN 1
	END
END
GO