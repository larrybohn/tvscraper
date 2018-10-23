CREATE TABLE [dbo].[Actors]
(
    [Id] INT NOT NULL PRIMARY KEY CLUSTERED IDENTITY(1,1), 
    [ApiId] INT NOT NULL, 
    [Name] NVARCHAR(250) NOT NULL, 
    [Birthday] DATE NULL
)

GO

CREATE INDEX [IX_Actors_ApiId] ON [dbo].[Actors] ([ApiId])
