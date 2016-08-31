#ifndef __DB_Bianse_H__ 
#define __DB_Bianse_H__ 
 
#include "cocos2d.h" 
 
USING_NS_CC; 
using namespace std; 
 
struct sqlite3; 
class DB_Bianse : public CCObject 
{ 
	public: 
		DB_Bianse(char ** column_value); 
		DB_Bianse(); 
		virtual ~DB_Bianse(); 
		 
		static sqlite3 *pDB ;//数据库指针 
		static vector<DB_Bianse*> resultVector ; 
		static CCArray* resultArray; 
		static CCDictionary* resultDictionary; 
		 
		static int result;//sqlite3_exec返回值 
		static char * errMsg;//错误信息 
		 
		static void open() ; 
		static void ExcuteQuery(string sqlstr) ; 
		static int loadRecord( void * para, int n_column, char ** column_value, char ** column_name ) ; 
		 
		static vector<DB_Bianse*> getQueryResult() ; 
		static CCArray* getQueryResultArray() ; 
		static CCDictionary* getQueryResultDictionary() ; 
		 
		static void ExcuteQueryNoneResult(string sqlstr) ; 
		static void close() ; 
		static void releaseVector(); 
		 
	public: 
		static string dbsrcFileName ; 
		static string dbdesFileName ; 
		static string dbTableName ; 
		static const int DB_Bianse_ID = 0 ; 
		static const int DB_Bianse_desc = 1 ; 
		static const int DB_Bianse_color_r = 2 ; 
		static const int DB_Bianse_color_g = 3 ; 
		static const int DB_Bianse_color_b = 4 ; 
		static const int DB_Bianse_bianse_type = 5 ; 
		static const int DB_Bianse_color_hue = 6 ; 
		 
	public: 
		int ID ; 
		string desc ; 
		float color_r ; 
		float color_g ; 
		float color_b ; 
		int bianse_type ; 
		float color_hue ; 
}; 
 
#endif // __DB_Bianse_H__ 
