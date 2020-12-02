ALTER TABLE Tasks
ADD ScheduleString varchar(50);
GO

INSERT INTO [KMS_Tasks].[dbo].[PriorityLevel](DisplayName, Description) VALUES( 'Emergency', '');
INSERT INTO [KMS_Tasks].[dbo].[PriorityLevel](DisplayName, Description) VALUES( 'High', '');
INSERT INTO [KMS_Tasks].[dbo].[PriorityLevel](DisplayName, Description) VALUES( 'Medium', '');
INSERT INTO [KMS_Tasks].[dbo].[PriorityLevel](DisplayName, Description) VALUES( 'Low', '');
INSERT INTO [KMS_Tasks].[dbo].[PriorityLevel](DisplayName, Description) VALUES( 'Free', 'Finish task in anytime');
GO