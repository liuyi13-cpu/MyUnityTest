///-------------------------------------------------------------
// DBManager.cpp
// DB管理器
///-------------------------------------------------------------

#ifndef __DB_MANAGER_H__
#define __DB_MANAGER_H__

#include "cocos2d.h"
#include "base/DB_gamenpc.h"
#include "base/XArmInfo.h"
#include "base/XDrugInfo.h"
#include "base/XMaterial.h"
#include "base/XTreasure.h"
#include "base/XFormulation.h"
#include "base/XGem.h"
#include "base/XGuaInfo.h"
#include "base/XTipMsg.h"
#include "base/XItemShop.h"
#include "base/DB_Text.h"
#include "base/XResourceConsume.h"
#include "base/DB_ActorSkill.h"
#include "base/DB_ActorSk_Pv.h"
#include "base/DB_guildset.h"
#include "base/DB_GuildSkill.h"
#include "base/XVIPSetting.h"
#include "base/DB_JingMai.h"
#include "base/DB_dungeon.h"
#include "base/DB_dungeon_drop.h"
#include "base/DB_gamequest.h"
#include "base/XGuaSetting.h"
#include "base/XLevelinfo.h"
#include "base/DB_heroProperty.h"
#include "base/XShopGoods.h"
#include "base/DB_ShuiBoWen.h"
#include "base/DB_SceneActor.h"
#include "base/DB_Bullet.h"
#include "base/DB_State.h"
#include "base/DB_MonsterProperty.h"
#include "base/DB_JingMaiAdd.h"
#include "base/DB_Environment.h"
#include "base/XXiShuSetting.h"
#include "base/DB_Particle.h"
#include "base/DB_Bianse.h"
#include "base/XSetcombat.h"
#include "base/XLoadingTip.h"
#include "base/DB_MonsterData.h"
#include "base/DB_MonsterDataPublic.h"
#include "base/XFashion.h"
#include "base/DB_Port.h"
#include "base/DB_Function.h"
#include "base/DSkill.h"
#include "base/XFormula.h"
#include "base/DB_Teach.h"
#include "base/DB_Sound.h"
#include "base/DB_city.h"
#include "base/DB_Gameover.h"
#include "base/DB_pvpaward.h"
#include "base/DChargeUpReward.h"
#include "base/XQuestTip.h"
#include "base/DB_PushNotification.h"
#include "base/DB_Huodong.h"
#include "base/DShenjiang.h"
#include "base/DB_ShenJiangProperty.h"
#include "base/DItemShenjiang.h"
#include "base/DB_Buff.h"
#include "base/DPvpDoushentaiAccReward.h"
#include "base/DPvpDoushentaiRankingReward.h"
#include "base/DRaffle.h"
#include "base/DGuildActivities.h"
#include "base/DB_guildStage.h"
#include "base/XTitle.h"
#include "base/DShilian.h"	//添加试炼之路副本, add by zyp，2015-9-21
#include "base/DGodweapon.h"
#include "base/DB_KuafuzhanReward.h"	//添加跨服战, add by zyp，2015-12-22
enum kDBType
{
	kDBTypeNPC,					// NPC
	kDBTypeArm,					// 装备
	kDBTypeDrug,				// 药品
	kDBTypeMaterial,			// 材料
	kDBTypeTreasure,			// 宝箱
	kDBTypeFormulaiton,			// 图纸
	kDBTypeGem,					// 宝石
	kDBTypeGua,					// 卦象
	kDBTypeGuaSetting,			// 卦象设置
	kDBTypeLevelinfo,			// 卦象开孔
	kDBTypeTipMsg,				// 系统提示
	kDBTypeShop,				// 商店
	kDBTypeShopGoods,			// 商店物品
	kDBTypeText,				// 文本
	kDBTypeResourceConsume,		// 资源消耗
	kDBTypeZX_ZhuDongSkill,		// 紫霞主动技能
	kDBTypeNMW_ZhuDongSkill,	// 牛魔王主动技能
	kDBTypeEnemy_ZhuDongSkill,	// 普通敌人主动技能
	kDBTypeZX_PassivitySkill,	// 紫霞被动技能
	kDBTypeGuild,				// 帮派
	kDBTypeGuildSkill,			// 帮派技能
	kDBTypeAllSkill,			// 技能名称汇总
	kDBTypeFunction,			// 开启新功能
	kDBTypeGameover,			// 挑战失败
	kDBTypeHuodong,				// 挑战失败
	kDBTypeVip,					// VIP等级设置
	kDBTypeJingMai,				// 经脉
	kDBTypeJingMaiAdd,			// 经脉额外属性
	kDBTypeDungeon,				// 副本
	kDBTypeDungeonDrops,		// 副本掉落
	kDBTypeQuest,				// 任务
	kDBTypeHeroProperty,		// 主角属性
	kDBTypeShuiBoWen,			// 水波纹类型
	kDBTypeMonsterResource,		// 敌兵类型
	kDBTypeSceneActor,
	kDBTypeBullet,				// 子弹类型
	kDBTypeState,				// 状态转换表
	kDBTypeMonsterProp,			// 怪属性表 
	kDBTypeBossProp,			// Boss属性表
	kDBTypeParticle,			// 粒子映射表
	kDBTypeBianse,				// 敌人变色配置表
	kDBTypeCombat,				// 战力系数
	kDBTypeLoadingTip,			// loading提示
	kDBTypeMonsterData,			// 怪数据分类型
	kDBTypeMonsterDataPublic,	// 怪数据公共
	kDBTypeFashions,			// 主角时装
	kDBTypeTeach,				// 教学
	kDBTypeSoundMap,			// 声音素材
	kDBTypeCity,				// 城镇
	kDBTypePVPAward,			// pvp奖励
	kDBTypePVPMatchAward,		// pvp斗神台奖励
	kDBTypePVPMatchScoreAward,  // pvp斗神台积分奖励
	kDBTypeChargeReward,		// 充值奖励
	kDBTypeQuestTip,			// 任务剧情
	kDBTypePush,				// 推送消息
	kDBTypeShenJiangProperty,	// 神将属性
    kDBTypeShenJiang,			// 神将
    kDBTypeShenJiangItem,		// 神将道具
    kDBTypeShenJiangArm,		// 神将装备
    kDBTypeSJ_ZhuDongSkill,		// 神将主动技能
	kDBTypeRaffle,				// 抽神将
	kDBTypeBuff,				// 技能buff表
	kDBTypeXXiShuSetting,
    kDBTypeXFormula,            // 进阶图纸
    kDBTypeGuildActivities,     // 帮派活动
    kDBTypeGuildBoss,           // 帮派副本
	kDBTypeTitle,               // 称号
	KDBTypeShiLianZhiLu,		// 试炼之路 //添加试炼之路副本, add by zyp，2015-9-21
	kDBTypeHunQi,               //魂器
	KDBTypeKuaFuZhanReward,		//跨服战奖励	//添加跨服战, add by zyp，2015-12-22
	kDBTypeMax,
};

