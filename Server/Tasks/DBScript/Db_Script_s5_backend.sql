ALTER TABLE Tasks
ADD ScheduleString varchar(50);
GO

INSERT INTO [KMS_Tasks].[dbo].[PriorityLevel](Id, DisplayName, Description) VALUES(1, 'Emergency', '');
INSERT INTO [KMS_Tasks].[dbo].[PriorityLevel](Id, DisplayName, Description) VALUES(2, 'High', '');
INSERT INTO [KMS_Tasks].[dbo].[PriorityLevel](Id, DisplayName, Description) VALUES(3, 'Medium', '');
INSERT INTO [KMS_Tasks].[dbo].[PriorityLevel](Id, DisplayName, Description) VALUES(4, 'Low', '');
INSERT INTO [KMS_Tasks].[dbo].[PriorityLevel](Id, DisplayName, Description) VALUES(5, 'Free', 'Finish task in anytime');
GO