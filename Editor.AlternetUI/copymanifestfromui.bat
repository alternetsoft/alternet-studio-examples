SETLOCAL EnableDelayedExpansion
cls

setlocal

set SCRIPT_HOME=%~dp0.

set MANIFEST=%SCRIPT_HOME%\..\..\..\AlternetUI\Source\Samples\ControlsSample\app.manifest

copy /Y "%MANIFEST%" "%SCRIPT_HOME%\AllQuickStarts\app.manifest"
copy /Y "%MANIFEST%" "%SCRIPT_HOME%\Bookmarks\app.manifest"
copy /Y "%MANIFEST%" "%SCRIPT_HOME%\CodeCompletion\app.manifest"
copy /Y "%MANIFEST%" "%SCRIPT_HOME%\CodeOutlining\app.manifest"
copy /Y "%MANIFEST%" "%SCRIPT_HOME%\CodeSnippets\app.manifest"
copy /Y "%MANIFEST%" "%SCRIPT_HOME%\Customize\app.manifest"
copy /Y "%MANIFEST%" "%SCRIPT_HOME%\Gutter\app.manifest"
copy /Y "%MANIFEST%" "%SCRIPT_HOME%\HyperText\app.manifest"
copy /Y "%MANIFEST%" "%SCRIPT_HOME%\LineStyles\app.manifest"
copy /Y "%MANIFEST%" "%SCRIPT_HOME%\Margin\app.manifest"
copy /Y "%MANIFEST%" "%SCRIPT_HOME%\Miscellaneous\app.manifest"
copy /Y "%MANIFEST%" "%SCRIPT_HOME%\Parsers\AdvancedSyntaxParsing\app.manifest"
copy /Y "%MANIFEST%" "%SCRIPT_HOME%\Parsers\PowerFxSyntaxParsing\app.manifest"
copy /Y "%MANIFEST%" "%SCRIPT_HOME%\Parsers\RoslynSyntaxParsing\app.manifest"
copy /Y "%MANIFEST%" "%SCRIPT_HOME%\Parsers\SQLDOMParser\app.manifest"
copy /Y "%MANIFEST%" "%SCRIPT_HOME%\Parsers\TextMateParsing\app.manifest"
copy /Y "%MANIFEST%" "%SCRIPT_HOME%\Parsers\TypeScriptParsing\app.manifest"
copy /Y "%MANIFEST%" "%SCRIPT_HOME%\Parsers\XAMLParsing\app.manifest"
copy /Y "%MANIFEST%" "%SCRIPT_HOME%\SearchReplace\app.manifest"
copy /Y "%MANIFEST%" "%SCRIPT_HOME%\Selection\app.manifest"
copy /Y "%MANIFEST%" "%SCRIPT_HOME%\SelectionAnchors\app.manifest"
copy /Y "%MANIFEST%" "%SCRIPT_HOME%\SyntaxHighlighting\app.manifest"
copy /Y "%MANIFEST%" "%SCRIPT_HOME%\UndoRedo\app.manifest"
copy /Y "%MANIFEST%" "%SCRIPT_HOME%\VisualTheme\app.manifest"
copy /Y "%MANIFEST%" "%SCRIPT_HOME%\WordWrap\app.manifest"

endlocal