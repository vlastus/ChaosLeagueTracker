CREATE TABLE [web].[MatchEvents] (
    [ID]           INT IDENTITY (1, 1) NOT NULL,
    [Match]        INT NOT NULL,
    [EventType]    INT NOT NULL,
    [Team]         INT NOT NULL,
    [SourcePlayer] INT NOT NULL,
    [TargetPlayer] INT NULL,
    [Result]       INT NULL,
    CONSTRAINT [PK_MatchEvents] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_MatchEvents_Matches] FOREIGN KEY ([Match]) REFERENCES [web].[Matches] ([ID]),
    CONSTRAINT [FK_MatchEvents_Players] FOREIGN KEY ([SourcePlayer]) REFERENCES [web].[Players] ([ID]),
    CONSTRAINT [FK_MatchEvents_Players1] FOREIGN KEY ([TargetPlayer]) REFERENCES [web].[Players] ([ID]),
    CONSTRAINT [FK_MatchEvents_Teams] FOREIGN KEY ([Team]) REFERENCES [web].[Teams] ([ID])
);

