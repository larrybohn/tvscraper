CREATE TABLE [dbo].[Shows]
(
    [Id] INT NOT NULL PRIMARY KEY CLUSTERED IDENTITY(1,1), 
    [ApiId] INT NOT NULL, 
    [Title] NVARCHAR(MAX) NOT NULL
)

GO

CREATE INDEX [IX_Shows_ApiId] ON [dbo].[Shows] ([ApiId])
