﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShowNTell.EntityFramework.ShowNTellDbContextFactories
{
    public class ShowNTellDbContextFactory : IShowNTellDbContextFactory
    {
        private readonly DbContextOptions _options;

        public ShowNTellDbContextFactory(DbContextOptions options)
        {
            _options = options;
        }

        public ShowNTellDbContextFactory(Action<DbContextOptionsBuilder> optionsBuilder)
        {
            DbContextOptionsBuilder builder = new DbContextOptionsBuilder();
            optionsBuilder(builder);
            _options = builder.Options;
        }

        public ShowNTellDbContext CreateDbContext()
        {
            return new ShowNTellDbContext(_options);
        }
    }
}
