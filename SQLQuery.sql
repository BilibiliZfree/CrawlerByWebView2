--建表语句--
CREATE TABLE [dbo].[JavaScriptCommand] (
    [Id]     INT  NOT NULL IDENTITY(1,1),
    [JSCode] TEXT NOT NULL,
    [status] INT  NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

--查询全表--
SELECT * FROM [JavaScriptCommand];

SELECT * FROM [JavaScriptCommand] WHERE [Id] = 1006;

--插入语句--
INSERT  INTO [JavaScriptCommand] ([JSCode],[status],[JSCount],[AssociatedWebAddress])
VALUES (N'测试',1,N'测试',N'https://lol.qq.com/tft/#/champion');

--插入列--
ALTER TABLE [JavaScriptCommand] add [AssociatedWebAddress] TEXT;

--删除语句--
DELETE FROM [JavaScriptCommand] WHERE [Id] = 7011;


--更新语句--
UPDATE [JavaScriptCommand] SET [status] = 0 WHERE [Id] = 6;

UPDATE [JavaScriptCommand] SET [status] = 0 WHERE [Id] = 1006;

UPDATE [JavaScriptCommand] SET [JSCode] = N'哔哩哔哩 (゜-゜)つロ 干杯~-bilibili',[status] = 0, [JSCount] = N'我，33，打',[AssociatedWebAddress]=N'https://www.bilibili.com/' WHERE [Id] = 7014;