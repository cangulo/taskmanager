﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskManagerAPI.EF.Context;

namespace TaskManagerAPI.EF.MigrationManager
{
    public class DBMigrationsManager : IDBMigrationsManager
    {
        private readonly TaskManagerDbContext _dbContext;
        private readonly ILogger _logger;


        public DBMigrationsManager(TaskManagerDbContext dbContext, ILogger<DBMigrationsManager> logger)
        {
            this._dbContext = dbContext;
            _logger = logger;
        }

        public void MigrateDB()
        {
            List<string> pendingMigrationsName = _dbContext.Database.GetPendingMigrations().ToList();
            try
            {
                if (pendingMigrationsName.Any())
                {
                    this._logger.LogInformation("Starting pending migrations");
                    var migrator = _dbContext.Database.GetService<IMigrator>();
                    foreach (var migrationName in pendingMigrationsName)
                    {
                        this._logger.LogInformation($"Starting migration {migrationName}");
                        migrator.Migrate(migrationName);
                    }
                }
                else
                {
                    this._logger.LogInformation("There are no pending migrations");
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "error doing the migrations");
            }

        }
    }
}
