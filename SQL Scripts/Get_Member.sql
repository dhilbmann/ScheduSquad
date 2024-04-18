-- ================================================
-- 
-- This procedure will return details for an 
-- individual member or all members if no Id is given
-- 
-- ================================================


CREATE PROCEDURE Get_Member
	@Id uniqueidentifier

AS
BEGIN

DECLARE @EXISTS bit = (SELECT COUNT(1) FROM Users WHERE UserPk = @Id)

IF @EXISTS = 1
	BEGIN
		SELECT UserPk AS 'Id',
			   FirstName,
			   LastName,
			   Email
		FROM Users 
		WHERE @Id = UserPk
	END
ELSE
	BEGIN
		RETURN 0
	END
END
GO