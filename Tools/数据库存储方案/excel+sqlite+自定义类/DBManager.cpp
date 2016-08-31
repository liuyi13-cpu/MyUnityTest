#include "DBManager.h"
#include "../Actor/Hero.h"
#include "../wrapper/wrapper.h"
#include "../Online/CMD/CmdMaker.h"
#include "../utils/Utils.h"
#include <algorithm>
#include "../GUI/home_scene/item_info/InfoUtils.h"


USING_NS_CC;

#define GET_DB_MAP(A, b) \
if (m_pDBCache->objectForKey(b) == NULL) \
{ \
	A::open(); \
	char str[100] = {0}; \
	sprintf(str,"select * from %s", A::dbTableName.c_str()); \
	A::ExcuteQuery(str); \
	m_pDBCache->setObject(A::getQueryResultDictionary(), b); \
	A::close(); \
}

#define GET_DB_ARRAY(A, b) \
if (m_pDBCache->objectForKey(b) == NULL) \
{ \
	A::open(); \
	char str[100] = {0}; \
	sprintf(str,"select * from %s", A::dbTableName.c_str()); \
	A::ExcuteQuery(str); \
	m_pDBCache->setObject(A::getQueryResultArray(), b); \
	A::close(); \
}

#define GET_DB_ARRAY_TABLENAME(A, b, tableName) \
	if (m_pDBCacheState->objectForKey(b) == NULL) \
{ \
	A::open(); \
	char str[100] = {0}; \
	sprintf(str,"select * from %s", tableName); \
	A::ExcuteQuery(str); \
	m_pDBCacheState->setObject(A::getQueryResultArray(), b); \
	A::close(); \
}

static const int PUBLIC_ERR_ID = 9;
static DBManager* s_pDBManager = NULL;

DBManager::DBManager()
	: m_pDBCache(NULL)
{
	m_pDBCache = CCDictionary::create();
	m_pDBCache->retain();

	m_pDBCacheState = CCDictionary::create();
	m_pDBCacheState->retain();
	
}

DBManager::~DBManager()
{
	purgeCachedData();
	CC_SAFE_RELEASE_NULL(m_pDBCache);
	CC_SAFE_RELEASE_NULL(m_pDBCacheState);
}

DBManager* DBManager::sharedDBManager()
{
	
	if (s_pDBManager == NULL)
	{
		s_pDBManager = new DBManager();
	}
	return s_pDBManager;
}

void DBManager::purge(void)
{
	CC_SAFE_RELEASE_NULL(s_pDBManager);
}

void DBManager::purgeCachedData()
{
	m_pDBCache->removeAllObjects();
	m_pDBCacheState->removeAllObjects();
}

XXiShuSetting* DBManager::getXiShuSettingDB()
{
	GET_DB_ARRAY(XXiShuSetting, kDBTypeXXiShuSetting);
	CCArray* list = (CCArray*)m_pDBCache->objectForKey(kDBTypeXXiShuSetting);
	if (list->count() > 0)
	{
		XXiShuSetting* m_xishuSetting = (XXiShuSetting*)(list->objectAtIndex(0));
		return m_xishuSetting;
	}
	return NULL;
}

/**
 * 获得Npc
 */
DB_gamenpc* DBManager::getNpc(string& name_id)
{
	GET_DB_MAP(DB_gamenpc, kDBTypeNPC);

	CCDictionary* dic = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeNPC);
	CCDictElement* pElement = NULL;
	CCDICT_FOREACH(dic, pElement)
	{
		DB_gamenpc* frameDict = dynamic_cast<DB_gamenpc*>(pElement->getObject());
		if (name_id.compare(frameDict->name_id) == 0)
		{
			return frameDict;
		}
	}
	return NULL;
}

/**
 * 获得Npc
 */
DB_gamenpc* DBManager::getNpc(int id)
{
	GET_DB_MAP(DB_gamenpc, kDBTypeNPC);

	CCDictionary* dic = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeNPC);
	CCDictElement* pElement = NULL;
	CCDICT_FOREACH(dic, pElement)
	{
		DB_gamenpc* frameDict = dynamic_cast<DB_gamenpc*>(pElement->getObject());
		if ((frameDict->ID) == id)
		{
			return frameDict;
		}
	}
	return NULL;
}


std::vector<DB_dungeon*> DBManager::getDropItemMissionKey(int pItemID)
{
	//技能表
	//DB_dungeon_drop::open(); 
	//char str[500] = {0}; 
 //   sprintf(str, "select * from %s where item_id1=%d or item_id2=%d or item_id3=%d or item_id4=%d or item_id5=%d or special_id=%d", DB_dungeon_drop::dbTableName.c_str(), pItemID, pItemID, pItemID, pItemID, pItemID, pItemID);
	//DB_dungeon_drop::ExcuteQuery(str); 
	

    vector<DB_dungeon_drop*> resultList;
    GET_DB_ARRAY(DB_dungeon_drop, kDBTypeDungeonDrops);
    CCArray* list = (CCArray*)m_pDBCache->objectForKey(kDBTypeDungeonDrops);
    CCObject* obj = NULL;
    CCARRAY_FOREACH(list, obj)
    {
        DB_dungeon_drop* drop = dynamic_cast<DB_dungeon_drop*>(obj);
        bool isTarget = false;
        if (drop->item_id1 == pItemID || drop->item_id2 == pItemID || drop->item_id3 == pItemID || drop->item_id4 == pItemID || drop->item_id5 == pItemID || drop->special_id == pItemID)
            isTarget = true;
        if (isTarget)
            resultList.push_back(drop);
    }


    vector<DB_dungeon*> dungeon;
	int maxDrop = -1;
	vector<DB_dungeon*>::iterator iter;
    DB_dungeon_drop* item = NULL;
    if (resultList.size()>0)
	{
        for (int i = 0; i<resultList.size(); i++)
		{
            item = resultList.at(i);
			DB_dungeon* temp = getDungeon(item->skey);
			if(temp)
				dungeon.push_back(temp);
		}
	}

	int miniLevel = INT_MAX;
	DB_dungeon* miniLevelItem = NULL;
	for(int i =0;i<dungeon.size();i++)
	{
		DB_dungeon* temp = dungeon.at(i);
		if(temp->monster_level<miniLevel)
		{
			miniLevel = temp->monster_level;
			miniLevelItem = temp;
		}
	}
	for (iter = dungeon.begin(); iter != dungeon.end(); iter++)
	{
		if((*iter)->ID == miniLevelItem->ID)
		{
			dungeon.erase(iter);
			break;
		}
	}

	DB_dungeon* maxDropitem = NULL;
	if(resultList.size()>0)
	{
		for(int i =0;i<resultList.size();i++)
		{
			item = resultList.at(i);
			DB_dungeon* temp = NULL;
			for(int i =0;i<dungeon.size();i++)
			{
				DB_dungeon* tmp = dungeon.at(i);
				if(item->skey.compare(tmp->skey) == 0)
				{
					temp = tmp;
					break;
				}
			}
			if(!temp) continue;
			if(pItemID== item->item_id1)
			{
				if(item->drop_num1>maxDrop)
				{
					maxDrop = item->drop_num1;
					maxDropitem = temp;
				}
			}
			if(pItemID== item->item_id2)
			{
				if(item->drop_num2>maxDrop)
				{
					maxDrop = item->drop_num2;
					maxDropitem = temp;
				}
			}
			if(pItemID== item->item_id3)
			{
				if(item->drop_num3>maxDrop)
				{
					maxDrop = item->drop_num3;
					maxDropitem = temp;
				}
			}
			if(pItemID== item->item_id4)
			{
				if(item->drop_num4>maxDrop)
				{
					maxDrop = item->drop_num4;
					maxDropitem = temp;
				}
			}
			if(pItemID== item->item_id5)
			{
				if(item->drop_num5>maxDrop)
				{
					maxDrop = item->drop_num5;
					maxDropitem = temp;
				}
			}
            if (pItemID == item->special_id)
            {
                if (item->special_num > maxDrop)
                {
                    maxDrop = item->special_num;
                    maxDropitem = temp;
                }
            }
		}
	}
	//CCLOG(str);
	for (iter = dungeon.begin(); iter != dungeon.end(); iter++)
	{
		if(maxDropitem && (*iter)->ID == maxDropitem->ID)
		{
			dungeon.erase(iter);
			break;
		}
	}

	int maxExp = 0;
	DB_dungeon* maxExpItem = NULL;
	for(int i =0;i<dungeon.size();i++)
	{
		DB_dungeon* temp = dungeon.at(i);
		if(temp->exp>maxExp)
		{
			maxExp = temp->exp;
			maxExpItem = temp;
		}
	}
	for (iter = dungeon.begin(); iter != dungeon.end(); iter++)
	{
		if(maxExpItem && (*iter)->ID == maxExpItem->ID)
		{
			dungeon.erase(iter);
			break;
		}
	}
	vector<DB_dungeon*> result;
	if(miniLevelItem)
		result.push_back(miniLevelItem);
	if(maxDropitem)
	{
		result.push_back(maxDropitem);
	}
	if(maxExpItem)
	{
		result.push_back(maxExpItem);
	}
	if(result.size()<3)
	{
		for(int i = result.size();i<3;i++)
		{
			result.push_back(NULL);
		}
	}
	return result;
}

XFormula* DBManager::getAdvancedItem(int ID)
{
	//技能表
	/*XFormula::open(); 
	char str[50] = {0}; 
	sprintf(str,"select * from %s where Equipment=%d", XFormula::dbTableName.c_str(),ID); 
	XFormula::ExcuteQuery(str); 
	XFormula* item = NULL;
	if(XFormula::getQueryResult().size()>0)
	{
		item = XFormula::getQueryResult().at(0);
	}
	XFormula::close();*/
    GET_DB_ARRAY(XFormula, kDBTypeXFormula);
    CCArray* list = (CCArray*)m_pDBCache->objectForKey(kDBTypeXFormula);
    CCObject* obj = NULL;
    CCARRAY_FOREACH(list, obj)
    {
        XFormula* item = dynamic_cast<XFormula*>(obj);
        if (item)
        {
            if (item->Equipment == ID)
                return item;
        }
    }
	return NULL;
}

