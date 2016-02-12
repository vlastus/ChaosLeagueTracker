CREATE TABLE [web].[PlayerTypes] (
    [ID]       INT          IDENTITY (1, 1) NOT NULL,
    [Race]     INT          NOT NULL,
    [Name]     VARCHAR (30) NOT NULL,
    [MA]       INT          CONSTRAINT [DF_PlayerTypes_MA] DEFAULT ((0)) NULL,
    [ST]       INT          CONSTRAINT [DF_PlayerTypes_ST] DEFAULT ((0)) NULL,
    [AG]       INT          CONSTRAINT [DF_PlayerTypes_AG] DEFAULT ((0)) NULL,
    [AV]       INT          CONSTRAINT [DF_PlayerTypes_AV] DEFAULT ((0)) NULL,
    [Value]    INT          CONSTRAINT [DF_PlayerTypes_Value] DEFAULT ((0)) NULL,
    [Skillset] VARCHAR (50) NULL,
    CONSTRAINT [PK_PlayerTypes] PRIMARY KEY CLUSTERED ([ID] ASC)
);

