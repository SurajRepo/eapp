﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using VPortal.Core.Data.Crud;

namespace VPortal.Core.Data.MsSQL
{
    public class MsSQLFactory : IDatabaseFactory
    {
        public IDbConnection Db { get; set; }
        public Dialect Dialect => Dialect.SQLServer;

        public QueryBuilder QueryBuilder { get; }

        private readonly string _connectionString;

        public IDbConnection GetConnection()
        {
            Db = new SqlConnection(_connectionString);
            return Db;
        }
        
        public MsSQLFactory(IConfigurationRoot configuration, IServiceProvider serviceProvider)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");// ConfigurationManager.ConnectionStrings[connectionStringName].ToString();
            Db = new SqlConnection(_connectionString);
        }

        public void Dispose()
        {
            if (Db.State == ConnectionState.Open)
                Db.Close();
            Db.Close();
        }
    }
}
