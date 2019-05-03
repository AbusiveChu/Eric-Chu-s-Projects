// Copyright 1998-2017 Epic Games, Inc. All Rights Reserved.

#pragma once

#include "TestPluginStyle.h"
#include "TestPluginCommands.h"
#include "LevelEditor.h"
#include "BlueprintGraphModule.h"
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

// Blueprint Alternative Files
#include "TestPlugin/Classes/BPAltData.h"
#include "SAlternativeView.h"
#include "TestPlugin/Classes/AlternativeScenarios.h"
#include "TestPlugin/Classes/AltMasterList.h"
#include "TestPlugin/Classes/SAltScenarioView.h"
#include "TestPlugin/Classes/SAltMergeView.h"

// Details panel files
#include "DetailCustomizations.h"
#include "Modules/ModuleInterface.h"
#include "Modules/ModuleManager.h"
#include "AssetRegistryModule.h"
#include "PropertyEditorModule.h"
#include "PropertyEditorDelegates.h"

class FToolBarBuilder;
class FMenuBuilder;

class FTestPluginModule : public IModuleInterface
{
public:
	/** IModuleInterface implementation */
	virtual void StartupModule() override;
	virtual void ShutdownModule() override;
	
	/** This function will be bound to Command (by default it will bring up plugin window) */
	void InvokeBPAltTab(TSharedPtr<FBlueprintEditor> Editor);

	void InvokeAltScenarioTab();

	void InvokeSelectiveMergeTab(TSharedPtr<FBlueprintEditor> Editor);

	// Doesn't do anything right now (want it to trigger when the merge ends)
	void CloseSelectiveMergeTab();

	TSharedRef<SWidget> SpawnBPAlt(TSharedPtr<FBlueprintEditor> BPEditor);

	static FORCEINLINE bool VerifyOrCreateDirectory(const FString& TestDir);

	FString FileName = FString("AltSaveFile");

	// Saving and Loading alternative data, NOTE: Selective Merge will NOT work properly after alternative data is reloaded after a crash/close
	void WriteAlternativeFile();
	void ReadAlternativeFile();

	// If the user wants to swap actors via the play button in Blueprint Editors
	static bool BPEditorSwitch;

private:

	void AddToolbarExtension(FToolBarBuilder& Builder);

	void AddLevelMenuExtension(FMenuBuilder& Builder);

	void AddLevelToolbarExtension(FToolBarBuilder& Builder);

	void AddBPMenuExtension(FMenuBuilder& Builder);

	/**
	//// Delegate Functions ////
	*/

	void OnEditorClosed();

	// Initialize the alternative details for all of the blueprints
	void OnRegisterLevelEditorTabs(TSharedPtr<FTabManager> LevelEditorTabManager);

	void OnLevelActorAdded(AActor* ActorAdded);

	void OnAlternativeSaved(const FAssetData & AlternativeSaved);

	// Update the alternatives list whenever a blueprint is compiled.
	void OnBlueprintPreCompile(UBlueprint* CompiledBP);

	void OnBlueprintReinstanced();

	void OnBlueprintCompiled();

	void OnPrePIEBegin(const bool bIsSimulating);

	void OnSelectiveMergeTabClosed(TSharedRef<SDockableTab> ClosedTab);

	// 
	void OnAssetRenamed(const FAssetData& NewData, const FString& OldPath);

	// Only applies to objects inside of the Blueprints Folder
	void AddAltPropertyToBlueprintObjects();

	void OnTabForegrounded(TSharedPtr<SDockTab> FGTab, TSharedPtr<SDockTab> BackgroundTab);
	// Adds the alternative property to ALL blueprints, VERY SLOW
	//void AddAltPropertyToAllBlueprintObjects();

	// 
	void OnRegisterBlueprintAlternativesTabForBlueprintEditor(FWorkflowAllowedTabSet & WFSet, FName BlueprintName, TSharedPtr< FBlueprintEditor > Editor);

	//
	void OnRegisterSelectiveAltMergeTabForBlueprintEditor(FWorkflowAllowedTabSet & WFSet, FName BlueprintName, TSharedPtr< FBlueprintEditor > Editor);

