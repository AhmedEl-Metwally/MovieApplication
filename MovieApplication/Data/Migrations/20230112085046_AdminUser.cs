﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieApplication.Data.Migrations
{
    public partial class AdminUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [security].[Users] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [FirstName], [LastName], [ProfilePicture]) VALUES (N'23b9c268-a24e-48e0-a8a7-47e733995568', N'admen', N'ADMEN', N'admen@test.com', N'ADMEN@TEST.COM', 0, N'AQAAAAEAACcQAAAAEIuYGCT7TKaaHJQoH78M3aBcKIwmIoRokoNq+U/LPD1KXGRTMOAcdKQk7eH45qD6gQ==', N'I4NIYX5Z3VQFLGX77RJ435OSIUTKBNDK', N'87fead70-85cf-401e-852a-b37806d84471', NULL, 0, 0, NULL, 1, 0, N'Adimntest', N'Adimntest', NULL )");


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM[security].[Users] WHERE Id='23b9c268-a24e-48e0-a8a7-47e733995568'");
        }
    }
}
