-- ================================================
-- 
-- This procedure will return details for an 
-- individual member or all members if no Id is given
-- 
-- ================================================


CREATE PROCEDURE Get_AllMembers

AS

SELECT UserPk AS 'Id',
		FirstName,
		LastName,
		Email
FROM Users
WHERE IsDeleted = 0

GO