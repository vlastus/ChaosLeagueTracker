CREATE TABLE [web].[Groups] (
    [ID]          INT          IDENTITY (1, 1) NOT NULL,
    [Competition] INT          NOT NULL,
    [Playoff]     INT          CONSTRAINT [DF_Groups_Playoff] DEFAULT ((0)) NULL,
    [Name]        VARCHAR (50) NULL,
    CONSTRAINT [PK_Groups] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Groups_Competitions] FOREIGN KEY ([Competition]) REFERENCES [web].[Competitions] ([ID])
);



