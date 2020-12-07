CREATE TABLE [dbo].[UserProjects] (
    [UserId]    BIGINT NOT NULL,
    [ProjectId] BIGINT NOT NULL,
    [RoleId]    INT    NOT NULL,
    CONSTRAINT [PK_UserProjects] PRIMARY KEY CLUSTERED ([UserId] ASC, [ProjectId] ASC, [RoleId] ASC),
    CONSTRAINT [FK_UserProjects_BelongsTo_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([UserId]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserProjects_Project] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project] ([Id]),
    CONSTRAINT [FK_UserProjects_ProjectRole] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[ProjectRoles] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_UserProjects_ProjectId]
    ON [dbo].[UserProjects]([ProjectId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_UserProjects_RoleId]
    ON [dbo].[UserProjects]([RoleId] ASC);

