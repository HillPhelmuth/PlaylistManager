CREATE TABLE [dbo].[UserData] (
    [ID]        INT           IDENTITY (1, 1) NOT NULL,
    [UserName]  VARCHAR (50)  NOT NULL,
    [UserInfo]  VARCHAR (MAX) NULL,
    [UserEmail] VARCHAR (255) NULL,
    CONSTRAINT [PK_UserData] PRIMARY KEY CLUSTERED ([ID] ASC)
);
GO
CREATE TABLE [dbo].[PlaylistData] (
    [ID]      INT          IDENTITY (1, 1) NOT NULL,
    [Name]    VARCHAR (50) NOT NULL,
    [User_ID] INT          NOT NULL,
    CONSTRAINT [PK_PlaylistData] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [fkIdx_16]
    ON [dbo].[PlaylistData]([User_ID] ASC);

CREATE TABLE [dbo].[VideosData] (
    [ID]           INT           IDENTITY (1, 1) NOT NULL,
    [Title]        VARCHAR (MAX) NOT NULL,
    [VideoID]      VARCHAR (MAX) NOT NULL,
    [Playlist_ID]  INT           NOT NULL,
    [PreferenceID] INT           NULL,
    [ThumbnailUrl] VARCHAR(MAX) NULL, 
    [Description] VARCHAR(MAX) NULL, 
    CONSTRAINT [PK_table_19] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_24] FOREIGN KEY ([Playlist_ID]) REFERENCES [dbo].[PlaylistData] ([ID])
);


GO
CREATE NONCLUSTERED INDEX [fkIdx_24]
    ON [dbo].[VideosData]([Playlist_ID] ASC);