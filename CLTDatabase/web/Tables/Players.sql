﻿CREATE TABLE [web].[Players] (
    [ID]         INT           IDENTITY (1, 1) NOT NULL,
    [Team]       INT           NOT NULL,
    [Type]       INT           NOT NULL,
    [Name]       VARCHAR (100) NOT NULL,
    [MA]         INT           CONSTRAINT [DF_Players_MA] DEFAULT ((0)) NULL,
    [ST]         INT           CONSTRAINT [DF_Players_ST] DEFAULT ((0)) NULL,
    [AG]         INT           CONSTRAINT [DF_Players_AG] DEFAULT ((0)) NULL,
    [AV]         INT           CONSTRAINT [DF_Players_AV] DEFAULT ((0)) NULL,
    [SPP]        INT           CONSTRAINT [DF_Players_SPP] DEFAULT ((0)) NULL,
    [INT]        INT           CONSTRAINT [DF_Players_INT] DEFAULT ((0)) NULL,
    [COMP]       INT           CONSTRAINT [DF_Players_COMP] DEFAULT ((0)) NULL,
    [TD]         INT           CONSTRAINT [DF_Players_TD] DEFAULT ((0)) NULL,
    [CAS]        INT           CONSTRAINT [DF_Players_CAS] DEFAULT ((0)) NULL,
    [Kills]      INT           CONSTRAINT [DF_Players_Kills] DEFAULT ((0)) NULL,
    [MVP]        INT           CONSTRAINT [DF_Players_MVP] DEFAULT ((0)) NULL,
    [Value]      INT           CONSTRAINT [DF_Players_Value] DEFAULT ((0)) NULL,
    [NI]         INT           CONSTRAINT [DF_Players_NI] DEFAULT ((0)) NULL,
    [MNG]        INT           CONSTRAINT [DF_Players_MNG] DEFAULT ((0)) NULL,
    [Journeyman] INT           CONSTRAINT [DF_Players_Journeyman] DEFAULT ((0)) NULL,
    [Status]     INT           CONSTRAINT [DF_Players_Status] DEFAULT ((1)) NULL,
    [Level]      INT           CONSTRAINT [DF_Players_Level] DEFAULT ((0)) NULL,
    [Number]     INT           NULL,
    CONSTRAINT [PK_Players] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Players_PlayerTypes] FOREIGN KEY ([Type]) REFERENCES [web].[PlayerTypes] ([ID]),
    CONSTRAINT [FK_Players_Teams] FOREIGN KEY ([Team]) REFERENCES [web].[Teams] ([ID])
);





