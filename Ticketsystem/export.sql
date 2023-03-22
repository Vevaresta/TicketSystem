CREATE TABLE "Clients" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Clients" PRIMARY KEY,
    "LastName" TEXT NULL,
    "FirstName" TEXT NULL,
    "Email" TEXT NULL,
    "StreetAndHouseNumber" TEXT NULL,
    "PostalCode" TEXT NULL,
    "City" TEXT NULL,
    "PhoneNumber" TEXT NULL,
    "ParticipantNumber" INTEGER NULL,
    "Course" TEXT NULL
);


CREATE TABLE "Permissions" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Permissions" PRIMARY KEY,
    "Name" TEXT NULL
);


CREATE TABLE "Roles" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Roles" PRIMARY KEY,
    "Name" TEXT NULL,
    "NormalizedName" TEXT NULL,
    "ConcurrencyStamp" TEXT NULL
);


CREATE TABLE "TicketStatuses" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_TicketStatuses" PRIMARY KEY,
    "Name" TEXT NULL
);


CREATE TABLE "TicketTypes" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_TicketTypes" PRIMARY KEY,
    "Name" TEXT NULL
);


CREATE TABLE "Users" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Users" PRIMARY KEY,
    "FirstName" TEXT NULL,
    "LastName" TEXT NULL,
    "UserName" TEXT NULL,
    "NormalizedUserName" TEXT NULL,
    "Email" TEXT NULL,
    "NormalizedEmail" TEXT NULL,
    "EmailConfirmed" INTEGER NOT NULL,
    "PasswordHash" TEXT NULL,
    "SecurityStamp" TEXT NULL,
    "ConcurrencyStamp" TEXT NULL,
    "PhoneNumber" TEXT NULL,
    "PhoneNumberConfirmed" INTEGER NOT NULL,
    "TwoFactorEnabled" INTEGER NOT NULL,
    "LockoutEnd" TEXT NULL,
    "LockoutEnabled" INTEGER NOT NULL,
    "AccessFailedCount" INTEGER NOT NULL
);


CREATE TABLE "RoleClaims" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_RoleClaims" PRIMARY KEY AUTOINCREMENT,
    "RoleId" TEXT NOT NULL,
    "ClaimType" TEXT NULL,
    "ClaimValue" TEXT NULL,
    CONSTRAINT "FK_RoleClaims_Roles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "Roles" ("Id") ON DELETE CASCADE
);


CREATE TABLE "RolePermissions" (
    "PermissionsId" TEXT NOT NULL,
    "RolesId" TEXT NOT NULL,
    CONSTRAINT "PK_RolePermissions" PRIMARY KEY ("PermissionsId", "RolesId"),
    CONSTRAINT "FK_RolePermissions_Permissions_PermissionsId" FOREIGN KEY ("PermissionsId") REFERENCES "Permissions" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_RolePermissions_Roles_RolesId" FOREIGN KEY ("RolesId") REFERENCES "Roles" ("Id") ON DELETE CASCADE
);


CREATE TABLE "Tickets" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Tickets" PRIMARY KEY AUTOINCREMENT,
    "TicketTypeId" TEXT NULL,
    "TicketStatusId" TEXT NULL,
    "ClientId" TEXT NULL,
    "Name" TEXT NOT NULL,
    "OrderDate" TEXT NOT NULL,
    "WorkOrder" TEXT NOT NULL,
    "DoBackup" INTEGER NOT NULL,
    "DataBackupByClient" INTEGER NOT NULL,
    "DataBackupByStaff" INTEGER NOT NULL,
    "DataBackupDone" INTEGER NOT NULL,
    CONSTRAINT "FK_Tickets_Clients_ClientId" FOREIGN KEY ("ClientId") REFERENCES "Clients" ("Id"),
    CONSTRAINT "FK_Tickets_TicketStatuses_TicketStatusId" FOREIGN KEY ("TicketStatusId") REFERENCES "TicketStatuses" ("Id"),
    CONSTRAINT "FK_Tickets_TicketTypes_TicketTypeId" FOREIGN KEY ("TicketTypeId") REFERENCES "TicketTypes" ("Id")
);


CREATE TABLE "PermissionsTriggered" (
    "UserId" TEXT NOT NULL,
    "PermissionId" TEXT NOT NULL,
    "DateTime" TEXT NOT NULL,
    "Comment" TEXT NULL,
    CONSTRAINT "PK_PermissionsTriggered" PRIMARY KEY ("UserId", "PermissionId"),
    CONSTRAINT "FK_PermissionsTriggered_Permissions_PermissionId" FOREIGN KEY ("PermissionId") REFERENCES "Permissions" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_PermissionsTriggered_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);


