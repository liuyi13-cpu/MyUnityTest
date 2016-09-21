﻿using System.Collections.Generic; 

/// <summary>
/// THIS SOURCE CODE WAS AUTO-GENERATED BY TOOL, DO NOT MODIFY IT!
/// </summary>
public class DB_Equip_slot : IDB_BaseInt
{
	const string SQL = @"select _index, id, name, attr1_ID, attr2_ID, attr3_ID, attr4_ID from conf_equip_slot";

	/// <summary>
	///序号
	/// </summary>
	public int _index { get; set; }
	/// <summary>
	///部位ID
	/// </summary>
	public int Id { get; set; }
	/// <summary>
	///名字
	/// </summary>
	public string Name { get; set; }
	/// <summary>
	///属性1
	/// </summary>
	public int Attr1_ID { get; set; }
	/// <summary>
	///属性2
	/// </summary>
	public int Attr2_ID { get; set; }
	/// <summary>
	///属性3
	/// </summary>
	public int Attr3_ID { get; set; }
	/// <summary>
	///属性4
	/// </summary>
	public int Attr4_ID { get; set; }

	static Dictionary<int, IDB_BaseInt> LoadDB()
	{
		Dictionary<int, IDB_BaseInt> tmp = new Dictionary<int, IDB_BaseInt>();
		DBHelper.Instance.Query(SQL, (reader) =>
		{
			if (reader == null) return;

			while (reader.Read())
			{
				DB_Equip_slot module = new DB_Equip_slot()
				{
					_index		 = reader.GetInt32(0),
					Id		 = reader.GetInt32(1),
					Name		 = reader.GetString(2),
					Attr1_ID		 = reader.GetInt32(3),
					Attr2_ID		 = reader.GetInt32(4),
					Attr3_ID		 = reader.GetInt32(5),
					Attr4_ID		 = reader.GetInt32(6),
				};
				tmp.Add(module.Id, module);
			}
		});
		return tmp;
	}
}