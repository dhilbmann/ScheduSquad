
begin transaction a

CREATE LOGIN testuser3 WITH PASSWORD = 'admin24!',
							DEFAULT_DATABASE = ScheduSquad
GO

CREATE USER testuser3 FOR LOGIN testuser3;

ALTER USER testuser3 WITH DEFAULT_SCHEMA= ScheduSquad;

EXEC sp_addrolemember 'db_owner', 'testuser2';
							
GO

commit transaction a