@echo off

del package\ChaosMod.dll.s3sa
..\..\bin\Gibbed.Sims3.Sassy.exe fakesign code\bin\Release\ChaosMod.dll package\ChaosMod.dll.s3sa
..\..\bin\Gibbed.Sims3.BuildPackage.exe ChaosModPainting.package package\files.xml

pause