class netSkillInfo;
class DBManager : public cocos2d::CCObject
{
public:
	DBManager();
	virtual ~DBManager();

	static DBManager *sharedDBManager();
	static void purge(void);
	void purgeCachedData();

	/**
	 * 获得Npc
	 */
	DB_gamenpc* getNpc(string& name_id);
	DB_gamenpc* getNpc(int id);



	/**
	 * 获得道具
	 */
	CCObject* getItem(int type, int ID);
    std::string getItemDesc(int ID);
	/**
	 * 获得道具
	 */
	XTipMsg* getTipMsg(int ID);

	/**
	 * 获得商店
	 */
	XItemShop* getItemShop(int ID);

	/************************************************************************/
	/* 获取文本                                                             */
	/************************************************************************/
	string getString(int ID);

	/**
	 * 资源消耗
	 */
	XResourceConsume* getResourceConsume(int ID);
	int getResourceConsumeSize();

	/************************************************************************/
	/* 根据功能所在位置获取功能列表                                                                     */
	/************************************************************************/

	vector<DB_Function*> getFunctionListByType(int type);

	DB_Function* getFunctionBySiteTag(int site,int tag);
	int getFunctionTagNumBySite(int site);
	/****
	****技能总表
	******/
	DB_ActorSk_Pv* getPassivitySkill(int ID);
	DB_ActorSk_Pv* getPassivityBySkillID(std::string skId);
	vector<DB_ActorSk_Pv*> getGuildSkill();
	
	DSkill* getSKillAllBySkillId(std::string skId);

	int getPassivitySkillCount(int type);
	vector<netSkillInfo*> getAllPassivitySkill(int type);
	vector<netSkillInfo*> getAllInitiaSkill(int type);
	vector<netSkillInfo*> getAllBangpaiSkill(int type);

	DB_ActorSkill* getInitiativeSkill(int ID);
	vector<DB_ActorSkill*> getAllInitiativeSkill(int type);

    DB_guildStage* getGuildBossByType(int type, int level);
    std::vector<DB_guildStage*> getGuildBoss();
    std::vector<DGuildActivities*> getGuildMainActivity();
    DGuildActivities* getGuildActivity(int id);
    /***得到主角的所有技能***/
	vector<netSkillInfo*> getAllSkill(int type);

	DB_guildset* getGuild(short level);
	DB_GuildSkill* getGuildSkill(const std::string& skillID);

	XVIPSetting* getVIP(int ID);
	int getVIPMaxNum();

	cocos2d::CCArray* getJingMai();
	cocos2d::CCArray* getJingMaiAdd();

	cocos2d::CCArray* getAllDungeons();
	DB_dungeon* getDungeon(const std::string& key);
	cocos2d::CCArray* getDungeonDrops(const std::string& key);

	cocos2d::CCArray* getAllQuests();

	XGuaSetting* getGuaSetting(int ID);
	DB_ActorSkill* getInSkillById(const string& skId);
	int getUnLockLevel(int holeNum);

	int getMaxExpLevelup(int level);

	DB_heroProperty* getHeroProperty(int ID);
	//获得选人界面任务数量
	int				 getXuanRenCounter();
	//通过选人界面顺序获得相应的heroProperty
	DB_heroProperty* getHRByXuanRenIndex(int index);
    
