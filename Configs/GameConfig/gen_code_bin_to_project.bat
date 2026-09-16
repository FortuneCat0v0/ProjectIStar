Cd /d %~dp0
echo %CD%

set WORKSPACE=../..
set LUBAN_DLL=%WORKSPACE%\Tools\Luban\Luban.dll
set CONF_ROOT=.
set DATA_OUTPATH=%WORKSPACE%/UnityProject/Assets/AssetRaw/Configs/bytes/
set JSON_OUTPATH=%CONF_ROOT%/Json/
set CODE_OUTPATH=%WORKSPACE%/UnityProject/Assets/GameScripts/HotFix/GameProto/GameConfig/

copy /y "%CONF_ROOT%\CustomTemplate\ConfigSystem.cs" "%WORKSPACE%\UnityProject\Assets\GameScripts\HotFix\GameProto\ConfigSystem.cs"
copy /y "%CONF_ROOT%\CustomTemplate\ExternalTypeUtil.cs" "%WORKSPACE%\UnityProject\Assets\GameScripts\HotFix\GameProto\ExternalTypeUtil.cs"

dotnet %LUBAN_DLL% ^
    -t client ^
    -c cs-bin ^
    -d bin ^
    -d json ^
    --conf %CONF_ROOT%\luban.conf ^
    -x code.lineEnding=crlf ^
    -x outputCodeDir=%CODE_OUTPATH% ^
    -x bin.outputDataDir=%DATA_OUTPATH% ^
    -x json.outputDataDir=%JSON_OUTPATH% ^
    -x json.indent=true
if not defined AI_MODE pause

