CREATE TABLE [dbo].[UsersCategories]
(
	[UserId] INT NOT NULL, 
    [CategoryId] INT NOT NULL, 
    CONSTRAINT [FK_UsersCategories_Users] FOREIGN KEY ([UserId]) REFERENCES [Users]([Id]), 
    CONSTRAINT [FK_UsersCategories_Categories] FOREIGN KEY ([CategoryId]) REFERENCES [Categories]([Id]) 
)
