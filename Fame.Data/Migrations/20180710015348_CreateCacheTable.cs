﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace Fame.Data.Migrations
{
    public partial class CreateCacheTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            SET ANSI_NULLS ON
            GO
            SET QUOTED_IDENTIFIER ON
            GO
            CREATE SCHEMA [Cache]
            GO
            CREATE TABLE [Cache].[Cache](
                [Id] [nvarchar](449) NOT NULL,
                [Value] [varbinary](max) NOT NULL,
                [ExpiresAtTime] [datetimeoffset](7) NOT NULL,
                [SlidingExpirationInSeconds] [bigint] NULL,
                [AbsoluteExpiration] [datetimeoffset](7) NULL
            ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
            GO
            SET ANSI_PADDING ON
            GO
            ALTER TABLE [Cache].[Cache] ADD PRIMARY KEY CLUSTERED 
            (
                [Id] ASC
            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
            GO
            CREATE NONCLUSTERED INDEX [Index_ExpiresAtTime] ON [Cache].[Cache]
            (
                [ExpiresAtTime] ASC
            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
            GO
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP TABLE [dbo].[Cache]
                GO
                DROP SCHEMA [Cache]
                GO
            ");
        }
    }
}
