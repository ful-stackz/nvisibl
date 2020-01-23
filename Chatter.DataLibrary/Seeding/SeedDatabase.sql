DELETE FROM [dbo].[Users];
DELETE FROM [dbo].[Friends];

SET IDENTITY_INSERT [dbo].[Users] ON
INSERT INTO [dbo].[Users] ([Id], [Username]) VALUES (1, N'John')
INSERT INTO [dbo].[Users] ([Id], [Username]) VALUES (2, N'Jane')
INSERT INTO [dbo].[Users] ([Id], [Username]) VALUES (3, N'Mike')
INSERT INTO [dbo].[Users] ([Id], [Username]) VALUES (4, N'Alice')
SET IDENTITY_INSERT [dbo].[Users] OFF

INSERT INTO [dbo].[Friends] ([User1Id], [User2Id]) VALUES (1, 2)
INSERT INTO [dbo].[Friends] ([User1Id], [User2Id]) VALUES (1, 3)
INSERT INTO [dbo].[Friends] ([User1Id], [User2Id]) VALUES (1, 4)
INSERT INTO [dbo].[Friends] ([User1Id], [User2Id]) VALUES (2, 3)
INSERT INTO [dbo].[Friends] ([User1Id], [User2Id]) VALUES (2, 4)
INSERT INTO [dbo].[Friends] ([User1Id], [User2Id]) VALUES (3, 4)
