
set "SCRIPT_DIR=%~dp0"

CD "%SCRIPT_DIR%..\src\UI\Ultima"

ng serve --ssl --port 4201

::PRINT %CD_DIR%
::PAUSE
