ALTER TABLE [dbo].[Project]  WITH CHECK ADD  CONSTRAINT [FK_Project_Project_ParentId] FOREIGN KEY([ParentId])
REFERENCES [dbo].[Project] ([Id])
GO

ALTER TABLE [dbo].[Project] CHECK CONSTRAINT [FK_Project_Project_ParentId]