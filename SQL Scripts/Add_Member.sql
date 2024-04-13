-- ================================================
-- 
-- This procedure will create a new member and adds 
-- them to the User table
-- 
-- ================================================


CREATE PROCEDURE Add_Member
	@Id uniqueidentifier,
	@FirstName nvarchar(35),
	@LastName nvarchar(35),
	@Email nvarchar(75),
	@PwHash char(128),
	@PwSalt char(128)

AS
BEGIN

DECLARE @AlreadyExists bit = (SELECT COUNT(1) from Users WHERE UserPk = @Id)

IF @AlreadyExists = 1
	BEGIN
	RETURN 0;
	END
ELSE
	BEGIN
	INSERT INTO Users ( UserPk, 
						FirstName, 
						LastName, 
						Email, 
						PwHash, 
						PwSalt)
	VALUES (
			@Id,
			@FirstName,
			@LastName,
			@Email,
			@PwHash,
			@PwSalt
	)
	END
END
GO