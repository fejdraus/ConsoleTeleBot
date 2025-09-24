CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" TEXT NOT NULL CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY,
    "ProductVersion" TEXT NOT NULL
);

BEGIN TRANSACTION;

CREATE TABLE "ParsingRules" (
    "Id" TEXT NOT NULL CONSTRAINT "PrimaryKey_Id" PRIMARY KEY,
    "ConsoleOutput" TEXT NULL,
    "RegexPattern" TEXT NULL,
    "Result" TEXT NULL,
    "QuietMessage" INTEGER NOT NULL
);

CREATE INDEX "IX_ParsingRules_ConsoleOutput" ON "ParsingRules" ("ConsoleOutput");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20230625102009_AddIndexConsoleOutput', '8.0.7');

COMMIT;

BEGIN TRANSACTION;

CREATE INDEX "IX_ParsingRules_RegexPattern" ON "ParsingRules" ("RegexPattern");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240902103646_AddIndexRegexPattern', '8.0.7');

COMMIT;

