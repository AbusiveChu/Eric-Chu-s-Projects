Eric Chu 2017 Using Unreal Engine 4.15.1 Source Code
The files stored in this folder are either changed or added files

Changed Files:
Path: \Engine\Source\Editor\Kismet\Public
	BlueprintEditor.h
	BlueprintEditorTabs.h
	BlueprintActionMenuUtils.h
	
Path: \Engine\Source\Editor\Kismet\Private
	BlueprintEditor.cpp
	BlueprintEditorCommands.h/.cpp
	SBlueprintToolbar.cpp
	BlueprintTabFactories.h/.cpp
	BlueprintEditorModes.cpp
	SBlueprintPalette.h/.cpp
	BlueprintEditorTabs.cpp
	SBlueprintLibraryPalette.h/.cpp
	BlueprintActionMenuUtils.cpp


Added Files:
*Note: Adding files into the Source Code adds the files to the incorrect path even if the solution
explorer indicates otherwise (Typically in \Engine\Intermediate\ProjectFiles), make sure the files 
are in the correct path. After the files are in the correct place run GenerateProjectFiles.bat with 
Visual Studios closed. And make sure to rebuild the project.

Path: \Engine\Source\Editor\Kismet\Private
	AlternativesData.cpp
	SAlternativesView.cpp
	
Path: \Engine\Source\Editor\Kismet\Public
	AlternativesData.h
	SAlternativesView.h

Path: \Engine\Source\Editor\LevelEditor\Private
	AlternativeLevelEditor.h
	AlternativeLevelEditor.cpp