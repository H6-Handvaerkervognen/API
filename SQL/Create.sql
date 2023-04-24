CREATE DATABASE haandvaerkervognen;

USE haandvaerkervognen;

CREATE TABLE [Users] 
(
	[Username] VARCHAR(20) PRIMARY KEY,
	[Password] VARCHAR(100) NOT NULL,
	[Salt] VARCHAR(100) NOT NULL
)

CREATE TABLE [Alarms]
(
	[Id] VARCHAR(20) PRIMARY KEY,
	[StartTime] VARCHAR(100), -- encrypted
	[EndTime] VARCHAR(100), -- encrypted
	[Name] VARCHAR(100), -- encrypted
	[AlarmOn] BIT,
	[Salt] VARCHAR(100)
)

CREATE TABLE [Pairs]
(
	-- Username
	[UserId] VARCHAR(20) NOT NULL,
	[AlarmId] VARCHAR(20) NOT NULL,

	
FOREIGN KEY ([UserId]) REFERENCES [Users]([Username]),
FOREIGN KEY ([AlarmId]) REFERENCES [Alarms]([Id])
)

--Tabel med krypt navn