	// 
	void OnRegisterLayoutExtenderForBlueprintEditor(FLayoutExtender& Extender);

	// Create the widget tab for the Blueprint alternatives
	TSharedRef<class SDockTab> OnSpawnBPAltTab(const class FSpawnTabArgs& SpawnTabArgs, UObject* InBlueprint);

	// Create the widget tab for Alternative Scenarios
	TSharedRef<class SDockTab> OnSpawnAltScenTab(const class FSpawnTabArgs& SpawnTabArgs);

	// Create the widget tab for Selective Merging
	TSharedRef<class SDockTab> OnSpawnSelectiveMergeTab(const class FSpawnTabArgs& SpawnTabArgs, TSharedPtr<FBlueprintEditor> Editor);

	TSharedRef<class SDockTab> SelectiveMergeTab = SNew(SDockTab);

	TSharedPtr<FDocumentTracker> DocumentManager;

	TSharedPtr<SDockTab> ActiveTab;
	TSharedPtr<SDockTab> ForegroundTab;

	/** Handle to the registered OnTabForegrounded delegate. */
	FDelegateHandle OnTabForegroundedDelegateHandle;

	// Instance of the current Blueprint Editor
	TSharedPtr<FBlueprintEditor> BPEdInst;

	// List of all of the Blueprint editors (used for the Merge)
	TArray<TSharedPtr<FBlueprintEditor>> BPEditors;

	/** Holds the tab manager that manages the front-end's tabs. */
	TSharedPtr<FTabManager> TabManager;

	// Make sure that we can only spawn 1 selective merge tab at a time
	bool IsSelectiveMergeOpen = false;


	// Blueprint reinstancing variables used to reset some alt data on blueprint compiled
	TSharedPtr<UBPAltData> AltDataReinstance;

	int32 BaseCheck;
	int32 SortedCheck;
	// Blueprint reinstancing variables End

	// Debug Variables for Watch
	int32 BaseIndex;

	UBlueprint* BPCheck;

	TArray<TArray<TSharedPtr<UBPAltData>>> CompAltML;
	// End Debug Variables for Watch

private:
	/** Handle for tracking asset editors opening */
	FDelegateHandle AssetEditorOpenedHandle;

	TArray<FName> TestPluginTabIDs;

	TSharedPtr<class FUICommandList> PluginCommands;
};

#define LOCTEXT_NAMESPACE "AlternativeViewSummoner"
//////////////////////////////////////////////////////////////////////////
// FAlternativeViewSummoner

struct FAlternativeViewSummoner : public FWorkflowTabFactory
{
public:
	FAlternativeViewSummoner(TSharedPtr<class FAssetEditorToolkit> InHostingApp);

	virtual TSharedRef<SWidget> CreateTabBody(const FWorkflowTabSpawnInfo& Info) const override;

	virtual FText GetTabToolTipText(const FWorkflowTabSpawnInfo& Info) const override
	{
		return LOCTEXT("AltViewTooltip", "A tab to create, delete and open Blueprint Alternatives");
	}

protected:
	TWeakPtr<class FBlueprintEditor> WeakBlueprintEditorPtr;
};

#undef LOCTEXT_NAMESPACE

#define LOCTEXT_NAMESPACE "AltMergeViewSummoner"
//////////////////////////////////////////////////////////////////////////
// FAlternativeViewSummoner

struct FAltMergeViewSummoner : public FWorkflowTabFactory
{
public:
	FAltMergeViewSummoner(TSharedPtr<class FAssetEditorToolkit> InHostingApp);

	virtual TSharedRef<SWidget> CreateTabBody(const FWorkflowTabSpawnInfo& Info) const override;

	virtual FText GetTabToolTipText(const FWorkflowTabSpawnInfo& Info) const override
	{
		return LOCTEXT("AltMergeViewTooltip", "Merge selected nodes over to other alternatives of the current blueprint");
	}

protected:
	TWeakPtr<class FBlueprintEditor> WeakBlueprintEditorPtr;
};

#undef LOCTEXT_NAMESPACE