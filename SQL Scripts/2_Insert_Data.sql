
DECLARE @COMMIT bit = 1
BEGIN TRANSACTION A

DECLARE @Id uniqueidentifier = NEWID()
exec Add_Member @Id, 'Cara', 'Perez', 'cara.c.perez@gmail.com', '$2y$10$ez7OOiHD9IR60EFHYDVXJusRea/jiGYmUQhflb4h8GoaS6yZ5GQ6q', '$2y$10$ez7OOiHD9IR60EFHYDVXJusRea/jiGYmUQhflb4h8GoaS6yZ5GQ6q'
SET  @Id = NEWID()
exec Add_Member @Id, 'David', 'Hilbmann', 'a@b.com', '$2y$10$ez7OOiHD9IR60EFHYDVXJusRea/jiGYmUQhflb4h8GoaS6yZ5GQ6q', '$2y$10$ez7OOiHD9IR60EFHYDVXJusRea/jiGYmUQhflb4h8GoaS6yZ5GQ6q'
SET  @Id = NEWID()
exec Add_Member @Id, 'Duncan', 'Clark', 'test@test.com', '$2y$10$ez7OOiHD9IR60EFHYDVXJusRea/jiGYmUQhflb4h8GoaS6yZ5GQ6q', '$2y$10$ez7OOiHD9IR60EFHYDVXJusRea/jiGYmUQhflb4h8GoaS6yZ5GQ6q'

SET  @Id = NEWID()
exec Add_Squad @Id, 'Prime Squad', 'First test squad', 'Online' 
SET  @Id = NEWID()
exec Add_Squad @Id, 'The Squad Pod', 'Another test squad', 'China' 
SET  @Id = NEWID()
exec Add_Squad @Id, 'Squadity Falls', 'One more test squad', 'The Moon'

DECLARE @SquadId uniqueidentifier = (SELECT s.SquadPK FROM Squads s WHERE s.SquadName = 'Prime Squad')
DECLARE @UserId uniqueidentifier = (SELECT u.UserPk FROM Users u WHERE u.FirstName = 'Cara')
SET  @Id = NEWID()
exec Add_Member_to_Squad @Id, @SquadId, @UserId, 1
SET @UserId =  (SELECT u.UserPk FROM Users u WHERE u.FirstName = 'David')
SET  @Id = NEWID()
exec Add_Member_to_Squad @Id, @SquadId, @UserId, 0
SET @UserId =  (SELECT u.UserPk FROM Users u WHERE u.FirstName = 'Duncan')
SET  @Id = NEWID()
exec Add_Member_to_Squad @Id, @SquadId, @UserId, 0

SET @SquadId  = (SELECT s.SquadPK FROM Squads s WHERE s.SquadName = 'The Squad Pod')
SET @UserId  = (SELECT u.UserPk FROM Users u WHERE u.FirstName = 'David')
SET  @Id = NEWID()
exec Add_Member_to_Squad @Id, @SquadId, @UserId, 1
SET @UserId =  (SELECT u.UserPk FROM Users u WHERE u.FirstName = 'Duncan')
SET  @Id = NEWID()
exec Add_Member_to_Squad @Id, @SquadId, @UserId, 0


SET @SquadId  = (SELECT s.SquadPK FROM Squads s WHERE s.SquadName = 'Squadity Falls')
SET @UserId  = (SELECT u.UserPk FROM Users u WHERE u.FirstName = 'Duncan')
SET  @Id = NEWID()
exec Add_Member_to_Squad @Id, @SquadId, @UserId, 1
SET @UserId =  (SELECT u.UserPk FROM Users u WHERE u.FirstName = 'Cara')
SET  @Id = NEWID()
exec Add_Member_to_Squad @Id, @SquadId, @UserId, 0

SET @UserId  = (SELECT u.UserPk FROM Users u WHERE u.FirstName = 'Duncan')
SET  @Id = NEWID()
exec Add_Availability @Id, @UserId, 1, '10:45:00','14:00:00'
SET  @Id = NEWID()
exec Add_Availability @Id, @UserId, 2, '9:30:00','16:00:00'
SET  @Id = NEWID()
exec Add_Availability @Id, @UserId, 3, '09:15:00','14:00:00'
SET  @Id = NEWID()
exec Add_Availability @Id, @UserId, 4, '12:30:00','15:45:00'


SET @UserId  = (SELECT u.UserPk FROM Users u WHERE u.FirstName = 'Cara')
SET  @Id = NEWID()
exec Add_Availability @Id, @UserId, 2, '09:00:00','12:30:00'
SET  @Id = NEWID()
exec Add_Availability @Id, @UserId, 3, '8:30:00','17:00:00'
SET  @Id = NEWID()
exec Add_Availability @Id, @UserId, 3, '05:15:00','11:00:00'
SET  @Id = NEWID()
exec Add_Availability @Id, @UserId, 5, '13:30:00','18:45:00'


SET @UserId  = (SELECT u.UserPk FROM Users u WHERE u.FirstName = 'David')
SET  @Id = NEWID()
exec Add_Availability @Id, @UserId, 3, '16:45:00','20:00:00'
SET  @Id = NEWID()
exec Add_Availability @Id, @UserId, 6, '12:15:00','22:00:00'
SET  @Id = NEWID()
exec Add_Availability @Id, @UserId, 4, '09:15:00','14:00:00'
SET  @Id = NEWID()
exec Add_Availability @Id, @UserId, 7, '11:30:00','17:30:00'

IF @COMMIT = 1
	BEGIN
		COMMIT TRANSACTION A
		SELECT 'COMMIT'
	END
ELSE
	BEGIN
		ROLLBACK TRANSACTION A
		SELECT 'ROLLBACK'
	END