XFormula* DBManager::getExchangeItem(int ID)
{
	//技能表
	/*XFormula::open(); 
	char str[50] = {0}; 
	sprintf(str,"select * from %s where Equipment=%d", XFormula::dbTableName.c_str(),ID); 
	XFormula::ExcuteQuery(str); 
	XFormula* item = NULL;
	if(XFormula::getQueryResult().size()>0)
	{
		item = XFormula::getQueryResult().at(0);
	}
	XFormula::close();*/
    GET_DB_ARRAY(XFormula, kDBTypeXFormula);
    CCArray* list = (CCArray*)m_pDBCache->objectForKey(kDBTypeXFormula);
    CCObject* obj = NULL;
    CCARRAY_FOREACH(list, obj)
    {
        XFormula* item = dynamic_cast<XFormula*>(obj);
        if (item)
        {
            if (item->ID == ID)
                return item;
        }
    }
	return NULL;
}

/**
 * 获得道具
 */
CCObject* DBManager::getItem(int type, int ID)
{
	kDBType DBType;

	switch (type)
	{
	case CMD_ShowPlayerPack_down::CMD_ShowPlayerPack_ITEM_ARM:
		{
			DBType = kDBTypeArm;
			GET_DB_MAP(XArmInfo, DBType);
		}
		break;

	case CMD_ShowPlayerPack_down::CMD_ShowPlayerPack_ITEM_DRUG:
		{
			DBType = kDBTypeDrug;
			GET_DB_MAP(XDrugInfo, DBType);
		}
		break;

	case CMD_ShowPlayerPack_down::CMD_ShowPlayerPack_ITEM_MATERIAL:
		{
			DBType = kDBTypeMaterial;
			GET_DB_MAP(XMaterial, DBType);
		}
		break;

	case CMD_ShowPlayerPack_down::CMD_ShowPlayerPack_ITEM_TREASURE:
		{
			DBType = kDBTypeTreasure;
			GET_DB_MAP(XTreasure, DBType);
		}
		break;

	case CMD_ShowPlayerPack_down::CMD_ShowPlayerPack_ITEM_FORMULATION:
		{
			DBType = kDBTypeFormulaiton;
			GET_DB_MAP(XFormulation, DBType);
		}
		break;

	case CMD_ShowPlayerPack_down::CMD_ShowPlayerPack_ITEM_GEM:
		{
			DBType = kDBTypeGem;
			GET_DB_MAP(XGem, DBType);
		}
		break;

	case CMD_ShowPlayerPack_down::CMD_ShowPlayerPack_ITEM_GUA:
		{
			DBType = kDBTypeGua;
			GET_DB_MAP(XGuaInfo, DBType);
		}
		break;
	case CMD_ShowPlayerPack_down::CMD_ShowPlayerPack_ITEM_FASHION:
		{
			DBType = kDBTypeFashions;
			GET_DB_MAP(XFashion, DBType);
		}
		break;
    case CMD_ShowPlayerPack_down::CMD_ShowPlayerPack_ITEM_SJITEM:
    {
        DBType = kDBTypeShenJiangItem;
        GET_DB_MAP(DItemShenjiang, DBType);
    }
        break;
    case CMD_ShowPlayerPack_down::CMD_ShowPlayerPack_ITEM_SHENJIANG:
    {
        DBType = kDBTypeShenJiang;
        GET_DB_MAP(DShenjiang, DBType);
    }
        break;
	case CMD_ShowPlayerPack_down::CMD_ShowPlayerPack_ITEM_TITLE:
	{
		DBType = kDBTypeTitle;
		GET_DB_MAP(XTitle,DBType);
	}
		break;
	case CMD_ShowPlayerPack_down::CMD_ShowPlayerPack_ITEM_HUNQI:
	{
		DBType = kDBTypeHunQi;
		GET_DB_MAP(DGodweapon, DBType);
	}
		break;
	default:
		CCAssert(false, "DBType is wrong");
		break;
	}

	CCDictionary* pDict = (CCDictionary*)m_pDBCache->objectForKey(DBType);
	//for(int i =0;i<m_pDBCache->allKeys()->count();i++)
	//{
	//	CCInteger* key = (CCInteger*)( m_pDBCache->allKeys()->objectAtIndex(i));
	//	if(key)
	//		CCLOG(Utils::itos(key->getValue()).c_str());
	//}
	if (pDict)
	{
		return pDict->objectForKey(ID);
	}
	return NULL;
}

/**
 * 获得道具
 */
string DBManager::getItemDesc(int ID)
{
    int type = InfoUtils::getItemType(ID);
	CCObject* pItem = getItem(type,ID);

	if(!pItem)
		return "";
	switch (type)
	{
	case CMD_ShowPlayerPack_down::CMD_ShowPlayerPack_ITEM_ARM:
		{
			return ((XArmInfo*)pItem)->Description;
		}
		break;

	case CMD_ShowPlayerPack_down::CMD_ShowPlayerPack_ITEM_DRUG:
		{
			return ((XDrugInfo*)pItem)->Description;
		}
		break;

	case CMD_ShowPlayerPack_down::CMD_ShowPlayerPack_ITEM_MATERIAL:
		{
			return ((XMaterial*)pItem)->Description;
		}
		break;

	case CMD_ShowPlayerPack_down::CMD_ShowPlayerPack_ITEM_TREASURE:
		{
			return ((XTreasure*)pItem)->Description;
		}
		break;

	case CMD_ShowPlayerPack_down::CMD_ShowPlayerPack_ITEM_FORMULATION:
		{
			return ((XFormulation*)pItem)->Description;
		}
		break;

	case CMD_ShowPlayerPack_down::CMD_ShowPlayerPack_ITEM_GEM:
		{
			return ((XGem*)pItem)->Description;
		}
		break;

	case CMD_ShowPlayerPack_down::CMD_ShowPlayerPack_ITEM_GUA:
		{
			return ((XGuaInfo*)pItem)->Description;
		}
		break;

	case CMD_ShowPlayerPack_down::CMD_ShowPlayerPack_ITEM_FASHION:
		{
			return ((XFashion*)pItem)->Description;
		}
		break;
    case CMD_ShowPlayerPack_down::CMD_ShowPlayerPack_ITEM_SJITEM:
    {
        return ((DItemShenjiang*)pItem)->Description;
    }
        break;
    case CMD_ShowPlayerPack_down::CMD_ShowPlayerPack_ITEM_SHENJIANG:
    {
        return ((DShenjiang*)pItem)->Description;
    }
        break;
	case CMD_ShowPlayerPack_down::CMD_ShowPlayerPack_ITEM_TITLE:
		{
			return ((XTitle*)pItem)->Description;
		}
	default:
		CCAssert(false, "DBType is wrong");
		break;
	}

	return "";
}

/**
 * 获得道具
 */
string DBManager::getItemName(int type, int ID)
{
	CCObject* pItem = getItem(type,ID);

	if(!pItem)
		return "";
	switch (type)
	{
	case CMD_ShowPlayerPack_down::CMD_ShowPlayerPack_ITEM_ARM:
		{
			return ((XArmInfo*)pItem)->sname;
		}
		break;

	case CMD_ShowPlayerPack_down::CMD_ShowPlayerPack_ITEM_DRUG:
		{
			return ((XDrugInfo*)pItem)->sname;
		}
		break;

	case CMD_ShowPlayerPack_down::CMD_ShowPlayerPack_ITEM_MATERIAL:
		{
			return ((XMaterial*)pItem)->sname;
		}
		break;

	case CMD_ShowPlayerPack_down::CMD_ShowPlayerPack_ITEM_TREASURE:
		{
			return ((XTreasure*)pItem)->sname;
		}
		break;

	case CMD_ShowPlayerPack_down::CMD_ShowPlayerPack_ITEM_FORMULATION:
		{
			return ((XFormulation*)pItem)->sname;
		}
		break;

	case CMD_ShowPlayerPack_down::CMD_ShowPlayerPack_ITEM_GEM:
		{
			return ((XGem*)pItem)->sname;
		}
		break;

	case CMD_ShowPlayerPack_down::CMD_ShowPlayerPack_ITEM_GUA:
		{
			return ((XGuaInfo*)pItem)->sname;
		}
		break;

	case CMD_ShowPlayerPack_down::CMD_ShowPlayerPack_ITEM_FASHION:
		{
			return ((XFashion*)pItem)->sname;
		}
		break;
    case CMD_ShowPlayerPack_down::CMD_ShowPlayerPack_ITEM_SJITEM:
    {
        return ((DItemShenjiang*)pItem)->sname;
    }
        break;
    case CMD_ShowPlayerPack_down::CMD_ShowPlayerPack_ITEM_SHENJIANG:
    {
        return ((DShenjiang*)pItem)->sname;
    }
        break;
	case CMD_ShowPlayerPack_down::CMD_ShowPlayerPack_ITEM_TITLE:
		{
			return ((XTitle*)pItem)->sname;
		}
	default:
		CCAssert(false, "DBType is wrong");
		break;
	}

	return "";
}

/**
 * 获得道具
 */
XTipMsg* DBManager::getTipMsg(int ID)
{
	GET_DB_MAP(XTipMsg, kDBTypeTipMsg);

	CCDictionary* dic = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeTipMsg);
	CCObject* pModel = dic->objectForKey(ID);
	if (pModel == NULL)
	{
		pModel = dic->objectForKey(PUBLIC_ERR_ID);
	}
	return dynamic_cast<XTipMsg*>(pModel);
}


/**
* 获得神将
*/
DShenjiang* DBManager::getShenJiang(int ID)
{
    GET_DB_MAP(DShenjiang, kDBTypeShenJiang);
    CCDictionary* dic = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeShenJiang);
    return dynamic_cast<DShenjiang*>(dic->objectForKey(ID));
}
/**
 * 获得商店
 */
XItemShop* DBManager::getItemShop(int ID)
{
	GET_DB_MAP(XItemShop, kDBTypeShop);
	CCDictionary* dic = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeShop);
	return dynamic_cast<XItemShop*>(dic->objectForKey(ID));
}


