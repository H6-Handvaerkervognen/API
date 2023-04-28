CREATE OR ALTER PROCEDURE CheckIfUserExists @Username VARCHAR(20)
AS
	SELECT ISNULL(
	(SELECT 1 FROM Users
	WHERE Username = @Username ), 0) as UserExists
GO

-- Creates a new user
CREATE OR ALTER PROCEDURE CreateUser @Username VARCHAR(20), @Password VARCHAR(200), @Salt VARBINARY(200), @Token VARCHAR(MAX)
AS
	INSERT INTO [Users]([Username], [Password], [Salt], [Token])
	VALUES (@Username, @Password, @Salt, @Token);
GO

CREATE OR ALTER PROCEDURE CheckIfTokenExists @Token VARCHAR(MAX)
AS
	SELECT ISNULL(
	(SELECT 1 FROM Users
	WHERE [Token] = @Token ), 0) as TokenExists
GO

-- Gets a user by the username
CREATE OR ALTER PROCEDURE GetUser @Username VARCHAR(20)
AS
	SELECT * FROM [Users] WHERE [Username] = @Username;
GO

-- Deletes a user
CREATE OR ALTER PROCEDURE DeleteUser @Username VARCHAR(20)
AS
	DELETE FROM [Users] WHERE [Username] = @Username;
GO
-- CHECK PAIR EXISTS
CREATE OR ALTER PROCEDURE CheckIfPairExists @Username VARCHAR(20), @AlarmId VARCHAR(200)
AS
	SELECT ISNULL(
	(SELECT 1 FROM [Pairs]
	WHERE [UserId] = @Username AND [AlarmId] = @AlarmId ), 0) as PairExists
GO

CREATE OR ALTER PROCEDURE CheckIfAlarmExists @AlarmId VARCHAR(200)
AS
	SELECT ISNULL(
	(SELECT 1 FROM [Pairs]
	WHERE [AlarmId] = @AlarmId ), 0) as PairExists
GO

-- Adds a pair between a user and an alarm (and inserts the alarm info in alarm table)
CREATE OR ALTER PROCEDURE AddPair @Username VARCHAR(20), @AlarmId VARCHAR(20), @StartTime VARBINARY(max), @EndTime VARBINARY(max), @Name VARBINARY(max)
AS
	IF NOT EXISTS(SELECT * FROM [Alarms] WHERE [Id] = @AlarmId)
	BEGIN
	INSERT INTO [Alarms]([Id], [StartTime], [EndTime], [Name], [AlarmOn])
	VALUES (@AlarmId, @StartTime, @EndTime, @Name, 0);
	END

	INSERT INTO [Pairs]([UserId], [AlarmId])
	VALUES (@Username, @AlarmId);
GO

-- Updates the active hours for an alarm
CREATE OR ALTER PROCEDURE UpdateActiveHours @AlarmId VARCHAR(20), @StartTime VARBINARY(MAX), @EndTime VARBINARY(MAX)
AS
	UPDATE [Alarms] SET [StartTime] = @StartTime, [EndTime] = @EndTime WHERE [Id] = @AlarmId;
GO

-- Gets info on an alarm by alarm id
CREATE OR ALTER PROCEDURE GetAlarmInfo @AlarmId VARCHAR(20)
AS
	SELECT * FROM [Alarms] WHERE [Id] = @AlarmId;
GO

-- Gets the time that specifies the active hours of the alarm
CREATE OR ALTER PROCEDURE GetActiveHours @AlarmId VARCHAR(20)
AS
	SELECT [StartTime], [EndTime] FROM [Alarms] WHERE [Id] = @AlarmId;
GO

--? Delete alarm if no pairs left
-- Deletes an alarm and user pairing
CREATE OR ALTER PROCEDURE DeletePairing  @AlarmId VARCHAR(20)
AS
	DELETE FROM [Pairs] WHERE [AlarmId] = @AlarmId;
GO

-- Gets all the alarms associated with a user
CREATE OR ALTER PROCEDURE GetAlarmsByUser @Username VARCHAR(20)
AS
	SELECT * FROM [Alarms] join Pairs on AlarmId = Id WHERE [UserId] = @Username;
GO

-- Changes the AlarmON to indicate the alarm is stopped 
CREATE OR ALTER PROCEDURE StopAlarm @AlarmId VARCHAR(20), @AlarmOn BIT
AS
	UPDATE [Alarms] SET [AlarmOn] = @AlarmOn WHERE [Id] = @AlarmId;
GO

-- Changes the AlarmON to indicate the alarm is startet 
CREATE OR ALTER PROCEDURE StartAlarm @AlarmId VARCHAR(20), @AlarmOn BIT
AS
	UPDATE [Alarms] SET [AlarmOn] = @AlarmOn WHERE [Id] = @AlarmId;
GO