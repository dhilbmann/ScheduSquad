-- ================================================
-- 
-- This procedure will get the password hash based
-- on the user id
-- 
-- ================================================


CREATE PROCEDURE Get_Password
	@memberId uniqueidentifier

AS
BEGIN


DECLARE @Exists bit = (SELECT COUNT(1) from Users WHERE UserPk = @memberId)

IF @Exists = 0
	BEGIN
		RETURN 0
	END
ELSE
	BEGIN
		SELECT TOP 1 u.PwHash as item
		FROM USERS u
		WHERE u.UserPk = @memberId
	END
END
GO

