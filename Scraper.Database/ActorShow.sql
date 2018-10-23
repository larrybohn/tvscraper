CREATE TABLE [dbo].[ActorShow]
(
    [ActorId] INT NOT NULL,
    [ShowId] INT NOT NULL,
    CONSTRAINT [FK_ActorShow_Actor] FOREIGN KEY (ActorId) REFERENCES [Actors]([Id]),
    CONSTRAINT [FK_ActorShow_Show] FOREIGN KEY (ShowId) REFERENCES [Shows]([Id]), 
    CONSTRAINT [PK_ActorShow] PRIMARY KEY ([ActorId], [ShowId])
)
