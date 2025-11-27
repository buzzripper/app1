
set "SCRIPT_DIR=%~dp0"

CD "%SCRIPT_DIR%..\UI\Angular"

ng serve --ssl

::PRINT %CD_DIR%
::PAUSE