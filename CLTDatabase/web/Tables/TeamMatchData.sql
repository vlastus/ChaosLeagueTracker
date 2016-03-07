CREATE TABLE [web].[TeamMatchData] (
    [ID]                INT IDENTITY (1, 1) NOT NULL,
    [Team]              INT NOT NULL,
    [Score]             INT CONSTRAINT [DF_TeamMatchData_Score] DEFAULT ((0)) NULL,
    [Gate]              INT CONSTRAINT [DF_TeamMatchData_Gate] DEFAULT ((0)) NULL,
    [Winnings]          INT CONSTRAINT [DF_TeamMatchData_Winnings] DEFAULT ((0)) NULL,
    [Fame]              INT CONSTRAINT [DF_TeamMatchData_Fame] DEFAULT ((0)) NULL,
    [FanFactorMod]      INT CONSTRAINT [DF_TeamMatchData_FanFactorMod] DEFAULT ((0)) NULL,
    [SpirallingExpense] INT CONSTRAINT [DF_TeamMatchData_SpirallingExpense] DEFAULT ((0)) NULL,
    [MVP]               INT NULL,
    CONSTRAINT [PK_TeamMatchData] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_TeamMatchData_Players] FOREIGN KEY ([MVP]) REFERENCES [web].[Players] ([ID]),
    CONSTRAINT [FK_TeamMatchData_Teams] FOREIGN KEY ([Team]) REFERENCES [web].[Teams] ([ID])
);



