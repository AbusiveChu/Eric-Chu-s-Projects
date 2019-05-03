// Copyright 1998-2017 Epic Games, Inc. All Rights Reserved.

#pragma once

#include "LevelEditor.h"
#include "BlueprintEditorModule.h"
#include "BlueprintEditorTabs.h"
#include "Widgets/Docking/SDockTab.h"
#include "Widgets/Layout/SBox.h"
#include "Widgets/Text/STextBlock.h"
#include "Toolkits/AssetEditorManager.h"
#include "Toolkits/AssetEditorToolkit.h"

#include "CoreMinimal.h"
#include "ModuleManager.h"
#include "UnrealEd.h"
#include "Engine.h"
#include "Widgets/SWidget.h"
#include "Widgets/SCompoundWidget.h"
#include "Widgets/Views/STableRow.h"
#include "Widgets/Views/SListView.h"
#include "MultiBox/MultiBoxBuilder.h"
#include "Widgets/Layout/SScrollBorder.h"
#include "Delegates/IDelegateInstance.h"
#include "Widgets/Docking/SDockableTab.h"
#include "LayoutExtender.h"

#include "Kismet2/BlueprintEditorUtils.h"
#include "Developer/BlueprintProfiler/Public/BlueprintProfilerModule.h"
#include "WorkflowOrientedApp/WorkflowCentricApplication.h"

// Details panel files
#include "DetailCustomizations.h"
#include "Modules/ModuleInterface.h"
#include "Modules/ModuleManager.h"
#include "AssetRegistryModule.h"
#include "PropertyEditorModule.h"
#include "PropertyEditorDelegates.h"

class FJustLogsModule : public IModuleInterface
{
public:

	/** IModuleInterface implementation */
	virtual void StartupModule() override;
	virtual void ShutdownModule() override;

	void OnGraphChanged(const struct FEdGraphEditAction& InAction);

	void OnBlueprintPreCompiled(UBlueprint* CompiledBP);
	void OnEditorClosed();

	void OnRegisterBlueprintAlternativesTabForBlueprintEditor(FWorkflowAllowedTabSet & WFSet, FName BlueprintName, TSharedPtr< FBlueprintEditor > Editor);

	// 
	void OnAssetRenamed(const FAssetData& NewData, const FString& OldPath);

	//
	void OnAssetAdded(const FAssetData& NewData);

	//
	void OnAssetRemoved(const FAssetData& NewData);

	void OnLevelActorAdded(AActor* ActorAdded);

	void OnLevelActorDeleted(AActor* ActorAdded);
};