string DBManager::getString(int ID)
{
	GET_DB_MAP(DB_Text, kDBTypeText);
	CCDictionary* dic = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeText);
	DB_Text* db = (dynamic_cast<DB_Text*>(dic->objectForKey(ID)));
	string talkText = "";
	if (m_hero && m_hero->getPlayerInfo())
	{
		switch (m_hero->getPlayerInfo()->Profession)
		{
		case NIU_MO:
			talkText = db->text_niumo;
			break;

		case ZI_XIA:
			talkText = db->text_zixia;
			break;

		case LING_HOU:
			talkText = db->text_linghou;
			break;

		default:
			break;
		}
	}

	if (talkText.empty())
	{
		talkText = db->text_niumo;
	}

	return talkText;
}

/**
 * 强化等级
 */
XResourceConsume* DBManager::getResourceConsume(int ID)
{
	GET_DB_MAP(XResourceConsume, kDBTypeResourceConsume);
	CCDictionary* dic = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeResourceConsume);
	return dynamic_cast<XResourceConsume*>(dic->objectForKey(ID));
}

int DBManager::getResourceConsumeSize()
{
	GET_DB_MAP(XResourceConsume, kDBTypeResourceConsume);
	CCDictionary* dic = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeResourceConsume);
	return dic->count();
}





DB_ActorSk_Pv* DBManager::getPassivitySkill(int ID)
{
	GET_DB_MAP(DB_ActorSk_Pv, kDBTypeZX_PassivitySkill);
	CCDictionary* dic = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeZX_PassivitySkill);
	return dynamic_cast<DB_ActorSk_Pv*>(dic->objectForKey(ID));
}


DB_ActorSk_Pv* DBManager::getPassivityBySkillID(std::string skId)
{
	GET_DB_MAP(DB_ActorSk_Pv, kDBTypeZX_PassivitySkill);
	CCDictionary* dic = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeZX_PassivitySkill);
	
	DB_ActorSk_Pv* curSkill = NULL;
	CCDictElement* pElement = NULL;

	CCDICT_FOREACH(dic,pElement)
	{
		DB_ActorSk_Pv* frameDict = dynamic_cast<DB_ActorSk_Pv*>(pElement->getObject());
		if(frameDict && frameDict->skillId == skId)
		{
			curSkill = frameDict;
		}
	}

	return curSkill;
}

vector<DB_ActorSk_Pv*> DBManager::getGuildSkill()
{
	GET_DB_MAP(DB_ActorSk_Pv, kDBTypeZX_PassivitySkill);
	CCDictionary* dic = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeZX_PassivitySkill);

	CCDictElement* pElement = NULL;
	vector<DB_ActorSk_Pv*> vec;

	CCDICT_FOREACH(dic,pElement)
	{
		DB_ActorSk_Pv* frameDict = dynamic_cast<DB_ActorSk_Pv*>(pElement->getObject());
		if(frameDict && frameDict->attribution == 3)
		{
			vec.push_back(frameDict);
		}
	}

	return vec;
}

/************************************************************************/
/* 根据技能ID获得技能总表信息                                                                     */
/************************************************************************/
DSkill* DBManager::getSKillAllBySkillId(std::string skId)
{
	GET_DB_MAP(DSkill, kDBTypeAllSkill);
	CCDictionary* dic = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeAllSkill);

	DSkill* curSkill = NULL;
	CCDictElement* pElement = NULL;

	CCDICT_FOREACH(dic,pElement)
	{
		DSkill* frameDict = dynamic_cast<DSkill*>(pElement->getObject());
		if(frameDict && frameDict->skillId == skId)
		{
			curSkill = frameDict;
		}
	}

	return curSkill;
}

//

DB_ActorSkill* DBManager::getInSkillById(const string& skId)
{
	if (m_pDBCache->objectForKey(kDBTypeZX_ZhuDongSkill) == NULL)
	{
		//技能表
		DB_ActorSkill::open(); 
		char str[30] = {0}; 
		sprintf(str,"select * from %s", DB_ActorSkill::dbTableName.c_str()); 
		DB_ActorSkill::ExcuteQuery(str); 
		m_pDBCache->setObject(DB_ActorSkill::getQueryResultDictionary(), kDBTypeZX_ZhuDongSkill);

		sprintf(str,"select * from %s", DB_ActorSkill::dbTableName_1.c_str()); 
		DB_ActorSkill::ExcuteQuery(str); 
		m_pDBCache->setObject(DB_ActorSkill::getQueryResultDictionary(), kDBTypeEnemy_ZhuDongSkill);

		DB_ActorSkill::close(); 
	}

	CCDictionary* dic = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeZX_ZhuDongSkill);
	DB_ActorSkill* curSkill = NULL;
	CCDictElement* pElement = NULL;
	CCDICT_FOREACH(dic,pElement)
	{
		DB_ActorSkill* frameDict = dynamic_cast<DB_ActorSkill*>(pElement->getObject());
		if(frameDict && frameDict->icon.length()> 1 &&  frameDict->onlyID.compare(skId) ==0)
		{
			curSkill = frameDict;
		}
	}

	return curSkill;
}

int DBManager::getPassivitySkillCount(int type)
{
	GET_DB_MAP(DB_ActorSk_Pv, kDBTypeZX_PassivitySkill);
	CCDictionary* dic = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeZX_PassivitySkill);
	return dic->count();
}


std::vector<DB_Gameover*> DBManager::getGameoverList()
{
	GET_DB_MAP(DB_Gameover, kDBTypeGameover);

	CCDictionary* dic = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeGameover);

	vector<DB_Gameover*> allGameover;
	CCDictElement* pElement = NULL;

	CCDICT_FOREACH(dic, pElement)
	{
		DB_Gameover* frameDict = dynamic_cast<DB_Gameover*>(pElement->getObject());

		if(frameDict)
		{
			bool isOpen = Utils::getFunctionUserDefault(frameDict->name);
			if(isOpen)
				allGameover.push_back(frameDict);
		}

	}


	return allGameover;
}

std::vector<DB_Huodong*> DBManager::getHuodongList()
{
	GET_DB_MAP(DB_Huodong, kDBTypeHuodong);

	CCDictionary* dic = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeHuodong);

	vector<DB_Huodong*> allHuodong;
	CCDictElement* pElement = NULL;

	CCDICT_FOREACH(dic, pElement)
	{
		DB_Huodong* frameDict = dynamic_cast<DB_Huodong*>(pElement->getObject());

		if(frameDict)
		{
			allHuodong.push_back(frameDict);
		}

	}

	return allHuodong;
}


std::vector<DPvpDoushentaiAccReward*> DBManager::getPvpMatchAwardList()
{
	GET_DB_MAP(DPvpDoushentaiAccReward, kDBTypePVPMatchAward);

	CCDictionary* dic = (CCDictionary*)m_pDBCache->objectForKey(kDBTypePVPMatchAward);

	vector<DPvpDoushentaiAccReward*> allAwards;
	CCDictElement* pElement = NULL;

	CCDICT_FOREACH(dic, pElement)
	{
		DPvpDoushentaiAccReward* frameDict = dynamic_cast<DPvpDoushentaiAccReward*>(pElement->getObject());

		if(frameDict)
		{
			if(m_hero && m_hero->getPlayerEquip())
			{
				int heroLevel = m_hero->getPlayerInfo()->Level;

				if(heroLevel >= frameDict->min_level 
					&& heroLevel <= frameDict->max_level )
					allAwards.push_back(frameDict);
			}
			
		}

	}

	return allAwards;
}


/************************************************************************/
/* 根据所在的位置获取功能                                                                     */
/************************************************************************/
vector<DB_Function*> DBManager::getFunctionListByType(int type)
{
	GET_DB_MAP(DB_Function, kDBTypeFunction);

	CCDictionary* dic = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeFunction);

	vector<DB_Function*> allFunction;
	CCDictElement* pElement = NULL;

	CCDICT_FOREACH(dic, pElement)
	{
		DB_Function* frameDict = dynamic_cast<DB_Function*>(pElement->getObject());

		if(frameDict && frameDict->site > 0)
		{
			switch(type)
			{
			case FUNCTION_TOP:
				if( frameDict->site == FUNCTION_TOP)
				{
					allFunction.push_back(frameDict);
				}
				break;
			case FUNCTION_BUTTOM_TOP:
				if( frameDict->site == FUNCTION_BUTTOM_TOP)
				{
					allFunction.push_back(frameDict);
				}
				break;
			case FUNCTION_BUTTOM_LEFT:
				if( frameDict->site == FUNCTION_BUTTOM_LEFT)
				{
					allFunction.push_back(frameDict);
				}
				break;
			default:
				break;
			}
			
		}

	}


	return allFunction;
}

DB_Function* DBManager::getFunctionByName(const string& name)
{
	GET_DB_MAP(DB_Function, kDBTypeFunction);
	CCDictionary* dic = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeFunction);
	CCDictElement* pElement = NULL;
	CCDICT_FOREACH(dic, pElement)
	{
		DB_Function* frameDict = dynamic_cast<DB_Function*>(pElement->getObject());
		if (frameDict->name.compare(name) == 0)
		{
			return frameDict;
		}
	}
	return NULL;
}

DB_Function* DBManager::getFunctionByTeachName(const string& name)
{
	GET_DB_MAP(DB_Function, kDBTypeFunction);
	CCDictionary* dic = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeFunction);
	CCDictElement* pElement = NULL;
	CCDICT_FOREACH(dic, pElement)
	{
		DB_Function* frameDict = dynamic_cast<DB_Function*>(pElement->getObject());
		if (frameDict->function_name.compare(name) == 0)
		{
			return frameDict;
		}
	}
	return NULL;
}


DB_Function* DBManager::getFunctionByNeedFuncName(const string& name)
{
	GET_DB_MAP(DB_Function, kDBTypeFunction);
	CCDictionary* dic = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeFunction);
	CCDictElement* pElement = NULL;
	CCDICT_FOREACH(dic, pElement)
	{
		DB_Function* frameDict = dynamic_cast<DB_Function*>(pElement->getObject());
		if (frameDict->need_func.compare(name) == 0)
		{
			return frameDict;
		}
	}
	return NULL;
}

