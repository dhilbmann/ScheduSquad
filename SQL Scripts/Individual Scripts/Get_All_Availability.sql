-- ================================================
-- 
-- This procedure will return a list of all 
-- availability for all members in a squad
-- 
-- ================================================


CREATE OR ALTER PROCEDURE [dbo].[Get_All_Availability]

AS
BEGIN


SELECT	AvailabilityPK as 'Id',
		DayEnum,
		StartTime,
		EndTime
FROM UserAvailability
WHERE IsDeleted = 0

END
GO