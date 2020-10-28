using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using NPoco;
using RefactorThis.Models;

namespace RefactorThis.DatabaseFactory
{
    public class DbFactory : IDbFactory
    {
        private readonly ConnectionStrings _connStrings;

        public DbFactory(IOptions<ConnectionStrings> connStringsAccessor)
        {
            _connStrings = connStringsAccessor.Value;
        }

        public IDatabase GetConnection()
        {
            return new Database(_connStrings.SqliteConnectionString, DatabaseType.SQLite, SqliteFactory.Instance);
        }
    }
}
