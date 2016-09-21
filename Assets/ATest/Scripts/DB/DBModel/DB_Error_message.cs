﻿using System.Collections.Generic; 

/// <summary>
/// THIS SOURCE CODE WAS AUTO-GENERATED BY TOOL, DO NOT MODIFY IT!
/// </summary>
public class DB_Error_message : IDB_BaseString
{
	const string SQL = @"select _index, name, notes, showType, error_code from conf_error_message";

	/// <summary>
	///序号
	/// </summary>
	public int _index { get; set; }
	/// <summary>
	///名称
	/// </summary>
	public string Name { get; set; }
	/// <summary>
	///备注
	/// </summary>
	public string Notes { get; set; }
	/// <summary>
	///显示类型 0-简单提示 1-弹板提示
	/// </summary>
	public int ShowType { get; set; }
	/// <summary>
	///服务器错误代号
	/// </summary>
	public int Error_code { get; set; }

	static Dictionary<string, IDB_BaseString> LoadDB()
	{
		Dictionary<string, IDB_BaseString> tmp = new Dictionary<string, IDB_BaseString>();
		DBHelper.Instance.Query(SQL, (reader) =>
		{
			if (reader == null) return;

			while (reader.Read())
			{
				DB_Error_message module = new DB_Error_message()
				{
					_index		 = reader.GetInt32(0),
					Name		 = reader.GetString(1),
					Notes		 = reader.GetString(2),
					ShowType		 = reader.GetInt32(3),
					Error_code		 = reader.GetInt32(4),
				};
				tmp.Add(module.Name, module);
			}
		});
		return tmp;
	}
}