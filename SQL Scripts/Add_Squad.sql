-- ================================================
-- 
-- This procedure will create a new Squad and adds 
-- it to the Squad table
-- 
-- ================================================


CREATE PROCEDURE Add_Squad
	@Id uniqueidentifier,
	@SquadName nvarchar(50),
	@SquadDesc nvarchar(500),
	@SquadLocation nvarchar(25)

AS
BEGIN

DECLARE @AlreadyExists bit = (SELECT SquadPK from Squads WHERE SquadPK = @Id)

IF @AlreadyExists = 1
	BEGIN
	RETURN 0;
	END
ELSE
	BEGIN
	INSERT INTO Squads (SquadPK, 
						SquadName, 
						SquadDesc, 
						SquadLocation)
	VALUES (
			@Id,
			@SquadName,
			@SquadDesc,
			@SquadLocation
	)
	END
END
GO