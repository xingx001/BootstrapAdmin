﻿//#define SQLite
//#define MySQL
//#define Npgsql
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;

namespace UnitTest
{
    public static class TestHelper
    {
        /// <summary>
        /// 获得当前工程解决方案目录
        /// </summary>
        /// <returns></returns>
        public static string RetrieveSolutionPath()
        {
            var dirSeparator = Path.DirectorySeparatorChar;
            var paths = AppContext.BaseDirectory.SpanSplit($"{dirSeparator}.vs{dirSeparator}");
            return paths.Count > 1 ? paths[0] : Path.Combine(AppContext.BaseDirectory, $"..{dirSeparator}..{dirSeparator}..{dirSeparator}..{dirSeparator}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public static string RetrievePath(string folder)
        {
            var soluFolder = RetrieveSolutionPath();
            return Path.Combine(soluFolder, folder);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void CopyLicense()
        {
            var licFile = RetrievePath($"UnitTest{Path.DirectorySeparatorChar}License{Path.DirectorySeparatorChar}Longbow.lic");

            var targetFile = Path.Combine(AppContext.BaseDirectory, "Longbow.lic");
            if (!File.Exists(targetFile)) File.Copy(licFile, targetFile, true);

#if SQLite
            CopySQLiteDBFile();
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        public static void CopySQLiteDBFile()
        {
            var dbPath = RetrievePath($"UnitTest{Path.DirectorySeparatorChar}DB{Path.DirectorySeparatorChar}UnitTest.db");
            var dbFile = Path.Combine(AppContext.BaseDirectory, "UnitTest.db");
            if (!File.Exists(dbFile)) File.Copy(dbPath, dbFile);
        }

        private const string SqlConnectionString = "Data Source=.;Initial Catalog=UnitTest;User ID=sa;Password=sa";
        private const string SQLiteConnectionString = "Data Source=UnitTest.db;";
        private const string MySqlConnectionString = "Server=localhost;Database=UnitTest;Uid=argozhang;Pwd=argo@163.com;SslMode=none;";
        private const string NpgSqlConnectionString = "Server=localhost;Database=UnitTest;User ID=argozhang;Password=sa;";

        public static void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(app => app.AddInMemoryCollection(new KeyValuePair<string, string>[] {
                new KeyValuePair<string, string>("ConnectionStrings:ba", SqlConnectionString),
                new KeyValuePair<string, string>("DB:0:Enabled", "true")
            }));
#if SQLite
            builder.ConfigureAppConfiguration(app => app.AddInMemoryCollection(new KeyValuePair<string, string>[] {
                new KeyValuePair<string, string>("DB:0:Enabled", "false"),
                new KeyValuePair<string, string>("DB:1:Enabled", "true"),
                new KeyValuePair<string, string>("DB:1:ConnectionStrings:ba", SQLiteConnectionString)
            }));
#endif

#if MySQL
            builder.ConfigureAppConfiguration(app => app.AddInMemoryCollection(new KeyValuePair<string, string>[] {
                new KeyValuePair<string, string>("DB:0:Enabled", "false"),
                new KeyValuePair<string, string>("DB:2:Enabled", "true"),
                new KeyValuePair<string, string>("DB:2:ConnectionStrings:ba", MySqlConnectionString)
            }));
#endif

#if Npgsql
            builder.ConfigureAppConfiguration(app => app.AddInMemoryCollection(new KeyValuePair<string, string>[] {
                new KeyValuePair<string, string>("DB:0:Enabled", "false"),
                new KeyValuePair<string, string>("DB:3:Enabled", "true"),
                new KeyValuePair<string, string>("DB:3:ConnectionStrings:ba", NpgSqlConnectionString)
            }));
#endif
        }

        public static IConfiguration CreateConfiguraton()
        {
            var config = new ConfigurationBuilder().AddInMemoryCollection(new KeyValuePair<string, string>[] {
                new KeyValuePair<string, string>("ConnectionStrings:ba", SqlConnectionString),
                new KeyValuePair<string, string>("DB:0:Enabled", "false"),

                new KeyValuePair<string, string>("DB:1:Enabled", "false"),
                new KeyValuePair<string, string>("DB:1:ProviderName", "SQLite"),
                new KeyValuePair<string, string>("DB:1:ConnectionStrings:ba", SQLiteConnectionString),

                new KeyValuePair<string, string>("DB:2:Enabled", "false"),
                new KeyValuePair<string, string>("DB:2:ProviderName", "MySql"),
                new KeyValuePair<string, string>("DB:2:ConnectionStrings:ba", MySqlConnectionString),

                new KeyValuePair<string, string>("DB:3:Enabled", "false"),
                new KeyValuePair<string, string>("DB:3:ProviderName", "NPgsql"),
                new KeyValuePair<string, string>("DB:3:ConnectionStrings:ba", NpgSqlConnectionString),
                new KeyValuePair<string, string>("LongbowCache:Enabled", "false")
            });

#if SQLite
            config.AddInMemoryCollection(new KeyValuePair<string, string>[] {
                new KeyValuePair<string, string>("DB:1:Enabled", "true")
            });
            CopySQLiteDBFile();
#endif

#if MySQL
            config.AddInMemoryCollection(new KeyValuePair<string, string>[] {
                new KeyValuePair<string, string>("DB:2:Enabled", "true")
            });
#endif

#if Npgsql
            config.AddInMemoryCollection(new KeyValuePair<string, string>[] {
                new KeyValuePair<string, string>("DB:3:Enabled", "true")
            });
#endif
            return config.Build();
        }
    }
}
