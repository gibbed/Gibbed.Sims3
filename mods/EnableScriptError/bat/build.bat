CALL bat\makedll.bat %1
IF NOT ERRORLEVEL 0 (
  ECHO IL patching failed for version %1!
  EXIT /B 1
)

CALL bat\makepackage.bat %1
IF NOT ERRORLEVEL 0 (
  ECHO Package build failed for version %1!
  EXIT /B 1
)
