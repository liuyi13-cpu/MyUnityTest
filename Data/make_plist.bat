@echo on

set TOOL=D:\work\github\MyUnityTest\Data\Tools\TexturePacker\bin\TexturePacker.exe
set OUTPATH=..\Assets\ATest\TexturePacker\
echo ********************************
echo *******  export plist  *********
echo ********************************

del /S/F/Q %OUTPATH%\*.png > NUL
del /S/F/Q %OUTPATH%\*.tpsheet > NUL

call %TOOL% Main --data %OUTPATH%\Main_{n}.tpsheet --sheet %OUTPATH%\Main_{n}.png --format unity-texture2d --size-constraints POT --multipack --trim-mode Crop

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