DB_Function* DBManager::getFunctionBySiteTag(int site,int tag)
{
	GET_DB_MAP(DB_Function, kDBTypeFunction);
	CCDictionary* dic = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeFunction);

	DB_Function* curFunction = NULL;
	CCDictElement* pElement = NULL;

	CCDICT_FOREACH(dic,pElement)
	{
		DB_Function* frameDict = dynamic_cast<DB_Function*>(pElement->getObject());
		if(frameDict && frameDict->site == site && frameDict->tag == tag)
		{
			curFunction = frameDict;
		}
	}

	return curFunction;
}

int DBManager::getFunctionTagNumBySite(int site)
{
	int num = 1;
	GET_DB_MAP(DB_Function, kDBTypeFunction);
	CCDictionary* dic = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeFunction);

	DB_Function* curFunction = NULL;
	CCDictElement* pElement = NULL;

	CCDICT_FOREACH(dic,pElement)
	{
		DB_Function* frameDict = dynamic_cast<DB_Function*>(pElement->getObject());
		if(frameDict && frameDict->site == site)
		{
			num++;
		}
	}

	return num;
}

std::map<std::string , DB_Sound*> DBManager::getSoundPath()
{
	GET_DB_MAP(DB_Sound, kDBTypeSoundMap);
	CCDictionary* dic = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeSoundMap);

	CCDictElement* pElement = NULL;
	std::map<std::string, DB_Sound*> soundMap;
	CCDICT_FOREACH(dic, pElement)
	{
		DB_Sound* frameDict = dynamic_cast<DB_Sound*>(pElement->getObject());
		soundMap.insert(pair<std::string, DB_Sound*>(frameDict->sound_key, frameDict));
	}
	return soundMap;
}


/***得到主角的所有被动技能  在加载时调用***/
vector<netSkillInfo*> DBManager::getAllPassivitySkill(int type)
{
	GET_DB_MAP(DB_ActorSk_Pv, kDBTypeZX_PassivitySkill);
	CCDictionary* dic = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeZX_PassivitySkill);

	vector<netSkillInfo*> allSkill;
	CCDictElement* pElement = NULL;

	CCDICT_FOREACH(dic, pElement)
	{
		DB_ActorSk_Pv* frameDict = dynamic_cast<DB_ActorSk_Pv*>(pElement->getObject());

		if(frameDict)
		{
			if ((m_hero->isMagicType() 
					&& string(frameDict->skillId) != string("zx_b_wugong_1") 
					&& string(frameDict->skillId) != string("zx_b_gaojiwugong_1") 
					&& frameDict->attribution == 1)
				|| ((!m_hero->isMagicType())
					&& string(frameDict->skillId) != string("zx_b_fagong_1") 
					&& string(frameDict->skillId) != string("zx_b_gaojifagong_1") 
					&& frameDict->attribution == 1)
				)
			{
				netSkillInfo* nsi = new netSkillInfo;
				nsi->autorelease();
				nsi->skillID = frameDict->skillId;
				nsi->skillLevel = -1;
				allSkill.push_back(nsi);
			}
		}
	}
	return allSkill;
}

/***得到主角的所有被动技能  在加载时调用***/
vector<netSkillInfo*> DBManager::getAllBangpaiSkill(int type)
{
	GET_DB_MAP(DB_ActorSk_Pv, kDBTypeZX_PassivitySkill);
	CCDictionary* dic = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeZX_PassivitySkill);

	vector<netSkillInfo*> allSkill;
	CCDictElement* pElement = NULL;

	CCDICT_FOREACH(dic, pElement)
	{
		DB_ActorSk_Pv* frameDict = dynamic_cast<DB_ActorSk_Pv*>(pElement->getObject());

		if(frameDict)
		{
			if ( frameDict->attribution == 3)
			{
				netSkillInfo* nsi = new netSkillInfo;
				nsi->autorelease();
				nsi->skillID = frameDict->skillId;
				nsi->skillLevel = -1;
				allSkill.push_back(nsi);

			}
		}

	}
	return allSkill;
}


DB_ActorSkill* DBManager::getShenJiangSkill(int ID)
{
    if (m_pDBCache->objectForKey(kDBTypeSJ_ZhuDongSkill) == NULL)
    {
        //技能表
        DB_ActorSkill::open();
        char str[100] = { 0 };
        sprintf(str, "select * from %s", DB_ActorSkill::dbTableName_2.c_str());
        DB_ActorSkill::ExcuteQuery(str);
        m_pDBCache->setObject(DB_ActorSkill::getQueryResultDictionary(), kDBTypeSJ_ZhuDongSkill);
        DB_ActorSkill::close();
    }

    GET_DB_MAP(DB_ActorSkill, kDBTypeSJ_ZhuDongSkill);
    CCDictionary* dic = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeSJ_ZhuDongSkill);
    
    return dynamic_cast<DB_ActorSkill*>(dic->objectForKey(ID));

}

DB_ActorSkill* DBManager::getInitiativeSkill(int ID)
{
	if (m_pDBCache->objectForKey(kDBTypeZX_ZhuDongSkill) == NULL)
	{
		//技能表
		DB_ActorSkill::open(); 
		char str[30] = {0}; 
		sprintf(str,"select * from %s", DB_ActorSkill::dbTableName.c_str()); 
		DB_ActorSkill::ExcuteQuery(str); 
		m_pDBCache->setObject(DB_ActorSkill::getQueryResultDictionary(), kDBTypeZX_ZhuDongSkill);

		sprintf(str,"select * from %s", DB_ActorSkill::dbTableName_1.c_str()); 
		DB_ActorSkill::ExcuteQuery(str); 
		m_pDBCache->setObject(DB_ActorSkill::getQueryResultDictionary(), kDBTypeEnemy_ZhuDongSkill);

		DB_ActorSkill::close(); 
	}

	CCDictionary* dic = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeZX_ZhuDongSkill);
	return dynamic_cast<DB_ActorSkill*>(dic->objectForKey(ID));
}

vector<netSkillInfo*> DBManager::getAllInitiaSkill(int type)
{
	GET_DB_MAP(DB_ActorSkill, kDBTypeZX_ZhuDongSkill);
	vector<netSkillInfo*> allSkill;
	CCDictElement* pElement = NULL;
	CCDictionary* skill = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeZX_ZhuDongSkill);

	CCDICT_FOREACH(skill, pElement)
	{
		DB_ActorSkill* frameDict = dynamic_cast<DB_ActorSkill*>(pElement->getObject());
		if(frameDict && frameDict->icon.length()> 1)
		{
			netSkillInfo* nsi = new netSkillInfo;
			nsi->autorelease();
			nsi->skillID = frameDict->onlyID;
			nsi->skillLevel = -1;

			allSkill.push_back(nsi);
		}
	}
	return allSkill;
}

struct Cmp_by_require_lv   
{
	bool operator()(const DB_ActorSkill *a, const DB_ActorSkill *b)
	{
		return a->required_level < b->required_level;
	}
};

vector<DB_ActorSkill*> DBManager::getAllInitiativeSkill(int type)
{
	vector<DB_ActorSkill*> allSkill;
	CCDictElement* pElement = NULL;

	GET_DB_MAP(DB_ActorSkill, kDBTypeZX_ZhuDongSkill);
	CCDictionary* skill = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeZX_ZhuDongSkill);

	CCDICT_FOREACH(skill, pElement)
	{
		DB_ActorSkill* frameDict = dynamic_cast<DB_ActorSkill*>(pElement->getObject());
		if(frameDict 
			&& (!frameDict->icon.empty())
			&& frameDict->role == m_hero->hrProp->ID)
		{
			allSkill.push_back(frameDict);
		}
	}

	std::stable_sort(allSkill.begin(),allSkill.end(),Cmp_by_require_lv());
	return allSkill;
}

DB_guildStage* DBManager::getGuildBossByType(int type,int level)
{
    GET_DB_MAP(DB_guildStage, kDBTypeGuildBoss);
    CCDictionary* dic = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeGuildBoss);
    CCDictElement* pElement = NULL;
    CCDICT_FOREACH(dic, pElement)
    {
        DB_guildStage* pModule = (DB_guildStage*)pElement->getObject();
        if (pModule->stageGroup == type && pModule->groupLevel == level)
        {
            return pModule;
        }
    }
    return NULL;
}
std::vector<DB_guildStage*> DBManager::getGuildBoss()
{
    GET_DB_MAP(DB_guildStage, kDBTypeGuildBoss);
    CCDictionary* dic = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeGuildBoss);
    CCDictElement* pElement = NULL;
    std::vector<DB_guildStage*> list;
    CCDICT_FOREACH(dic, pElement)
    {
        DB_guildStage* pModule = (DB_guildStage*)pElement->getObject();
        list.push_back(pModule);
    }
    return list;
}

