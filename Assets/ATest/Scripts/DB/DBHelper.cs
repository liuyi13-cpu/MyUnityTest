using com.fanxing.consts;
using Mono.Data.Sqlite;
using System;
using System.IO;
using System.Text;

public class DBHelper
{
    SqliteConnection conn;
    static DBHelper instance;

    DBHelper()
    {
    }

    public static DBHelper Instance
    {
        get
        {
            if (instance == null)
                instance = new DBHelper();
            return instance;
        }
    }

    // 解压db文件
    public void ExtractDb(string dbZipPath)
    {
        ZIPHelper.Extract(dbZipPath, Common.dbPath);
    }

    // 打开数据库
    public void OpenConnection(string dbPath)
    {
        string connStr = "Data Source=" + dbPath;
        try
        {
	        conn = new SqliteConnection(connStr);
	        conn.Open();
        }
        catch (Exception e)
        {
            Debugger.LogError(e.Message);
        }
    }

    // 附加数据库
    public void AttachDb(string path, string dbName = "localDb")
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("attach database '{0}' as {1}", path, dbName);
        Execute(sb.ToString());
    }

    // 关闭连接
    public void CloseConnection()
    {
        if (conn != null)
        {
            conn.Close();
            conn.Dispose();
            conn = null;
            SqliteConnection.ClearAllPools();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }

    // 执行查询，并返回由查询返回的结果集中的第一行的第一列
    public object QuerySingle(string sqlQuery, params SqliteParameter[] parameter)
    {
        using (var dbCommand = conn.CreateCommand())
        {
            dbCommand.CommandText = sqlQuery;
            if (parameter != null && parameter.Length > 0)
            {
                dbCommand.Parameters.AddRange(parameter);
            }   
            object obj = dbCommand.ExecuteScalar();
            return obj;
        }
    }

    // 执行并返回影响的行数
    public int Execute(string sqlQuery, SqliteParameter[] parameter = null)
    {
        using (var dbCommand = conn.CreateCommand())
        {
            dbCommand.CommandText = sqlQuery;
            if (parameter != null && parameter.Length > 0)
            {
                dbCommand.Parameters.AddRange(parameter);
            }
            int ret = dbCommand.ExecuteNonQuery();
            return ret;
        }
    }

    // 执行succeedAction，并提交数据库
    public void ExecuteTransaction(Action succeedAction, Action failedAction = null)
    {
        SqliteTransaction trans = conn.BeginTransaction();
        try
        {
            succeedAction();
            trans.Commit();
        }
        catch (Exception e)
        {
            Debugger.LogError(e.Message);
            trans.Rollback();
            if (failedAction != null)
            {
                failedAction();
            }
        }
    }

    // 查询
    public void Query(string sql, Action<SqliteDataReader> call, params SqliteParameter[] parameter)
    {
        //Debug.LogWarning("Query: " + sql);
        if (string.IsNullOrEmpty(sql)) return;
        if (call == null) return;

        using (var dbCommand = conn.CreateCommand())
        {
            dbCommand.CommandText = sql;
            if (parameter != null && parameter.Length > 0)
            {
                dbCommand.Parameters.AddRange(parameter);
            }
            var reader = dbCommand.ExecuteReader();
            call(reader);

            if (reader != null && !reader.IsClosed)
            {
                reader.Close();
                reader = null;
            }
        }
    }

    // 查询1
    public void Query(StringBuilder sql, Action<SqliteDataReader> call, params SqliteParameter[] parameter)
    {
        Query(sql.ToString(), call, parameter);
    }

    // 获取某一列数据
    public byte[] GetBytes(SqliteDataReader reader, int col)
    {
        const int SIZE = 128;
        byte[] buffer = new byte[SIZE];
        long bytesRead;
        long fieldOffset = 0;
        using (MemoryStream ms = new MemoryStream())
        {
            while ((bytesRead = reader.GetBytes(col, fieldOffset, buffer, 0, buffer.Length)) > 0)
            {
                ms.Write(buffer, 0, (int)bytesRead);
                fieldOffset += bytesRead;
            }
            return ms.ToArray();
        }
    }
}
