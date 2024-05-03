-- ================================================
-- 
-- This procedure will return details for an 
-- all members 
-- 
-- ================================================


CREATE OR ALTER PROCEDURE Get_AllMembers

AS

SELECT UserPk AS 'Id',
		FirstName,
		LastName,
		Email
FROM Users
WHERE IsDeleted = 0

GO