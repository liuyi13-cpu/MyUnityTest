#include "DB_Bianse.h" 
#include "libGCExtensions/wxSqlite3/sqlite3.h" 
#include "../../utils/Utils.h" 
#include "../../utils/CCUserDefaultManager.h" 
 
string DB_Bianse::dbsrcFileName = "db/bianse.db"; 
string DB_Bianse::dbdesFileName = "bianse.db"; 
string DB_Bianse::dbTableName = "d_bianse"; 
 
sqlite3* DB_Bianse::pDB = NULL;//数据库指针 
int DB_Bianse::result	=	0; 
char* DB_Bianse::errMsg = NULL; 
vector<DB_Bianse*> DB_Bianse::resultVector ; 
CCArray* DB_Bianse::resultArray = NULL; 
CCDictionary* DB_Bianse::resultDictionary = NULL; 
 
void DB_Bianse::open() 
{ 
	#if (CC_TARGET_PLATFORM == CC_PLATFORM_ANDROID) 
	// sqlite不能读取ANDROID apk里的db文件 
	string despath = CCFileUtils::sharedFileUtils()->getWritablePath() + dbdesFileName; 
	string despathMD5 = CCFileUtils::sharedFileUtils()->getWritablePath() + CCFileUtils::sharedFileUtils()->MD5Filename(dbdesFileName.c_str()); 
	string newValue = CCUserDefault::sharedUserDefault()->getStringForKey(KEY_OF_CODEVERSION);
	newValue += "_" + Utils::itos(CCUserDefault::sharedUserDefault()->getIntegerForKey(KEY_OF_RESOURCEVERSION));
	string key = dbdesFileName;
	string value = CCUserDefault::sharedUserDefault()->getStringForKey(key.c_str());
	if (!CCFileUtils::sharedFileUtils()->isFileExist(despath) || 
		value.empty() || 
		value.compare(newValue) != 0)
	{ 
		Utils::FileCopy(dbsrcFileName, despathMD5); 
		CCUserDefault::sharedUserDefault()->setStringForKey(key.c_str(), newValue);
	}
	despath = despathMD5; 
	#else 
	string despath = CCFileUtils::sharedFileUtils()->MD5fullPathForFilename(dbsrcFileName.c_str()); 
	#endif 
	 
	result = sqlite3_open( despath.c_str() , &pDB); 
	CCAssert( result == SQLITE_OK , "打开数据库失败(DB_Bianse)"); 
	 
	sqlite3_key(pDB, "woshixiaopingguo", 4); 
	 
	resultArray = CCArray::create(); 
	resultArray->retain(); 
	 
	resultDictionary = CCDictionary::create(); 
	resultDictionary->retain(); 
} 
 
void DB_Bianse::close() 
{ 
	releaseVector(); 
	 
	CC_SAFE_RELEASE(resultArray); 
	 
	CC_SAFE_RELEASE(resultDictionary); 
	 
	sqlite3_close(pDB); 
} 
 
void DB_Bianse::releaseVector() 
{ 
	for (unsigned int i = 0;i < resultVector.size();i ++) 
	{ 
		((DB_Bianse*)resultVector.at(i))->release(); 
	} 
	resultVector.clear(); 
} 
 
void DB_Bianse::ExcuteQuery(string sqlstr) 
{ 
	releaseVector();//Utils::Unicode2Utf8(Utils::Acsi2WideByte(sqlstr)).c_str() 
	sqlite3_exec( pDB, sqlstr.c_str() , loadRecord, NULL, &errMsg ); 
} 
 
int DB_Bianse::loadRecord( void * para, int n_column, char ** column_value, char ** column_name ) 
{ 
	DB_Bianse* npc = new DB_Bianse(column_value) ; 
	resultVector.push_back(npc) ; 
	resultArray->addObject(npc); 
	resultDictionary->setObject(npc, npc->ID); 
	return 0; 
} 
 
vector<DB_Bianse*> DB_Bianse::getQueryResult() 
{ 
	return resultVector; 
} 
 
CCArray* DB_Bianse::getQueryResultArray() 
{ 
	return resultArray; 
} 
 
CCDictionary* DB_Bianse::getQueryResultDictionary() 
{ 
	return resultDictionary; 
} 
 
void DB_Bianse::ExcuteQueryNoneResult(string sqlstr) 
{ 
	releaseVector(); 
	sqlite3_exec( pDB, sqlstr.c_str() , NULL, NULL, &errMsg ); 
} 
 
DB_Bianse::DB_Bianse(char ** column_value) 
{ 
	ID = atoi(column_value[DB_Bianse_ID]); 
	desc = string(column_value[DB_Bianse_desc]); 
	color_r = atof(column_value[DB_Bianse_color_r]); 
	color_g = atof(column_value[DB_Bianse_color_g]); 
	color_b = atof(column_value[DB_Bianse_color_b]); 
	bianse_type = atoi(column_value[DB_Bianse_bianse_type]); 
	color_hue = atof(column_value[DB_Bianse_color_hue]); 
} 
 
DB_Bianse::DB_Bianse() 
{ 
	 
} 
 
DB_Bianse::~DB_Bianse() 
{ 
	 
} 
