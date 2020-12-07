INSERT INTO [KMS_Tasks].[dbo].[ProjectRoles](Name) Values('Owner')
INSERT INTO [KMS_Tasks].[dbo].[ProjectRoles](Name, Description) Values('PM', 'Project Manager')
INSERT INTO [KMS_Tasks].[dbo].[ProjectRoles](Name) Values('Leader')
INSERT INTO [KMS_Tasks].[dbo].[ProjectRoles](Name, Description) Values('QA', 'Quality Assurance')
INSERT INTO [KMS_Tasks].[dbo].[ProjectRoles](Name, Description) Values('Dev', 'Developer')
INSERT INTO [KMS_Tasks].[dbo].[ProjectRoles](Name, Description) Values('BA', 'Business Analyst')
INSERT INTO [KMS_Tasks].[dbo].[ProjectRoles](Name) Values('Member')
GO

INSERT INTO [KMS_Tasks].[dbo].[PriorityLevel](DisplayName) VALUES('Emergency')
INSERT INTO [KMS_Tasks].[dbo].[PriorityLevel](DisplayName) VALUES('High')
INSERT INTO [KMS_Tasks].[dbo].[PriorityLevel](DisplayName) VALUES('Medium')
INSERT INTO [KMS_Tasks].[dbo].[PriorityLevel](DisplayName) VALUES('Low')
INSERT INTO [KMS_Tasks].[dbo].[PriorityLevel](DisplayName) VALUES('Anytime')
GO