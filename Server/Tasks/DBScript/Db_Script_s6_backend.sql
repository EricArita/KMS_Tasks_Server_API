INSERT INTO [KMS_Tasks].[dbo].[ProjectRoles](Id, Name) Values(1, 'Owner')
INSERT INTO [KMS_Tasks].[dbo].[ProjectRoles](Id, Name, Description) Values(2, 'PM', 'Project Manager')
INSERT INTO [KMS_Tasks].[dbo].[ProjectRoles](Id, Name) Values(3, 'Leader')
INSERT INTO [KMS_Tasks].[dbo].[ProjectRoles](Id, Name, Description) Values(4, 'QA', 'Quality Assurance')
INSERT INTO [KMS_Tasks].[dbo].[ProjectRoles](Id, Name, Description) Values(5, 'Dev', 'Developer')
INSERT INTO [KMS_Tasks].[dbo].[ProjectRoles](Id, Name, Description) Values(6, 'BA', 'Business Analyst')
INSERT INTO [KMS_Tasks].[dbo].[ProjectRoles](Id, Name) Values(7, 'Member')
GO

INSERT INTO [KMS_Tasks].[dbo].[PriorityLevel](DisplayName) VALUES('Emergency')
INSERT INTO [KMS_Tasks].[dbo].[PriorityLevel](DisplayName) VALUES('High')
INSERT INTO [KMS_Tasks].[dbo].[PriorityLevel](DisplayName) VALUES('Medium')
INSERT INTO [KMS_Tasks].[dbo].[PriorityLevel](DisplayName) VALUES('Low')
INSERT INTO [KMS_Tasks].[dbo].[PriorityLevel](DisplayName) VALUES('Anytime')
GO