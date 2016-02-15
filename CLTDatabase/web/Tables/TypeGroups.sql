CREATE TABLE [web].[TypeGroups] (
    [ID]         INT IDENTITY (1, 1) NOT NULL,
    [PlayerType] INT NOT NULL,
    [SkillGroup] INT NOT NULL,
    [IsDouble]   INT CONSTRAINT [DF_TypeGroups_IsDouble] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_TypeGroups] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_TypeGroups_PlayerTypes] FOREIGN KEY ([PlayerType]) REFERENCES [web].[PlayerTypes] ([ID])
);

