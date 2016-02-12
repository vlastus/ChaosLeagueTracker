CREATE TABLE [web].[PlayerSkills] (
    [ID]     INT IDENTITY (1, 1) NOT NULL,
    [Player] INT NOT NULL,
    [Skill]  INT NOT NULL,
    CONSTRAINT [PK_PlayerSkills] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_PlayerSkills_Players] FOREIGN KEY ([Player]) REFERENCES [web].[Players] ([ID])
);

