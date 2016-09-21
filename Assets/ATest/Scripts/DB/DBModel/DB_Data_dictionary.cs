using System.Collections.Generic;

public class DB_Data_dictionary
{
    const string SQL = @"select key, value from conf_data_dictionary";
    static public Dictionary<string, string> text = null;

    static void LoadDB()
    {
        DBHelper.Instance.Query(SQL, (reader) =>
        {
            if (reader == null) return;

            while (reader.Read())
            {
                text[reader.GetString(0)] = reader.GetString(1);
            }
        });
    }

    public static string getText(string key)
    {
        if (text == null)
        {
            text = new Dictionary<string, string>();
            LoadDB();
        }

        return text[key];
    }
}
