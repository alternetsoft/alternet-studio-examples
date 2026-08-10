SETLOCAL EnableDelayedExpansion
cls

setlocal

set SCRIPT_HOME=%~dp0.

set MANIFEST=%SCRIPT_HOME%\..\..\..\..\AlternetUI\Source\Samples\ControlsSample\app.manifest

copy /Y "%MANIFEST%" "%SCRIPT_HOME%\CallMethod\app.manifest"
copy /Y "%MANIFEST%" "%SCRIPT_HOME%\CallMethod.Python\app.manifest"
copy /Y "%MANIFEST%" "%SCRIPT_HOME%\CallMethod.TypeScript\app.manifest"
copy /Y "%MANIFEST%" "%SCRIPT_HOME%\CustomAssembly\app.manifest"
copy /Y "%MANIFEST%" "%SCRIPT_HOME%\CustomAssembly.Python\app.manifest"
copy /Y "%MANIFEST%" "%SCRIPT_HOME%\Debugger\app.manifest"
copy /Y "%MANIFEST%" "%SCRIPT_HOME%\ExpressionEvaluation\app.manifest"
copy /Y "%MANIFEST%" "%SCRIPT_HOME%\ExpressionEvaluation.Python\app.manifest"
copy /Y "%MANIFEST%" "%SCRIPT_HOME%\IsolatedScript\app.manifest"
copy /Y "%MANIFEST%" "%SCRIPT_HOME%\IsolatedScript.Python\app.manifest"
copy /Y "%MANIFEST%" "%SCRIPT_HOME%\MemoryAssembly\app.manifest"
copy /Y "%MANIFEST%" "%SCRIPT_HOME%\MemoryAssembly.Python\app.manifest"
copy /Y "%MANIFEST%" "%SCRIPT_HOME%\Mini.TypeScript\app.manifest"
copy /Y "%MANIFEST%" "%SCRIPT_HOME%\ObjectReference\app.manifest"
copy /Y "%MANIFEST%" "%SCRIPT_HOME%\ObjectReference.Python\app.manifest"
copy /Y "%MANIFEST%" "%SCRIPT_HOME%\ObjectReference.TypeScript\app.manifest"
copy /Y "%MANIFEST%" "%SCRIPT_HOME%\PackageReference\app.manifest"
copy /Y "%MANIFEST%" "%SCRIPT_HOME%\ScriptHostObject\app.manifest"
copy /Y "%MANIFEST%" "%SCRIPT_HOME%\Threading\app.manifest"
copy /Y "%MANIFEST%" "%SCRIPT_HOME%\Threading.Python\app.manifest"

endlocal