using System.Collections.Generic; 

public class DB_Equip_adhere_attr : IDB_BaseInt
{
	const string SQL = @"select _index, id, equip_levelregion, equip_level, attr_type, probregion_floor, probregion_upper, valueregion_floor, valueregion_upper, is_exclusive from conf_equip_adhere_attr";

	/// <summary>
	///序号
	/// </summary>
	public int _index { get; set; }
	/// <summary>
	///部位ID
	/// </summary>
	public int Id { get; set; }
	/// <summary>
	///装备等级段
	/// </summary>
	public int Equip_levelregion { get; set; }
	/// <summary>
	///装备等级
	/// </summary>
	public int Equip_level { get; set; }
	/// <summary>
	///属性类型
	/// </summary>
	public int Attr_type { get; set; }
	/// <summary>
	///概率区间下限
	/// </summary>
	public int Probregion_floor { get; set; }
	/// <summary>
	///概率区间上限
	/// </summary>
	public int Probregion_upper { get; set; }
	/// <summary>
	///取值下限
	/// </summary>
	public float Valueregion_floor { get; set; }
	/// <summary>
	///取值上限
	/// </summary>
	public float Valueregion_upper { get; set; }
	/// <summary>
	///是否专属
	/// </summary>
	public int Is_exclusive { get; set; }

	static Dictionary<int, IDB_BaseInt> LoadDB()
	{
		Dictionary<int, IDB_BaseInt> tmp = new Dictionary<int, IDB_BaseInt>();
		DBHelper.Instance.Query(SQL, (reader) =>
		{
			if (reader == null) return;

			while (reader.Read())
			{
				DB_Equip_adhere_attr module = new DB_Equip_adhere_attr()
				{
					_index		 = reader.GetInt32(0),
					Id		 = reader.GetInt32(1),
					Equip_levelregion		 = reader.GetInt32(2),
					Equip_level		 = reader.GetInt32(3),
					Attr_type		 = reader.GetInt32(4),
					Probregion_floor		 = reader.GetInt32(5),
					Probregion_upper		 = reader.GetInt32(6),
					Valueregion_floor		 = reader.GetFloat(7),
					Valueregion_upper		 = reader.GetFloat(8),
					Is_exclusive		 = reader.GetInt32(9),
				};
				tmp.Add(module.Id, module);
			}
		});
		return tmp;
	}
}
