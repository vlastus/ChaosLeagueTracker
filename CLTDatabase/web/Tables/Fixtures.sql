CREATE TABLE [web].[Fixtures] (
    [ID]    INT IDENTITY (1, 1) NOT NULL,
    [Group] INT NOT NULL,
    [Round] INT NULL,
    [Team1] INT NOT NULL,
    [Team2] INT NOT NULL,
    CONSTRAINT [PK_Fixtures] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Fixtures_Groups] FOREIGN KEY ([Group]) REFERENCES [web].[Groups] ([ID]),
    CONSTRAINT [FK_Fixtures_Teams] FOREIGN KEY ([Team1]) REFERENCES [web].[Teams] ([ID]),
    CONSTRAINT [FK_Fixtures_Teams1] FOREIGN KEY ([Team2]) REFERENCES [web].[Teams] ([ID])
);

