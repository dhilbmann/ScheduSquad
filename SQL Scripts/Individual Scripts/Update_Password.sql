-- ================================================
-- 
-- This procedure will update the password hash and
-- salt based on the user id
-- 
-- ================================================


CREATE OR ALTER PROCEDURE Update_Password
	@memberId uniqueidentifier,
	@PwHash varchar(128),
	@PwSalt varchar(128)

AS
BEGIN


DECLARE @Exists bit = (SELECT COUNT(1) from Users WHERE UserPk = @memberId)

IF @Exists = 0
	BEGIN
		RETURN 0
	END
ELSE
	BEGIN
		UPDATE Users 
		SET 
			PwHash = @PwHash, 
			PwSalt = @PwSalt
		WHERE UserPk = @memberId
	END
END
GO

