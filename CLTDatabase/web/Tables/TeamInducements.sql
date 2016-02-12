CREATE TABLE [web].[TeamInducements] (
    [ID]            INT IDENTITY (1, 1) NOT NULL,
    [TeamMatchData] INT NOT NULL,
    [Type]          INT NOT NULL,
    [Value]         INT NULL,
    CONSTRAINT [PK_TeamInducements] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_TeamInducements_TeamMatchData] FOREIGN KEY ([TeamMatchData]) REFERENCES [web].[TeamMatchData] ([ID])
);

