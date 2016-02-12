CREATE TABLE [web].[Competitions] (
    [ID]     INT          IDENTITY (1, 1) NOT NULL,
    [Name]   VARCHAR (50) NULL,
    [Status] INT          CONSTRAINT [DF_Competitions_Status] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Competitions] PRIMARY KEY CLUSTERED ([ID] ASC)
);

