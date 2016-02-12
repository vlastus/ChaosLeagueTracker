CREATE TABLE [web].[Matches] (
    [ID]          INT  IDENTITY (1, 1) NOT NULL,
    [Competition] INT  NOT NULL,
    [Round]       INT  CONSTRAINT [DF_Matches_Round] DEFAULT ((0)) NULL,
    [Team1Data]   INT  NOT NULL,
    [Team2Data]   INT  NOT NULL,
    [Date]        DATE NOT NULL,
    CONSTRAINT [PK_Matches] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Matches_Competitions] FOREIGN KEY ([Competition]) REFERENCES [web].[Competitions] ([ID]),
    CONSTRAINT [FK_Matches_TeamMatchData] FOREIGN KEY ([Team1Data]) REFERENCES [web].[TeamMatchData] ([ID]),
    CONSTRAINT [FK_Matches_TeamMatchData1] FOREIGN KEY ([Team2Data]) REFERENCES [web].[TeamMatchData] ([ID])
);

