﻿using System.Collections.Generic; 

/// <summary>
/// THIS SOURCE CODE WAS AUTO-GENERATED BY TOOL, DO NOT MODIFY IT!
/// </summary>
public class DB_Buff_info : IDB_BaseIntDouble
{
	const string SQL = @"select _index, id, buff_level, name, comment, description, lasting_time, interval, buff_tipsId, effect, action_buff_type, icon, is_overall, son_target_type, son_target_number, son_buff1, son_buff1_level, son_buff1_condition_logic, son_buff1_condition, son_buff1_time, son_buff2, son_buff2_level, son_buff2_condition_logic, son_buff2_condition, son_buff2_time, status_change, attr1, attr1_change_number, attr2, attr2_change_number, attr3, attr3_change_number, attr4, attr4_change_number, attr5, attr5_change_number, attr6, attr6_change_number, skill_group, son_skill1, son_skill1_level, son_skill1_condition_logic, son_skill1_condition, son_skill1_time, son_skill2, son_skill2_level, son_skill2_condition_logic, son_skill2_condition, son_skill2_time from conf_buff_info";

	/// <summary>
	///序号
	/// </summary>
	public int _index { get; set; }
	/// <summary>
	///buffID
	/// </summary>
	public int Id { get; set; }
	/// <summary>
	///buff等级
	/// </summary>
	public int Buff_level { get; set; }
	/// <summary>
	///buff名称
	/// </summary>
	public string Name { get; set; }
	/// <summary>
	///备注
	/// </summary>
	public string Comment { get; set; }
	/// <summary>
	///buff描述
	/// </summary>
	public string Description { get; set; }
	/// <summary>
	///持续时间
	/// </summary>
	public float Lasting_time { get; set; }
	/// <summary>
	///间隔时间
	/// </summary>
	public float Interval { get; set; }
	/// <summary>
	///状态提示
	/// </summary>
	public int Buff_tipsId { get; set; }
	/// <summary>
	///buff特效
	/// </summary>
	public int Effect { get; set; }
	/// <summary>
	///行为状态
	/// </summary>
	public int Action_buff_type { get; set; }
	/// <summary>
	///buff图标
	/// </summary>
	public int Icon { get; set; }
	/// <summary>
	///是否全局
	/// </summary>
	public int Is_overall { get; set; }
	/// <summary>
	///子目标类型
	/// </summary>
	public int Son_target_type { get; set; }
	/// <summary>
	///子目标数量
	/// </summary>
	public int Son_target_number { get; set; }
	/// <summary>
	///子buff1ID
	/// </summary>
	public int Son_buff1 { get; set; }
	/// <summary>
	///子buff1等级
	/// </summary>
	public int Son_buff1_level { get; set; }
	/// <summary>
	///子buff1触发条件逻辑
	/// </summary>
	public int Son_buff1_condition_logic { get; set; }
	/// <summary>
	///子buff1触发条件
	/// </summary>
	public float Son_buff1_condition { get; set; }
	/// <summary>
	///子buff1触发时间
	/// </summary>
	public float Son_buff1_time { get; set; }
	/// <summary>
	///子buff2ID
	/// </summary>
	public int Son_buff2 { get; set; }
	/// <summary>
	///子buff2等级
	/// </summary>
	public int Son_buff2_level { get; set; }
	/// <summary>
	///子buff2触发条件逻辑
	/// </summary>
	public int Son_buff2_condition_logic { get; set; }
	/// <summary>
	///子buff2触发条件
	/// </summary>
	public float Son_buff2_condition { get; set; }
	/// <summary>
	///子buff2触发时间
	/// </summary>
	public float Son_buff2_time { get; set; }
	/// <summary>
	///状态改变
	/// </summary>
	public int Status_change { get; set; }
	/// <summary>
	///属性1
	/// </summary>
	public int Attr1 { get; set; }
	/// <summary>
	///属性1改变数值
	/// </summary>
	public float Attr1_change_number { get; set; }
	/// <summary>
	///属性2
	/// </summary>
	public int Attr2 { get; set; }
	/// <summary>
	///属性2改变数值
	/// </summary>
	public float Attr2_change_number { get; set; }
	/// <summary>
	///属性3
	/// </summary>
	public int Attr3 { get; set; }
	/// <summary>
	///属性3改变数值
	/// </summary>
	public float Attr3_change_number { get; set; }
	/// <summary>
	///属性4
	/// </summary>
	public int Attr4 { get; set; }
	/// <summary>
	///属性4改变数值
	/// </summary>
	public float Attr4_change_number { get; set; }
	/// <summary>
	///属性5
	/// </summary>
	public int Attr5 { get; set; }
	/// <summary>
	///属性5改变数值
	/// </summary>
	public float Attr5_change_number { get; set; }
	/// <summary>
	///属性6
	/// </summary>
	public int Attr6 { get; set; }
	/// <summary>
	///属性6改变数值
	/// </summary>
	public float Attr6_change_number { get; set; }
	/// <summary>
	///技能组
	/// </summary>
	public int Skill_group { get; set; }
	/// <summary>
	///子技能1
	/// </summary>
	public int Son_skill1 { get; set; }
	/// <summary>
	///子技能1等级
	/// </summary>
	public int Son_skill1_level { get; set; }
	/// <summary>
	///子技能1触发条件逻辑
	/// </summary>
	public int Son_skill1_condition_logic { get; set; }
	/// <summary>
	///子技能1触发条件
	/// </summary>
	public float Son_skill1_condition { get; set; }
	/// <summary>
	///子技能1触发时间
	/// </summary>
	public float Son_skill1_time { get; set; }
	/// <summary>
	///子技能2
	/// </summary>
	public int Son_skill2 { get; set; }
	/// <summary>
	///子技能2等级
	/// </summary>
	public int Son_skill2_level { get; set; }
	/// <summary>
	///子技能2触发条件逻辑
	/// </summary>
	public int Son_skill2_condition_logic { get; set; }
	/// <summary>
	///子技能2触发条件
	/// </summary>
	public float Son_skill2_condition { get; set; }
	/// <summary>
	///子技能2触发时间
	/// </summary>
	public float Son_skill2_time { get; set; }

