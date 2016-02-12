CREATE TABLE [web].[Log] (
    [ID]      INT            IDENTITY (1, 1) NOT NULL,
    [Created] DATETIME       NULL,
    [Action]  VARCHAR (50)   NULL,
    [Message] VARCHAR (4000) NULL,
    CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED ([ID] ASC)
);

