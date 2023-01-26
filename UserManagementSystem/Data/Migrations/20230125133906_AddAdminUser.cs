using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [Security].[Users] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [Fname], [Lname], [ProfilePic]) VALUES (N'd4176ae5-e7a0-46f0-b577-797871d0864e', N'Admin', N'ADMIN', N'Admin@test.com', N'ADMIN@TEST.COM', 0, N'AQAAAAIAAYagAAAAEGnyMx04f74I0XiSLO+MPErt8iPQVPEkF6bX1eveOH+p4m8McDrDds4wmcpT4FF9CA==', N'I63DDDZAR3PTGLTU4CMUKNAHVQKXDYB3', N'b2ef04ed-cd83-40b9-b274-8fccec7a8379', NULL, 0, 0, NULL, 1, 0, N'islam', N'mohamed', null)\r\n");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE from [Security].[Users] WHERE Id ='d4176ae5-e7a0-46f0-b577-797871d0864e' ");
        }
    }
}
