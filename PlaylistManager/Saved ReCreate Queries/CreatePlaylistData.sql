CREATE TABLE [dbo].[PlaylistData] (
    [ID]      INT          IDENTITY (1, 1) NOT NULL,
    [Name]    VARCHAR (50) NOT NULL,
    [User_ID] INT          NOT NULL,
    CONSTRAINT [PK_PlaylistData] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [fkIdx_16]
    ON [dbo].[PlaylistData]([User_ID] ASC);