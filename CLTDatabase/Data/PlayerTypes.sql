/*
USE [BLOODBOWL]
GO
SET IDENTITY_INSERT [web].[PlayerTypes] ON 

GO
INSERT [web].[PlayerTypes] ([ID], [Race], [Name], [MA], [ST], [AG], [AV], [Value], [Skillset]) VALUES (1, 10, N'Blitzer', 7, 3, 3, 8, 90000, N'101')
GO
INSERT [web].[PlayerTypes] ([ID], [Race], [Name], [MA], [ST], [AG], [AV], [Value], [Skillset]) VALUES (3, 10, N'Lineman', 6, 3, 3, 8, 50000, NULL)
GO
INSERT [web].[PlayerTypes] ([ID], [Race], [Name], [MA], [ST], [AG], [AV], [Value], [Skillset]) VALUES (4, 10, N'Catcher', 8, 2, 3, 7, 70000, N'301|304')
GO
INSERT [web].[PlayerTypes] ([ID], [Race], [Name], [MA], [ST], [AG], [AV], [Value], [Skillset]) VALUES (5, 10, N'Thrower', 6, 3, 3, 8, 70000, N'112|406')
GO
INSERT [web].[PlayerTypes] ([ID], [Race], [Name], [MA], [ST], [AG], [AV], [Value], [Skillset]) VALUES (6, 10, N'Ogre', 5, 5, 2, 9, 140000, N'205|210|606|611|622')
GO
SET IDENTITY_INSERT [web].[PlayerTypes] OFF
GO
*/