	cocos2d::CCArray* getShopGoods(int goodID);


	DB_ShuiBoWen*	getShuiBoWenByID(int ID);
	int				getShuiBoWenCounter();

	DB_Particle*	getParticleByID(int ID);

	DB_Bianse*	getBianseByID(int ID);

	//DB_MonsterResource*	getMonsterResourceBySpriteAnim(int spriteIndex, int animIndex);
	//DB_MonsterResource*	getMonsterResourceByID(int ID);

	//获取当前等级全部类型的分类型怪数据
	map<std::string, DB_MonsterData *> getAllTypeDBMonsterDataCurLevel(int curLevel, std::string diffcult);

	//获取当前等级怪数据公共部分
	map<std::string, DB_MonsterDataPublic*> getDBMonsterDataPublicCurLevel(int curLevel, std::string diffcult);

	string getResNameByID(int resIndex, int type);

	DB_Bullet*	getBulletByID(int ID);


	vector<DB_State *> getDBStateListByTableName(std::string tableName);

	//map<int, DB_MonsterProperty*> getDBMonsterPropertyByName(std::string tableName, std::string monsterName);

	DB_MonsterProperty* getDBMonsterPropertyByGTSindex(std::string tableName, int gts_index);
	
	XSetcombat* getCombat(int index);

	cocos2d::CCArray* getLoadingTip(int level);
	string getItemName(int type, int ID);
	cocos2d::CCArray* getAllDungeonMaps();
	cocos2d::CCArray* getAllFashions(char heroType);
	XFashion* getFashion(int ID);
	XFormula* getAdvancedItem(int ID);
	std::vector<DB_dungeon*> getDropItemMissionKey(int pItemID);
    //获取某一区域副本list
    std::vector<DB_dungeon*> getRegionDungeons(const std::string& regionKey);

	DB_Function* getFunctionByTeachName(const string& name);
	DB_Function* getFunctionByNeedFuncName(const string& name);
	DB_Function* getFunctionByName(const string& name);

	std::vector<DB_Teach*> getTeachList(const string& name);

	std::vector<DB_Gameover*> getGameoverList();
    std::vector<DB_dungeon*> getTargetBossDungeonList(const string& name,int type);
	std::vector<DB_dungeon*> getDungeonList(int type = -1);
	std::vector<DB_dungeon*> getGVEDungeonList();
	std::vector<DB_dungeon*> getLingShiBenDungeonList();
	std::vector<DB_dungeon*> getJinQianBenDungeonList();
	std::vector<DB_dungeon*> getJingYanBenDungeonList();
	std::vector<DB_dungeon*> getWuXianTaDungeonList(int chapter);
	DB_dungeon* getWuXianTaDungeon(int dungeonId);

	std::vector<DB_Huodong*> getHuodongList();
	std::vector<DPvpDoushentaiAccReward*> getPvpMatchAwardList();

	std::map<std::string , DB_Sound*> getSoundPath();

	//获得城镇
	DB_city* getCity(const string& name);

	DB_pvpaward*getPVPAward(int rank);
	std::vector<DB_pvpaward*> getPVPAwardList();

	//添加跨服战, add by zyp，2015-12-22
	std::vector<DB_KuafuzhanReward*> getKuaFuZhanAwardList();
	DB_KuafuzhanReward* getKuaFuZhanAward(int level);
	//end zyp

	std::vector<DPvpDoushentaiRankingReward*> getPVPMatchScoreAwardList();

	// 任务剧情
	std::vector<XQuestTip*> getQuestTipList(const string& name);

	// 推送
	DB_PushNotification* getPushInfo(int ID);
    DShenjiang* getShenJiang(int ID);
	DB_ShenJiangProperty* getShenJiangProperty(int ID);
    DB_ActorSkill* getShenJiangSkill(int ID);
    
    int getSJMaxExpLevelup(int level);
    XXiShuSetting* getXiShuSettingDB();

	int getXunBaoPrice(int type);
	int getXunBaoRenFreeTimes(int type);
	DB_Buff* getBuff(int buffID);
    DB_MonsterProperty* getDBMonsterPropertyByName(std::string monsterName);

    XTitle*  getTitle(int ID);
    cocos2d::CCArray* getAllTitles();
	XFormula* getExchangeItem(int ID);
	DB_dungeon* getWuXianTaNextDungeon(DB_dungeon* curDungen);

	//添加试炼之路副本, add by zyp，2015-9-21
	std::vector<DB_dungeon*> getShiLianZhiLuDungeonList();
	std::vector<DShilian*> getShiLianZhiLuDataListAll();
	std::vector<DShilian*> getShiLianZhiLuDataList(int category);
	//end zyp

private:
	cocos2d::CCDictionary* m_pDBCache;				// DB Cache KEY = int
	cocos2d::CCDictionary* m_pDBCacheState;			// DB Cache KEY = string
};
static bool cmpDungeonInfoByLv(const DB_dungeon* p1, const DB_dungeon* p2);
#endif // __DB_MANAGER_H__
