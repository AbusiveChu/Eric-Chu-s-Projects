// Copyright 1998-2017 Epic Games, Inc. All Rights Reserved.

#pragma once

#include "CoreMinimal.h"
#include "Framework/Commands/Commands.h"
#include "TestPluginStyle.h"

#define LOCTEXT_NAMESPACE "AltBPCommands"

class FTestPluginCommands : public TCommands<FTestPluginCommands>
{
public:

	FTestPluginCommands()
		: TCommands<FTestPluginCommands>(TEXT("TestPlugin"), NSLOCTEXT("Contexts", "TestPlugin", "TestPlugin Plugin"), NAME_None, FTestPluginStyle::GetStyleSetName())
	{
	}

	// TCommands<> interface
	virtual void RegisterCommands() override;

public:
	TSharedPtr< FUICommandInfo > OpenBPAltWindow;
	TSharedPtr< FUICommandInfo > OpenAltScenarioWindow;
	TSharedPtr< FUICommandInfo > OpenSelectiveMergeWindow;
	TSharedPtr< FUICommandInfo > SaveAlternativeData;
	TSharedPtr< FUICommandInfo > LoadAlternativeData;
};


#undef LOCTEXT_NAMESPACE