CREATE TABLE [web].[CompTeams] (
    [ID]           INT IDENTITY (1, 1) NOT NULL,
    [Team]         INT NOT NULL,
    [Competititon] INT NOT NULL,
    CONSTRAINT [PK_CompTeams] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_CompTeams_Competitions] FOREIGN KEY ([Competititon]) REFERENCES [web].[Groups] ([ID]),
    CONSTRAINT [FK_CompTeams_Teams] FOREIGN KEY ([Team]) REFERENCES [web].[Teams] ([ID])
);

