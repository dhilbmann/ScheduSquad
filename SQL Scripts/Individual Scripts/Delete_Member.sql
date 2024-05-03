-- ================================================
-- 
-- This procedure will delete a member from 
-- the Member table
-- 
-- ================================================


CREATE OR ALTER PROCEDURE Delete_Member
	@Id uniqueidentifier

AS
BEGIN

DECLARE @Exists bit = (SELECT COUNT(1) from Users WHERE UserPk = @Id)

IF @Exists = 0
	BEGIN
	RETURN 0
	END
ELSE
	BEGIN
	UPDATE Users SET IsDeleted = 1 WHERE UserPk = @Id
	RETURN 1
	END
END
GO