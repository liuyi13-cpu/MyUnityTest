@echo on

set TOOL=Tools\TexturePacker\bin\TexturePacker.exe
set OUTPATH=..\Assets\ATest\TexturePacker
echo ********************************
echo *******  export plist  *********
echo ********************************


rem set DATA_IN_PATH=Main,MOS_bullet
set DATA_IN_PATH=Main
for %%i in (%DATA_IN_PATH%) do (
	rem del /S/F/Q /AH %OUTPATH%\%DATA_IN_PATH%.png.meta > NUL
	rem del /S/F/Q %OUTPATH%\%DATA_IN_PATH%.tpsheet > NUL
	call %TOOL% %DATA_IN_PATH% --data %OUTPATH%\%DATA_IN_PATH%.tpsheet --sheet %OUTPATH%\%DATA_IN_PATH%.png --format unity-texture2d --size-constraints POT --multipack --algorithm MaxRects --trim-mode None
	rem call %TOOL% Main --data %OUTPATH%\Main_{n}.tpsheet --sheet %OUTPATH%\Main_{n}.png --format unity-texture2d --size-constraints POT --multipack --algorithm MaxRects --trim-mode None
)


rem xcopy /y /s homegamescene ..\Resources\image

rem del /S/F/Q ..\..\..\..\projectHunter\Resources\plist\*.png > NUL
rem del /S/F/Q ..\..\..\..\projectHunter\Resources\plist\*.plist > NUL

rem xcopy /y /s ..\Resources\image ..\..\..\..\projectHunter\Resources\image
rem xcopy /y /s ..\Resources\plist ..\..\..\..\projectHunter\Resources\plist


if not %ERRORLEVEL%==0 (
    goto Error
)

goto end
	
:Error
echo *** build process stopped, an error occured ***

:end
echo *** Everything is ok ***

rem pause