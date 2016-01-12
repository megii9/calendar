CREATE TABLE [dbo].[Events]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [CategoryId] INT NOT NULL, 
    [CreatedBy] INT NOT NULL, 
    [Date] DATE NOT NULL , 
    [TimeFrom] TIME(0) NULL, 
    [TimeTo] TIME(0) NULL, 
    [IsAllDayEvent] BIT NOT NULL DEFAULT 0, 
    [Subject] NVARCHAR(50) NOT NULL, 
    [Body] NVARCHAR(MAX) NULL, 
    CONSTRAINT [FK_Events_Categories] FOREIGN KEY ([CategoryId]) REFERENCES [Categories]([Id]),
    CONSTRAINT [FK_Events_Users] FOREIGN KEY ([CreatedBy]) REFERENCES [Users]([Id])
)
