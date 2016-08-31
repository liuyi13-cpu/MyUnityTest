///-------------------------------------------------------------
// DBManager.cpp
// DB������
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
#include "base/DShilian.h"	//�������֮·����, add by zyp��2015-9-21
#include "base/DGodweapon.h"
#include "base/DB_KuafuzhanReward.h"	//��ӿ��ս, add by zyp��2015-12-22
enum kDBType
{
	kDBTypeNPC,					// NPC
	kDBTypeArm,					// װ��
	kDBTypeDrug,				// ҩƷ
	kDBTypeMaterial,			// ����
	kDBTypeTreasure,			// ����
	kDBTypeFormulaiton,			// ͼֽ
	kDBTypeGem,					// ��ʯ
	kDBTypeGua,					// ����
	kDBTypeGuaSetting,			// ��������
	kDBTypeLevelinfo,			// ���󿪿�
	kDBTypeTipMsg,				// ϵͳ��ʾ
	kDBTypeShop,				// �̵�
	kDBTypeShopGoods,			// �̵���Ʒ
	kDBTypeText,				// �ı�
	kDBTypeResourceConsume,		// ��Դ����
	kDBTypeZX_ZhuDongSkill,		// ��ϼ��������
	kDBTypeNMW_ZhuDongSkill,	// ţħ����������
	kDBTypeEnemy_ZhuDongSkill,	// ��ͨ������������
	kDBTypeZX_PassivitySkill,	// ��ϼ��������
	kDBTypeGuild,				// ����
	kDBTypeGuildSkill,			// ���ɼ���
	kDBTypeAllSkill,			// �������ƻ���
	kDBTypeFunction,			// �����¹���
	kDBTypeGameover,			// ��սʧ��
	kDBTypeHuodong,				// ��սʧ��
	kDBTypeVip,					// VIP�ȼ�����
	kDBTypeJingMai,				// ����
	kDBTypeJingMaiAdd,			// ������������
	kDBTypeDungeon,				// ����
	kDBTypeDungeonDrops,		// ��������
	kDBTypeQuest,				// ����
	kDBTypeHeroProperty,		// ��������
	kDBTypeShuiBoWen,			// ˮ��������
	kDBTypeMonsterResource,		// �б�����
	kDBTypeSceneActor,
	kDBTypeBullet,				// �ӵ�����
	kDBTypeState,				// ״̬ת����
	kDBTypeMonsterProp,			// �����Ա� 
	kDBTypeBossProp,			// Boss���Ա�
	kDBTypeParticle,			// ����ӳ���
	kDBTypeBianse,				// ���˱�ɫ���ñ�
	kDBTypeCombat,				// ս��ϵ��
	kDBTypeLoadingTip,			// loading��ʾ
	kDBTypeMonsterData,			// �����ݷ�����
	kDBTypeMonsterDataPublic,	// �����ݹ���
	kDBTypeFashions,			// ����ʱװ
	kDBTypeTeach,				// ��ѧ
	kDBTypeSoundMap,			// �����ز�
	kDBTypeCity,				// ����
	kDBTypePVPAward,			// pvp����
	kDBTypePVPMatchAward,		// pvp����̨����
	kDBTypePVPMatchScoreAward,  // pvp����̨���ֽ���
	kDBTypeChargeReward,		// ��ֵ����
	kDBTypeQuestTip,			// �������
	kDBTypePush,				// ������Ϣ
	kDBTypeShenJiangProperty,	// ������
    kDBTypeShenJiang,			// ��
    kDBTypeShenJiangItem,		// �񽫵���
    kDBTypeShenJiangArm,		// ��װ��
    kDBTypeSJ_ZhuDongSkill,		// ����������
	kDBTypeRaffle,				// ����
	kDBTypeBuff,				// ����buff��
	kDBTypeXXiShuSetting,
    kDBTypeXFormula,            // ����ͼֽ
    kDBTypeGuildActivities,     // ���ɻ
    kDBTypeGuildBoss,           // ���ɸ���
	kDBTypeTitle,               // �ƺ�
	KDBTypeShiLianZhiLu,		// ����֮· //�������֮·����, add by zyp��2015-9-21
	kDBTypeHunQi,               //����
	KDBTypeKuaFuZhanReward,		//���ս����	//��ӿ��ս, add by zyp��2015-12-22
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
	 * ���Npc
	 */
	DB_gamenpc* getNpc(string& name_id);
	DB_gamenpc* getNpc(int id);



	/**
	 * ��õ���
	 */
	CCObject* getItem(int type, int ID);
    std::string getItemDesc(int ID);
	/**
	 * ��õ���
	 */
	XTipMsg* getTipMsg(int ID);

	/**
	 * ����̵�
	 */
	XItemShop* getItemShop(int ID);

	/************************************************************************/
	/* ��ȡ�ı�                                                             */
	/************************************************************************/
	string getString(int ID);

	/**
	 * ��Դ����
	 */
	XResourceConsume* getResourceConsume(int ID);
	int getResourceConsumeSize();

	/************************************************************************/
	/* ���ݹ�������λ�û�ȡ�����б�                                                                     */
	/************************************************************************/

	vector<DB_Function*> getFunctionListByType(int type);

	DB_Function* getFunctionBySiteTag(int site,int tag);
	int getFunctionTagNumBySite(int site);
	/****
	****�����ܱ�
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
    /***�õ����ǵ����м���***/
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
	//���ѡ�˽�����������
	int				 getXuanRenCounter();
	//ͨ��ѡ�˽���˳������Ӧ��heroProperty
	DB_heroProperty* getHRByXuanRenIndex(int index);
    
	cocos2d::CCArray* getShopGoods(int goodID);


	DB_ShuiBoWen*	getShuiBoWenByID(int ID);
	int				getShuiBoWenCounter();

	DB_Particle*	getParticleByID(int ID);

	DB_Bianse*	getBianseByID(int ID);

	//DB_MonsterResource*	getMonsterResourceBySpriteAnim(int spriteIndex, int animIndex);
	//DB_MonsterResource*	getMonsterResourceByID(int ID);

	//��ȡ��ǰ�ȼ�ȫ�����͵ķ����͹�����
	map<std::string, DB_MonsterData *> getAllTypeDBMonsterDataCurLevel(int curLevel, std::string diffcult);

	//��ȡ��ǰ�ȼ������ݹ�������
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
    //��ȡĳһ���򸱱�list
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

	//��ó���
	DB_city* getCity(const string& name);

	DB_pvpaward*getPVPAward(int rank);
	std::vector<DB_pvpaward*> getPVPAwardList();

	//��ӿ��ս, add by zyp��2015-12-22
	std::vector<DB_KuafuzhanReward*> getKuaFuZhanAwardList();
	DB_KuafuzhanReward* getKuaFuZhanAward(int level);
	//end zyp

	std::vector<DPvpDoushentaiRankingReward*> getPVPMatchScoreAwardList();

	// �������
	std::vector<XQuestTip*> getQuestTipList(const string& name);

	// ����
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

	//�������֮·����, add by zyp��2015-9-21
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
