using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Npgsql;

/// <summary>
/// Подключение базы
/// </summary>
namespace abtest
{
    public class DBConnect : IDisposable
    {
        public readonly static string StaticConnectionStringDB = ConfigurationManager.ConnectionStrings["abtest"].ConnectionString;
        protected string ConnectionStringDB = ConfigurationManager.ConnectionStrings["abtest"].ConnectionString;
        public string ConnectionString => ConnectionStringDB;

        protected string ProviderDB = "System.Data.SqlClient";

        protected List<string> DebugDB = new List<string>();

        public bool DebugEnable = false;
        public DBConnect()
        {
        }
        public DBConnect(bool debug_enable)
        {
            DebugEnable = debug_enable;
        }
        public DBConnect(string connectionStr)
        {
            if (string.IsNullOrEmpty(connectionStr))
                throw new ArgumentNullException($"Connection string {nameof(connectionStr)} cannot be empty");
            ConnectionStringDB = connectionStr;
        }

        #region OldMethods
        public IEnumerable<dynamic> DBQuery(string command)
        {
            using (var db = new NpgsqlConnection(ConnectionStringDB))
            {
                return db.Query(command);
            }
        }
        public IEnumerable<dynamic> DBQuery(string command, params object[] args)
        {
            using (var db = new NpgsqlConnection(ConnectionStringDB))
            {
                return db.Query(command, QHelper.GenParams(args));
            }
        }
        public dynamic DBQuerySingle(string command)
        {
            using (var db = new NpgsqlConnection(ConnectionStringDB))
            {
                return db.QueryFirstOrDefault(command);
            }
        }
        public dynamic DBQuerySingle(string command, params object[] args)
        {
            using (var db = new NpgsqlConnection(ConnectionStringDB))
            {
                return db.QueryFirstOrDefault(command, QHelper.GenParams(args));
            }
        }
        public dynamic DBExecute(string command, params object[] args)
        {
            using (var db = new NpgsqlConnection(ConnectionStringDB))
            {
                return db.Execute(command, QHelper.GenParams(args));
            }
        }
        #endregion

        public static IEnumerable<dynamic> Query(string command)
        {
            using (var db = new NpgsqlConnection(StaticConnectionStringDB))
            {
                return db.Query<dynamic>(command) as IEnumerable<dynamic>;
            }
        }
        public static IEnumerable<dynamic> Query(string command, params object[] args)
        {
            using (var db = new NpgsqlConnection(StaticConnectionStringDB))
            {
                return db.Query<dynamic>(command, QHelper.GenParams(args)) as IEnumerable<dynamic>;
            }
        }
        public static dynamic QueryFirstOrDefault(string command, params object[] args)
        {
            using (var db = new NpgsqlConnection(StaticConnectionStringDB))
            {
                return db.QueryFirstOrDefault(command, QHelper.GenParams(args));
            }
        }
        public static T QueryFirstOrDefault<T>(string command, params object[] args)
        {
            using (var db = new NpgsqlConnection(StaticConnectionStringDB))
            {
                return db.QueryFirstOrDefault<T>(command, QHelper.GenParams(args));
            }
        }
        public static IEnumerable<T> Query<T>(string command, params object[] args)
        {
            using (var db = new NpgsqlConnection(StaticConnectionStringDB))
            {
                return db.Query<T>(command, QHelper.GenParams(args)) as IEnumerable<T>;
            }
        }

        public static void Execute(string command, params object[] args)
        {
            using (var db = new NpgsqlConnection(StaticConnectionStringDB))
            {
                db.Query(command, QHelper.GenParams(args));
            }
        }


        public void Dispose()
        {

        }

        public void Close()
        {
            //OpenDB.Close();
        }

        ~DBConnect()
        {
            // OpenDB.Close();
            // OpenDB.Dispose();
            DebugDB.Clear();

            // NpgsqlConnection.ClearAllPools();
        }
    }
    static class QHelper
    {
        // INSERT INTO abc (field0, field1, field2) VALUES (@0, @1, @2)
        public static string Make(string query, params object[] args)
        {
            if (query == null || query == string.Empty)
                throw new ArgumentNullException("DBConnect: Empty query exception");

            if (!query.Contains("@") && args.Length > 0)
                throw new ArgumentException("DBConnect: Wrong request parameters");

            if (args == null)
                throw new ArgumentNullException();

            string String = query;

            int i = 0;
            for (i = 0; i < args.Length; i++)
            {
                var correction = args[i].ToString();

                if (args[i] == null)
                    correction = "NULL";

                if (args[i] is string)
                    if ((string)args[i] == string.Empty)
                        correction = "''";
                    else
                        correction = $"'{correction}'";

                String = String.Replace("@" + i, correction);
            }

            return String;
        }
        public static DynamicParameters GenParams(params object[] args)
        {
            if (args == null)
                throw new ArgumentNullException();

            var dbArgs = new DynamicParameters();
            for (int i = 0; i < args.Length; i++)
                dbArgs.Add("@" + i, args[i]);

            return dbArgs;
        }
        public static IEnumerable<DynamicParameters> GenBatchParams(params object[] argsArray)
        {
            foreach (object[] args in argsArray)
            {
                yield return GenParams(args[0], args[1]);
            }
        }
    }
}