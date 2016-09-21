﻿using System.Collections.Generic; 

/// <summary>
/// THIS SOURCE CODE WAS AUTO-GENERATED BY TOOL, DO NOT MODIFY IT!
/// </summary>
public class DB_Title : IDB_BaseInt
{
	const string SQL = @"select _index, id, comment, title, num from conf_title";

	/// <summary>
	///序号
	/// </summary>
	public int _index { get; set; }
	/// <summary>
	///位置编号
	/// </summary>
	public int Id { get; set; }
	/// <summary>
	///备注
	/// </summary>
	public string Comment { get; set; }
	/// <summary>
	///标题
	/// </summary>
	public string Title { get; set; }
	/// <summary>
	///使用模板
	/// </summary>
	public int Num { get; set; }

	static Dictionary<int, IDB_BaseInt> LoadDB()
	{
		Dictionary<int, IDB_BaseInt> tmp = new Dictionary<int, IDB_BaseInt>();
		DBHelper.Instance.Query(SQL, (reader) =>
		{
			if (reader == null) return;

			while (reader.Read())
			{
				DB_Title module = new DB_Title()
				{
					_index		 = reader.GetInt32(0),
					Id		 = reader.GetInt32(1),
					Comment		 = reader.GetString(2),
					Title		 = reader.GetString(3),
					Num		 = reader.GetInt32(4),
				};
				tmp.Add(module.Id, module);
			}
		});
		return tmp;
	}
}