std::vector<DGuildActivities*> DBManager::getGuildMainActivity()
{
    GET_DB_MAP(DGuildActivities, kDBTypeGuildActivities);
    CCDictionary* dic = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeGuildActivities);
    CCDictElement* pElement = NULL;
    std::vector<DGuildActivities*> list;
    CCDICT_FOREACH(dic, pElement)
    {
        DGuildActivities* pModule = (DGuildActivities*)pElement->getObject();
        if (pModule->type == 0)
        {
            list.push_back(pModule);
        }
    }
    return list;
}
DGuildActivities* DBManager::getGuildActivity(int id)
{
    GET_DB_MAP(DGuildActivities, kDBTypeGuildActivities);
    CCDictionary* dic = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeGuildActivities);
    CCDictElement* pElement = NULL;
    CCDICT_FOREACH(dic, pElement)
    {
        DGuildActivities* pModule = (DGuildActivities*)pElement->getObject();
        if (pModule->ID == id)
        {
            return pModule;
        }
    }
    return NULL;
}
/***得到主角的所有主动技能  在加载时调用***/
vector<netSkillInfo*> DBManager::getAllSkill(int type)
{
	if (m_pDBCache->objectForKey(kDBTypeZX_ZhuDongSkill) == NULL)
	{
		//技能表
		DB_ActorSkill::open(); 
		char str[30] = {0}; 
		sprintf(str,"select * from %s", DB_ActorSkill::dbTableName.c_str()); 
		DB_ActorSkill::ExcuteQuery(str); 
		m_pDBCache->setObject(DB_ActorSkill::getQueryResultDictionary(), kDBTypeZX_ZhuDongSkill);

		sprintf(str,"select * from %s", DB_ActorSkill::dbTableName_1.c_str()); 
		DB_ActorSkill::ExcuteQuery(str); 
		m_pDBCache->setObject(DB_ActorSkill::getQueryResultDictionary(), kDBTypeEnemy_ZhuDongSkill);

		DB_ActorSkill::close(); 
	}

	vector<netSkillInfo*> allSkill;
	CCDictElement* pElement = NULL;

	CCDictionary* skill = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeZX_ZhuDongSkill);
	CCDICT_FOREACH(skill, pElement)
	{
		DB_ActorSkill* frameDict = dynamic_cast<DB_ActorSkill*>(pElement->getObject());
		if(frameDict && frameDict->icon.length()> 1)
		{
			netSkillInfo* nsi = new netSkillInfo;
			nsi->skillID = frameDict->skillId;
			nsi->skillLevel = -1;
		}
	}
	return allSkill;
}

DB_guildset* DBManager::getGuild(short level)
{
	GET_DB_MAP(DB_guildset, kDBTypeGuild);
	CCDictionary* dic = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeGuild);
	return dynamic_cast<DB_guildset*>(dic->objectForKey(level));
}

DB_GuildSkill* DBManager::getGuildSkill(const string& skillID)
{
	GET_DB_MAP(DB_GuildSkill, kDBTypeGuildSkill);
	CCDictionary* dic = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeGuildSkill);
	CCDictElement* pElement = NULL;
	CCDICT_FOREACH(dic, pElement)
	{
		DB_GuildSkill* pModule = (DB_GuildSkill*)pElement->getObject();
		if (pModule->skillId.compare(skillID) == 0)
		{
			return pModule;
		}
	}
	return NULL;
}

XVIPSetting* DBManager::getVIP(int ID)
{
	GET_DB_MAP(XVIPSetting, kDBTypeVip);
	CCDictionary* dic = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeVip);
	return dynamic_cast<XVIPSetting*>(dic->objectForKey(ID));
}

int DBManager::getVIPMaxNum()
{
	GET_DB_MAP(XVIPSetting, kDBTypeVip);
	CCDictionary* dic = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeVip);
	return dic->count()-1;
}

CCArray* DBManager::getJingMai()
{
	GET_DB_ARRAY(DB_JingMai, kDBTypeJingMai);
	CCArray* list = (CCArray*)m_pDBCache->objectForKey(kDBTypeJingMai);
	return list;
}

CCArray* DBManager::getJingMaiAdd()
{
	GET_DB_ARRAY(DB_JingMaiAdd, kDBTypeJingMaiAdd);
	CCArray* list = (CCArray*)m_pDBCache->objectForKey(kDBTypeJingMaiAdd);
	return list;
}

CCArray* DBManager::getAllDungeons()
{
	GET_DB_ARRAY(DB_dungeon, kDBTypeDungeon);
	CCArray* list = (CCArray*)m_pDBCache->objectForKey(kDBTypeDungeon);
	return list;
}

XFashion* DBManager::getFashion(int ID)
{
	GET_DB_MAP(XFashion, kDBTypeFashions);
	CCDictionary* list = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeFashions);
	CCDictElement* pElement = NULL;
	CCDICT_FOREACH(list, pElement)
	{
		XFashion* pModule = (XFashion*)pElement->getObject();
		if(pModule->ID == ID)
			return pModule;
	}
	return NULL;
}

CCArray* DBManager::getAllFashions(char heroType)
{
	GET_DB_MAP(XFashion, kDBTypeFashions);
	CCArray* list = CCArray::create();;//CCArray*)m_pDBCache->objectForKey(kDBTypeFashions);
	//for(int i =0;i<list->count();i++)
	//{
	//	XFashion* item = ((XFashion*)list->objectAtIndex(i));
	//	int itemId = item->ID;
	//	int jobId = Utils::getItemId(itemId);
	//	if(jobId != itemId)
	//	{
	//		list->removeObject(item);
	//		i--;
	//	}
	//}
	CCDictionary* listDic = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeFashions);
	CCDictElement* pElement = NULL;
	CCDICT_FOREACH(listDic, pElement)
	{
		XFashion* pModule = (XFashion*)pElement->getObject();
		if(pModule->jobLimit == m_hero->getPlayerInfo()->Profession)
		{
			list->addObject(pModule);
		}
	}

	CCArray* fashions = CCArray::create();
	CCArray* cloths = CCArray::create();
	CCArray* weapons = CCArray::create();
	fashions->addObject(cloths);
	fashions->addObject(weapons);
	for(int i =0;i<list->count();i++)
	{
		XFashion* item = ((XFashion*)list->objectAtIndex(i));
		if(item->subtype == 2)
		{
			bool isExist = false;
			for(int j =0;j<cloths->count();j++)
			{
				XFashion* temp = ((XFashion*)cloths->objectAtIndex(j));
				if(item->category == temp->category)
				{
					isExist = true;
					break;
				}
			}
			if(!isExist)
				cloths->addObject(item);
		}
		else
		{
			bool isExist = false;
			for(int j =0;j<weapons->count();j++)
			{
				XFashion* temp = ((XFashion*)weapons->objectAtIndex(j));
				if(item->category == temp->category)
				{
					isExist = true;
					break;
				}
			}
			if(!isExist)
				weapons->addObject(item);
		}
	}
	return fashions;
}


std::vector<DB_dungeon*> DBManager::getTargetBossDungeonList(const string& name,int type)
{
    GET_DB_ARRAY(DB_dungeon, kDBTypeDungeon);
    CCArray* list = (CCArray*)m_pDBCache->objectForKey(kDBTypeDungeon);

    vector<DB_dungeon*> allDungeons;
    CCObject* obj = NULL;
    CCARRAY_FOREACH(list, obj)
    {
        DB_dungeon* info = (DB_dungeon*)obj;
        if (info && info->previewskill.compare(name)==0)
        {
            if (type > 0)
            {
                if (info->level_difficulty == type)
                    allDungeons.push_back(info);
            }
            else
                allDungeons.push_back(info);
        }
    }
    stable_sort(allDungeons.begin(), allDungeons.end(), cmpDungeonInfoByLv);
    return allDungeons;
}


static bool cmpDungeonInfoByLv(const DB_dungeon* p1, const DB_dungeon* p2)
{
    if (p1->monster_level > p2->monster_level)
    {
        return false;
    }
    else if (p1->monster_level < p2->monster_level)
    {
        return true;
    }
    return false;
}

std::vector<DB_dungeon*> DBManager::getDungeonList(int type)
{
	GET_DB_ARRAY(DB_dungeon, kDBTypeDungeon);
	CCArray* list = (CCArray*)m_pDBCache->objectForKey(kDBTypeDungeon);

	vector<DB_dungeon*> allDungeons;
	CCObject* obj = NULL;
	CCARRAY_FOREACH(list, obj)
	{
		DB_dungeon* info = (DB_dungeon* )obj;
		if(info)
		{
			if(type >0 )
			{
				if(info->level_difficulty == type)
					allDungeons.push_back(info);
			}
			else
				allDungeons.push_back(info);
		}
	}

	return allDungeons;
}

std::vector<DB_dungeon*> DBManager::getGVEDungeonList()
{
	GET_DB_ARRAY(DB_dungeon, kDBTypeDungeon);
	CCArray* list = (CCArray*)m_pDBCache->objectForKey(kDBTypeDungeon);

	vector<DB_dungeon*> allDungeons;
	CCObject* obj = NULL;
	CCARRAY_FOREACH(list, obj)
	{
		DB_dungeon* info = (DB_dungeon* )obj;
		if(info)
		{
			if(info->playerNum>1)
			{
				allDungeons.push_back(info);
			}
		}
	}

	return allDungeons;
}

std::vector<DB_dungeon*> DBManager::getLingShiBenDungeonList()
{
	GET_DB_ARRAY(DB_dungeon, kDBTypeDungeon);
	CCArray* list = (CCArray*)m_pDBCache->objectForKey(kDBTypeDungeon);

	vector<DB_dungeon*> allDungeons;
	CCObject* obj = NULL;
	CCARRAY_FOREACH(list, obj)
	{
		DB_dungeon* info = (DB_dungeon* )obj;
		if(info)
		{
			if (info->region_key == "GEM"&& m_hero->getPlayerInfo()->Level >= info->level_down)
			{
				allDungeons.push_back(info);
			}
		}
	}

	return allDungeons;
}

std::vector<DB_dungeon*> DBManager::getJinQianBenDungeonList()
{
	GET_DB_ARRAY(DB_dungeon, kDBTypeDungeon);
	CCArray* list = (CCArray*)m_pDBCache->objectForKey(kDBTypeDungeon);

	vector<DB_dungeon*> allDungeons;
	CCObject* obj = NULL;
	CCARRAY_FOREACH(list, obj)
	{
		DB_dungeon* info = (DB_dungeon* )obj;
		if(info)
		{
			if(info->region_key == "GOLD")
			{
				allDungeons.push_back(info);
			}
		}
	}

	return allDungeons;
}

