﻿using System.Collections.Generic; 

/// <summary>
/// THIS SOURCE CODE WAS AUTO-GENERATED BY TOOL, DO NOT MODIFY IT!
/// </summary>
public class DB_Drop : IDB_BaseInt
{
	const string SQL = @"select _index, id, comment, drop_item1_type, drop_item1, drop_item1_rate, drop_item1_count, drop_item2_type, drop_item2, drop_item2_rate, drop_item2_count, drop_item3_type, drop_item3, drop_item3_rate, drop_item3_count, drop_item4_type, drop_item4, drop_item4_rate, drop_item4_count, drop_item5_type, drop_item5, drop_item5_rate, drop_item5_count, drop_item6_type, drop_item6, drop_item6_rate, drop_item6_count, drop_item7_type, drop_item7, drop_item7_rate, drop_item7_count, drop_item8_type, drop_item8, drop_item8_rate, drop_item8_count, drop_item9_type, drop_item9, drop_item9_rate, drop_item9_count, drop_item10_type, drop_item10, drop_item10_rate, drop_item10_count from conf_drop";

	/// <summary>
	///序号
	/// </summary>
	public int _index { get; set; }
	/// <summary>
	///掉落组ID
	/// </summary>
	public int Id { get; set; }
	/// <summary>
	///备注
	/// </summary>
	public string Comment { get; set; }
	/// <summary>
	///掉落1类型
	/// </summary>
	public int Drop_item1_type { get; set; }
	/// <summary>
	///掉落物品ID1
	/// </summary>
	public int Drop_item1 { get; set; }
	/// <summary>
	///掉落物品1概率
	/// </summary>
	public int Drop_item1_rate { get; set; }
	/// <summary>
	///掉落物品1数量
	/// </summary>
	public int Drop_item1_count { get; set; }
	/// <summary>
	///掉落2类型
	/// </summary>
	public int Drop_item2_type { get; set; }
	/// <summary>
	///掉落物品ID2
	/// </summary>
	public int Drop_item2 { get; set; }
	/// <summary>
	///掉落物品2概率
	/// </summary>
	public int Drop_item2_rate { get; set; }
	/// <summary>
	///掉落物品2数量
	/// </summary>
	public int Drop_item2_count { get; set; }
	/// <summary>
	///掉落3类型
	/// </summary>
	public int Drop_item3_type { get; set; }
	/// <summary>
	///掉落物品ID3
	/// </summary>
	public int Drop_item3 { get; set; }
	/// <summary>
	///掉落物品3概率
	/// </summary>
	public int Drop_item3_rate { get; set; }
	/// <summary>
	///掉落物品3数量
	/// </summary>
	public int Drop_item3_count { get; set; }
	/// <summary>
	///掉落4类型
	/// </summary>
	public int Drop_item4_type { get; set; }
	/// <summary>
	///掉落物品ID4
	/// </summary>
	public int Drop_item4 { get; set; }
	/// <summary>
	///掉落物品4概率
	/// </summary>
	public int Drop_item4_rate { get; set; }
	/// <summary>
	///掉落物品4数量
	/// </summary>
	public int Drop_item4_count { get; set; }
	/// <summary>
	///掉落5类型
	/// </summary>
	public int Drop_item5_type { get; set; }
	/// <summary>
	///掉落物品ID5
	/// </summary>
	public int Drop_item5 { get; set; }
	/// <summary>
	///掉落物品5概率
	/// </summary>
	public int Drop_item5_rate { get; set; }
	/// <summary>
	///掉落物品5数量
	/// </summary>
	public int Drop_item5_count { get; set; }
	/// <summary>
	///掉落6类型
	/// </summary>
	public int Drop_item6_type { get; set; }
	/// <summary>
	///掉落物品ID6
	/// </summary>
	public int Drop_item6 { get; set; }
	/// <summary>
	///掉落物品6概率
	/// </summary>
	public int Drop_item6_rate { get; set; }
	/// <summary>
	///掉落物品6数量
	/// </summary>
	public int Drop_item6_count { get; set; }
	/// <summary>
	///掉落7类型
	/// </summary>
	public int Drop_item7_type { get; set; }
	/// <summary>
	///掉落物品ID7
	/// </summary>
	public int Drop_item7 { get; set; }
	/// <summary>
	///掉落物品7概率
	/// </summary>
	public int Drop_item7_rate { get; set; }
	/// <summary>
	///掉落物品7数量
	/// </summary>
	public int Drop_item7_count { get; set; }
	/// <summary>
	///掉落8类型
	/// </summary>
	public int Drop_item8_type { get; set; }
	/// <summary>
	///掉落物品ID8
	/// </summary>
	public int Drop_item8 { get; set; }
	/// <summary>
	///掉落物品8概率
	/// </summary>
	public int Drop_item8_rate { get; set; }
	/// <summary>
	///掉落物品8数量
	/// </summary>
	public int Drop_item8_count { get; set; }
	/// <summary>
	///掉落9类型
	/// </summary>
	public int Drop_item9_type { get; set; }
	/// <summary>
	///掉落物品ID9
	/// </summary>
	public int Drop_item9 { get; set; }
	/// <summary>
	///掉落物品9概率
	/// </summary>
	public int Drop_item9_rate { get; set; }
	/// <summary>
	///掉落物品9数量
	/// </summary>
	public int Drop_item9_count { get; set; }
	/// <summary>
	///掉落10类型
	/// </summary>
	public int Drop_item10_type { get; set; }
	/// <summary>
	///掉落物品ID10
	/// </summary>
	public int Drop_item10 { get; set; }
	/// <summary>
	///掉落物品10概率
	/// </summary>
	public int Drop_item10_rate { get; set; }
	/// <summary>
	///掉落物品10数量
	/// </summary>
	public int Drop_item10_count { get; set; }

