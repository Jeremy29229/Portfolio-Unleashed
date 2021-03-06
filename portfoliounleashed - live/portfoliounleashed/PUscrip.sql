-- Script Date: 6/11/2014 2:50 PM  - ErikEJ.SqlCeScripting version 3.5.2.38
-- Database information:
-- Locale Identifier: 1033
-- Encryption Mode: 
-- Case Sensitive: False
-- Database: C:\Users\dchetney\Documents\7th Quarter\Dot web\PortfolioUnleashed\PortfolioUnleashed\PortfolioUnleashed\App_Data\PortfolioUnleashed.sdf
-- ServerVersion: 4.0.8876.1
-- DatabaseSize: 468 KB
-- SpaceAvailable: 3.999 GB
-- Created: 6/11/2014 2:49 PM

-- User Table information:
-- Number of tables: 19
-- ContactInfo: 3 row(s)
-- Contribution: 7 row(s)
-- ContributionMedium: 5 row(s)
-- Education: 2 row(s)
-- FeaturedPortfolio: 2 row(s)
-- Link: 3 row(s)
-- Notification: 41 row(s)
-- Portfolio: 13 row(s)
-- Project: 5 row(s)
-- Project_Portfolio: 3 row(s)
-- ProjectMedium: 4 row(s)
-- ProjectPermission: 6 row(s)
-- QuickReference: 2 row(s)
-- User: 5 row(s)
-- UserSetting: 5 row(s)
-- webpages_Membership: 5 row(s)
-- webpages_OAuthMembership: 0 row(s)
-- webpages_Roles: 2 row(s)
-- webpages_UsersInRoles: 5 row(s)