std::vector<DB_dungeon*> DBManager::getJingYanBenDungeonList()
{
	GET_DB_ARRAY(DB_dungeon, kDBTypeDungeon);
	CCArray* list = (CCArray*)m_pDBCache->objectForKey(kDBTypeDungeon);

	vector<DB_dungeon*> allDungeons;
	CCObject* obj = NULL;
	CCARRAY_FOREACH(list, obj)
	{
		DB_dungeon* info = (DB_dungeon* )obj;
		if(info)
		{
			if(info->region_key == "EXP")
			{
				allDungeons.push_back(info);
			}
		}
	}

	return allDungeons;
}
std::vector<DB_dungeon*> DBManager::getWuXianTaDungeonList(int chapter)
{
	GET_DB_ARRAY(DB_dungeon, kDBTypeDungeon);
	CCArray* list = (CCArray*)m_pDBCache->objectForKey(kDBTypeDungeon);

	vector<DB_dungeon*> allDungeons;
	CCObject* obj = NULL;
	CCARRAY_FOREACH(list, obj)
	{
		DB_dungeon* info = (DB_dungeon* )obj;
		if(info)
		{
			if(info->region_key == "tongtianta" && info->chapter == chapter)
			{
				allDungeons.push_back(info);
			}
		}
	}

	return allDungeons;
}
DB_dungeon* DBManager::getWuXianTaDungeon(int dungeonId)
{
	GET_DB_ARRAY(DB_dungeon, kDBTypeDungeon);
	CCArray* list = (CCArray*)m_pDBCache->objectForKey(kDBTypeDungeon);

	CCObject* obj = NULL;
	CCARRAY_FOREACH(list, obj)
	{
		DB_dungeon* info = (DB_dungeon* )obj;
		if (info->ID == dungeonId)
		{
			return info;
		}
	}
	return NULL;
}
DB_dungeon* DBManager::getWuXianTaNextDungeon(DB_dungeon* curDungen)
{
	vector<DB_dungeon*> wuxiantaList = getWuXianTaDungeonList(curDungen->chapter);
	int curDungenIndex = -1;
	for(int i =0;i<wuxiantaList.size()-1;i++){
		if(curDungen->ID == wuxiantaList.at(i)->ID){
			curDungenIndex = i;
			break;
		}
	}
	
	if(curDungenIndex>=0){
		return wuxiantaList.at(curDungenIndex+1);
	}

	return NULL;
}

//添加试炼之路副本, add by zyp，2015-9-21
//得到所有15个试炼之路的副本表
std::vector<DB_dungeon*> DBManager::getShiLianZhiLuDungeonList()
{
	GET_DB_ARRAY(DB_dungeon, kDBTypeDungeon);
	CCArray* list = (CCArray*)m_pDBCache->objectForKey(kDBTypeDungeon);

	vector<DB_dungeon*> allDungeons;
	CCObject* obj = NULL;
	CCARRAY_FOREACH(list, obj)
	{
		DB_dungeon* info = (DB_dungeon* )obj;
		if(info)
		{
			if(info->region_key == "lasttest")
			{
				allDungeons.push_back(info);
			}
		}
	}

	return allDungeons;
}

//得到试炼之路数据表
std::vector<DShilian*> DBManager::getShiLianZhiLuDataListAll()
{
	GET_DB_ARRAY(DShilian, KDBTypeShiLianZhiLu);
	CCArray* list = (CCArray*)m_pDBCache->objectForKey(KDBTypeShiLianZhiLu);

	vector<DShilian*> allData;
	CCObject* obj = NULL;
	CCARRAY_FOREACH(list, obj)
	{
		DShilian* info = (DShilian*)obj;
		if(info)
		{
			allData.push_back(info);
		}
	}

	return allData;
}

std::vector<DShilian*> DBManager::getShiLianZhiLuDataList(int category)
{
	GET_DB_ARRAY(DShilian, KDBTypeShiLianZhiLu);
	CCArray* list = (CCArray*)m_pDBCache->objectForKey(KDBTypeShiLianZhiLu);
	
	vector<DShilian*> allData;
	CCObject* obj = NULL;
	CCARRAY_FOREACH(list, obj)
	{
		DShilian* info = (DShilian*)obj;
		if(info)
		{
			if(info->category == category)
			{
				allData.push_back(info);
			}
		}
	}

	return allData;
}
//end zyp

CCArray* DBManager::getAllDungeonMaps()
{
	CCArray* list = getAllDungeons();
	CCArray* mapKeys = CCArray::create();
	for(int i =0;i<list->count();i++)
	{
		DB_dungeon* db = (DB_dungeon*)list->objectAtIndex(i);
		CCString* keys = CCString::create(db->region_key);
		bool isExist = false;
		for(int j =0;j<mapKeys->count();j++)
		{
			CCString* temp = ((CCString*)mapKeys->objectAtIndex(j));
			if(temp->compare(keys->getCString()) == 0)
				isExist = true;
		}
		if(!isExist)
			mapKeys->addObject(keys);
	}
	return mapKeys;
}

std::vector<DB_dungeon*> DBManager::getRegionDungeons(const std::string& regionKey)
{
    long tmp = CSystem::currentTimeMillis();
    GET_DB_ARRAY(DB_dungeon, kDBTypeDungeon);
    CCArray* dblist = (CCArray*)m_pDBCache->objectForKey(kDBTypeDungeon);
    vector<DB_dungeon*> list;
    CCObject* obj = NULL;
    CCARRAY_FOREACH(dblist, obj)
    {
        DB_dungeon* info = (DB_dungeon*)obj;
        if (info->region_key.compare(regionKey) == 0)
        {
            list.push_back(info);
        }
    }
    //DB_dungeon::open();
    //char str[50] = { 0 };
    //sprintf(str, "select * from %s where region_key='%s'", DB_dungeon::dbTableName.c_str(), regionKey.c_str());
    //DB_dungeon::ExcuteQuery(str);
    //vector<DB_dungeon*> list;
    //if (DB_dungeon::getQueryResult().size() > 0)
    //{
    //    list = DB_dungeon::getQueryResult();
    //}
    //DB_dungeon::close();

    CCLOG("DBManager::getRegionDungeons use Time is %lld", CSystem::currentTimeMillis() - tmp);
    return list;
}
DB_dungeon* DBManager::getDungeon(const std::string& key)
{
	GET_DB_ARRAY(DB_dungeon, kDBTypeDungeon);
	CCArray* list = (CCArray*)m_pDBCache->objectForKey(kDBTypeDungeon);

	CCObject* obj = NULL;
	CCARRAY_FOREACH(list, obj)
	{
		DB_dungeon* info = (DB_dungeon* )obj;
		if (info->skey.compare(key) == 0)
		{
			return info;
		}
	}
	return NULL;
}

cocos2d::CCArray* DBManager::getDungeonDrops(const std::string& key)
{
	GET_DB_ARRAY(DB_dungeon_drop, kDBTypeDungeonDrops);
	CCArray* list = (CCArray*)m_pDBCache->objectForKey(kDBTypeDungeonDrops);

	CCArray* arr = CCArray::create();
	CCObject* obj = NULL;
	CCARRAY_FOREACH(list, obj)
	{
		DB_dungeon_drop* info = (DB_dungeon_drop* )obj;
		if (info->skey.compare(key) == 0)
		{
			arr->addObject(obj);
		}
	}
	return arr;
}

CCArray* DBManager::getAllQuests()
{
	GET_DB_ARRAY(DB_gamequest, kDBTypeQuest);
	CCArray* list = (CCArray*)m_pDBCache->objectForKey(kDBTypeQuest);
	return list;
}

XGuaSetting* DBManager::getGuaSetting(int ID)
{
	GET_DB_MAP(XGuaSetting, kDBTypeGuaSetting);
	CCDictionary* dic = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeGuaSetting);

	return dynamic_cast<XGuaSetting*>(dic->objectForKey(ID));
}

int DBManager::getUnLockLevel(int holeNum)
{
	GET_DB_ARRAY(XLevelinfo, kDBTypeLevelinfo);
	CCArray* list = (CCArray*)m_pDBCache->objectForKey(kDBTypeLevelinfo);

	CCObject* obj = NULL;
	CCARRAY_FOREACH(list, obj)
	{
		XLevelinfo* info = (XLevelinfo* )obj;
		if (info->holenum >= holeNum)
		{
			return info->ID;
		}
	}
	return -1;
}


int DBManager::getSJMaxExpLevelup(int level)
{
    GET_DB_ARRAY(XLevelinfo, kDBTypeLevelinfo);
    CCArray* list = (CCArray*)m_pDBCache->objectForKey(kDBTypeLevelinfo);

    CCObject* obj = NULL;
    CCARRAY_FOREACH(list, obj)
    {
        XLevelinfo* info = (XLevelinfo*)obj;
        if (info->ID == level)
        {
            return info->sjlevelup_exp;
        }
    }
    return -1;
}

int DBManager::getMaxExpLevelup(int level)
{
    GET_DB_ARRAY(XLevelinfo, kDBTypeLevelinfo);
    CCArray* list = (CCArray*)m_pDBCache->objectForKey(kDBTypeLevelinfo);

    CCObject* obj = NULL;
    CCARRAY_FOREACH(list, obj)
    {
        XLevelinfo* info = (XLevelinfo*)obj;
        if (info->ID == level)
        {
            return info->levelup_exp;
        }
    }
	return -1;
}

DB_heroProperty* DBManager::getHeroProperty(int ID)
{
	/*GET_DB_MAP(DB_heroProperty, kDBTypeHeroProperty);
	CCDictionary* dic = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeHeroProperty);
	return dynamic_cast<DB_heroProperty*>(dic->objectForKey(ID));*/

	GET_DB_ARRAY(DB_heroProperty, kDBTypeHeroProperty);
	CCArray* list = (CCArray*)m_pDBCache->objectForKey(kDBTypeHeroProperty);
	CCObject* obj = NULL;
	CCARRAY_FOREACH(list, obj)
	{
		DB_heroProperty* info = (DB_heroProperty* )obj;
		if (info->ID == ID)
		{
			return info;
		}
	}
	return NULL;
}

int	DBManager::getXuanRenCounter()
{

	GET_DB_ARRAY(DB_heroProperty, kDBTypeHeroProperty);
	CCArray* list = (CCArray*)m_pDBCache->objectForKey(kDBTypeHeroProperty);
	CCObject* obj = NULL;
	int currentIndex = 0;
	CCARRAY_FOREACH(list, obj)
	{
		DB_heroProperty* info = (DB_heroProperty* )obj;
		if (info->xuanren_index > currentIndex)
		{
			currentIndex = info->xuanren_index;
		}
	}
	return currentIndex;
}

