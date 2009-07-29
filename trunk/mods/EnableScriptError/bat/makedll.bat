@echo off
IF NOT EXIST "..\assemblies\%1\SimIFace.dll" (
  ECHO SimIFace.dll for version %1 does not exist!
) ELSE (
  IF NOT EXIST "code\%1" (
    mkdir "code\%1"
  ) ELSE (
    del /Q "code\%1\*"
  )
  
  ..\..\misc\il\ildasm.exe /NOBAR /OUT="code\%1\SimIFace.il" "..\assemblies\%1\SimIFace.dll"
  ECHO Disassembled SimIFace.dll for version %1!
  ECHO Attempting to patch...
  patch -d "code/%1/" < il.patch
  ECHO Assembling SimIFace.dll for version %1!
  ..\..\misc\il\ilasm.exe /RESOURCE="code\%1\SimIFace.res" /DLL /OUTPUT="code\%1\SimIFace.dll" "code\%1\SimIFace.il" > "code\%1\output.txt"
  ECHO Made SimIFace.dll for version %1!
  EXIT /B
)