using System;
using System.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zxw.Framework.NetCore.CodeGenerator;
using Zxw.Framework.NetCore.DbContextCore;
using Zxw.Framework.NetCore.Extensions;
using Zxw.Framework.NetCore.IoC;
using Zxw.Framework.NetCore.Options;

namespace Zxw.Framework.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestGetDataTable()
        {
            BuildService();
            var dbContext = AspectCoreContainer.Resolve<IDbContextCore>();
            var dt1 = dbContext.GetCurrentDatabaseAllTables();
            Assert.IsNotNull(dt1);
            foreach (DataRow row in dt1.Rows)
            {
                var dt2 = dbContext.GetTableColumns(row["TableName"].ToString());
                Assert.IsNotNull(dt2);
            }
        }

        [TestMethod]
        public void TestGetDataTableList()
        {
            BuildService();
            var dbContext = AspectCoreContainer.Resolve<IDbContextCore>();
            var tables = dbContext.GetCurrentDatabaseTableList();
            Assert.IsNotNull(tables);
        }

        [TestMethod]
        public void TestGenerateEntities()
        {
            BuildService();
            CodeGenerator.GenerateEntities("Zxw.Framework.Website.Models","F:\\Test");
        }

        public IServiceProvider BuildService()
        {
            IServiceCollection services = new ServiceCollection();

            //在这里注册EF上下文
            services = RegisterPostgreSQLContext(services);

            services.AddOptions();
            return AspectCoreContainer.BuildServiceProvider(services);//接入AspectCore.Injector
        }
        /// <summary>
        /// 注册SQLServer上下文
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public IServiceCollection RegisterSqlServerContext(IServiceCollection services)
        {
            services.Configure<DbContextOption>(options =>
            {
                options.ConnectionString = "initial catalog=NetCoreDemo;data source=127.0.0.1;password=admin123!@#;User id=sa;MultipleActiveResultSets=True;";
                //options.ModelAssemblyName = "Zxw.Framework.Website.Models";
            });
            services.AddScoped<IDbContextCore, SqlServerDbContext>(); //注入EF上下文
            return services;
        }
        /// <summary>
        /// 注册MySQL上下文
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public IServiceCollection RegisterMySqlContext(IServiceCollection services)
        {
            services.Configure<DbContextOption>(options =>
            {
                options.ConnectionString = "Server=183.230.47.18;Database=NetCoreDemo; User ID=root;Password=qazwsxedc123456;port=3306;CharSet=utf8;pooling=true;";
                //options.ModelAssemblyName = "Zxw.Framework.Website.Models";
            });
            services.AddScoped<IDbContextCore, MySqlDbContext>(); //注入EF上下文
            return services;
        }
        /// <summary>
        /// 注册PostgreSQL上下文
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public IServiceCollection RegisterPostgreSQLContext(IServiceCollection services)
        {
            services.Configure<DbContextOption>(options =>
            {
                options.ConnectionString = "User ID=postgres;Password=admin123!@#;Host=localhost;Port=5432;Database=ZxwPgDemo;Pooling=true;";
                options.ModelAssemblyName = "Zxw.Framework.Website.Models";
            });
            services.AddScoped<IDbContextCore, PostgreSQLDbContext>(); //注入EF上下文
            return services;
        }
        /// <summary>
        /// 注册SQLite上下文
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public IServiceCollection RegisterSQLiteContext(IServiceCollection services)
        {
            services.Configure<DbContextOption>(options =>
            {
                options.ConnectionString = "Data Source=F:\\EF6.db;Version=3;";
                //options.ModelAssemblyName = "Zxw.Framework.Website.Models";
            });
            services.AddScoped<IDbContextCore, SQLiteDbContext>(); //注入EF上下文
            return services;
        }
    }
}
