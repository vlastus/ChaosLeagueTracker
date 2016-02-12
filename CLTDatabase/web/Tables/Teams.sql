CREATE TABLE [web].[Teams] (
    [ID]           INT          IDENTITY (1, 1) NOT NULL,
    [Race]         INT          NOT NULL,
    [Owner]        INT          NOT NULL,
    [Name]         VARCHAR (50) NOT NULL,
    [Rerolls]      INT          CONSTRAINT [DF_Teams_Rerolls] DEFAULT ((0)) NOT NULL,
    [Fanfactor]    INT          CONSTRAINT [DF_Teams_Fanfactor] DEFAULT ((0)) NULL,
    [Asscoaches]   INT          CONSTRAINT [DF_Teams_Asscoaches] DEFAULT ((0)) NULL,
    [Cheerleaders] INT          CONSTRAINT [DF_Teams_Cheerleaders] DEFAULT ((0)) NULL,
    [Apothecary]   INT          CONSTRAINT [DF_Teams_Apothecary] DEFAULT ((0)) NULL,
    [Value]        INT          CONSTRAINT [DF_Teams_Value] DEFAULT ((0)) NULL,
    [Treasury]     INT          CONSTRAINT [DF_Teams_Treasury] DEFAULT ((0)) NULL,
    [Status]       INT          CONSTRAINT [DF_Teams_Status] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Teams] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Teams_Users] FOREIGN KEY ([Owner]) REFERENCES [web].[Users] ([ID])
);