	static Dictionary<int, Dictionary<int, IDB_BaseIntDouble>> LoadDB()
	{
		Dictionary<int, Dictionary<int, IDB_BaseIntDouble>> tmp = new Dictionary<int, Dictionary<int, IDB_BaseIntDouble>>();
		DBHelper.Instance.Query(SQL, (reader) =>
		{
			if (reader == null) return;

			while (reader.Read())
			{
				DB_Buff_info module = new DB_Buff_info()
				{
					_index		 = reader.GetInt32(0),
					Id		 = reader.GetInt32(1),
					Buff_level		 = reader.GetInt32(2),
					Name		 = reader.GetString(3),
					Comment		 = reader.GetString(4),
					Description		 = reader.GetString(5),
					Lasting_time		 = reader.GetFloat(6),
					Interval		 = reader.GetFloat(7),
					Buff_tipsId		 = reader.GetInt32(8),
					Effect		 = reader.GetInt32(9),
					Action_buff_type		 = reader.GetInt32(10),
					Icon		 = reader.GetInt32(11),
					Is_overall		 = reader.GetInt32(12),
					Son_target_type		 = reader.GetInt32(13),
					Son_target_number		 = reader.GetInt32(14),
					Son_buff1		 = reader.GetInt32(15),
					Son_buff1_level		 = reader.GetInt32(16),
					Son_buff1_condition_logic		 = reader.GetInt32(17),
					Son_buff1_condition		 = reader.GetFloat(18),
					Son_buff1_time		 = reader.GetFloat(19),
					Son_buff2		 = reader.GetInt32(20),
					Son_buff2_level		 = reader.GetInt32(21),
					Son_buff2_condition_logic		 = reader.GetInt32(22),
					Son_buff2_condition		 = reader.GetFloat(23),
					Son_buff2_time		 = reader.GetFloat(24),
					Status_change		 = reader.GetInt32(25),
					Attr1		 = reader.GetInt32(26),
					Attr1_change_number		 = reader.GetFloat(27),
					Attr2		 = reader.GetInt32(28),
					Attr2_change_number		 = reader.GetFloat(29),
					Attr3		 = reader.GetInt32(30),
					Attr3_change_number		 = reader.GetFloat(31),
					Attr4		 = reader.GetInt32(32),
					Attr4_change_number		 = reader.GetFloat(33),
					Attr5		 = reader.GetInt32(34),
					Attr5_change_number		 = reader.GetFloat(35),
					Attr6		 = reader.GetInt32(36),
					Attr6_change_number		 = reader.GetFloat(37),
					Skill_group		 = reader.GetInt32(38),
					Son_skill1		 = reader.GetInt32(39),
					Son_skill1_level		 = reader.GetInt32(40),
					Son_skill1_condition_logic		 = reader.GetInt32(41),
					Son_skill1_condition		 = reader.GetFloat(42),
					Son_skill1_time		 = reader.GetFloat(43),
					Son_skill2		 = reader.GetInt32(44),
					Son_skill2_level		 = reader.GetInt32(45),
					Son_skill2_condition_logic		 = reader.GetInt32(46),
					Son_skill2_condition		 = reader.GetFloat(47),
					Son_skill2_time		 = reader.GetFloat(48),
				};
				if (!tmp.ContainsKey(module.Id))
				{
					var tmpsub = new Dictionary<int, IDB_BaseIntDouble>();
					tmp.Add(module.Id, tmpsub);
				}
				tmp[module.Id].Add(module.Buff_level, module);
			}
		});
		return tmp;
	}
}