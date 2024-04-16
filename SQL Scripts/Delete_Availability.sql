-- ================================================
-- 
-- This procedure will delete a Squad from 
-- the Squad table
-- 
-- ================================================


CREATE PROCEDURE Delete_Availability
	@Id uniqueidentifier

AS
BEGIN

DECLARE @Exists bit = (SELECT COUNT(1) from UserAvailability WHERE AvailabilityPK = @Id)

IF @Exists = 0
	BEGIN
	RETURN 0
	END
ELSE
	BEGIN
	UPDATE UserAvailability SET IsDeleted = 1 WHERE AvailabilityPK = @Id
	RETURN 1
	END
END
GO