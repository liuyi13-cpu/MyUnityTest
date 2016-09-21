﻿using System.Collections.Generic; 

/// <summary>
/// THIS SOURCE CODE WAS AUTO-GENERATED BY TOOL, DO NOT MODIFY IT!
/// </summary>
public class DB_Strategy_zone_link : IDB_BaseIntDouble
{
	const string SQL = @"select _index, strategy_zone_id, unlocked_strategy_zone from conf_strategy_zone_link";

	/// <summary>
	///序号
	/// </summary>
	public int _index { get; set; }
	/// <summary>
	///战区id
	/// </summary>
	public int Strategy_zone_id { get; set; }
	/// <summary>
	///战区连接
	/// </summary>
	public int Unlocked_strategy_zone { get; set; }

	static Dictionary<int, Dictionary<int, IDB_BaseIntDouble>> LoadDB()
	{
		Dictionary<int, Dictionary<int, IDB_BaseIntDouble>> tmp = new Dictionary<int, Dictionary<int, IDB_BaseIntDouble>>();
		DBHelper.Instance.Query(SQL, (reader) =>
		{
			if (reader == null) return;

			while (reader.Read())
			{
				DB_Strategy_zone_link module = new DB_Strategy_zone_link()
				{
					_index		 = reader.GetInt32(0),
					Strategy_zone_id		 = reader.GetInt32(1),
					Unlocked_strategy_zone		 = reader.GetInt32(2),
				};
				if (!tmp.ContainsKey(module.Strategy_zone_id))
				{
					var tmpsub = new Dictionary<int, IDB_BaseIntDouble>();
					tmp.Add(module.Strategy_zone_id, tmpsub);
				}
				tmp[module.Strategy_zone_id].Add(module.Unlocked_strategy_zone, module);
			}
		});
		return tmp;
	}
}