﻿using Microsoft.EntityFrameworkCore;
using MockPremierLeague.API.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MockPremierLeague.Test
{
    public static class DbContextMocker
    {
        public static AppDbContext GetMockPremierLeagueImportersDbContext(string dbName)
        {
            // Create options for DbContext instance
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            // Create instance of DbContext
            var dbContext = new AppDbContext(options);

            // Add entities in memory
            dbContext.Seed();

            return dbContext;
        }
    } 
}