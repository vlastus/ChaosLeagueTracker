CREATE VIEW web.GroupTables
AS
SELECT final.*, uu.Name 'Username', tt.Name 'Teamname'
FROM (
SELECT        g.ID, cti.Team, cti.Poradi, cti.Body, cti.TDfor, cti.TDagainst, cti.Zapasy
FROM            web.Groups g CROSS APPLY
                             (SELECT        ct.Team, ISNULL(gtable.Body, 0) 'Body', ISNULL(gtable.TDfor, 0) 'TDfor', ISNULL(gtable.TDagainst, 0) 'TDagainst', ISNULL(gtable.Zapasy, 0) 'Zapasy', ROW_NUMBER() OVER (ORDER BY gtable.Body
                                                          DESC, gtable.TDfor DESC) 'Poradi'
FROM            web.CompTeams ct LEFT JOIN
                             (SELECT        ptable.Team, GroupID, SUM(Points) 'Body', SUM(TDfor) 'TDfor', SUM(TDagainst) 'TDagainst', COUNT(*) 'Zapasy'
                               FROM            web.CompTeams ctx LEFT JOIN
                                                             (SELECT        g.ID 'GroupID', tmd1.Team, CASE WHEN tmd1.Score > tmd2.Score THEN 3 WHEN tmd1.Score = tmd2.Score THEN 1 ELSE 0 END 'Points', tmd1.Score 'TDfor', 
                                                                                         tmd2.Score 'TDagainst'
                                                               FROM            web.Groups g INNER JOIN
                                                                                         web.Matches m ON m.Competition = g.ID INNER JOIN
                                                                                         web.TeamMatchData tmd1 ON tmd1.ID = m.Team1Data INNER JOIN
                                                                                         web.TeamMatchData tmd2 ON tmd2.ID = m.Team2Data
                                                               UNION
                                                               SELECT        g.ID 'GroupID', tmd2.Team, CASE WHEN tmd2.Score > tmd1.Score THEN 3 WHEN tmd1.Score = tmd2.Score THEN 1 ELSE 0 END 'T2Points', tmd2.Score 'TDfor', 
                                                                                        tmd1.Score 'TDagainst'
                                                               FROM            web.Groups g INNER JOIN
                                                                                        web.Matches m ON m.Competition = g.ID INNER JOIN
                                                                                        web.TeamMatchData tmd1 ON tmd1.ID = m.Team1Data INNER JOIN
                                                                                        web.TeamMatchData tmd2 ON tmd2.ID = m.Team2Data) ptable ON ptable.Team = ctx.Team
                               GROUP BY GroupID, ptable.Team) gtable ON ct.Competititon = gtable.GroupID AND ct.Team = gtable.Team
WHERE        ct.Competititon = g.ID) cti) final
JOIN web.Teams tt ON tt.ID = final.Team
JOIN web.Users uu ON uu.ID = tt.Owner
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'web', @level1type = N'VIEW', @level1name = N'GroupTables';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'web', @level1type = N'VIEW', @level1name = N'GroupTables';