//通过选人界面顺序获得相应的heroProperty
DB_heroProperty* DBManager::getHRByXuanRenIndex(int index)
{
	GET_DB_ARRAY(DB_heroProperty, kDBTypeHeroProperty);
	CCArray* list = (CCArray*)m_pDBCache->objectForKey(kDBTypeHeroProperty);
	CCObject* obj = NULL;
	CCARRAY_FOREACH(list, obj)
	{
		DB_heroProperty* info = (DB_heroProperty* )obj;
		if (info->xuanren_index == index)
		{
			return info;
		}
	}
	return NULL;
}

int	DBManager::getShuiBoWenCounter()
{
	GET_DB_MAP(DB_ShuiBoWen, kDBTypeShuiBoWen);
	CCDictionary* dic = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeShuiBoWen);
	return dic->count();
}
DB_ShuiBoWen* DBManager::getShuiBoWenByID(int ID)
{
	GET_DB_MAP(DB_ShuiBoWen, kDBTypeShuiBoWen);
	CCDictionary* dic = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeShuiBoWen);
	return dynamic_cast<DB_ShuiBoWen*>(dic->objectForKey(ID));
}
/*
DB_MonsterResource*	DBManager::getMonsterResourceBySpriteAnim(int spriteIndex, int animIndex)
{
	GET_DB_ARRAY(DB_MonsterResource, kDBTypeMonsterResource);
	CCArray* list = (CCArray*)m_pDBCache->objectForKey(kDBTypeMonsterResource);

	CCObject* obj = NULL;
	CCARRAY_FOREACH(list, obj)
	{
		DB_MonsterResource* info = (DB_MonsterResource* )obj;
		if (info->gts_sprite_index == spriteIndex)
		{
			if(spriteIndex == 6)
			{
				if(animIndex >= 20 && info->gts_anim_index == 20)
					return info;

				if(animIndex < 20 && info->gts_anim_index == 0)
					return info;
			} else 
				return info;
		}
	}
	return NULL;
}

DB_MonsterResource* DBManager::getMonsterResourceByID(int ID)
{
	GET_DB_ARRAY(DB_MonsterResource, kDBTypeMonsterResource);
	CCArray* list = (CCArray*)m_pDBCache->objectForKey(kDBTypeMonsterResource);

	CCObject* obj = NULL;
	CCARRAY_FOREACH(list, obj)
	{
		DB_MonsterResource* info = (DB_MonsterResource* )obj;
		if (info->ID == ID)
		{
			return info;
		}
	}
	return NULL;
}*/
cocos2d::CCArray* DBManager::getShopGoods(int goodID)
{
	GET_DB_ARRAY(XShopGoods, kDBTypeShopGoods);
	CCArray* list = (CCArray*)m_pDBCache->objectForKey(kDBTypeShopGoods);

	CCArray* arr = CCArray::create();
	CCObject* obj = NULL;
	CCARRAY_FOREACH(list, obj)
	{
		XShopGoods* info = (XShopGoods* )obj;
		if (info->shopID == goodID)
		{
			arr->addObject(obj);
		}
	}
	return arr;
}

string DBManager::getResNameByID(int resIndex, int type)
{
	GET_DB_ARRAY(DB_SceneActor, kDBTypeSceneActor);
	CCArray* list = (CCArray*)m_pDBCache->objectForKey(kDBTypeSceneActor);

	CCObject* obj = NULL;
	CCARRAY_FOREACH(list, obj)
	{
		DB_SceneActor* info = (DB_SceneActor* )obj;
		if (info->gts_sprite_index == resIndex && info->type == type)
		{
			return info->ResName;
		}
	}
	CCLOG("!!! error  res name is null type:%d  index %d",type,resIndex);
	return NULL;
}

DB_Particle* DBManager::getParticleByID(int ID)
{
	GET_DB_MAP(DB_Particle, kDBTypeParticle);
	CCDictionary* dic = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeParticle);
	return dynamic_cast<DB_Particle*>(dic->objectForKey(ID));
}

/************************************************************************/
/* 获得变色方案                                                                     */
/************************************************************************/
DB_Bianse* DBManager::getBianseByID(int ID)
{
	GET_DB_MAP(DB_Bianse, kDBTypeBianse);
	CCDictionary* dic = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeBianse);
	return dynamic_cast<DB_Bianse*>(dic->objectForKey(ID));
}



DB_Bullet* DBManager::getBulletByID(int ID)
{
	GET_DB_MAP(DB_Bullet, kDBTypeBullet);
	CCDictionary* dic = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeBullet);
	return dynamic_cast<DB_Bullet*>(dic->objectForKey(ID));
}


vector<DB_State *> DBManager::getDBStateListByTableName(std::string tableName)
{
	char cacheKey[100];
	sprintf(cacheKey, "%d%s", kDBTypeState, tableName.c_str());
	GET_DB_ARRAY_TABLENAME(DB_State, cacheKey, tableName.c_str());


	CCArray* list = (CCArray*)m_pDBCacheState->objectForKey(cacheKey);
	vector<DB_State *> actionList;

	CCObject* obj = NULL;
	CCARRAY_FOREACH(list, obj)
	{
		DB_State* info = (DB_State* )obj;
		actionList.push_back(info);
	}


	return actionList;
}


DB_MonsterProperty* DBManager::getDBMonsterPropertyByName(std::string monsterName)
{
	////怪属性表
 //   DB_MonsterProperty::open();
 //   char str[100] = { 0 };
 //   sprintf(str, "select * from %s where monster_id=\"%s\" ", DB_MonsterProperty::dbTableName.c_str(), monsterName.c_str());
 //   DB_MonsterProperty::ExcuteQuery(str);
 //   DB_MonsterProperty* item = NULL;
 //   if (DB_MonsterProperty::getQueryResult().size() > 0)
 //   {
 //       item = DB_MonsterProperty::getQueryResult().at(0);
 //   }
 //   DB_MonsterProperty::close();
    GET_DB_ARRAY(DB_MonsterProperty, kDBTypeMonsterProp);
    CCArray* list = (CCArray*)m_pDBCache->objectForKey(kDBTypeMonsterProp);

    CCObject* obj = NULL;
    CCARRAY_FOREACH(list, obj)
    {
        DB_MonsterProperty* info = (DB_MonsterProperty*)obj;
        if (info->monster_id.compare(monsterName) == 0)
            return info;
    }
    return NULL;
}

DB_MonsterProperty* DBManager::getDBMonsterPropertyByGTSindex(std::string tableName, int gts_index)
{
	//怪属性表
	char cacheKey[100];
	sprintf(cacheKey, "%d%s", kDBTypeMonsterProp, tableName.c_str());

	GET_DB_ARRAY_TABLENAME(DB_MonsterProperty, cacheKey, tableName.c_str());
	CCArray* list = (CCArray*)m_pDBCacheState->objectForKey(cacheKey);

	CCObject* obj = NULL;
	//map<int, DB_MonsterProperty*> msMap;
	CCARRAY_FOREACH(list, obj)
	{
		DB_MonsterProperty *prop = (DB_MonsterProperty *)obj;
		if(prop->gts_index == gts_index)
		{
			//msMap.insert(pair<int, DB_MonsterProperty*>(prop->level, prop));
			return prop;
		}
	}

	return NULL;
}

XSetcombat* DBManager::getCombat(int index)
{
	GET_DB_MAP(XSetcombat, kDBTypeCombat);
	CCDictionary* dic = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeCombat);
	return dynamic_cast<XSetcombat*>(dic->objectForKey(index));
}

//获取当前等级全部类型的分类型怪数据
//diffcult暂时没用了
map<std::string, DB_MonsterData *> DBManager::getAllTypeDBMonsterDataCurLevel(int curLevel, std::string diffcult)
{
	GET_DB_ARRAY(DB_MonsterData, kDBTypeMonsterData);
	CCArray* list = (CCArray*)m_pDBCache->objectForKey(kDBTypeMonsterData);

	CCObject* obj = NULL;
	map<std::string, DB_MonsterData *> monsterData;
	CCARRAY_FOREACH(list, obj)
	{
		DB_MonsterData* info = (DB_MonsterData* )obj;
		if (info->level == curLevel)
		{
			monsterData.insert(pair<std::string, DB_MonsterData *>(info->type.append(info->battlefield), info));
		}
	}
	
	return monsterData;
}
//diffcult暂时没用了
map<std::string, DB_MonsterDataPublic*> DBManager::getDBMonsterDataPublicCurLevel(int curLevel, std::string diffcult)
{
	GET_DB_ARRAY(DB_MonsterDataPublic, kDBTypeMonsterDataPublic);
	CCArray* list = (CCArray*)m_pDBCache->objectForKey(kDBTypeMonsterDataPublic);

	CCObject* obj = NULL;
	
	map<std::string, DB_MonsterDataPublic *> monsterDataPublic;
	CCARRAY_FOREACH(list, obj)
	{
		DB_MonsterDataPublic* info = (DB_MonsterDataPublic* )obj;
		if (info->level == curLevel)
		{
			//return info;
			monsterDataPublic.insert(pair<std::string, DB_MonsterDataPublic *>(info->battlefield, info));
		}
	}

	return monsterDataPublic;
}
cocos2d::CCArray* DBManager::getLoadingTip(int level)
{
	GET_DB_ARRAY(XLoadingTip, kDBTypeLoadingTip);
	CCArray* list = (CCArray*)m_pDBCache->objectForKey(kDBTypeLoadingTip);

	CCArray* arr = CCArray::create();
	CCObject* obj = NULL;
	CCARRAY_FOREACH(list, obj)
	{
		XLoadingTip* info = (XLoadingTip*)obj;
		if (info->level == 0)
		{
			// 公共
			arr->addObject(obj);
		}
		else if (level <= info->level 
					&& info->level - level < 10)
		{
			// 合适等级
			arr->addObject(obj);
		}
	}
	return arr;
}

