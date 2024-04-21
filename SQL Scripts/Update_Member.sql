-- ================================================
-- 
-- This procedure will update details on the
-- specified member
-- 
-- ================================================


CREATE PROCEDURE Update_Member
	@Id uniqueidentifier,
	@FirstName nvarchar(35),
	@LastName nvarchar(35),
	@Email nvarchar(75)

AS
BEGIN

DECLARE @Exists bit = (SELECT COUNT(1) from Users WHERE UserPk = @Id)

IF @Exists = 0
	BEGIN
	RETURN 0
	END
ELSE
	BEGIN
		UPDATE Users 
		SET 
			FirstName = @FirstName, 
			LastName = @LastName,
			Email = @Email
		WHERE UserPk = @Id
	RETURN 1
	END
END
GO