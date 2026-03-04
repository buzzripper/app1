USE [Auth]
GO

INSERT [dbo].[Tenant] ([Id], [Name], [Slug], [AuthMode], [ExternalAuthority], [ExternalClientId], [ExternalClientSecret], [ADDcHost], [ADDomain], [ADLdapPort], [ADBaseDn], [IsActive], [CreatedAt]) VALUES (N'a1000000-0000-0000-0000-000000000001', N'Acme Corp', N'acme', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[Tenant] ([Id], [Name], [Slug], [AuthMode], [ExternalAuthority], [ExternalClientId], [ExternalClientSecret], [ADDcHost], [ADDomain], [ADLdapPort], [ADBaseDn], [IsActive], [CreatedAt]) VALUES (N'a1000000-0000-0000-0000-000000000002', N'Contoso', N'contoso', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[Tenant] ([Id], [Name], [Slug], [AuthMode], [ExternalAuthority], [ExternalClientId], [ExternalClientSecret], [ADDcHost], [ADDomain], [ADLdapPort], [ADBaseDn], [IsActive], [CreatedAt]) VALUES (N'a1000000-0000-0000-0000-000000000003', N'Fabrikam (External)', N'fabrikam', 2, N'https://dyvenix.ciamlogin.com/1c3cdcca-ba60-4ad2-9892-626f5d92bc09/v2.0/', N'70efb084-7211-4e52-ab93-a7ea03abc0f8', N'Tev8Q~b1tGnOvcQY0.DoWjbgMhNko6APvtVAnbq_', NULL, NULL, NULL, NULL, 1, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[Tenant] ([Id], [Name], [Slug], [AuthMode], [ExternalAuthority], [ExternalClientId], [ExternalClientSecret], [ADDcHost], [ADDomain], [ADLdapPort], [ADBaseDn], [IsActive], [CreatedAt]) VALUES (N'a1000000-0000-0000-0000-000000000004', N'Ad Corp 1', N'adcorp1', 1, NULL, NULL, NULL, N'adcorp1.local', N'ADCORP1', 389, N'DC=adcorp1,DC=local', 1, CAST(N'2026-03-02T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[Tenant] ([Id], [Name], [Slug], [AuthMode], [ExternalAuthority], [ExternalClientId], [ExternalClientSecret], [ADDcHost], [ADDomain], [ADLdapPort], [ADBaseDn], [IsActive], [CreatedAt]) VALUES (N'a1000000-0000-0000-0000-000000000005', N'Ad Corp 2', N'adcorp2', 1, NULL, NULL, NULL, N'adcorp1.local', N'ADCORP2', 389, N'DC=adcorp2,DC=local', 1, CAST(N'2026-03-02T00:00:00.0000000' AS DateTime2))
GO


INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp], TenantId) VALUES (N'20660F28-36FE-45C0-9A53-88777E0799A2', N'Role1', N'ROLE1', N'2B4D18C0-CDF1-4E90-B0DB-19FB49B96948', N'a1000000-0000-0000-0000-000000000001')
GO


SET IDENTITY_INSERT [dbo].[AspNetRoleClaims] ON 
GO
INSERT [dbo].[AspNetRoleClaims] ([Id], [RoleId], [ClaimType], [ClaimValue]) VALUES (1, N'20660F28-36FE-45C0-9A53-88777E0799A2', N'perm', N'ad:read')
GO
INSERT [dbo].[AspNetRoleClaims] ([Id], [RoleId], [ClaimType], [ClaimValue]) VALUES (4, N'20660F28-36FE-45C0-9A53-88777E0799A2', N'perm', N'ad:write')
GO
SET IDENTITY_INSERT [dbo].[AspNetRoleClaims] OFF
GO


INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [TenantId]) VALUES (N'34656577-A40C-4DF1-9B4C-2546EDC2C316', N'user1@adcorp1.local', N'USER1@ADCORP1.LOCAL', N'user1@adcorp1.local', N'USER1@ADCORP1.LOCAL', 1, NULL, N'5DRXZN5G2CZVLLCKMG7DH53HNIYRN76O', N'beeed0bc-3884-423c-82b0-90900c062a31', NULL, 0, 0, NULL, 1, 0, N'a1000000-0000-0000-0000-000000000004')
GO
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [TenantId]) VALUES (N'34656577-A40C-4DF1-9B4C-2546EDC2C317', N'user2@adcorp1.local', N'USER2@ADCORP1.LOCAL', N'user2@adcorp1.local', N'USER2@ADCORP1.LOCAL', 1, NULL, N'5DRXZN5G2CZVLLCKMG7DH53HNIYRN76P', N'beeed0bc-3884-423c-82b0-90900c062a32', NULL, 0, 0, NULL, 1, 0, N'a1000000-0000-0000-0000-000000000004')
GO
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [TenantId]) VALUES (N'34656577-A40C-4DF1-9B4C-2546EDC2C318', N'usera@adcorp2.local', N'USERA@ADCORP2.LOCAL', N'usera@adcorp2.local', N'USERA@ADCORP2.LOCAL', 1, NULL, N'5DRXZN5G2CZVLLCKMG7DH53HNIYRN76Q', N'beeed0bc-3884-423c-82b0-90900c062a33', NULL, 0, 0, NULL, 1, 0, N'a1000000-0000-0000-0000-000000000004')
GO
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [TenantId]) VALUES (N'34656577-A40C-4DF1-9B4C-2546EDC2C319', N'userb@adcorp2.local', N'USERB@ADCORP2.LOCAL', N'userb@adcorp2.local', N'USERB@ADCORP2.LOCAL', 1, NULL, N'5DRXZN5G2CZVLLCKMG7DH53HNIYRN76R', N'beeed0bc-3884-423c-82b0-90900c062a34', NULL, 0, 0, NULL, 1, 0, N'a1000000-0000-0000-0000-000000000004')
GO
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [TenantId]) VALUES (N'8b49be4c-4301-45ab-aac6-a5b6a986710b', N'user.one@dyvenix.onmicrosoft.com', N'USER.ONE@DYVENIX.ONMICROSOFT.COM', N'user.one@dyvenix.onmicrosoft.com', N'USER.ONE@DYVENIX.ONMICROSOFT.COM', 1, NULL, N'4ZWXWKY3XKUUANMX2SKSUDFXIIXKITYN', N'c8f973a2-4b8a-430c-8aae-1588bb1765b6', NULL, 0, 0, NULL, 1, 0, N'a1000000-0000-0000-0000-000000000003')
GO
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [TenantId]) VALUES (N'e811debb-80a3-4774-9b54-c0ec9b9f8ad3', N'acme@test.com', N'ACME@TEST.COM', N'acme@test.com', N'ACME@TEST.COM', 1, N'AQAAAAIAAYagAAAAEKnRCXSyLsC4aHO6/LZMaMGbDfUBPZCj/z2LjRKTtwDAdgwrPAL5SnlvSCbyHErL4Q==', N'UV2N6PQ3C6RNIWOYSSUZB3OTXXWRM4FO', N'87632ccf-6858-4da6-903f-9b63c5f09149', NULL, 0, 0, NULL, 1, 0, N'a1000000-0000-0000-0000-000000000001')
GO
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [TenantId]) VALUES (N'f256be8c-60b7-4b05-b8d6-fe91e31663d5', N'contoso@test.com', N'CONTOSO@TEST.COM', N'contoso@test.com', N'CONTOSO@TEST.COM', 1, N'AQAAAAIAAYagAAAAEL9fPltGANwYNJgFCnmo/c9ziS1YqKJhMSNr6Zf2ZcD+Q4Y4atk/43IjA4IaE4vZpg==', N'5DRXZN5G2CZVLLCKMG7DH53HNIYRN76N', N'beeed0bc-3884-423c-82b0-90900c062a30', NULL, 0, 0, NULL, 1, 0, N'a1000000-0000-0000-0000-000000000002')
GO


INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'e811debb-80a3-4774-9b54-c0ec9b9f8ad3', N'20660F28-36FE-45C0-9A53-88777E0799A2')
GO


SET IDENTITY_INSERT [dbo].[AspNetUserClaims] ON
GO
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (1, N'e811debb-80a3-4774-9b54-c0ec9b9f8ad3', N'perm', N'auth:admin')
GO
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (2, N'e811debb-80a3-4774-9b54-c0ec9b9f8ad3', N'perm', N'auth:write')
GO
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (4, N'e811debb-80a3-4774-9b54-c0ec9b9f8ad3', N'perm', N'app:read')
GO
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (6, N'e811debb-80a3-4774-9b54-c0ec9b9f8ad3', N'perm', N'app:write')
GO
SET IDENTITY_INSERT [dbo].[AspNetUserClaims] OFF
GO

INSERT [dbo].[OpenIddictApplications] ([Id], [ApplicationType], [ClientId], [ClientSecret], [ClientType], [ConcurrencyToken], [ConsentType], [DisplayName], [DisplayNames], [JsonWebKeySet], [Permissions], [PostLogoutRedirectUris], [Properties], [RedirectUris], [Requirements], [Settings]) VALUES (N'3fb789f7-d57f-426f-a145-4b8974877f02', NULL, N'app1Apis', N'AQAAAAEAACcQAAAAECWFtvfy95ENJ7dnjKiF+H4mZ/T9mZgjLfMLywZDFjjq0A/qaxnsPI0JDmL8ltXrAA==', N'confidential', N'967a749f-907d-4d59-9476-6e688a291ac1', NULL, NULL, NULL, NULL, N'["ept:introspection"]', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[OpenIddictApplications] ([Id], [ApplicationType], [ClientId], [ClientSecret], [ClientType], [ConcurrencyToken], [ConsentType], [DisplayName], [DisplayNames], [JsonWebKeySet], [Permissions], [PostLogoutRedirectUris], [Properties], [RedirectUris], [Requirements], [Settings]) VALUES (N'40b69416-18ca-47c8-be69-f211bd693be1', NULL, N'rs_dataEventRecordsApi', N'AQAAAAEAACcQAAAAEOml71shvXWMFu5oh5TCPrmMlLV3wD5WvrOjiw5osAQhGDjFmoNrIgjDykzV9IqWmA==', N'confidential', N'ec4af63b-c3de-4267-9883-92457de846ac', NULL, NULL, NULL, NULL, N'["ept:introspection"]', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[OpenIddictApplications] ([Id], [ApplicationType], [ClientId], [ClientSecret], [ClientType], [ConcurrencyToken], [ConsentType], [DisplayName], [DisplayNames], [JsonWebKeySet], [Permissions], [PostLogoutRedirectUris], [Properties], [RedirectUris], [Requirements], [Settings]) VALUES (N'55c5a7e7-49f4-4e14-8cfb-541ddc22d7bf', NULL, N'angularclient', NULL, N'public', N'87666d5b-9829-48ea-bead-6c4a5ab4574e', N'explicit', N'angular client PKCE', N'{"fr-FR":"Application cliente MVC"}', NULL, N'["ept:authorization","ept:end_session","ept:token","ept:revocation","gt:authorization_code","gt:refresh_token","rst:code","scp:email","scp:profile","scp:roles","scp:dataEventRecords"]', N'["https://localhost:4200"]', NULL, N'["https://localhost:4200"]', N'["ft:pkce"]', NULL)
GO
INSERT [dbo].[OpenIddictApplications] ([Id], [ApplicationType], [ClientId], [ClientSecret], [ClientType], [ConcurrencyToken], [ConsentType], [DisplayName], [DisplayNames], [JsonWebKeySet], [Permissions], [PostLogoutRedirectUris], [Properties], [RedirectUris], [Requirements], [Settings]) VALUES (N'a451ed62-3e12-41f3-a420-c6bffc328e16', NULL, N'blazorcodeflowpkceclient', N'AQAAAAEAACcQAAAAEJo0T57WudmBpAeIxRgecRCkqNz7Dsxg9JMQzZKVcs0YfPR0CkCKvbN/ud8ft41r1A==', N'confidential', N'332e986d-984c-419c-953a-1622a1a1fa11', N'explicit', N'Blazor code PKCE', N'{"fr-FR":"Application cliente MVC"}', NULL, N'["ept:authorization","ept:end_session","ept:token","ept:revocation","gt:authorization_code","gt:refresh_token","rst:code","scp:email","scp:profile","scp:roles","scp:dataEventRecords"]', N'["https://localhost:44348/signout-callback-oidc","https://localhost:5001/signout-callback-oidc"]', NULL, N'["https://localhost:44348/signin-oidc","https://localhost:5001/signin-oidc"]', N'["ft:pkce"]', NULL)
GO
INSERT [dbo].[OpenIddictApplications] ([Id], [ApplicationType], [ClientId], [ClientSecret], [ClientType], [ConcurrencyToken], [ConsentType], [DisplayName], [DisplayNames], [JsonWebKeySet], [Permissions], [PostLogoutRedirectUris], [Properties], [RedirectUris], [Requirements], [Settings]) VALUES (N'adb87d9b-a298-4136-8817-e1b16ef505ef', NULL, N'portal-bff', N'AQAAAAEAACcQAAAAEGWrtAz2bVMfYVveTBe/YekwnXHThoIGdfyl2QHtYT9QKHpJbo88RMwNHJFCK7ecGg==', N'confidential', N'848d59b4-69c9-4d79-95a1-4d9fc62b4b75', N'implicit', N'Portal BFF', NULL, NULL, N'["ept:authorization","ept:end_session","ept:token","ept:revocation","gt:authorization_code","gt:refresh_token","rst:code","scp:email","scp:profile","scp:roles","scp:app1-api"]', N'["https://localhost:5001/signout-callback-oidc","https://acme.localhost:5001/signout-callback-oidc","https://contoso.localhost:5001/signout-callback-oidc","https://fabrikam.localhost:5001/signout-callback-oidc","https://adcorp1.localhost:5001/signout-callback-oidc","https://adcorp2.localhost:5001/signout-callback-oidc"]', NULL, N'["https://localhost:5001/signin-oidc","https://acme.localhost:5001/signin-oidc","https://contoso.localhost:5001/signin-oidc","https://fabrikam.localhost:5001/signin-oidc","https://adcorp1.localhost:5001/signin-oidc","https://adcorp2.localhost:5001/signin-oidc"]', N'["ft:pkce"]', NULL)
GO
INSERT [dbo].[OpenIddictApplications] ([Id], [ApplicationType], [ClientId], [ClientSecret], [ClientType], [ConcurrencyToken], [ConsentType], [DisplayName], [DisplayNames], [JsonWebKeySet], [Permissions], [PostLogoutRedirectUris], [Properties], [RedirectUris], [Requirements], [Settings]) VALUES (N'e39279da-6dfa-4d54-b57c-a00f8248b526', NULL, N'CC', N'AQAAAAEAACcQAAAAEJwFk6meRQwBJ+5GyRM5em5jB/7UWTRg3VYxKcZVEKIU476lCgAQwdw/V83gyaGCPw==', N'confidential', N'106c6b77-bee2-4a3e-a021-d4ced9ac7665', NULL, N'CC for protected API', NULL, NULL, N'["ept:authorization","ept:token","gt:client_credentials","scp:dataEventRecords"]', NULL, NULL, NULL, NULL, NULL)
GO
