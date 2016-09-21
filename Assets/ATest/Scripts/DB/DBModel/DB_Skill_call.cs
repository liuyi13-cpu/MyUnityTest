﻿using System.Collections.Generic; 

/// <summary>
/// THIS SOURCE CODE WAS AUTO-GENERATED BY TOOL, DO NOT MODIFY IT!
/// </summary>
public class DB_Skill_call : IDB_BaseInt
{
	const string SQL = @"select _index, id, tank_id, alive_time, pos_id from conf_skill_call";

	/// <summary>
	///序号
	/// </summary>
	public int _index { get; set; }
	/// <summary>
	///召唤ID
	/// </summary>
	public int Id { get; set; }
	/// <summary>
	///坦克ID
	/// </summary>
	public int Tank_id { get; set; }
	/// <summary>
	///存活时间
	/// </summary>
	public float Alive_time { get; set; }
	/// <summary>
	///加载位置ID
	/// </summary>
	public int Pos_id { get; set; }

	static Dictionary<int, IDB_BaseInt> LoadDB()
	{
		Dictionary<int, IDB_BaseInt> tmp = new Dictionary<int, IDB_BaseInt>();
		DBHelper.Instance.Query(SQL, (reader) =>
		{
			if (reader == null) return;

			while (reader.Read())
			{
				DB_Skill_call module = new DB_Skill_call()
				{
					_index		 = reader.GetInt32(0),
					Id		 = reader.GetInt32(1),
					Tank_id		 = reader.GetInt32(2),
					Alive_time		 = reader.GetFloat(3),
					Pos_id		 = reader.GetInt32(4),
				};
				tmp.Add(module.Id, module);
			}
		});
		return tmp;
	}
}