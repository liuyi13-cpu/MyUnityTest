using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Text;
using Data;
using Utils;

/// <summary>
/// 基础数据的加载管理
/// </summary>
public class BaseDataManager
{
    #region Singleton
    static BaseDataManager mInstance;

    public static BaseDataManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new BaseDataManager();
            }
            return mInstance;
        }
    }

    public static void Free()
    {
        if (mInstance != null)
        {
            mInstance.protoPath = null;
            mInstance.serializer = null;
        }
        mInstance = null;
    }
    #endregion

    private string protoPath;

    Serialize serializer;
	public string loginURL;

    private BaseDataManager()
    {
        protoPath = Application.persistentDataPath + "/GameResources/";
        serializer = new Serialize();

    }

    public T LoadProtoData<T>(string name)
    {
        string md5Name = CommonUtils.GenerateMD5(Encoding.UTF8.GetBytes(name));
        string filePath = protoPath + md5Name;
        DebugUtils.Log(DebugUtils.Type.Data, "load proto file " + name + ", path = " + filePath);
        byte[] data = null;
        if (File.Exists(filePath))
        {
            data = File.ReadAllBytes(filePath);
        }
        else
        {
            filePath = name.Substring(0, name.IndexOf('.'));
            TextAsset asset = Resources.Load<TextAsset>("Bytes/" + filePath);
            if (asset != null)
            {
                data = asset.bytes;
            }
            else
            {
                Debuger.LogError("Proto file load failed: " + filePath);
                return default(T);
            }
        }
        return Deserialize<T>(data);
    }

    public byte[] Serialize(object dataObject)
    {
        DebugUtils.Assert(dataObject != null);
        byte[] buffer = null;
        using (MemoryStream m = new MemoryStream()) // use a cached memory stream instead?
        {
            serializer.Serialize(m, dataObject);
            m.Position = 0;
            int length = (int)m.Length;
            buffer = new byte[length];
            m.Read(buffer, 0, length);
        }
        return buffer;
    }

    public T Deserialize<T>(byte[] data)
    {
        //DebugUtils.Assert( data != null && data.Length > 0 );
        T dataObject;
        using (MemoryStream m = new MemoryStream(data,0,data.Length))
        {
            dataObject = (T)serializer.Deserialize(m, null, typeof(T));
        }
        return dataObject;
    }

    //储存游戏数据 背包，天赋...
    public void SavaData(byte[] data, string path)
    {
        Stream sw;
        FileInfo f = new FileInfo(Application.persistentDataPath + path);
        if (f.Exists)
        {
            f.Delete();
            f = new FileInfo(Application.persistentDataPath + path);
            sw = f.Create();
        }
        else
        {
            sw = f.Create();
        }
        sw.Write(data, 0, data.Length);
        sw.Close();
        sw.Dispose();
    }


    //读取游戏数据
    public void LoadData(ref byte[] data, string path)
    {
        Stream sw;
        FileInfo f = new FileInfo(Application.persistentDataPath + path);
        if (f.Exists)
        {
            sw = f.OpenRead();
            data = new byte[sw.Length];
            int length = sw.Read(data, 0, data.Length);
            DebugUtils.Assert(sw.Length == length);
            sw.Close();
            sw.Dispose();
        }
    }

    public IEnumerator LoadGameBaseData(Action loadEndCB)
    {
		ProxyManager.baseValueProxy.SetProtoValue(LoadProtoData<BaseValueProto>("base_value.bytes"));
        yield return 1;
        ProxyManager.textProxy.SetProtoValue(LoadProtoData<TextProto>("Text.bytes"));
        yield return 1;
        ProxyManager.announcementProxy.SetProtoValue(LoadProtoData<AnnouncementProto>("announcement.bytes"));
        yield return 1;
        Resources.UnloadUnusedAssets();
        yield return 1;

        if (loadEndCB != null)
        {
            loadEndCB();
        }
        else
        {
            DebugUtils.LogWarning(DebugUtils.Type.Data, "LoadGameBaseData CB = null.");
        }
    }

    public IEnumerator LoadGameAdvData(Action loadEndCB)
    {
        //uluaMgr = LuaScriptMgr.Instance;

        //if (GameConstants.GameDebug && uluaMgr == null)
        //{
        //    uluaMgr = new LuaScriptMgr();
        //    uluaMgr.Start();
        //}

        //uluaMgr.DoFile("DataManager.lua");
        //uluaMgr.DoFile("gamedesign.lua");
        GameManager gm = GameManager.GetSingleton();


        //uluaMgr.lua["DataManager.baseValueData"] = baseValueData;
       // ProxyManager.weaponProxy.SetProtoValue(LoadProtoData<weapo>("baseValue.bytes"));
        //yield return 1;
		ProxyManager.equipmentAppearProxy.SetProtoValue(LoadProtoData<EquipmentAppearanceProto>("equipment_appearance.bytes"));
        yield return 1;
		ProxyManager.modificationAppearProxy.SetProtoValue(LoadProtoData<ModificationAppearanceProto>("modification_appearance.bytes"));
		yield return 1;
        //ProxyManager.w2mRulesProxy.SetProtoValue(LoadProtoData<WeaponModifyRuleProto>("weaponModifyRule.bytes"));
        //yield return 1;
        ProxyManager.equipmentProxy.SetProtoValue(LoadProtoData<EquipmentProto>("equipment.bytes"));
        yield return 1;
        ProxyManager.weaponModifyPosProxy.SetProtoValue(LoadProtoData<WeaponModifyPos>("WM.bytes"));
        yield return 1;
        ProxyManager.itemProxy.SetProtoValue(LoadProtoData<ItemProto>("item.bytes"));
        yield return 1;
        //ProxyManager.mapProxy.SetProtoValue(LoadProtoData<MapDataProto>("1_1_1.bytes"));
        //yield return 1;
        //ProxyManager.battlePointProxy.SetProtoValue(LoadProtoData<BattlePointProto>("battlePoint.bytes"));
        //yield return 1;
        ProxyManager.enemyProxy.SetProtoValue(LoadProtoData<EnemyProto>("enemy.bytes"));
        yield return 1;
        ProxyManager.buffProxy.SetProtoValue(LoadProtoData<BuffProto>("buff.bytes"));
        yield return 1;
        ProxyManager.skillProxy.SetProtoValue(LoadProtoData<SkillProto>("skill.bytes"));
        yield return 1;
		ProxyManager.skillEffectProxy.SetProtoValue(LoadProtoData<SkillEffectProto>("skill_effect.bytes"));
        yield return 1;
        ProxyManager.levelProxy.SetProtoValue(LoadProtoData<RoundsProto>("rounds.bytes"));
        yield return 1;
        ProxyManager.sceneProxy.SetProtoValue(LoadProtoData<SceneProto>("scene.bytes"));
        yield return 1;
        ProxyManager.shopProxy.SetProtoValue(LoadProtoData<ShopProto>("shop.bytes"));
        yield return 1;
		ProxyManager.shopItemProxy.SetProtoValue(LoadProtoData<ShopItemProto>("shop_item.bytes"));
        yield return 1;
        ProxyManager.partnerProxy.SetProtoValue(LoadProtoData<PartnerProto>("partner.bytes"));
        yield return 1;
		ProxyManager.atlasBDProxy.SetProtoValue(LoadProtoData<AtlasProto>("atlas.bytes"));
		yield return 1;
        ProxyManager.skillDataProxy.InitBaseData(ProxyManager.baseValueProxy.GetStringValue("hero_skill_slot_level"));
        yield return 1;
        ProxyManager.skillExchangeProxy.SetProtoValue(LoadProtoData<SkillExchangeProto>("skill_exchange.bytes"));
		yield return 1;
		ProxyManager.camouflagProxy.SetProtoValue(LoadProtoData<CamouflageProto>("camouflage.bytes"));
        yield return 1;
        ProxyManager.tacticsExpProxy.SetProtoValue(LoadProtoData<TacticsExpProto>("tactics_exp.bytes"));
        yield return 1;
        ProxyManager.vipProxy.SetProtoValue(LoadProtoData<VipProto>("vip.bytes"));
        yield return 1;
        ProxyManager.gunFireProxy.SetProtoValue(LoadProtoData<GunfireProto>("gunfire_index.bytes"));
		yield return 1;
		ProxyManager.carrierProxy.SetProtoValue(LoadProtoData<VehicleProto>("vehicle.bytes"));
        yield return 1;
        ProxyManager.funcUnlockProxy.SetProtoValue(LoadProtoData<FunctionUnlockProto>("function_unlock.bytes"));
        yield return 1;
        ProxyManager.missionDataProxy.SetProtoValue(LoadProtoData<DailyMissionProto>("daily_mission.bytes"));
		yield return 1;
		ProxyManager.combatSoulProxy.SetProtoValue(LoadProtoData<CombatSoulProto>("combat_soul.bytes"));
        yield return 1;
		ProxyManager.titleDataProxy.SetProtoValue(LoadProtoData<TitleProto>("title_list.bytes"));
        yield return 1;
        ProxyManager.activityProxy.SetActivityData(LoadProtoData<ActivityProto>("activity.bytes"));
        yield return 1;
        ProxyManager.activityProxy.SetFoundationData(LoadProtoData<ActivityFundProto>("activity_fund.bytes"));
        yield return 1;
        ProxyManager.activityProxy.SetPrizeData(LoadProtoData<ActivityPrizeProto>("activity_prize.bytes"));
        yield return 1;
        ProxyManager.militaryRankProxy.SetProtoValue(LoadProtoData<MilitaryRankProto>("military_rank.bytes"));
        yield return 1;
        ProxyManager.playerExpProxy.SetProtoValue(LoadProtoData<LevelProto>("level.bytes"));
		yield return 1;
		ProxyManager.partnerMissionProxy.SetProtoValue(LoadProtoData<PartnerMissionProto>("partner_mission.bytes"));
		yield return 1;
		ProxyManager.payProxy.SetProtoValue(LoadProtoData<PayProto>("pay.bytes"));
		yield return 1;
        Resources.UnloadUnusedAssets();
        yield return 1;
		ResLoaderLua.GetSingleton ().LoadLuaFile ();
        if (loadEndCB != null)
        {
            loadEndCB();
        }
        else
        {
            DebugUtils.LogWarning(DebugUtils.Type.Data, "LoadGameBaseData CB = null.");
        }
    }



	void Update()
	{
		
	}

}