CREATE TABLE [webpages_Roles] (
  [RoleId] int IDENTITY (3,1) NOT NULL
, [RoleName] nvarchar(256) NOT NULL
);
GO
CREATE TABLE [webpages_OAuthMembership] (
  [Provider] nvarchar(30) NOT NULL
, [ProviderUserId] nvarchar(100) NOT NULL
, [UserId] int NOT NULL
);
GO
CREATE TABLE [webpages_Membership] (
  [UserId] int NOT NULL
, [CreateDate] datetime NULL
, [ConfirmationToken] nvarchar(128) NULL
, [IsConfirmed] bit DEFAULT 0 NULL
, [LastPasswordFailureDate] datetime NULL
, [PasswordFailuresSinceLastSuccess] int DEFAULT 0 NOT NULL
, [Password] nvarchar(128) NOT NULL
, [PasswordChangedDate] datetime NULL
, [PasswordSalt] nvarchar(128) NOT NULL
, [PasswordVerificationToken] nvarchar(128) NULL
, [PasswordVerificationTokenExpirationDate] datetime NULL
);
GO
CREATE TABLE [UserSetting] (
  [Id] int IDENTITY (25,1) NOT NULL
, [UserId] int NOT NULL
, [IsPublic] bit DEFAULT 1 NOT NULL
, [IsEmailedForAdminMessage] bit DEFAULT 0 NOT NULL
, [IsEmailedForCollaborationAddition] bit DEFAULT 0 NOT NULL
, [IsEmailedForCollaborationRemoval] bit DEFAULT 0 NOT NULL
, [IsEmailedForCollaborationRequest] bit DEFAULT 0 NOT NULL
, [IsEmailedForViewReport] bit DEFAULT 0 NOT NULL
, [IsEmailedForGlobalAccouncement] bit DEFAULT 0 NOT NULL
, [IsEmailedForLeaveCard] bit DEFAULT 0 NOT NULL
, [IsEmailedForPortfolioFeedback] bit DEFAULT 0 NOT NULL
);
GO
CREATE TABLE [User] (
  [Id] int IDENTITY (15,1) NOT NULL
, [FirstName] nvarchar(50) NULL
, [LastName] nvarchar(50) NULL
, [Email] nvarchar(254) NOT NULL
, [LastDailyReportSendTime] datetime NULL
, [TotalViews] int DEFAULT 0 NOT NULL
, [ViewsSinceLastReport] int DEFAULT 0 NOT NULL
, [UserSettingId] int NULL
);
GO
CREATE TABLE [webpages_UsersInRoles] (
  [UserId] int NOT NULL
, [RoleId] int NOT NULL
);
GO
CREATE TABLE [QuickReference] (
  [QuickReferenceId] int NOT NULL
, [UserId] int NOT NULL
);
GO
CREATE TABLE [Project] (
  [Id] int IDENTITY (22,1) NOT NULL
, [Title] nvarchar(200) NULL
, [Description] nvarchar(2000) NULL
, [Template] int NOT NULL
, [CollaborationSetting] int NOT NULL
);
GO
CREATE TABLE [ProjectPermission] (
  [UserId] int NOT NULL
, [ProjectId] int NOT NULL
, [IsProjectEditor] bit NOT NULL
, [IsProjectMaster] bit DEFAULT 0 NOT NULL
);
GO
CREATE TABLE [ProjectMedium] (
  [Id] int IDENTITY (7,1) NOT NULL
, [ProjectId] int NOT NULL
, [MediumType] int NOT NULL
, [Caption] nvarchar(200) NULL
, [Link] nvarchar(2000) NULL
);
GO
CREATE TABLE [Portfolio] (
  [Id] int IDENTITY (22,1) NOT NULL
, [UserId] int NOT NULL
, [Title] nvarchar(200) NULL
, [Description] nvarchar(2000) NULL
, [Visibility] int NOT NULL
, [IsMainPortfolio] bit NOT NULL
, [URL] nvarchar(2000) NULL
);
GO
CREATE TABLE [Project_Portfolio] (
  [ProjectId] int NOT NULL
, [PortfolioId] int NOT NULL
);
GO
CREATE TABLE [Notification] (
  [Id] int IDENTITY (148,1) NOT NULL
, [UserId] int NOT NULL
, [isSeen] bit NOT NULL
, [Title] nvarchar(500) NULL
, [Description] nvarchar(1000) NULL
, [URL] nvarchar(2000) NULL
, [Sender] nvarchar(256) NULL
, [NotificationType] int NOT NULL
, [TimeStamp] datetime NOT NULL
, [SenderId] int NULL
);
GO
CREATE TABLE [Link] (
  [Id] int IDENTITY (4,1) NOT NULL
, [DisplayText] nvarchar(200) NULL
, [URL] nvarchar(2000) NULL
, [UserId] int NOT NULL
);
GO
CREATE TABLE [FeaturedPortfolio] (
  [Id] int IDENTITY (3,1) NOT NULL
, [PortfolioId] int NOT NULL
);
GO
CREATE TABLE [Education] (
  [Id] int IDENTITY (3,1) NOT NULL
, [School] nvarchar(50) NULL
, [Degree] nvarchar(50) NULL
, [StartYear] smallint NULL
, [EndYear] smallint NULL
, [UserId] int NOT NULL
);
GO
CREATE TABLE [Contribution] (
  [UserId] int NOT NULL
, [ProjectId] int NOT NULL
, [Title] nvarchar(200) NULL
, [Description] nvarchar(2000) NULL
);
GO
CREATE TABLE [ContributionMedium] (
  [MediumType] int NOT NULL
, [Caption] nvarchar(200) NULL
, [Link] nvarchar(2000) NULL
, [UserId] int NOT NULL
, [ProjectId] int NOT NULL
, [Id] int IDENTITY (9,1) NOT NULL
);
GO
CREATE TABLE [ContactInfo] (
  [Id] int IDENTITY (4,1) NOT NULL
, [Title] nvarchar(200) NULL
, [Information] nvarchar(2000) NULL
, [UserId] int NOT NULL
);
GO
SET IDENTITY_INSERT [webpages_Roles] ON;
GO
INSERT INTO [webpages_Roles] ([RoleId],[RoleName]) VALUES (1,N'Admin');
GO
INSERT INTO [webpages_Roles] ([RoleId],[RoleName]) VALUES (2,N'User');
GO
SET IDENTITY_INSERT [webpages_Roles] OFF;
GO
INSERT INTO [webpages_Membership] ([UserId],[CreateDate],[ConfirmationToken],[IsConfirmed],[LastPasswordFailureDate],[PasswordFailuresSinceLastSuccess],[Password],[PasswordChangedDate],[PasswordSalt],[PasswordVerificationToken],[PasswordVerificationTokenExpirationDate]) VALUES (5,{ts '2014-05-28 20:21:27.247'},NULL,1,{ts '2014-06-02 08:51:25.627'},0,N'AHWGS9zZfGMqnEPrvRGyJfQTsBHv9ag/1UU+ISKVAQqJ3xITOa1tZViWtR8NgN8zEA==',{ts '2014-05-28 20:21:27.247'},N'',NULL,NULL);
GO
INSERT INTO [webpages_Membership] ([UserId],[CreateDate],[ConfirmationToken],[IsConfirmed],[LastPasswordFailureDate],[PasswordFailuresSinceLastSuccess],[Password],[PasswordChangedDate],[PasswordSalt],[PasswordVerificationToken],[PasswordVerificationTokenExpirationDate]) VALUES (8,{ts '2014-05-30 20:19:00.547'},NULL,1,NULL,0,N'ALI9lwBMlNOJAu5IRtuDytyZ12p9XGh8Am79lYncm6fXq6zZA18K6Dm1GKIB5bg2iA==',{ts '2014-05-30 20:19:00.547'},N'',NULL,NULL);
GO
INSERT INTO [webpages_Membership] ([UserId],[CreateDate],[ConfirmationToken],[IsConfirmed],[LastPasswordFailureDate],[PasswordFailuresSinceLastSuccess],[Password],[PasswordChangedDate],[PasswordSalt],[PasswordVerificationToken],[PasswordVerificationTokenExpirationDate]) VALUES (9,{ts '2014-06-02 05:57:46.437'},NULL,1,{ts '2014-06-08 23:43:23.877'},0,N'ALsH0UA1jM7YsSW2yWFifEwqWKpWkYriN2OfcGme+PG4fOJPAUqKbyG2dBKsTwdLBQ==',{ts '2014-06-02 05:57:46.437'},N'',NULL,NULL);
GO
INSERT INTO [webpages_Membership] ([UserId],[CreateDate],[ConfirmationToken],[IsConfirmed],[LastPasswordFailureDate],[PasswordFailuresSinceLastSuccess],[Password],[PasswordChangedDate],[PasswordSalt],[PasswordVerificationToken],[PasswordVerificationTokenExpirationDate]) VALUES (13,{ts '2014-06-07 04:15:13.197'},N'4PT9EcJqxdKUwS5h6-A3kQ2',1,{ts '2014-06-07 04:38:29.533'},0,N'AEzd+tLgwTEx4xuWZi+Xa9w09DPwglKT3FV4lh+Qms/e1pHReQI82FKdooUyrF5Vhw==',{ts '2014-06-07 04:15:13.197'},N'',NULL,NULL);
GO
INSERT INTO [webpages_Membership] ([UserId],[CreateDate],[ConfirmationToken],[IsConfirmed],[LastPasswordFailureDate],[PasswordFailuresSinceLastSuccess],[Password],[PasswordChangedDate],[PasswordSalt],[PasswordVerificationToken],[PasswordVerificationTokenExpirationDate]) VALUES (14,{ts '2014-06-08 23:33:20.253'},N'4ACr6Gd6C4_hIdL1kMN7tQ2',1,NULL,0,N'AEBKdZw0uUwKU4hG+SA3ZiXHSnNrR+wNQoJBofAif6iEkMYEV5ihL6Vlpc/5o5J9YA==',{ts '2014-06-08 23:33:20.253'},N'',NULL,NULL);
GO
SET IDENTITY_INSERT [UserSetting] ON;
GO
INSERT INTO [UserSetting] ([Id],[UserId],[IsPublic],[IsEmailedForAdminMessage],[IsEmailedForCollaborationAddition],[IsEmailedForCollaborationRemoval],[IsEmailedForCollaborationRequest],[IsEmailedForViewReport],[IsEmailedForGlobalAccouncement],[IsEmailedForLeaveCard],[IsEmailedForPortfolioFeedback]) VALUES (20,5,0,0,0,0,0,0,0,0,0);
GO
INSERT INTO [UserSetting] ([Id],[UserId],[IsPublic],[IsEmailedForAdminMessage],[IsEmailedForCollaborationAddition],[IsEmailedForCollaborationRemoval],[IsEmailedForCollaborationRequest],[IsEmailedForViewReport],[IsEmailedForGlobalAccouncement],[IsEmailedForLeaveCard],[IsEmailedForPortfolioFeedback]) VALUES (21,8,1,0,0,0,0,0,0,0,0);
GO
INSERT INTO [UserSetting] ([Id],[UserId],[IsPublic],[IsEmailedForAdminMessage],[IsEmailedForCollaborationAddition],[IsEmailedForCollaborationRemoval],[IsEmailedForCollaborationRequest],[IsEmailedForViewReport],[IsEmailedForGlobalAccouncement],[IsEmailedForLeaveCard],[IsEmailedForPortfolioFeedback]) VALUES (22,9,1,0,0,0,0,0,0,0,0);
GO
INSERT INTO [UserSetting] ([Id],[UserId],[IsPublic],[IsEmailedForAdminMessage],[IsEmailedForCollaborationAddition],[IsEmailedForCollaborationRemoval],[IsEmailedForCollaborationRequest],[IsEmailedForViewReport],[IsEmailedForGlobalAccouncement],[IsEmailedForLeaveCard],[IsEmailedForPortfolioFeedback]) VALUES (23,13,1,0,0,0,0,0,0,0,0);
GO
INSERT INTO [UserSetting] ([Id],[UserId],[IsPublic],[IsEmailedForAdminMessage],[IsEmailedForCollaborationAddition],[IsEmailedForCollaborationRemoval],[IsEmailedForCollaborationRequest],[IsEmailedForViewReport],[IsEmailedForGlobalAccouncement],[IsEmailedForLeaveCard],[IsEmailedForPortfolioFeedback]) VALUES (24,14,1,0,0,0,0,0,0,0,0);
GO
SET IDENTITY_INSERT [UserSetting] OFF;
GO
SET IDENTITY_INSERT [User] ON;
GO
INSERT INTO [User] ([Id],[FirstName],[LastName],[Email],[LastDailyReportSendTime],[TotalViews],[ViewsSinceLastReport],[UserSettingId]) VALUES (5,N'Jeremy',N'Jones',N'jeremy29229@yahoo.com',{ts '2014-06-09 08:40:52.590'},9,0,20);
GO
INSERT INTO [User] ([Id],[FirstName],[LastName],[Email],[LastDailyReportSendTime],[TotalViews],[ViewsSinceLastReport],[UserSettingId]) VALUES (8,N'User',N'Test',N'User@test.com',{ts '2014-06-10 03:31:19.147'},1,0,21);
GO
INSERT INTO [User] ([Id],[FirstName],[LastName],[Email],[LastDailyReportSendTime],[TotalViews],[ViewsSinceLastReport],[UserSettingId]) VALUES (9,N'Testy',N'McTestTest',N'testy@test.com',{ts '2014-06-08 23:43:28.443'},2,0,22);
GO
INSERT INTO [User] ([Id],[FirstName],[LastName],[Email],[LastDailyReportSendTime],[TotalViews],[ViewsSinceLastReport],[UserSettingId]) VALUES (13,N'Admin',N'Test',N'admin@test.com',{ts '2014-06-08 22:34:36.217'},5,1,23);
GO
INSERT INTO [User] ([Id],[FirstName],[LastName],[Email],[LastDailyReportSendTime],[TotalViews],[ViewsSinceLastReport],[UserSettingId]) VALUES (14,N'User',N'Greg',N'UserGreg@test.com',{ts '2014-06-10 06:07:39.790'},15,0,24);
GO
SET IDENTITY_INSERT [User] OFF;
GO
INSERT INTO [webpages_UsersInRoles] ([UserId],[RoleId]) VALUES (5,1);
GO
INSERT INTO [webpages_UsersInRoles] ([UserId],[RoleId]) VALUES (8,2);
GO
INSERT INTO [webpages_UsersInRoles] ([UserId],[RoleId]) VALUES (9,2);
GO
INSERT INTO [webpages_UsersInRoles] ([UserId],[RoleId]) VALUES (13,1);
GO
INSERT INTO [webpages_UsersInRoles] ([UserId],[RoleId]) VALUES (14,2);
GO
INSERT INTO [QuickReference] ([QuickReferenceId],[UserId]) VALUES (9,8);
GO
INSERT INTO [QuickReference] ([QuickReferenceId],[UserId]) VALUES (14,8);
GO
SET IDENTITY_INSERT [Project] ON;
GO
INSERT INTO [Project] ([Id],[Title],[Description],[Template],[CollaborationSetting]) VALUES (13,N'Bunny Home',N'In this project I built a bunny home. Every rabbit needs a place to call home; but for an owner, creating the perfect living space can be a bit of a head ache. How big should a hutch be? What''s the best materials? Is indoors better than out? Your rabbit''s accommodation is its whole world. Your rabbit cannot hop on the bus to the gym for exercise, pop to the cinema when bored or visit the shops to pick out a new bed to sleep in. As a rabbit owner it is your responsibility to make sure that your rabbit''s accommodation meets all of its physical and mental needs.',0,0);
GO
INSERT INTO [Project] ([Id],[Title],[Description],[Template],[CollaborationSetting]) VALUES (14,N'Birdy Home',N'This project involved creating a large scale aviary for a local bird owner. The owner had 6 birds at the time and would recently be getting more, and so, wanted a larger space to house them all. Every aviary is going to be different, depending on the species of birds you keep in it, your climate and whether you are breeding birds or simply providing an outdoor play area for your pet. Some people prefer a patio or solarium-style of aviary connected to their house. Others opt for a free-standing facility. Some aviaries resemble a greenhouse, while others look more like a barn or chicken coop. In general, outdoor aviaries are much more spacious than traditional cages, and they allow birds more space to move around and exercise. They offer a more natural environment and give pet birds exposure to fresh air and unfiltered sunlight — an important source of vitamin D3.',1,2);
GO
INSERT INTO [Project] ([Id],[Title],[Description],[Template],[CollaborationSetting]) VALUES (15,N'Aquarium k',N'Keeping an aquarium can provide immeasurable rewards and satisfaction. It does, however, require some work as well, and before you venture into the hobby, you''ll need some fundamental information. The best way to begin is with a basic understanding of what happens in a successful aquarium. You''ll also need to know how to select a good aquarium store, one that can provide you with reliable equipment, service, and advice. Finally, you''ll need to take the first step in planning your aquarium: selecting a tank and a suitable location for it. Before setting up your first aquarium, be sure you have time to care for it, and do some research. Aquarium fish originate in different parts of the world, so they have different needs and behaviors. If you find out about the fish you want to keep in your aquarium, you will be much less likely to have compatibility issues after making purchases. Internet search engines like Google are good sources of fish species profiles. Preferred water parameters plus adult sizes and behaviors are essential factors to consider.',2,1);
GO
INSERT INTO [Project] ([Id],[Title],[Description],[Template],[CollaborationSetting]) VALUES (16,N'Project With Permissions',N'User Test is a Project Editor of this project, in addition to other contributors who are editors.',0,2);
GO
INSERT INTO [Project] ([Id],[Title],[Description],[Template],[CollaborationSetting]) VALUES (17,N'Project with Full Contributions',N'This project has contributions that include contribution details and content',0,0);
GO
SET IDENTITY_INSERT [Project] OFF;
GO
INSERT INTO [ProjectPermission] ([UserId],[ProjectId],[IsProjectEditor],[IsProjectMaster]) VALUES (8,17,1,0);
GO
INSERT INTO [ProjectPermission] ([UserId],[ProjectId],[IsProjectEditor],[IsProjectMaster]) VALUES (14,13,1,0);
GO
INSERT INTO [ProjectPermission] ([UserId],[ProjectId],[IsProjectEditor],[IsProjectMaster]) VALUES (14,14,1,0);
GO
INSERT INTO [ProjectPermission] ([UserId],[ProjectId],[IsProjectEditor],[IsProjectMaster]) VALUES (14,15,1,0);
GO
INSERT INTO [ProjectPermission] ([UserId],[ProjectId],[IsProjectEditor],[IsProjectMaster]) VALUES (14,16,1,0);
GO
INSERT INTO [ProjectPermission] ([UserId],[ProjectId],[IsProjectEditor],[IsProjectMaster]) VALUES (14,17,1,0);
GO
SET IDENTITY_INSERT [ProjectMedium] ON;
GO
INSERT INTO [ProjectMedium] ([Id],[ProjectId],[MediumType],[Caption],[Link]) VALUES (3,13,0,N'One sample rabbit home',N'http://www.therabbithouse.com/images/rabbitcage.gif');
GO
INSERT INTO [ProjectMedium] ([Id],[ProjectId],[MediumType],[Caption],[Link]) VALUES (4,14,0,N'Inside the aviary',N'http://www.birdchannel.com/images/bird-aviary-inside.jpg');
GO
INSERT INTO [ProjectMedium] ([Id],[ProjectId],[MediumType],[Caption],[Link]) VALUES (5,15,0,N'Fish',N'http://www.myaquariumclub.com/images/fbfiles/images/fish3_026.JPG');
GO
INSERT INTO [ProjectMedium] ([Id],[ProjectId],[MediumType],[Caption],[Link]) VALUES (6,17,0,N'This image is me doing a cool thing.',N'http://www.w3schools.com/images/pulpit.jpg');
GO
SET IDENTITY_INSERT [ProjectMedium] OFF;
GO
SET IDENTITY_INSERT [Portfolio] ON;
GO
INSERT INTO [Portfolio] ([Id],[UserId],[Title],[Description],[Visibility],[IsMainPortfolio],[URL]) VALUES (1,5,N'Test Portfolio',N'Maybe later.',0,0,NULL);
GO
INSERT INTO [Portfolio] ([Id],[UserId],[Title],[Description],[Visibility],[IsMainPortfolio],[URL]) VALUES (2,5,N'The Best Around',N'For the first time since 1967, when the City Council first banned the sale and growing of germ, you can get Cereal Germ. Once milk is legalized, you might be able to enjoy this breakfast treat in its original form.',0,1,NULL);
GO
INSERT INTO [Portfolio] ([Id],[UserId],[Title],[Description],[Visibility],[IsMainPortfolio],[URL]) VALUES (10,5,N'Black Beard''s Booty',N'I make all the best decisions, here are some of the dicisions that I made. I am a leader.',0,0,NULL);
GO
INSERT INTO [Portfolio] ([Id],[UserId],[Title],[Description],[Visibility],[IsMainPortfolio],[URL]) VALUES (12,5,N'The Expended 3',N'An Oculus Rift first person biography of Ray Charles.',0,0,NULL);
GO
INSERT INTO [Portfolio] ([Id],[UserId],[Title],[Description],[Visibility],[IsMainPortfolio],[URL]) VALUES (13,5,N'All in the Family part 2',N'Im a great guy who just needs to find a bro to give my couch to. It''s in great condition, like me. It has burgundy uphosltry, and short, teakwood legs. Still nice and firm after years of use. Seats 3 but I''ve managed to fit 5 guys on it. Call if interested.',0,0,NULL);
GO
INSERT INTO [Portfolio] ([Id],[UserId],[Title],[Description],[Visibility],[IsMainPortfolio],[URL]) VALUES (14,5,N'All in the Family part 2',N'Im a great guy who just needs to find a bro to give my couch to. It''s in great condition, like me. It has burgundy uphosltry, and short, teakwood legs. Still nice and firm after years of use. Seats 3 but I''ve managed to fit 5 guys on it. Call if interested.',0,0,NULL);
GO
INSERT INTO [Portfolio] ([Id],[UserId],[Title],[Description],[Visibility],[IsMainPortfolio],[URL]) VALUES (15,5,N'The Reckoning Redemption: Revelations',N'A first person, sandbox puzzle shooter with moral choices. Better with Kinect!',0,0,NULL);
GO
INSERT INTO [Portfolio] ([Id],[UserId],[Title],[Description],[Visibility],[IsMainPortfolio],[URL]) VALUES (16,5,N'Je Suis Pretentious',N'Hi! My name is (name), and I (occupation). *Place examples of your work below*',0,0,NULL);
GO
INSERT INTO [Portfolio] ([Id],[UserId],[Title],[Description],[Visibility],[IsMainPortfolio],[URL]) VALUES (17,5,N'The Expended 3',N'An Oculus Rift first person biography of Ray Charles.',0,0,NULL);
GO
INSERT INTO [Portfolio] ([Id],[UserId],[Title],[Description],[Visibility],[IsMainPortfolio],[URL]) VALUES (18,5,N'Fish Quest Online Unleashed',N'A gurl was walkin2 skewel wit her bf n they were crossin da rode. she se "bbz will u luv me 4evr" he said "NO.."" da gurl cryed N ran across da rode b4 da green man came on the sine. boy was cryin and wnet 2 pic up her body. she was ded. he whispered 2 her corpse "I ment 2 sey i will luv u FIVE-ever..." (dat mean he luv her moar den 4evr)
xxx~*...LIKE DIS IF U CRY EVERY TIM...*~xxx',0,0,NULL);
GO
INSERT INTO [Portfolio] ([Id],[UserId],[Title],[Description],[Visibility],[IsMainPortfolio],[URL]) VALUES (19,5,N'Fish Quest Online Unleashed',N'A gurl was walkin2 skewel wit her bf n they were crossin da rode. she se "bbz will u luv me 4evr" he said "NO.."" da gurl cryed N ran across da rode b4 da green man came on the sine. boy was cryin and wnet 2 pic up her body. she was ded. he whispered 2 her corpse "I ment 2 sey i will luv u FIVE-ever..." (dat mean he luv her moar den 4evr)
xxx~*...LIKE DIS IF U CRY EVERY TIM...*~xxx',0,0,NULL);
GO
INSERT INTO [Portfolio] ([Id],[UserId],[Title],[Description],[Visibility],[IsMainPortfolio],[URL]) VALUES (20,14,N'Animal Home Construction',N'This portfolio is specifically for showcasing animal homes.',0,1,NULL);
GO
INSERT INTO [Portfolio] ([Id],[UserId],[Title],[Description],[Visibility],[IsMainPortfolio],[URL]) VALUES (21,5,N'The Reckoning Redemption: Revelations',N'A first person, sandbox puzzle shooter with moral choices. Better with Kinect!',0,0,NULL);
GO
SET IDENTITY_INSERT [Portfolio] OFF;
GO
INSERT INTO [Project_Portfolio] ([ProjectId],[PortfolioId]) VALUES (13,20);
GO
INSERT INTO [Project_Portfolio] ([ProjectId],[PortfolioId]) VALUES (14,20);
GO
INSERT INTO [Project_Portfolio] ([ProjectId],[PortfolioId]) VALUES (15,20);
GO
SET IDENTITY_INSERT [Notification] ON;
GO
INSERT INTO [Notification] ([Id],[UserId],[isSeen],[Title],[Description],[URL],[Sender],[NotificationType],[TimeStamp],[SenderId]) VALUES (84,5,1,N'Announcement',N'This is a test announcement.',NULL,N'Admin Jeremy Jones',4,{ts '2014-06-07 04:43:22.617'},5);
GO
INSERT INTO [Notification] ([Id],[UserId],[isSeen],[Title],[Description],[URL],[Sender],[NotificationType],[TimeStamp],[SenderId]) VALUES (86,9,0,N'Announcement',N'This is a test announcement.',NULL,N'Admin Jeremy Jones',4,{ts '2014-06-07 04:43:22.730'},5);
GO
INSERT INTO [Notification] ([Id],[UserId],[isSeen],[Title],[Description],[URL],[Sender],[NotificationType],[TimeStamp],[SenderId]) VALUES (87,13,0,N'Announcement',N'This is a test announcement.',NULL,N'Admin Jeremy Jones',4,{ts '2014-06-07 04:43:22.753'},5);
GO
INSERT INTO [Notification] ([Id],[UserId],[isSeen],[Title],[Description],[URL],[Sender],[NotificationType],[TimeStamp],[SenderId]) VALUES (88,5,1,N'Announcement',N'Hello, users of Portfolio Unleashed. There is planned maintenance on the site at 5:00AM eastern standard time on 6/7/14. We expect this downtime to last no more than 30 minutes.',NULL,N'Admin Jeremy Jones',4,{ts '2014-06-07 04:53:41.413'},5);
GO
INSERT INTO [Notification] ([Id],[UserId],[isSeen],[Title],[Description],[URL],[Sender],[NotificationType],[TimeStamp],[SenderId]) VALUES (90,9,0,N'Announcement',N'Hello, users of Portfolio Unleashed. There is planned maintenance on the site at 5:00AM eastern standard time on 6/7/14. We expect this downtime to last no more than 30 minutes.',NULL,N'Admin Jeremy Jones',4,{ts '2014-06-07 04:53:41.680'},5);
GO
INSERT INTO [Notification] ([Id],[UserId],[isSeen],[Title],[Description],[URL],[Sender],[NotificationType],[TimeStamp],[SenderId]) VALUES (91,13,0,N'Announcement',N'Hello, users of Portfolio Unleashed. There is planned maintenance on the site at 5:00AM eastern standard time on 6/7/14. We expect this downtime to last no more than 30 minutes.',NULL,N'Admin Jeremy Jones',4,{ts '2014-06-07 04:53:41.703'},5);
GO
INSERT INTO [Notification] ([Id],[UserId],[isSeen],[Title],[Description],[URL],[Sender],[NotificationType],[TimeStamp],[SenderId]) VALUES (92,5,1,N'Announcement',N'Nope',NULL,N'Admin Jeremy Jones',4,{ts '2014-06-07 23:45:15.637'},5);
GO
INSERT INTO [Notification] ([Id],[UserId],[isSeen],[Title],[Description],[URL],[Sender],[NotificationType],[TimeStamp],[SenderId]) VALUES (94,9,0,N'Announcement',N'Nope',NULL,N'Admin Jeremy Jones',4,{ts '2014-06-07 23:45:15.893'},5);
GO
INSERT INTO [Notification] ([Id],[UserId],[isSeen],[Title],[Description],[URL],[Sender],[NotificationType],[TimeStamp],[SenderId]) VALUES (95,13,0,N'Announcement',N'Nope',NULL,N'Admin Jeremy Jones',4,{ts '2014-06-07 23:45:15.913'},5);
GO
INSERT INTO [Notification] ([Id],[UserId],[isSeen],[Title],[Description],[URL],[Sender],[NotificationType],[TimeStamp],[SenderId]) VALUES (96,5,1,N'View Report',N'Greetings! You have received 7 views since 6/6/2014 12:00:00 AM and 7 total views on your profile and portfolios.',NULL,N'Automated',2,{ts '2014-06-08 04:57:33.147'},NULL);
GO
INSERT INTO [Notification] ([Id],[UserId],[isSeen],[Title],[Description],[URL],[Sender],[NotificationType],[TimeStamp],[SenderId]) VALUES (97,5,1,N'Announcement',N'Test',NULL,N'Admin Jeremy Jones',4,{ts '2014-06-08 07:53:07.837'},5);
GO
INSERT INTO [Notification] ([Id],[UserId],[isSeen],[Title],[Description],[URL],[Sender],[NotificationType],[TimeStamp],[SenderId]) VALUES (99,9,0,N'Announcement',N'Test',NULL,N'Admin Jeremy Jones',4,{ts '2014-06-08 07:53:08.087'},5);
GO
INSERT INTO [Notification] ([Id],[UserId],[isSeen],[Title],[Description],[URL],[Sender],[NotificationType],[TimeStamp],[SenderId]) VALUES (100,13,0,N'Announcement',N'Test',NULL,N'Admin Jeremy Jones',4,{ts '2014-06-08 07:53:08.107'},5);
GO
INSERT INTO [Notification] ([Id],[UserId],[isSeen],[Title],[Description],[URL],[Sender],[NotificationType],[TimeStamp],[SenderId]) VALUES (101,5,1,N'Announcement',N'n',NULL,N'Admin Jeremy Jones',4,{ts '2014-06-08 08:07:11.710'},5);
GO
INSERT INTO [Notification] ([Id],[UserId],[isSeen],[Title],[Description],[URL],[Sender],[NotificationType],[TimeStamp],[SenderId]) VALUES (103,9,0,N'Announcement',N'n',NULL,N'Admin Jeremy Jones',4,{ts '2014-06-08 08:07:11.830'},5);
GO
INSERT INTO [Notification] ([Id],[UserId],[isSeen],[Title],[Description],[URL],[Sender],[NotificationType],[TimeStamp],[SenderId]) VALUES (104,13,0,N'Announcement',N'n',NULL,N'Admin Jeremy Jones',4,{ts '2014-06-08 08:07:11.850'},5);
GO
INSERT INTO [Notification] ([Id],[UserId],[isSeen],[Title],[Description],[URL],[Sender],[NotificationType],[TimeStamp],[SenderId]) VALUES (105,5,1,N'Announcement',N'n
\
',NULL,N'Admin Jeremy Jones',4,{ts '2014-06-08 08:10:11.137'},5);
GO
INSERT INTO [Notification] ([Id],[UserId],[isSeen],[Title],[Description],[URL],[Sender],[NotificationType],[TimeStamp],[SenderId]) VALUES (107,9,0,N'Announcement',N'n
\
',NULL,N'Admin Jeremy Jones',4,{ts '2014-06-08 08:10:11.240'},5);
GO
INSERT INTO [Notification] ([Id],[UserId],[isSeen],[Title],[Description],[URL],[Sender],[NotificationType],[TimeStamp],[SenderId]) VALUES (108,13,0,N'Announcement',N'n
\
',NULL,N'Admin Jeremy Jones',4,{ts '2014-06-08 08:10:11.260'},5);
GO
INSERT INTO [Notification] ([Id],[UserId],[isSeen],[Title],[Description],[URL],[Sender],[NotificationType],[TimeStamp],[SenderId]) VALUES (109,5,1,N'Announcement',N'blah blah',NULL,N'Admin Jeremy Jones',4,{ts '2014-06-08 22:17:51.977'},5);
GO
INSERT INTO [Notification] ([Id],[UserId],[isSeen],[Title],[Description],[URL],[Sender],[NotificationType],[TimeStamp],[SenderId]) VALUES (111,9,0,N'Announcement',N'blah blah',NULL,N'Admin Jeremy Jones',4,{ts '2014-06-08 22:17:52.220'},5);
GO
INSERT INTO [Notification] ([Id],[UserId],[isSeen],[Title],[Description],[URL],[Sender],[NotificationType],[TimeStamp],[SenderId]) VALUES (112,13,0,N'Announcement',N'blah blah',NULL,N'Admin Jeremy Jones',4,{ts '2014-06-08 22:17:52.240'},5);
GO
INSERT INTO [Notification] ([Id],[UserId],[isSeen],[Title],[Description],[URL],[Sender],[NotificationType],[TimeStamp],[SenderId]) VALUES (113,13,0,N'View Report',N'Greetings! You have received 4 views since 6/6/2014 12:00:00 AM and 4 total views on your profile and portfolios.',NULL,N'Automated',2,{ts '2014-06-08 22:34:35.967'},NULL);
GO
INSERT INTO [Notification] ([Id],[UserId],[isSeen],[Title],[Description],[URL],[Sender],[NotificationType],[TimeStamp],[SenderId]) VALUES (118,9,0,N'You have been added to Project With Permissions',N'User Test has added you as a collaborator to Project With Permissions.',N'~/Project/ProjectCatalog/9',N'Automated',5,{ts '2014-06-08 23:41:12.753'},8);
GO
INSERT INTO [Notification] ([Id],[UserId],[isSeen],[Title],[Description],[URL],[Sender],[NotificationType],[TimeStamp],[SenderId]) VALUES (120,9,0,N'You have been added to Project with Full Contributions',N'User Test has added you as a collaborator to Project with Full Contributions.',N'~/Project/ProjectCatalog/9',N'Automated',5,{ts '2014-06-08 23:43:05.790'},8);
GO
INSERT INTO [Notification] ([Id],[UserId],[isSeen],[Title],[Description],[URL],[Sender],[NotificationType],[TimeStamp],[SenderId]) VALUES (122,9,0,N'View Report',N'Greetings! You have received 2 views since 6/6/2014 12:00:00 AM and 2 total views on your profile and portfolios.',NULL,N'Automated',2,{ts '2014-06-08 23:43:28.417'},NULL);
GO
INSERT INTO [Notification] ([Id],[UserId],[isSeen],[Title],[Description],[URL],[Sender],[NotificationType],[TimeStamp],[SenderId]) VALUES (128,5,1,N'View Report',N'Greetings! You have received 2 views since 6/7/2014 and 9 total views on your profile and portfolios.',NULL,N'Automated',2,{ts '2014-06-09 08:40:52.377'},NULL);
GO
INSERT INTO [Notification] ([Id],[UserId],[isSeen],[Title],[Description],[URL],[Sender],[NotificationType],[TimeStamp],[SenderId]) VALUES (134,14,0,N'You have been added to Test Project',N'User Test has added you as a collaborator to Test Project.',N'~/Project/ProjectCatalog/14',N'Automated',5,{ts '2014-06-09 20:10:16.777'},8);
GO
INSERT INTO [Notification] ([Id],[UserId],[isSeen],[Title],[Description],[URL],[Sender],[NotificationType],[TimeStamp],[SenderId]) VALUES (135,8,0,N'View Report',N'Greetings! You have received 1 views since 6/8/2014 and 1 total views on your profile and portfolios.',NULL,N'Automated',2,{ts '2014-06-10 03:31:18.903'},NULL);
GO
INSERT INTO [Notification] ([Id],[UserId],[isSeen],[Title],[Description],[URL],[Sender],[NotificationType],[TimeStamp],[SenderId]) VALUES (136,14,0,N'You have been added to TEEEEEEEST',N'User Test has added you as a collaborator to TEEEEEEEST.',N'~/Project/ProjectCatalog/14',N'Automated',5,{ts '2014-06-10 06:06:42.513'},8);
GO
INSERT INTO [Notification] ([Id],[UserId],[isSeen],[Title],[Description],[URL],[Sender],[NotificationType],[TimeStamp],[SenderId]) VALUES (137,14,0,N'View Report',N'Greetings! You have received 13 views since 6/8/2014 and 15 total views on your profile and portfolios.',NULL,N'Automated',2,{ts '2014-06-10 06:07:39.750'},NULL);
GO
INSERT INTO [Notification] ([Id],[UserId],[isSeen],[Title],[Description],[URL],[Sender],[NotificationType],[TimeStamp],[SenderId]) VALUES (138,5,1,N'Announcement 6/11/2014',N'You''re all towels!',NULL,N'Admin Jeremy Jones',4,{ts '2014-06-11 17:42:52.830'},5);
GO
INSERT INTO [Notification] ([Id],[UserId],[isSeen],[Title],[Description],[URL],[Sender],[NotificationType],[TimeStamp],[SenderId]) VALUES (139,8,0,N'Announcement 6/11/2014',N'You''re all towels!',NULL,N'Admin Jeremy Jones',4,{ts '2014-06-11 17:42:53.060'},5);
GO
INSERT INTO [Notification] ([Id],[UserId],[isSeen],[Title],[Description],[URL],[Sender],[NotificationType],[TimeStamp],[SenderId]) VALUES (140,9,0,N'Announcement 6/11/2014',N'You''re all towels!',NULL,N'Admin Jeremy Jones',4,{ts '2014-06-11 17:42:53.080'},5);
GO
INSERT INTO [Notification] ([Id],[UserId],[isSeen],[Title],[Description],[URL],[Sender],[NotificationType],[TimeStamp],[SenderId]) VALUES (141,13,0,N'Announcement 6/11/2014',N'You''re all towels!',NULL,N'Admin Jeremy Jones',4,{ts '2014-06-11 17:42:53.103'},5);
GO
INSERT INTO [Notification] ([Id],[UserId],[isSeen],[Title],[Description],[URL],[Sender],[NotificationType],[TimeStamp],[SenderId]) VALUES (142,14,0,N'Announcement 6/11/2014',N'You''re all towels!',NULL,N'Admin Jeremy Jones',4,{ts '2014-06-11 17:42:53.127'},5);
GO
INSERT INTO [Notification] ([Id],[UserId],[isSeen],[Title],[Description],[URL],[Sender],[NotificationType],[TimeStamp],[SenderId]) VALUES (143,5,1,N'Expected Downtime 6/11/2014',N'This are going down son.',NULL,N'Admin Jeremy Jones',4,{ts '2014-06-11 17:44:16.133'},5);
GO
INSERT INTO [Notification] ([Id],[UserId],[isSeen],[Title],[Description],[URL],[Sender],[NotificationType],[TimeStamp],[SenderId]) VALUES (144,8,0,N'Expected Downtime 6/11/2014',N'This are going down son.',NULL,N'Admin Jeremy Jones',4,{ts '2014-06-11 17:44:16.153'},5);
GO
INSERT INTO [Notification] ([Id],[UserId],[isSeen],[Title],[Description],[URL],[Sender],[NotificationType],[TimeStamp],[SenderId]) VALUES (145,9,0,N'Expected Downtime 6/11/2014',N'This are going down son.',NULL,N'Admin Jeremy Jones',4,{ts '2014-06-11 17:44:16.177'},5);
GO
INSERT INTO [Notification] ([Id],[UserId],[isSeen],[Title],[Description],[URL],[Sender],[NotificationType],[TimeStamp],[SenderId]) VALUES (146,13,0,N'Expected Downtime 6/11/2014',N'This are going down son.',NULL,N'Admin Jeremy Jones',4,{ts '2014-06-11 17:44:16.197'},5);
GO
INSERT INTO [Notification] ([Id],[UserId],[isSeen],[Title],[Description],[URL],[Sender],[NotificationType],[TimeStamp],[SenderId]) VALUES (147,14,0,N'Expected Downtime 6/11/2014',N'This are going down son.',NULL,N'Admin Jeremy Jones',4,{ts '2014-06-11 17:44:16.217'},5);
GO
SET IDENTITY_INSERT [Notification] OFF;
GO
SET IDENTITY_INSERT [Link] ON;
GO
INSERT INTO [Link] ([Id],[DisplayText],[URL],[UserId]) VALUES (1,N'Google',N'www.google.com',8);
GO
INSERT INTO [Link] ([Id],[DisplayText],[URL],[UserId]) VALUES (2,N'My site',N'www.google.com',8);
GO
INSERT INTO [Link] ([Id],[DisplayText],[URL],[UserId]) VALUES (3,N'My site',N'www.google.com',14);
GO
SET IDENTITY_INSERT [Link] OFF;
GO
SET IDENTITY_INSERT [FeaturedPortfolio] ON;
GO
INSERT INTO [FeaturedPortfolio] ([Id],[PortfolioId]) VALUES (1,20);
GO
INSERT INTO [FeaturedPortfolio] ([Id],[PortfolioId]) VALUES (2,19);
GO
SET IDENTITY_INSERT [FeaturedPortfolio] OFF;
GO
SET IDENTITY_INSERT [Education] ON;
GO
INSERT INTO [Education] ([Id],[School],[Degree],[StartYear],[EndYear],[UserId]) VALUES (1,N'Neumont',N'BSGD',2012,2015,8);
GO
INSERT INTO [Education] ([Id],[School],[Degree],[StartYear],[EndYear],[UserId]) VALUES (2,N'Hogwarts',N'Going to State',2000,3000,14);
GO
SET IDENTITY_INSERT [Education] OFF;
GO
INSERT INTO [Contribution] ([UserId],[ProjectId],[Title],[Description]) VALUES (9,16,NULL,NULL);
GO
INSERT INTO [Contribution] ([UserId],[ProjectId],[Title],[Description]) VALUES (9,17,N'Snack guy',N'I brought snacks');
GO
INSERT INTO [Contribution] ([UserId],[ProjectId],[Title],[Description]) VALUES (14,13,N'Cheif engineer',N' I was the overseer for the project as a whole. I held meetings, and was the primary decider when it came to making decisions regarding design, materials, and budget.');
GO
INSERT INTO [Contribution] ([UserId],[ProjectId],[Title],[Description]) VALUES (14,14,N'Cheif engineer',N'I oversaw all crucial design decisions and construction. I was heavily involved in creating the blueprints for the design, including the gathering of measurements and necessary dimensions.');
GO
INSERT INTO [Contribution] ([UserId],[ProjectId],[Title],[Description]) VALUES (14,15,N'Fish Food manager',N' Feeding - Over feeding is very common in aquarium keeping and is a major cause of fish illness. Left over food becomes toxic and compromises fish immune systems, making then vulnerable to opportunistic diseases and parasites. Healthy fish always act hungry, so over feeding is difficult to avoid. Most experienced aquarists feed the fish twice a day but no more than they eat in a minute or two. Fish can go without food for several days without harm; some experts recommend fasting them once a week.');
GO
INSERT INTO [Contribution] ([UserId],[ProjectId],[Title],[Description]) VALUES (14,16,NULL,NULL);
GO
INSERT INTO [Contribution] ([UserId],[ProjectId],[Title],[Description]) VALUES (14,17,N'Entertainment guy',N'I entertained the team at random');
GO
SET IDENTITY_INSERT [ContributionMedium] ON;
GO
INSERT INTO [ContributionMedium] ([MediumType],[Caption],[Link],[UserId],[ProjectId],[Id]) VALUES (0,N'The largest house I built',N'http://www.therabbithouse.com/gallery/photo1_26.jpg',14,13,2);
GO
INSERT INTO [ContributionMedium] ([MediumType],[Caption],[Link],[UserId],[ProjectId],[Id]) VALUES (0,N'Birds',N'http://www.birdchannel.com/images/budgies-in-bird-aviary.jpg',14,14,3);
GO
INSERT INTO [ContributionMedium] ([MediumType],[Caption],[Link],[UserId],[ProjectId],[Id]) VALUES (0,N'fish',N'http://www.myaquariumclub.com/images/fbfiles/images/fish3_028.JPG',14,15,4);
GO
INSERT INTO [ContributionMedium] ([MediumType],[Caption],[Link],[UserId],[ProjectId],[Id]) VALUES (0,N'One snack',N'http://www.myaquariumclub.com/images/fbfiles/images/fish3_028.JPG',9,17,6);
GO
INSERT INTO [ContributionMedium] ([MediumType],[Caption],[Link],[UserId],[ProjectId],[Id]) VALUES (0,N'Entertainment tool',N'http://i.imgur.com/Z6C60NU.jpg',14,17,7);
GO
SET IDENTITY_INSERT [ContributionMedium] OFF;
GO
SET IDENTITY_INSERT [ContactInfo] ON;
GO
INSERT INTO [ContactInfo] ([Id],[Title],[Information],[UserId]) VALUES (1,N'Home Email',N'example@example.com',8);
GO
INSERT INTO [ContactInfo] ([Id],[Title],[Information],[UserId]) VALUES (2,N'Home Email',N'example@example.com',14);
GO
INSERT INTO [ContactInfo] ([Id],[Title],[Information],[UserId]) VALUES (3,N'Home Phone',N'555-5555',14);
GO
SET IDENTITY_INSERT [ContactInfo] OFF;
GO
ALTER TABLE [webpages_Roles] ADD CONSTRAINT [PK__webpages_Roles__000000000000020C] PRIMARY KEY ([RoleId]);
GO
ALTER TABLE [webpages_OAuthMembership] ADD CONSTRAINT [PK__webpages_OAuthMembership__00000000000001E0] PRIMARY KEY ([Provider],[ProviderUserId]);
GO
ALTER TABLE [webpages_Membership] ADD CONSTRAINT [PK__webpages_Membership__0000000000000202] PRIMARY KEY ([UserId]);
GO
ALTER TABLE [UserSetting] ADD CONSTRAINT [PK_UserSetting] PRIMARY KEY ([Id]);
GO
ALTER TABLE [User] ADD CONSTRAINT [PK_User] PRIMARY KEY ([Id]);
GO
ALTER TABLE [webpages_UsersInRoles] ADD CONSTRAINT [PK__webpages_UsersInRoles__000000000000021B] PRIMARY KEY ([UserId],[RoleId]);
GO
ALTER TABLE [QuickReference] ADD CONSTRAINT [PK_QuickReference] PRIMARY KEY ([QuickReferenceId],[UserId]);
GO
ALTER TABLE [Project] ADD CONSTRAINT [PK_Project] PRIMARY KEY ([Id]);
GO
ALTER TABLE [ProjectPermission] ADD CONSTRAINT [PK_ProjectPermission] PRIMARY KEY ([UserId],[ProjectId]);
GO
ALTER TABLE [ProjectMedium] ADD CONSTRAINT [PK_ProjectMedium] PRIMARY KEY ([Id]);
GO
ALTER TABLE [Portfolio] ADD CONSTRAINT [PK_Portfolio] PRIMARY KEY ([Id]);
GO
ALTER TABLE [Project_Portfolio] ADD CONSTRAINT [PK__Project_Portfolio__000000000000014D] PRIMARY KEY ([ProjectId],[PortfolioId]);
GO
ALTER TABLE [Notification] ADD CONSTRAINT [PK_Notification] PRIMARY KEY ([Id]);
GO
ALTER TABLE [Link] ADD CONSTRAINT [PK_Link] PRIMARY KEY ([Id]);
GO
ALTER TABLE [FeaturedPortfolio] ADD CONSTRAINT [PK_FeaturedPortfolio] PRIMARY KEY ([Id]);
GO
ALTER TABLE [Education] ADD CONSTRAINT [PK_Education] PRIMARY KEY ([Id]);
GO
ALTER TABLE [Contribution] ADD CONSTRAINT [PK_Contribution] PRIMARY KEY ([UserId],[ProjectId]);
GO
ALTER TABLE [ContributionMedium] ADD CONSTRAINT [PK__ContributionMedium__00000000000002A8] PRIMARY KEY ([Id]);
GO
ALTER TABLE [ContactInfo] ADD CONSTRAINT [PK_ContactInfo] PRIMARY KEY ([Id]);
GO
CREATE UNIQUE INDEX [UQ__webpages_Roles__0000000000000211] ON [webpages_Roles] ([RoleName] ASC);
GO
CREATE UNIQUE INDEX [UQ__UserSetting__000000000000031E] ON [UserSetting] ([Id] ASC);
GO
CREATE UNIQUE INDEX [UQ__UserSetting__0000000000000325] ON [UserSetting] ([UserId] ASC);
GO
CREATE UNIQUE INDEX [UQ__User__0000000000000065] ON [User] ([Id] ASC);
GO
CREATE UNIQUE INDEX [UQ__User__000000000000006A] ON [User] ([Email] ASC);
GO
CREATE UNIQUE INDEX [UQ__Project__0000000000000118] ON [Project] ([Id] ASC);
GO
CREATE UNIQUE INDEX [UQ__ProjectMedium__0000000000000264] ON [ProjectMedium] ([Id] ASC);
GO
CREATE UNIQUE INDEX [UQ__Portfolio__0000000000000124] ON [Portfolio] ([Id] ASC);
GO
CREATE UNIQUE INDEX [UQ__Notification__00000000000000D4] ON [Notification] ([Id] ASC);
GO
CREATE UNIQUE INDEX [UQ__Link__000000000000007C] ON [Link] ([Id] ASC);
GO
CREATE UNIQUE INDEX [UQ__FeaturedPortfolio__0000000000000312] ON [FeaturedPortfolio] ([Id] ASC);
GO
CREATE UNIQUE INDEX [UQ__Education__00000000000000A0] ON [Education] ([Id] ASC);
GO
CREATE UNIQUE INDEX [UQ__ContributionMedium__00000000000002A3] ON [ContributionMedium] ([Id] ASC);
GO
CREATE UNIQUE INDEX [UQ__ContactInfo__00000000000000B3] ON [ContactInfo] ([Id] ASC);
GO
ALTER TABLE [User] ADD CONSTRAINT [UserSetting_User] FOREIGN KEY ([UserSettingId]) REFERENCES [UserSetting]([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO
ALTER TABLE [webpages_UsersInRoles] ADD CONSTRAINT [fk_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [webpages_Roles]([RoleId]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO
ALTER TABLE [webpages_UsersInRoles] ADD CONSTRAINT [fk_UserId] FOREIGN KEY ([UserId]) REFERENCES [User]([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO
ALTER TABLE [QuickReference] ADD CONSTRAINT [User_QuickReference] FOREIGN KEY ([UserId]) REFERENCES [User]([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO
ALTER TABLE [ProjectPermission] ADD CONSTRAINT [Project_ProjectPermission] FOREIGN KEY ([ProjectId]) REFERENCES [Project]([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO
ALTER TABLE [ProjectPermission] ADD CONSTRAINT [User_ProjectPermission] FOREIGN KEY ([UserId]) REFERENCES [User]([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO
ALTER TABLE [ProjectMedium] ADD CONSTRAINT [Project_ProjectMedium] FOREIGN KEY ([ProjectId]) REFERENCES [Project]([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO
ALTER TABLE [Portfolio] ADD CONSTRAINT [User_Portfolio] FOREIGN KEY ([UserId]) REFERENCES [User]([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO
ALTER TABLE [Project_Portfolio] ADD CONSTRAINT [Portfolio_Project_Portfolio] FOREIGN KEY ([PortfolioId]) REFERENCES [Portfolio]([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO
ALTER TABLE [Project_Portfolio] ADD CONSTRAINT [Project_Project_Portfolio] FOREIGN KEY ([ProjectId]) REFERENCES [Project]([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO
ALTER TABLE [Notification] ADD CONSTRAINT [UserProject] FOREIGN KEY ([UserId]) REFERENCES [User]([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO
ALTER TABLE [Link] ADD CONSTRAINT [UserLink] FOREIGN KEY ([UserId]) REFERENCES [User]([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO
ALTER TABLE [FeaturedPortfolio] ADD CONSTRAINT [Portfolio_FeaturedPortfolio] FOREIGN KEY ([PortfolioId]) REFERENCES [Portfolio]([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO
ALTER TABLE [Education] ADD CONSTRAINT [UserEducation] FOREIGN KEY ([UserId]) REFERENCES [User]([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO
ALTER TABLE [Contribution] ADD CONSTRAINT [Project_Contribution] FOREIGN KEY ([ProjectId]) REFERENCES [Project]([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO
ALTER TABLE [Contribution] ADD CONSTRAINT [User_Contribution] FOREIGN KEY ([UserId]) REFERENCES [User]([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO
ALTER TABLE [ContributionMedium] ADD CONSTRAINT [Contribution_ContributionMedium] FOREIGN KEY ([UserId], [ProjectId]) REFERENCES [Contribution]([UserId], [ProjectId]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO
ALTER TABLE [ContactInfo] ADD CONSTRAINT [UserContactInfo] FOREIGN KEY ([UserId]) REFERENCES [User]([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