std::vector<DB_Teach*> DBManager::getTeachList(const string& name)
{
	vector<DB_Teach*> vec;

	GET_DB_ARRAY(DB_Teach, kDBTypeTeach);
	CCArray* list = (CCArray*)m_pDBCache->objectForKey(kDBTypeTeach);
	CCObject* obj = NULL;
	CCARRAY_FOREACH(list, obj)
	{
		DB_Teach* info = (DB_Teach*)obj;
		if (info->function_name.compare(name) == 0)
		{
			vec.push_back(info);
		}
	}
	return vec;
}

DB_city* DBManager::getCity(const string& name)
{
	GET_DB_ARRAY(DB_city, kDBTypeCity);
	CCArray* list = (CCArray*)m_pDBCache->objectForKey(kDBTypeCity);
	CCObject* obj = NULL;
	CCARRAY_FOREACH(list, obj)
	{
		DB_city* info = (DB_city* )obj;
		if (info->skey.compare(name) == 0)
		{
			return info;
		}
	}
	return NULL;
}

DB_pvpaward* DBManager::getPVPAward(int rank)
{
	GET_DB_ARRAY(DB_pvpaward, kDBTypePVPAward);
	CCArray* list = (CCArray*)m_pDBCache->objectForKey(kDBTypePVPAward);
	CCObject* obj = NULL;
	CCARRAY_FOREACH(list, obj)
	{
		DB_pvpaward* info = (DB_pvpaward* )obj;
		if (info->rankmin <= rank && info->rankmax >= rank)
		{
			return info;
		}
	}

	int maxLevel = 10;
	DB_pvpaward* max = NULL;
	CCARRAY_FOREACH(list, obj)
	{
		DB_pvpaward* info = (DB_pvpaward* )obj;
		if (info->rankmin < maxLevel)
		{
			maxLevel = info->rankmin;
			max = info;
		}
	}
	
	return max;
}

std::vector<DB_pvpaward*> DBManager::getPVPAwardList()
{
	GET_DB_ARRAY(DB_pvpaward, kDBTypePVPAward);
	CCArray* list = (CCArray*)m_pDBCache->objectForKey(kDBTypePVPAward);
	CCObject* obj = NULL;
	
	vector<DB_pvpaward*> allAwards;

	CCARRAY_FOREACH(list, obj)
	{
		DB_pvpaward* info = (DB_pvpaward* )obj;
		if(info)
		{
			allAwards.push_back(info);
		}

	}

	return allAwards;
}

std::vector<DPvpDoushentaiRankingReward*> DBManager::getPVPMatchScoreAwardList()
{
	GET_DB_ARRAY(DPvpDoushentaiRankingReward, kDBTypePVPMatchScoreAward);
	CCArray* list = (CCArray*)m_pDBCache->objectForKey(kDBTypePVPMatchScoreAward);
	CCObject* obj = NULL;

	vector<DPvpDoushentaiRankingReward*> allAwards;

	CCARRAY_FOREACH(list, obj)
	{
		DPvpDoushentaiRankingReward* info = (DPvpDoushentaiRankingReward* )obj;
		if(info)
		{
			allAwards.push_back(info);
		}

	}

	return allAwards;
}

//添加跨服战, add by zyp，2015-12-22
//跨服战，奖励列表
std::vector<DB_KuafuzhanReward*> DBManager::getKuaFuZhanAwardList()
{
	GET_DB_ARRAY(DB_KuafuzhanReward, KDBTypeKuaFuZhanReward);
	CCArray* list = (CCArray*)m_pDBCache->objectForKey(KDBTypeKuaFuZhanReward);
	CCObject* obj = NULL;
	
	vector<DB_KuafuzhanReward*> allAwards;

	CCARRAY_FOREACH(list, obj)
	{
		DB_KuafuzhanReward* info = (DB_KuafuzhanReward* )obj;
		if(info)
		{
			allAwards.push_back(info);
		}

	}

	return allAwards;
}

//跨服战，根据等级获得数据
DB_KuafuzhanReward* DBManager::getKuaFuZhanAward(int level)
{
	GET_DB_ARRAY(DB_KuafuzhanReward, KDBTypeKuaFuZhanReward);
	CCArray* list = (CCArray*)m_pDBCache->objectForKey(KDBTypeKuaFuZhanReward);
	CCObject* obj = NULL;

	vector<DB_KuafuzhanReward*> allAwards;

	CCARRAY_FOREACH(list, obj)
	{
		DB_KuafuzhanReward* info = (DB_KuafuzhanReward* )obj;
		if(info && info->ID == level)
		{
			return info;
		}
	}
	return NULL;
}
//end zyp

// 任务剧情
vector<XQuestTip*> DBManager::getQuestTipList(const string& name)
{
	vector<XQuestTip*> vec;

	GET_DB_ARRAY(XQuestTip, kDBTypeQuestTip);
	CCArray* list = (CCArray*)m_pDBCache->objectForKey(kDBTypeQuestTip);
	CCObject* obj = NULL;
	CCARRAY_FOREACH(list, obj)
	{
		XQuestTip* info = (XQuestTip*)obj;
		if (info->taskId.compare(name) == 0)
		{
			vec.push_back(info);
		}
	}
	return vec;
}

//推送消息
DB_PushNotification* DBManager::getPushInfo(int ID)
{
	GET_DB_MAP(DB_PushNotification, kDBTypePush);
	CCDictionary* dic = (CCDictionary*)m_pDBCache->objectForKey(kDBTypePush);
	return dynamic_cast<DB_PushNotification*>(dic->objectForKey(ID));
}

DB_ShenJiangProperty* DBManager::getShenJiangProperty(int ID)
{
	/*GET_DB_MAP(DB_heroProperty, kDBTypeHeroProperty);
	CCDictionary* dic = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeHeroProperty);
	return dynamic_cast<DB_heroProperty*>(dic->objectForKey(ID));*/

	GET_DB_ARRAY(DB_ShenJiangProperty, kDBTypeShenJiangProperty);
	CCArray* list = (CCArray*)m_pDBCache->objectForKey(kDBTypeShenJiangProperty);
	CCObject* obj = NULL;
	CCARRAY_FOREACH(list, obj)
	{
		DB_ShenJiangProperty* info = (DB_ShenJiangProperty* )obj;
		if (info->ID == ID)
		{
			return info;
		}
	}
	return NULL;
}

int DBManager::getXunBaoPrice(int type)
{
	GET_DB_ARRAY(DRaffle, kDBTypeRaffle);
	CCArray* list = (CCArray*)m_pDBCache->objectForKey(kDBTypeRaffle);
	CCObject* obj = NULL;
	CCARRAY_FOREACH(list, obj)
	{
		DRaffle* info = (DRaffle* )obj;
		if (info->ctype == type)
		{
			return info->price;
		}
	}
	return 0;
}

int DBManager::getXunBaoRenFreeTimes(int type)
{
	GET_DB_ARRAY(DRaffle, kDBTypeRaffle);
	CCArray* list = (CCArray*)m_pDBCache->objectForKey(kDBTypeRaffle);
	CCObject* obj = NULL;
	CCARRAY_FOREACH(list, obj)
	{
		DRaffle* info = (DRaffle* )obj;
		if (info->ctype == type)
		{
			return info->freeTimes;
		}
	}
	return 0;
}

/**
 * Buff
 */
DB_Buff* DBManager::getBuff(int buffID)
{
	GET_DB_MAP(DB_Buff, kDBTypeBuff);

	CCDictionary* dic = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeBuff);
	CCDictElement* pElement = NULL;
	CCDICT_FOREACH(dic, pElement)
	{
		DB_Buff* frameDict = dynamic_cast<DB_Buff*>(pElement->getObject());
		if (frameDict->ID == buffID)
		{
			return frameDict;
		}
	}
	return NULL;
}

XTitle* DBManager::getTitle(int ID)
{
	GET_DB_MAP(XTitle, kDBTypeTitle);
	CCDictionary* list = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeTitle);
	CCDictElement* pElement = NULL;
	CCDICT_FOREACH(list, pElement)
	{
		XTitle* pModule = (XTitle*)pElement->getObject();
		if(pModule->ID == ID)
			return pModule;
	}
	return NULL;
}

CCArray* DBManager::getAllTitles()
{
	GET_DB_MAP(XTitle, kDBTypeTitle);
	CCArray* list = CCArray::create();

	CCDictionary* listDic = (CCDictionary*)m_pDBCache->objectForKey(kDBTypeTitle);
	CCDictElement* pElement = NULL;
	CCDICT_FOREACH(listDic, pElement)
	{
		XTitle* pModule = (XTitle*)pElement->getObject();
		list->addObject(pModule);
	}

	CCArray* titles = CCArray::create();
	CCArray* billboards = CCArray::create();
	CCArray* levels = CCArray::create();
	CCArray* gems = CCArray::create();
	CCArray* guilds = CCArray::create();
	CCArray* vips = CCArray::create();
	CCArray* backs = CCArray::create();
	CCArray* shengJiangs = CCArray::create();
	CCArray* lianGuas = CCArray::create();
	CCArray* jingShus = CCArray::create();

	titles->addObject(billboards);
	titles->addObject(levels);
	titles->addObject(gems);
	titles->addObject(guilds);
	titles->addObject(vips);
	titles->addObject(backs);
	titles->addObject(shengJiangs);
	titles->addObject(lianGuas);
	titles->addObject(jingShus);

	for(int i = 0;i < list->count();i++)
	{
		XTitle* item = ((XTitle*)list->objectAtIndex(i));
		if(item->seriesID == 1)
		{
			billboards->addObject(item);
		}
		else if(item->seriesID == 2)
		{
			levels->addObject(item);
		}else if(item->seriesID == 3)
		{
			gems->addObject(item);
		}else if(item->seriesID == 4)
		{
			guilds->addObject(item);
		}
		else if(item->seriesID == 5)
		{
			vips->addObject(item);
		}
		else if(item->seriesID == 6)
		{
			backs->addObject(item);
		}else if (item->seriesID == 7)
		{
			shengJiangs->addObject(item);
		}else if (item->seriesID == 8)
		{
			lianGuas->addObject(item);
		}else if (item->seriesID == 9)
		{
			jingShus->addObject(item);
		}
	}
	return titles;
}

