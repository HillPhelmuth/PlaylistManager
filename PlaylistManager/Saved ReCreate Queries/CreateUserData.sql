CREATE TABLE [dbo].[UserData] (
    [ID]        INT           IDENTITY (1, 1) NOT NULL,
    [UserName]  VARCHAR (50)  NOT NULL,
    [UserInfo]  VARCHAR (MAX) NULL,
    [UserEmail] VARCHAR (255) NULL,
    CONSTRAINT [PK_UserData] PRIMARY KEY CLUSTERED ([ID] ASC)
);
