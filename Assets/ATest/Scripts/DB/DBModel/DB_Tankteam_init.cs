﻿using System.Collections.Generic; 

/// <summary>
/// THIS SOURCE CODE WAS AUTO-GENERATED BY TOOL, DO NOT MODIFY IT!
/// </summary>
public class DB_Tankteam_init : IDB_BaseInt
{
	const string SQL = @"select _index, team_order_id, comment, team_name_key, ui_sprite_id, capacity, init_lock_state, order_ui_sprite_id from conf_tankteam_init";

	/// <summary>
	///序号
	/// </summary>
	public int _index { get; set; }
	/// <summary>
	///编队顺序id号
	/// </summary>
	public int Team_order_id { get; set; }
	/// <summary>
	///备注（策划）
	/// </summary>
	public string Comment { get; set; }
	/// <summary>
	///键值
	/// </summary>
	public string Team_name_key { get; set; }
	/// <summary>
	///编队图标
	/// </summary>
	public int Ui_sprite_id { get; set; }
	/// <summary>
	///编队容量
	/// </summary>
	public int Capacity { get; set; }
	/// <summary>
	///编队锁定初始状态(1解锁0锁定)
	/// </summary>
	public int Init_lock_state { get; set; }
	/// <summary>
	///编队左边选择编队对应的图片
	/// </summary>
	public int Order_ui_sprite_id { get; set; }

	static Dictionary<int, IDB_BaseInt> LoadDB()
	{
		Dictionary<int, IDB_BaseInt> tmp = new Dictionary<int, IDB_BaseInt>();
		DBHelper.Instance.Query(SQL, (reader) =>
		{
			if (reader == null) return;

			while (reader.Read())
			{
				DB_Tankteam_init module = new DB_Tankteam_init()
				{
					_index		 = reader.GetInt32(0),
					Team_order_id		 = reader.GetInt32(1),
					Comment		 = reader.GetString(2),
					Team_name_key		 = reader.GetString(3),
					Ui_sprite_id		 = reader.GetInt32(4),
					Capacity		 = reader.GetInt32(5),
					Init_lock_state		 = reader.GetInt32(6),
					Order_ui_sprite_id		 = reader.GetInt32(7),
				};
				tmp.Add(module.Team_order_id, module);
			}
		});
		return tmp;
	}
}