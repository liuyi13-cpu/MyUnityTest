﻿using System.Collections.Generic; 

/// <summary>
/// THIS SOURCE CODE WAS AUTO-GENERATED BY TOOL, DO NOT MODIFY IT!
/// </summary>
public class DB_Strategy_line_boss : IDB_BaseInt
{
	const string SQL = @"select _index, sector_id, strategy_random_stay, boss_battle_id, boss_icon, boss_level, boss_id, boss_name from conf_strategy_line_boss";

	/// <summary>
	///序号
	/// </summary>
	public int _index { get; set; }
	/// <summary>
	///防线BOSS的id
	/// </summary>
	public int Sector_id { get; set; }
	/// <summary>
	///BOSS位置
	/// </summary>
	public string Strategy_random_stay { get; set; }
	/// <summary>
	///boss战id（conf_activities_battle_public.xlsx中读）
	/// </summary>
	public int Boss_battle_id { get; set; }
	/// <summary>
	///BOSS使用icon
	/// </summary>
	public int Boss_icon { get; set; }
	/// <summary>
	///BOSS等级（字库）
	/// </summary>
	public string Boss_level { get; set; }
	/// <summary>
	///BOSS数据（npc表conf_npc_tank_public.xlsx）
	/// </summary>
	public int Boss_id { get; set; }
	/// <summary>
	///BOSS名称
	/// </summary>
	public string Boss_name { get; set; }

	static Dictionary<int, IDB_BaseInt> LoadDB()
	{
		Dictionary<int, IDB_BaseInt> tmp = new Dictionary<int, IDB_BaseInt>();
		DBHelper.Instance.Query(SQL, (reader) =>
		{
			if (reader == null) return;

			while (reader.Read())
			{
				DB_Strategy_line_boss module = new DB_Strategy_line_boss()
				{
					_index		 = reader.GetInt32(0),
					Sector_id		 = reader.GetInt32(1),
					Strategy_random_stay		 = reader.GetString(2),
					Boss_battle_id		 = reader.GetInt32(3),
					Boss_icon		 = reader.GetInt32(4),
					Boss_level		 = reader.GetString(5),
					Boss_id		 = reader.GetInt32(6),
					Boss_name		 = reader.GetString(7),
				};
				tmp.Add(module.Sector_id, module);
			}
		});
		return tmp;
	}
}