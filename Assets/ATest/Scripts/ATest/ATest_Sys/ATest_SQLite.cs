using Mono.Data.Sqlite;
using System;
using System.IO;
using System.Text;

public class ATest_SQLite : ATest_Base
{
    public override void Test()
    {
        string dbFile = @"test.db";
        string connenctStr = string.Format(@"Data Source={0};Pooling=true;FailIfMissing=false", dbFile);

        // SqliteConnection m_Connection1 = new SqliteConnection(connenctStr);
        // Debugger.LogWarning("m_Connection : " + m_Connection.GetHashCode());
        // m_Connection1.Open();
        SqliteConnection m_Connection = new SqliteConnection(connenctStr);
        // Debugger.LogWarning("m_Connection : " + m_Connection.GetHashCode());
        m_Connection.Open();

        using (var com = m_Connection.CreateCommand())
        {
            com.CommandText = @"select 1";
            com.ExecuteNonQuery();
            com.Cancel();
        }
        m_Connection.Close();
        m_Connection.Dispose();
        SqliteConnection.ClearAllPools();
        GC.Collect();
        GC.WaitForPendingFinalizers();
        try
        {
            File.Delete(dbFile);
        }
        catch (Exception e)
        {
            throw e;
        }
    }
}