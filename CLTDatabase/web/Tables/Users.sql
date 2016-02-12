CREATE TABLE [web].[Users] (
    [ID]             INT           IDENTITY (1, 1) NOT NULL,
    [Name]           VARCHAR (50)  NOT NULL,
    [Status]         INT           CONSTRAINT [DF_Users_Status] DEFAULT ((1)) NOT NULL,
    [Authentication] VARCHAR (100) NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([ID] ASC)
);

