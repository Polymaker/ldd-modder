using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Data.SQLite;
using System.Linq.Expressions;

namespace LDDModder.PaletteMaker.DB
{
    static class DbHelper
    {
        public static string GetTableName<T>()
        {
            var tableAttr = typeof(T).GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.TableAttribute>();
            return tableAttr?.Name ?? typeof(T).Name;
        }

        public static string GetTableName(Type type)
        {
            var tableAttr = type.GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.TableAttribute>();
            return tableAttr?.Name ?? type.Name;
        }

        public static void InitializeInsertCommand<T>(SQLiteCommand cmd)
        {
            cmd.Parameters.Clear();

            var insertColumns = new List<string>();
            var insertParameters = new List<string>();

            foreach (var prop in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (prop.GetMethod.IsPrivate)
                    continue;

                var ignoreAttr = prop.GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.NotMappedAttribute>();
                if (ignoreAttr != null)
                    continue;

                var dbGenAttr = prop.GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedAttribute>();
                if (dbGenAttr != null &&
                    dbGenAttr.DatabaseGeneratedOption != System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    continue;

                var fkAttr = prop.GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.ForeignKeyAttribute>();
                if (fkAttr != null)
                    continue;

                var columnAttr = prop.GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.ColumnAttribute>();

                string columnName = columnAttr?.Name ?? prop.Name;

                insertColumns.Add(columnName);
                insertParameters.Add("$" + prop.Name);

                System.Data.DbType paramType = System.Data.DbType.String;

                if (prop.PropertyType.Name.ToLower().Contains("int"))
                    paramType = System.Data.DbType.Int32;
                else if (prop.PropertyType.Name.ToLower().Contains("bool"))
                    paramType = System.Data.DbType.Boolean;

                cmd.Parameters.Add("$" + prop.Name, paramType);
            }
            cmd.CommandText = $"INSERT INTO {GetTableName<T>()} ({string.Join(",", insertColumns)}) VALUES ({string.Join(",", insertParameters)})";
        }

        public static void InitializeInsertCommand<T>(SQLiteCommand cmd, Expression<Func<T, object>> columnOrder)
        {
            var columnProperties = new List<PropertyInfo>();

            if (columnOrder.Body is NewExpression ne)
            {
                foreach (var member in ne.Arguments.OfType<MemberExpression>())
                {
                    if (member.Member is PropertyInfo pi)
                        columnProperties.Add(pi);
                }
            }

            cmd.Parameters.Clear();

            var insertColumns = new List<string>();
            var insertParameters = new List<string>();

            foreach (var prop in columnProperties)
            {
                if (prop.GetMethod.IsPrivate)
                    continue;

                var dbGenAttr = prop.GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedAttribute>();
                if (dbGenAttr != null &&
                    dbGenAttr.DatabaseGeneratedOption != System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    continue;

                var fkAttr = prop.GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.ForeignKeyAttribute>();
                if (fkAttr != null)
                    continue;

                var columnAttr = prop.GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.ColumnAttribute>();

                string columnName = columnAttr?.Name ?? prop.Name;

                var ignoreAttr = prop.GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.NotMappedAttribute>();
                if (ignoreAttr != null)
                {
                    var mappedProp = typeof(T)
                        .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                        .FirstOrDefault(x => x.GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.ColumnAttribute>()?.Name == prop.Name);

                    if (mappedProp == null)
                        continue;
                }

                insertColumns.Add(columnName);
                insertParameters.Add("$" + prop.Name);

                System.Data.DbType paramType = System.Data.DbType.String;

                if (prop.PropertyType.Name.ToLower().Contains("int"))
                    paramType = System.Data.DbType.Int32;
                else if (prop.PropertyType.Name.ToLower().Contains("bool"))
                    paramType = System.Data.DbType.Boolean;

                cmd.Parameters.Add("$" + prop.Name, paramType);
            }
            cmd.CommandText = $"INSERT INTO {GetTableName<T>()} ({string.Join(",", insertColumns)}) VALUES ({string.Join(",", insertParameters)})";
        }

        public static void OrderCommandParameters(SQLiteCommand cmd, params string[] parameterNames)
        {
            var tmpParams = new List<Tuple<string, System.Data.DbType>>();
            for (int i = 0; i < cmd.Parameters.Count; i++)
                tmpParams.Add(new Tuple<string, System.Data.DbType>(cmd.Parameters[i].ParameterName, cmd.Parameters[i].DbType));

            cmd.Parameters.Clear();

            for (int i = 0; i < parameterNames.Length; i++)
            {
                var cmdParam = tmpParams.FirstOrDefault(x => x.Item1 == parameterNames[i] || x.Item1 == "$" + parameterNames[i]);
                cmd.Parameters.Add(cmdParam.Item1, cmdParam.Item2);
            }
        }

        public static void OrderCommandParameters<T>(SQLiteCommand cmd, Expression<Func<T, object>> fields)
        {
            var tmpParams = new List<Tuple<string, System.Data.DbType>>();
            for (int i = 0; i < cmd.Parameters.Count; i++)
                tmpParams.Add(new Tuple<string, System.Data.DbType>(cmd.Parameters[i].ParameterName, cmd.Parameters[i].DbType));

            cmd.Parameters.Clear();

            var parameterNames = new List<string>();

            if (fields.Body is NewExpression ne)
            {
                foreach (var member in ne.Arguments.OfType<MemberExpression>())
                {
                    if (member.Member is PropertyInfo pi)
                        parameterNames.Add(pi.Name);
                }
            }

            for (int i = 0; i < parameterNames.Count; i++)
            {
                var cmdParam = tmpParams.FirstOrDefault(x => x.Item1 == parameterNames[i] || x.Item1 == "$" + parameterNames[i]);
                cmd.Parameters.Add(cmdParam.Item1, cmdParam.Item2);
            }
        }

        public static int InsertAutoIncObject(SQLiteCommand insertCmd, SQLiteCommand rowidCmd, params object[] values)
        {
            for (int i = 0; i < values.Length; i++)
                insertCmd.Parameters[i].Value = values[i];
            insertCmd.ExecuteNonQuery();

            long rowid = (long)rowidCmd.ExecuteScalar();
            return (int)rowid;
        }

        public static void InsertWithParameters(SQLiteCommand insertCmd, params object[] values)
        {
            for (int i = 0; i < values.Length; i++)
                insertCmd.Parameters[i].Value = values[i];
            insertCmd.ExecuteNonQuery();
        }

        public static int InsertWithParameters(SQLiteCommand insertCmd, SQLiteCommand rowidCmd, params object[] values)
        {
            for (int i = 0; i < values.Length; i++)
                insertCmd.Parameters[i].Value = values[i];
            insertCmd.ExecuteNonQuery();

            long rowid = (long)rowidCmd.ExecuteScalar();
            return (int)rowid;
        }

        public static void InsertObject<T>(SQLiteCommand insertCmd, T objectToInsert)
        {
            for (int i = 0; i < insertCmd.Parameters.Count; i++)
            {
                string propName = insertCmd.Parameters[i].ParameterName.Substring(1);
                var propInfo = typeof(T).GetProperty(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (propInfo != null)
                    insertCmd.Parameters[i].Value = propInfo.GetValue(objectToInsert);
            }
            insertCmd.ExecuteNonQuery();
        }
    }
}