CREATE TABLE "UserClaims" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_UserClaims" PRIMARY KEY AUTOINCREMENT,
    "UserId" TEXT NOT NULL,
    "ClaimType" TEXT NULL,
    "ClaimValue" TEXT NULL,
    CONSTRAINT "FK_UserClaims_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);


CREATE TABLE "UserLogins" (
    "LoginProvider" TEXT NOT NULL,
    "ProviderKey" TEXT NOT NULL,
    "ProviderDisplayName" TEXT NULL,
    "UserId" TEXT NOT NULL,
    CONSTRAINT "PK_UserLogins" PRIMARY KEY ("LoginProvider", "ProviderKey"),
    CONSTRAINT "FK_UserLogins_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);


CREATE TABLE "UserRoles" (
    "UserId" TEXT NOT NULL,
    "RoleId" TEXT NOT NULL,
    CONSTRAINT "PK_UserRoles" PRIMARY KEY ("UserId", "RoleId"),
    CONSTRAINT "FK_UserRoles_Roles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "Roles" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_UserRoles_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);


CREATE TABLE "UserTokens" (
    "UserId" TEXT NOT NULL,
    "LoginProvider" TEXT NOT NULL,
    "Name" TEXT NOT NULL,
    "Value" TEXT NULL,
    CONSTRAINT "PK_UserTokens" PRIMARY KEY ("UserId", "LoginProvider", "Name"),
    CONSTRAINT "FK_UserTokens_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);


CREATE TABLE "Devices" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Devices" PRIMARY KEY,
    "TicketId" INTEGER NOT NULL,
    "Name" TEXT NULL,
    "DeviceType" TEXT NULL,
    "Manufacturer" TEXT NULL,
    "SerialNumber" TEXT NULL,
    "Accessories" TEXT NULL,
    "Comments" TEXT NULL,
    CONSTRAINT "FK_Devices_Tickets_TicketId" FOREIGN KEY ("TicketId") REFERENCES "Tickets" ("Id") ON DELETE CASCADE
);


CREATE TABLE "TicketChanges" (
    "UserId" TEXT NOT NULL,
    "TicketId" INTEGER NOT NULL,
    "DateTime" TEXT NOT NULL,
    "Comment" TEXT NULL,
    CONSTRAINT "PK_TicketChanges" PRIMARY KEY ("UserId", "TicketId"),
    CONSTRAINT "FK_TicketChanges_Tickets_TicketId" FOREIGN KEY ("TicketId") REFERENCES "Tickets" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_TicketChanges_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);


CREATE TABLE "TicketUsers" (
    "TicketId" INTEGER NOT NULL,
    "UserId" TEXT NOT NULL,
    CONSTRAINT "PK_TicketUsers" PRIMARY KEY ("UserId", "TicketId"),
    CONSTRAINT "FK_TicketUsers_Tickets_TicketId" FOREIGN KEY ("TicketId") REFERENCES "Tickets" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_TicketUsers_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);


CREATE TABLE "Software" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Software" PRIMARY KEY,
    "DeviceId" TEXT NULL,
    "Name" TEXT NULL,
    "Comments" TEXT NULL,
    CONSTRAINT "FK_Software_Devices_DeviceId" FOREIGN KEY ("DeviceId") REFERENCES "Devices" ("Id") ON DELETE CASCADE
);


CREATE INDEX "IX_Devices_TicketId" ON "Devices" ("TicketId");


CREATE INDEX "IX_PermissionsTriggered_PermissionId" ON "PermissionsTriggered" ("PermissionId");


CREATE INDEX "IX_RoleClaims_RoleId" ON "RoleClaims" ("RoleId");


CREATE INDEX "IX_RolePermissions_RolesId" ON "RolePermissions" ("RolesId");


CREATE UNIQUE INDEX "RoleNameIndex" ON "Roles" ("NormalizedName");


CREATE INDEX "IX_Software_DeviceId" ON "Software" ("DeviceId");


CREATE INDEX "IX_TicketChanges_TicketId" ON "TicketChanges" ("TicketId");


CREATE INDEX "IX_Tickets_ClientId" ON "Tickets" ("ClientId");


CREATE INDEX "IX_Tickets_TicketStatusId" ON "Tickets" ("TicketStatusId");


CREATE INDEX "IX_Tickets_TicketTypeId" ON "Tickets" ("TicketTypeId");


CREATE INDEX "IX_TicketUsers_TicketId" ON "TicketUsers" ("TicketId");


CREATE INDEX "IX_UserClaims_UserId" ON "UserClaims" ("UserId");


CREATE INDEX "IX_UserLogins_UserId" ON "UserLogins" ("UserId");


CREATE INDEX "IX_UserRoles_RoleId" ON "UserRoles" ("RoleId");


CREATE INDEX "EmailIndex" ON "Users" ("NormalizedEmail");


CREATE UNIQUE INDEX "UserNameIndex" ON "Users" ("NormalizedUserName");


