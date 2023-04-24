CREATE DATABASE haandvaerkervognen;

USE haandvaerkervognen;

CREATE TABLE [Users] 
(
	[Username] VARCHAR(20) PRIMARY KEY,
	[Password] VARCHAR(200) NOT NULL,
	[Salt] VARCHAR(200) NOT NULL
)

CREATE TABLE [Alarms]
(
	[Id] VARCHAR(200) PRIMARY KEY,
	[StartTime] VARCHAR(200), -- encrypted
	[EndTime] VARCHAR(200), -- encrypted
	[Name] VARCHAR(200), -- encrypted
	[AlarmOn] BIT,
	[Salt] VARCHAR(200)
)

CREATE TABLE [Pairs]
(
	-- Username
	[UserId] VARCHAR(20) NOT NULL,
	[AlarmId] VARCHAR(200) NOT NULL,

	
FOREIGN KEY ([UserId]) REFERENCES [Users]([Username]),
FOREIGN KEY ([AlarmId]) REFERENCES [Alarms]([Id])
)

--Tabel med krypt navn