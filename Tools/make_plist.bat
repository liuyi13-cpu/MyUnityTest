@echo om

set DATA_PATH=..\projectHunter\Resources\db\
set TOOL=..\..\tools\texturePacker\bin\TexturePacker.exe

echo ********************************
echo *******  export plist  *********
echo ********************************

del /S/F/Q ..\Resources\plist\*.png > NUL
del /S/F/Q ..\Resources\plist\*.plist > NUL
del /S/F/Q ..\Resources\image\*.png > NUL
del /S/F/Q ..\Resources\image\*.jpg > NUL
del /S/F/Q ..\Resources\fonts\*.png > NUL
del /S/F/Q ..\Resources\fonts\*.fnt > NUL

call %TOOL% Effects --data ..\Resources\plist\Effects.plist --sheet ..\Resources\plist\Effects.png --format cocos2d --size-constraints AnySize --trim-mode None
call %TOOL% Effects_GongYong_01 --data ..\Resources\plist\Effects_GongYong_01.plist --sheet ..\Resources\plist\Effects_GongYong_01.png --format cocos2d --size-constraints AnySize --trim-mode None
call %TOOL% Effects_ZhuCheng_01 --data ..\Resources\plist\Effects_ZhuCheng_01.plist --sheet ..\Resources\plist\Effects_ZhuCheng_01.png --format cocos2d --size-constraints AnySize --trim-mode None

call %TOOL% gamescene --data ..\Resources\plist\gamescene.plist --sheet ..\Resources\plist\gamescene.png --format cocos2d --size-constraints AnySize --trim-mode None
call %TOOL% homescene --data ..\Resources\plist\homescene.plist --sheet ..\Resources\plist\homescene.png --format cocos2d --size-constraints AnySize --trim-mode None
call %TOOL% homescene2 --data ..\Resources\plist\homescene2.plist --sheet ..\Resources\plist\homescene2.png --format cocos2d --size-constraints AnySize --trim-mode None
call %TOOL% liangua --data ..\Resources\plist\liangua.plist --sheet ..\Resources\plist\liangua.png --format cocos2d --size-constraints AnySize --trim-mode None
call %TOOL% mainmenu --data ..\Resources\plist\mainmenu.plist --sheet ..\Resources\plist\mainmenu.png --format cocos2d --size-constraints AnySize --trim-mode None
call %TOOL% public --data ..\Resources\plist\public.plist --sheet ..\Resources\plist\public.png --format cocos2d --size-constraints AnySize --trim-mode None
call %TOOL% xuanguan --data ..\Resources\plist\xuanguan.plist --sheet ..\Resources\plist\xuanguan.png --format cocos2d --size-constraints AnySize --trim-mode None
call %TOOL% shizhuang --data ..\Resources\plist\shizhuang.plist --sheet ..\Resources\plist\shizhuang.png --format cocos2d --size-constraints AnySize --trim-mode None
call %TOOL% shenjiang --data ..\Resources\plist\shenjiang.plist --sheet ..\Resources\plist\shenjiang.png --format cocos2d --size-constraints AnySize --trim-mode None
call %TOOL% loadingscene --data ..\Resources\plist\loadingscene.plist --sheet ..\Resources\plist\loadingscene.png --format cocos2d --size-constraints AnySize --trim-mode None
call %TOOL% huodshouc --data ..\Resources\plist\huodshouc.plist --sheet ..\Resources\plist\huodshouc.png --format cocos2d --size-constraints AnySize --trim-mode None
call %TOOL% chenghao --data ..\Resources\plist\chenghao.plist --sheet ..\Resources\plist\chenghao.png --format cocos2d --size-constraints AnySize --trim-mode None
call %TOOL% bangpai --data ..\Resources\plist\bangpai.plist --sheet ..\Resources\plist\bangpai.png --format cocos2d --size-constraints AnySize --trim-mode None
call %TOOL% zhouka --data ..\Resources\plist\zhouka.plist --sheet ..\Resources\plist\zhouka.png --format cocos2d --size-constraints AnySize --trim-mode None

xcopy /y /s homegamescene ..\Resources\image
xcopy /y /s fntFont ..\Resources\fonts
rem xcopy /y /s tmp ..\Resources\plist

del /S/F/Q ..\..\..\..\projectHunter\Resources\plist\*.png > NUL
del /S/F/Q ..\..\..\..\projectHunter\Resources\plist\*.plist > NUL
del /S/F/Q ..\..\..\..\projectHunter\Resources\image\*.png > NUL
del /S/F/Q ..\..\..\..\projectHunter\Resources\image\*.jpg > NUL
del /S/F/Q ..\..\..\..\projectHunter\Resources\fonts\*.png > NUL
del /S/F/Q ..\..\..\..\projectHunter\Resources\fonts\*.fnt > NUL

xcopy /y /s ..\Resources\image ..\..\..\..\projectHunter\Resources\image
xcopy /y /s ..\Resources\plist ..\..\..\..\projectHunter\Resources\plist
xcopy /y /s ..\Resources\fonts ..\..\..\..\projectHunter\Resources\fonts

if not %ERRORLEVEL%==0 (
    goto Error
)

goto end
	
:Error
echo *** build process stopped, an error occured ***

:end
echo *** Everything is ok ***

rem pause