	static Dictionary<int, IDB_BaseInt> LoadDB()
	{
		Dictionary<int, IDB_BaseInt> tmp = new Dictionary<int, IDB_BaseInt>();
		DBHelper.Instance.Query(SQL, (reader) =>
		{
			if (reader == null) return;

			while (reader.Read())
			{
				DB_Drop module = new DB_Drop()
				{
					_index		 = reader.GetInt32(0),
					Id		 = reader.GetInt32(1),
					Comment		 = reader.GetString(2),
					Drop_item1_type		 = reader.GetInt32(3),
					Drop_item1		 = reader.GetInt32(4),
					Drop_item1_rate		 = reader.GetInt32(5),
					Drop_item1_count		 = reader.GetInt32(6),
					Drop_item2_type		 = reader.GetInt32(7),
					Drop_item2		 = reader.GetInt32(8),
					Drop_item2_rate		 = reader.GetInt32(9),
					Drop_item2_count		 = reader.GetInt32(10),
					Drop_item3_type		 = reader.GetInt32(11),
					Drop_item3		 = reader.GetInt32(12),
					Drop_item3_rate		 = reader.GetInt32(13),
					Drop_item3_count		 = reader.GetInt32(14),
					Drop_item4_type		 = reader.GetInt32(15),
					Drop_item4		 = reader.GetInt32(16),
					Drop_item4_rate		 = reader.GetInt32(17),
					Drop_item4_count		 = reader.GetInt32(18),
					Drop_item5_type		 = reader.GetInt32(19),
					Drop_item5		 = reader.GetInt32(20),
					Drop_item5_rate		 = reader.GetInt32(21),
					Drop_item5_count		 = reader.GetInt32(22),
					Drop_item6_type		 = reader.GetInt32(23),
					Drop_item6		 = reader.GetInt32(24),
					Drop_item6_rate		 = reader.GetInt32(25),
					Drop_item6_count		 = reader.GetInt32(26),
					Drop_item7_type		 = reader.GetInt32(27),
					Drop_item7		 = reader.GetInt32(28),
					Drop_item7_rate		 = reader.GetInt32(29),
					Drop_item7_count		 = reader.GetInt32(30),
					Drop_item8_type		 = reader.GetInt32(31),
					Drop_item8		 = reader.GetInt32(32),
					Drop_item8_rate		 = reader.GetInt32(33),
					Drop_item8_count		 = reader.GetInt32(34),
					Drop_item9_type		 = reader.GetInt32(35),
					Drop_item9		 = reader.GetInt32(36),
					Drop_item9_rate		 = reader.GetInt32(37),
					Drop_item9_count		 = reader.GetInt32(38),
					Drop_item10_type		 = reader.GetInt32(39),
					Drop_item10		 = reader.GetInt32(40),
					Drop_item10_rate		 = reader.GetInt32(41),
					Drop_item10_count		 = reader.GetInt32(42),
				};
				tmp.Add(module.Id, module);
			}
		});
		return tmp;
	}
}