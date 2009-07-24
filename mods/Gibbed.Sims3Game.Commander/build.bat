@echo off

del GibbedCommander.package
del package\Gibbed.Sims3Game.Commander.dll.s3sa
..\..\bin\Gibbed.Sims3.Sassy.exe fakesign code\bin\Release\Gibbed.Sims3Game.Commander.dll package\Gibbed.Sims3Game.Commander.dll.s3sa
..\..\bin\Gibbed.Sims3.BuildPackage.exe GibbedCommander.package package\files.xml

pause