using System.Collections.Generic; 

public class DB_Activities_battle_flag : IDB_BaseString
{
	const string SQL = @"select _index, key, map, load_pos from conf_activities_battle_flag";

	/// <summary>
	///序号
	/// </summary>
	public int _index { get; set; }
	/// <summary>
	///旗子
	/// </summary>
	public string Key { get; set; }
	/// <summary>
	///场景
	/// </summary>
	public string Map { get; set; }
	/// <summary>
	///旗子加载位置
	/// </summary>
	public string Load_pos { get; set; }

	static Dictionary<string, IDB_BaseString> LoadDB()
	{
		Dictionary<string, IDB_BaseString> tmp = new Dictionary<string, IDB_BaseString>();
		DBHelper.Instance.Query(SQL, (reader) =>
		{
			if (reader == null) return;

			while (reader.Read())
			{
				DB_Activities_battle_flag module = new DB_Activities_battle_flag()
				{
					_index		 = reader.GetInt32(0),
					Key		 = reader.GetString(1),
					Map		 = reader.GetString(2),
					Load_pos		 = reader.GetString(3),
				};
				tmp.Add(module.Key, module);
			}
		});
		return tmp;
	}
}
