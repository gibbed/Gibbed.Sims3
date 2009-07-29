@ECHO OFF
IF NOT EXIST "package\%1\" (
  ECHO Package directory for version %1 does not exist!
  EXIT /B 1
)

IF EXIST "package\%1\SimIFace.dll.s3sa" (
  DEL "package\%1\SimIFace.dll.s3sa"
)

..\..\bin\Gibbed.Sims3.Sassy.exe fakesign "code\%1\SimIFace.dll" "package\%1\SimIFace.dll.s3sa"
..\..\bin\Gibbed.Sims3.BuildPackage.exe "enablescripterror_%1.package" "package\%1\files.xml"
