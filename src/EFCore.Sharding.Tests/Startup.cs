﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace EFCore.Sharding.Tests
{
    [TestClass]
    public class Startup
    {
        /// <summary>
        /// 所有单元测试开始前
        /// </summary>
        /// <param name="context"></param>
        [AssemblyInitialize]
        public static void Begin(TestContext context)
        {
            ServiceCollection services = new ServiceCollection();
            services.UseEFCoreSharding(config =>
            {
                config.UseDatabase(Config.SQLITE1, DatabaseType.SQLite);
                config.UseDatabase<ICustomDbAccessor>(Config.SQLITE1, DatabaseType.SQLite);
                config.AddAbsDb(DatabaseType.SQLite)
                    .AddPhysicDb(ReadWriteType.Read | ReadWriteType.Write, Config.SQLITE1)
                    .AddPhysicDbGroup()
                    .SetHashModShardingRule<Base_UnitTest>(nameof(Base_UnitTest.Id), 3);
            });

            ServiceProvider = services.BuildServiceProvider();
        }

        /// <summary>
        /// 所有单元测试结束后
        /// </summary>
        [AssemblyCleanup]
        public static void End()
        {

        }

        public static IServiceProvider ServiceProvider;
    }
}
