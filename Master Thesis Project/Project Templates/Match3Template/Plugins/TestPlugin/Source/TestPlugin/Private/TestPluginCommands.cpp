// Copyright 1998-2017 Epic Games, Inc. All Rights Reserved.

#include "TestPluginCommands.h"

#define LOCTEXT_NAMESPACE "FTestPluginModule"

void FTestPluginCommands::RegisterCommands()
{
	UI_COMMAND(OpenBPAltWindow, "BPAlternatives", "Invoke the BP Alternatives Tab", EUserInterfaceActionType::Button, FInputGesture());
	UI_COMMAND(OpenAltScenarioWindow, "AltScenario", "Invoke the Alternative Scenario Tab", EUserInterfaceActionType::Button, FInputGesture());
	UI_COMMAND(OpenSelectiveMergeWindow, "SelectiveMerge", "Invoke the Selective Tab", EUserInterfaceActionType::Button, FInputGesture());
	UI_COMMAND(SaveAlternativeData, "SaveAltData", "Save all Alternative related data", EUserInterfaceActionType::Button, FInputGesture());
	UI_COMMAND(LoadAlternativeData, "LoadAltData", "Load all Alternative related data", EUserInterfaceActionType::Button, FInputGesture());
}

#undef LOCTEXT_NAMESPACE
