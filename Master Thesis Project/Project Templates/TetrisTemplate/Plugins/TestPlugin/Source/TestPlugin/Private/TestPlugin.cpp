// Copyright 1998-2017 Epic Games, Inc. All Rights Reserved.

#include "TestPlugin.h"

#include "ILevelViewport.h"

static const FName AlternativeScenarioTabName("ScenarioTab");
static const FName TestPluginTabName("TestPluginTab");
static const FName SelectiveMergeTabName("SelectiveMergeTab");
static const FName BPEditorTabName = FName("BPAlt");

bool FTestPluginModule::BPEditorSwitch = true;

#define LOCTEXT_NAMESPACE "FTestPluginModule"

void FTestPluginModule::StartupModule()
{
	UE_LOG(LogTemp, Log, TEXT("Alternative Plugin Startup"));

	// This code will execute after your module is loaded into memory; the exact timing is specified in the .uplugin file per-module
	FTestPluginStyle::Initialize();
	FTestPluginStyle::ReloadTextures();

	FTestPluginCommands::Register();

	PluginCommands = MakeShareable(new FUICommandList);

	//ReadAlternativeFile();

	// Map a command for when the alternative scenario menu button is clicked
	PluginCommands->MapAction(
		FTestPluginCommands::Get().OpenAltScenarioWindow,
		FExecuteAction::CreateRaw(this, &FTestPluginModule::InvokeAltScenarioTab),
		FCanExecuteAction());

	PluginCommands->MapAction(
		FTestPluginCommands::Get().SaveAlternativeData,
		FExecuteAction::CreateRaw(this, &FTestPluginModule::WriteAlternativeFile),
		FCanExecuteAction());

	PluginCommands->MapAction(
		FTestPluginCommands::Get().LoadAlternativeData,
		FExecuteAction::CreateRaw(this, &FTestPluginModule::ReadAlternativeFile),
		FCanExecuteAction());

	FBlueprintEditorModule& BlueprintEditorModule = FModuleManager::LoadModuleChecked<FBlueprintEditorModule>("Kismet");
	{
		// Grab Blueprint editor from this delegate
		BlueprintEditorModule.OnRegisterTabsForEditor().AddRaw(this, &FTestPluginModule::OnRegisterBlueprintAlternativesTabForBlueprintEditor);
		
		//BlueprintEditorModule.OnRegisterTabsForEditor().AddRaw(this, &FTestPluginModule::OnRegisterSelectiveAltMergeTabForBlueprintEditor);

		BlueprintEditorModule.OnRegisterLayoutExtensions().AddRaw(this, &FTestPluginModule::OnRegisterLayoutExtenderForBlueprintEditor);

		const TSharedRef<FUICommandList> EditorCommands = BlueprintEditorModule.GetsSharedBlueprintEditorCommands();

		EditorCommands->MapAction(
			FTestPluginCommands::Get().OpenAltScenarioWindow,
			FExecuteAction::CreateRaw(this, &FTestPluginModule::InvokeAltScenarioTab),
			FCanExecuteAction());
	}

	//FBlueprintGraphModule& BlueprintGraphModule = FModuleManager::LoadModuleChecked<FBlueprintGraphModule>("BlueprintGraph"); 

	FLevelEditorModule& LevelEditorModule = FModuleManager::LoadModuleChecked<FLevelEditorModule>("LevelEditor");
	{
		LevelEditorModule.OnRegisterTabs().AddRaw(this, &FTestPluginModule::OnRegisterLevelEditorTabs);

		// Register the buttons in the level editor to spawn the ScenarioTab
		TSharedPtr<FExtender> MenuExtender = MakeShareable(new FExtender());
		MenuExtender->AddMenuExtension("WindowLayout", EExtensionHook::After, PluginCommands,
			FMenuExtensionDelegate::CreateRaw(this, &FTestPluginModule::AddLevelMenuExtension));

		LevelEditorModule.GetMenuExtensibilityManager()->AddExtender(MenuExtender);

		TSharedPtr<FExtender> ToolbarExtender = MakeShareable(new FExtender());
		ToolbarExtender->AddToolBarExtension("Settings", EExtensionHook::After, PluginCommands,
			FToolBarExtensionDelegate::CreateRaw(this, &FTestPluginModule::AddLevelToolbarExtension));

		LevelEditorModule.GetToolBarExtensibilityManager()->AddExtender(ToolbarExtender);
	}

	FAssetRegistryModule& AssetRegistryModule = FModuleManager::LoadModuleChecked<FAssetRegistryModule>("AssetRegistry");
	{
		AssetRegistryModule.Get().OnAssetRenamed().AddRaw(this, &FTestPluginModule::OnAssetRenamed);
	}

	{
		GEditor->OnLevelActorAdded().AddRaw(this, &FTestPluginModule::OnLevelActorAdded);
		GEditor->OnBlueprintPreCompile().AddRaw(this, &FTestPluginModule::OnBlueprintPreCompile);
		GEditor->OnBlueprintCompiled().AddRaw(this, &FTestPluginModule::OnBlueprintCompiled);
		GEditor->OnBlueprintReinstanced().AddRaw(this, &FTestPluginModule::OnBlueprintReinstanced);
		GEditor->OnEditorClose().AddRaw(this, &FTestPluginModule::OnEditorClosed);

		FEditorDelegates::PreBeginPIE.AddRaw(this, &FTestPluginModule::OnPrePIEBegin);
	}

	// Register tab spawner for Alternative Scenarios
	FGlobalTabmanager::Get()->RegisterTabSpawner(AlternativeScenarioTabName,
		FOnSpawnTab::CreateRaw(this, &FTestPluginModule::OnSpawnAltScenTab))
		.SetDisplayName(LOCTEXT("FAltScenarioTitle", "Alternative Scenarios"))
		.SetMenuType(ETabSpawnerMenuType::Hidden);
	
	UE_LOG(LogTemp, Warning, TEXT("Scenario Tab registered"));
}

void FTestPluginModule::OnEditorClosed()
{
	UE_LOG(LogTemp, Log, TEXT("Closed EE"));
}

void FTestPluginModule::OnLevelActorAdded(AActor* ActorAdded)
{
	AddAltPropertyToBlueprintObjects();
}

void FTestPluginModule::OnRegisterLevelEditorTabs(TSharedPtr<FTabManager> LevelEditorTabManager)
{
	AddAltPropertyToBlueprintObjects();
}

void FTestPluginModule::AddAltPropertyToBlueprintObjects()
{
	FPropertyEditorModule& PropertyModule = FModuleManager::LoadModuleChecked<FPropertyEditorModule>("PropertyEditor");

	// Get all of the assets for the project
	FAssetRegistryModule& AssetRegistryModule = FModuleManager::LoadModuleChecked<FAssetRegistryModule>("AssetRegistry");
	TArray<FAssetData> AssetData;

	// Asset registry filter to find blueprints
	FARFilter Filter;
	Filter.PackagePaths.Add("/Game/StudyBlueprints");

	AssetRegistryModule.Get().GetAssets(Filter, AssetData);
	AssetRegistryModule.Get().OnAssetAdded().AddRaw(this, &FTestPluginModule::OnAlternativeSaved);

	TArray<UObject*> BlueprintObjects;

	// Sort to see if the asset is in a blueprint
	for (auto Asset : AssetData)
	{
		if (Cast<UBlueprint>(Asset.GetAsset()))
			BlueprintObjects.Add(Asset.GetAsset());

		//else
			//UE_LOG(LogTemp, Warning, TEXT("No blueprint"));
	}

	for (auto BPObj : BlueprintObjects)
	{
		FString NameTemp = BPObj->GetName();
		NameTemp += TEXT("_C");

		PropertyModule.RegisterCustomClassLayout(
			FName(*NameTemp), FOnGetDetailCustomizationInstance::CreateStatic(&FAlternativeDetails::MakeInstance));

		//UE_LOG(LogTemp, Warning, TEXT("Added property to object: %s"), *NameTemp);
	}
}

void FTestPluginModule::OnPrePIEBegin(const bool bIsSimulating)
{
	ActiveTab = FGlobalTabmanager::Get()->GetActiveTab();

	//UE_LOG(LogTemp, Log, TEXT("Pre Play in Editor: %s"),
	//	*ActiveTab->GetTabManager()->GetOwnerTab()->GetTabLabel().ToString());

	// Find the name of the active tab owner
	FString BPName = ActiveTab->GetTabManager()->GetOwnerTab()->GetTabLabel().ToString();

	if (BPName.Contains("*"))
	{
		BPName.RemoveAt(BPName.Find("*"));
	}

	FName altTag = "BPAlternatives";
	int32 BaseIndex = -1;
	int32 SortedIndex = -1;

	bool Alt = false;
	for (int i = 0; i < AltMasterList::SharedInstance()->SortedAlternatives.Num(); i++)
	{
		for (int j = 0; j < AltMasterList::SharedInstance()->SortedAlternatives[i].Num(); j++)
		{
			//UE_LOG(LogTemp, Log, TEXT("Alt Name: %s"), *AltMasterList::SharedInstance()->SortedAlternatives[i][j].Get()->
				///AlternativeBP->GetName());
			if (AltMasterList::SharedInstance()->SortedAlternatives[i][j].Get()->
				AlternativeBP->GetName().Compare(BPName) == 0 && !Alt)
			{
				BaseIndex = i;
				SortedIndex = j;

				//UE_LOG(LogTemp, Error, TEXT("Got it"));
				Alt = true;
			}
			if (Alt)
				break;
		}

		if (Alt)
			break;
	}

	if (!Alt)
	{
		UE_LOG(LogTemp, Warning, TEXT("Not Alt BP Editor"))
		return;

	}
	TArray<AActor*> AltActors;

	FString CName;
	bool ActorCheck = false;
	for (TActorIterator<AActor> ActorItr(GWorld); ActorItr; ++ActorItr)
	{
		//UE_LOG(LogTemp, Log, TEXT("Actor: %s"), *ActorItr->GetClass()->GetName());
		for (int i = 0;i < AltMasterList::SharedInstance()->SortedAlternatives[BaseIndex].Num(); i++)
		{
			CName = AltMasterList::SharedInstance()->SortedAlternatives[BaseIndex][i]->AlternativeName;

			CName.Append("_C");

			if (ActorItr->GetClass()->GetName().Compare(CName)== 0)
			{
				AltActors.Add(*ActorItr);
				ActorCheck = true;
			}
		}
	}

	// Only switch the actors out if there is 1 instance of it (maybe change this later to be an option)
	if (ActorCheck && AltActors.Num() == 1)
	{
		BPName.Append("_C");

		UObject* ClassPackage = ANY_PACKAGE;

		UClass* c = FindObject<UClass>(ClassPackage, *BPName);
		FActorSpawnParameters SpawnInfo;

		FTransform* ActorTransform = new FTransform(AltActors[0]->GetActorTransform());

		// Create the new actor and spawn it into the persistent Slevel
		AActor* NewActor = AltActors[0]->GetWorld()->SpawnActor(c, ActorTransform, SpawnInfo);

		NewActor->EditorReplacedActor(AltActors[0]);

		FString ActorLabel = AltActors[0]->GetActorLabel();

		UE_LOG(LogTemp, Log, TEXT("Actor switched via BPEditor play button: %s"), *BPName);
		// Needs to be called before EditorDestroyActor()
		GWorld->PersistentLevel->Modify();

		// Destroy the current actor
		if (GWorld->EditorDestroyActor(AltActors[0], true))
		{
			NewActor->SetActorLabel(ActorLabel);

			// Add AlternativeBlueprint Tag to the Actor
			NewActor->Tags.Add(altTag);

			// Select the new actor
			//GEditor->SelectNone(true, true);
			//GEditor->SelectActor(NewActor, true, true);
		}
	}
	else if (AltActors.Num() != 0)
	{ 
		UE_LOG(LogTemp, Error, TEXT("Too many AltActors for in BPEditor swap: %d"), AltActors.Num());
	}
}

void FTestPluginModule::OnRegisterBlueprintAlternativesTabForBlueprintEditor(FWorkflowAllowedTabSet & WFSet, FName BlueprintName, TSharedPtr<FBlueprintEditor> Editor)
{
	if (WFSet.GetFactory(AlternativeBlueprintTabs::AltViewID).IsValid())
	{
		WFSet.UnregisterFactory(AlternativeBlueprintTabs::AltViewID);
	}
	// Register a factory for Blueprint alternatives
	WFSet.RegisterFactory(MakeShared<FAlternativeViewSummoner>(Editor));
	
	// Old Code
	/*
	if (Editor.IsValid())
	{
		// Map a command for when the menu button is clicked
		if (PluginCommands->IsActionMapped(FTestPluginCommands::Get().OpenBPAltWindow))
		{
			UE_LOG(LogTemp, Warning, TEXT("Unmap action"));
			PluginCommands->UnmapAction(FTestPluginCommands::Get().OpenBPAltWindow);
		}
		PluginCommands->MapAction(
			FTestPluginCommands::Get().OpenBPAltWindow,
			FExecuteAction::CreateRaw(this, &FTestPluginModule::InvokeBPAltTab, Editor),
			FCanExecuteAction());
	}
		

		UE_LOG(LogTemp, Warning, TEXT("Blueprint Editor Tab Manager Valid:  %s"), *Editor.Get()->GetBlueprintObj()->GetName());
	
		
		// Register Tab Spawner for Blueprint Alternatives
		if (!FGlobalTabmanager::Get()->CanSpawnTab(BPEditorTabName))
		{
			UE_LOG(LogTemp, Warning, TEXT("Register BPAlt Tab"));

			FGlobalTabmanager::Get()->RegisterTabSpawner(BPEditorTabName,
				FOnSpawnTab::CreateRaw(this, &FTestPluginModule::OnSpawnBPAltTab, Cast<UObject>(Editor.Get()->GetBlueprintObj())))
				.SetDisplayName(LOCTEXT("FBPAltTitle", "Blueprint Alternatives"))
				.SetMenuType(ETabSpawnerMenuType::Hidden);
		}
		
		// Register tab spawner for Selective Merging
		if (!FGlobalTabmanager::Get()->CanSpawnTab(SelectiveMergeTabName))
		{
			UE_LOG(LogTemp, Warning, TEXT("Register Selective Merge Tab"));

			FGlobalTabmanager::Get()->RegisterTabSpawner(SelectiveMergeTabName,
				FOnSpawnTab::CreateRaw(this, &FTestPluginModule::OnSpawnSelectiveMergeTab, Editor))
				.SetDisplayName(LOCTEXT("SAltMergeTitle", "Selective Merge"))
				.SetMenuType(ETabSpawnerMenuType::Hidden);
		}

		// Register menu and tool bar button for the Selective Merge tab
		PluginCommands->MapAction(
			FTestPluginCommands::Get().OpenSelectiveMergeWindow,
			FExecuteAction::CreateRaw(this, &FTestPluginModule::InvokeSelectiveMergeTab, Editor),
			FCanExecuteAction());

		TSharedPtr<FExtender> MenuExtender = MakeShareable(new FExtender());
		MenuExtender->AddMenuExtension("WindowLayout", EExtensionHook::After, PluginCommands,
			FMenuExtensionDelegate::CreateRaw(this, &FTestPluginModule::AddBPMenuExtension));
		
		Editor->AddMenuExtender(MenuExtender);
		
		// Register Toolbar Button for the Blueprint Alternatives tab
		TSharedPtr<FExtender> ToolbarExtender = MakeShareable(new FExtender());
		
		ToolbarExtender->AddToolBarExtension("Settings", EExtensionHook::After, PluginCommands,
			FToolBarExtensionDelegate::CreateRaw(this, &FTestPluginModule::AddToolbarExtension));
		
		Editor->AddToolbarExtender(ToolbarExtender);
		
		FText label = FText::FromString(Editor.Get()->GetBlueprintObj()->GetName());
		
		TSharedRef<FTabManager> TabManager = FGlobalTabmanager::Get()->NewTabManager(Editor.Get()->GetTabManager()->GetOwnerTab().ToSharedRef());
	
		// Check to see if there is an alternative saved for the blueprint in the editor
		bool Alt = false;
		for (int i = 0; i < AltMasterList::SharedInstance()->SortedAlternatives.Num(); i++)
		{
			for (int j = 0; j < AltMasterList::SharedInstance()->SortedAlternatives[i].Num(); j++)
			{
				// Compare the names
				if (AltMasterList::SharedInstance()->SortedAlternatives[i][j]->AlternativeBP->GetName() == Editor.Get()->GetBlueprintObj()->GetName())
				{
					// Check to see if the alterantive view is already open for a specfic blueprint editor
					TSharedRef<SAlternativeView> AltView =
						SNew(SAlternativeView)
						.BPEditor(Editor)
						.AltData(AltMasterList::SharedInstance()->SortedAlternatives[i]);

					FGlobalTabmanager::Get()->RestoreDocumentTab("BPAlt",
						FTabManager::ESearchPreference::PreferLiveTab,
						SNew(SDockTab)
						.Label(label)
						.TabRole(ETabRole::DocumentTab)
						[
							AltView
						]);

					UE_LOG(LogTemp, Warning, TEXT("WE GOT EM"));
					Alt = true;
					break;
				}
			}
			if (Alt)
			{
				break;
			}
		}

		if (!Alt)
		{
			UE_LOG(LogTemp, Warning, TEXT("First Time Alt"));

			TArray<TSharedPtr<UBPAltData>> EmptyAltData;

			FGlobalTabmanager::Get()->InsertNewDocumentTab("BPAlt",
				FTabManager::ESearchPreference::PreferLiveTab,
				SNew(SDockTab)
				.Label(label)
				.TabRole(ETabRole::DocumentTab)
				[
					SNew(SAlternativeView)
					.BPEditor(Editor)
					.AltData(EmptyAltData)
				]);

			if (Editor.Get()->GetTabManager().Get()->CanSpawnTab("BPAlt") && Editor->GetTabManager()->GetOwnerTab().IsValid())
			{
				UE_LOG(LogTemp, Warning, TEXT("We good"));

				Editor.Get()->GetTabManager().Get()->InvokeTab(BPEditorTabName);
			}
			else
				UE_LOG(LogTemp, Warning, TEXT("Cannot spawn BPAlt tab"));

		}

		if (!BPEditors.Contains(Editor))
		{
			//Editor->OnGraphEditorFocused;
			
			BPEditors.Add(Editor);
		}
	}
	else
	{
		UE_LOG(LogTemp, Warning, TEXT("Blueprint Editor TabManager is NOT Valid"));
	}
	*/
}

void FTestPluginModule::OnRegisterSelectiveAltMergeTabForBlueprintEditor(FWorkflowAllowedTabSet & WFSet, FName BlueprintName, TSharedPtr<FBlueprintEditor> Editor)
{
	if (WFSet.GetFactory(AlternativeBlueprintTabs::AltSelectiveMergeID).IsValid())
	{
		WFSet.UnregisterFactory(AlternativeBlueprintTabs::AltSelectiveMergeID);
	}

	// Register a factory for Selective Merge View
	WFSet.RegisterFactory(MakeShared<FAltMergeViewSummoner>(Editor));
}

void FTestPluginModule::OnRegisterLayoutExtenderForBlueprintEditor(FLayoutExtender& Extender)
{
	//Extender.ExtendLayout(FBlueprintEditorTabs::DetailsID, ELayoutExtensionPosition::Before, FTabManager::FTab(AlternativeBlueprintTabs::AltSelectiveMergeID, ETabState::ClosedTab));

	Extender.ExtendLayout(FBlueprintEditorTabs::DetailsID, ELayoutExtensionPosition::Before, FTabManager::FTab(AlternativeBlueprintTabs::AltViewID, ETabState::ClosedTab));
}

void FTestPluginModule::OnSelectiveMergeTabClosed(TSharedRef<SDockableTab> ClosedTab) 
{
	IsSelectiveMergeOpen = false;
}

  
void FTestPluginModule::OnAlternativeSaved(const FAssetData & AlternativeSaved)
{
	//if (AltMasterList::SharedInstance()->GetBaseIndex(AlternativeSaved.GetAsset()->GetClass()->GetName()) != -1)
	//	UE_LOG(LogTemp, Warning, TEXT("Alternative Asset Saved: %s"), *AlternativeSaved.GetAsset()->GetClass()->GetName());
}

void FTestPluginModule::OnBlueprintReinstanced()
{
	WriteAlternativeFile();
	//UE_LOG(LogTemp, Warning, TEXT("Reinst"));
}

void FTestPluginModule::OnBlueprintCompiled()
{
	// Make sure that there is an alternative for a blueprint
	/*if (BaseIndex != -1)
	{
		TArray<TArray<TSharedPtr<UBPAltData>>> TempAltML;
		TArray<TSharedPtr<UBPAltData>> TempAltArr;
		for (int i = 0; i < CompAltML.Num(); i++)
		{
			for (int j = 0; j < CompAltML[i].Num(); j++)
			{
				TempAltArr.Add(CompAltML[i][j]);
			}
			TempAltML.Add(TempAltArr);
		}
		AltMasterList::SharedInstance()->SortedAlternatives = TempAltML;

		UE_LOG(LogTemp, Warning, TEXT("Compiled"));
	}*/
}

void FTestPluginModule::OnBlueprintPreCompile(UBlueprint* CompiledBP)
{
	int32 TempBaseCheck = AltMasterList::SharedInstance()->GetBaseIndex(CompiledBP);
	int32 TempSortedCheck = AltMasterList::SharedInstance()->GetSortedIndex(CompiledBP);
	
	if (TempBaseCheck != -1 && TempSortedCheck != -1)
	{
		BaseCheck = TempBaseCheck;
		SortedCheck = TempSortedCheck;

		//UE_LOG(LogTemp, Error, TEXT("PreCompiled Blueprint: %s is an alterantive"), *CompiledBP->GetName());
		AltDataReinstance.Reset();
		AltDataReinstance = AltMasterList::SharedInstance()->SortedAlternatives[BaseCheck][SortedCheck];
	}
	else
	{
		//UE_LOG(LogTemp, Error, TEXT("PreCompiled Blueprint: %s is NOT an alterantive"), *CompiledBP->GetName());
	}

	// I'M NOT SURE WHY BUT DO NOT REMOVE THIS, YOU WON'T BE ABLE TO SAVE ALTERNATIVES OTHERWISE 
	// THIS CODE WAS SIMPLY MEAN'T FOR DEBUG PURPOSES SO I COULD KEEP TRACK OF THE MASTER LIST VIA A WATCH
	{
		CompAltML.Empty();
		for (int i = 0; i < AltMasterList::SharedInstance()->SortedAlternatives.Num(); i++)
		{
			TArray<TSharedPtr<UBPAltData>> TempDataArr;
			for (int j = 0; j < AltMasterList::SharedInstance()->SortedAlternatives[i].Num(); j++)
			{
				UBPAltData* TempData = new UBPAltData();

				TempData->ReconstructData(AltMasterList::SharedInstance()->SortedAlternatives[i][j]->AlternativeName,
					AltMasterList::SharedInstance()->SortedAlternatives[i][j]->AlternativeBP,
					AltMasterList::SharedInstance()->SortedAlternatives[i][j]->index);

				TempDataArr.Add(MakeShareable(TempData));
			}
			CompAltML.Add(TempDataArr);
		}

		BPCheck = CompiledBP;
		BaseIndex = AltMasterList::SharedInstance()->GetBaseIndex(BPCheck);
		if (BaseIndex != -1)
		{
			TArray<TArray<TSharedPtr<UBPAltData>>> PreAltML = AltMasterList::SharedInstance()->SortedAlternatives;

			//UE_LOG(LogTemp, Warning, TEXT("PreCompiled"));
		}
	}
}

// Make sure to update the alternative list when an asset is renamed
void FTestPluginModule::OnAssetRenamed(const FAssetData& NewData, const FString& OldPath)
{
	if (UBlueprint* RenamedBlueprint = Cast<UBlueprint>(NewData.GetAsset()))
	{
		int32 BaseIndex = AltMasterList::SharedInstance()->GetBaseIndex(RenamedBlueprint);
		int32 SortedIndex = AltMasterList::SharedInstance()->GetSortedIndex(RenamedBlueprint);

		if (BaseIndex != -1 || SortedIndex != -1)
		{
			TSharedPtr<UBPAltData> RenamedAltData = AltMasterList::SharedInstance()->SortedAlternatives[BaseIndex][SortedIndex];

			AltMasterList::SharedInstance()->OnRenameAlternative(RenamedAltData, RenamedBlueprint->GetName());
		}
	}
}

TSharedRef<SWidget> FTestPluginModule::SpawnBPAlt(TSharedPtr<FBlueprintEditor> BPEditor)
{
	return SNew(SAlternativeView)
		.BlueprintObj(BPEditor->GetBlueprintObj());
}

TSharedRef<SDockTab> FTestPluginModule::OnSpawnBPAltTab(const FSpawnTabArgs& SpawnTabArgs, UObject* InBlueprint)
{
	return SNew(SDockTab)
		.TabRole(ETabRole::DocumentTab)
		[
			SNew(SAlternativeView)
			.BlueprintObj(InBlueprint)
		];
}

TSharedRef<class SDockTab> FTestPluginModule::OnSpawnAltScenTab(const FSpawnTabArgs& SpawnTabArgs)
{
	return SNew(SDockTab)
		.TabRole(ETabRole::DocumentTab)
		[
			SNew(SAltScenarioView)
		];
}

TSharedRef<class SDockTab> FTestPluginModule::OnSpawnSelectiveMergeTab(const class FSpawnTabArgs& SpawnTabArgs, TSharedPtr<FBlueprintEditor> Editor)
{
	auto OnTabClosed = [this](TSharedRef<SDockTab>)
	{
		// Tab closed 
		IsSelectiveMergeOpen = false;
		UE_LOG(LogTemp, Warning, TEXT("Selective Merge Tab Closed"));
	};

	auto OnTabActivated = [this](TSharedRef<SDockTab>, ETabActivationCause)
	{
		// Tab activated
	};

	if (FGlobalTabmanager::Get()->CanSpawnTab(SelectiveMergeTabName) )
	{
		check(Editor.IsValid());

		IsSelectiveMergeOpen = true;

		TSharedRef<SAltMergeView> Content = SNew(SAltMergeView)
			.BPEditor(Editor);

		UE_LOG(LogTemp, Warning, TEXT("Create Selective Merge Tab"));

		FText label = FText::FromString("Selective Merge");

		SelectiveMergeTab =
			SNew(SDockTab)
			.TabRole(ETabRole::MajorTab)
			.Label(label)
			.OnTabClosed_Lambda(OnTabClosed)
			.OnTabActivated_Lambda(OnTabActivated)
			[
				Content
			];

		return SelectiveMergeTab;
	}
	else
	{
		UE_LOG(LogTemp, Warning, TEXT("Cannot open selective merge tab."));

		return SNew(SDockTab)
			.TabRole(ETabRole::MajorTab)
			[
				SNew(SAltMergeView)
			];
	}
}

void FTestPluginModule::ShutdownModule()
{
	// This function may be called during shutdown to clean up your module.  For modules that support dynamic reloading,
	// we call this function before unloading the module.
	FTestPluginStyle::Shutdown();

	//CloseSelectiveMergeTab();

	//WriteAlternativeFile();

	FTestPluginCommands::Unregister();
	
	FGlobalTabmanager::Get()->UnregisterTabSpawner(TestPluginTabName);
	FGlobalTabmanager::Get()->UnregisterTabSpawner(AlternativeScenarioTabName);
	FGlobalTabmanager::Get()->UnregisterTabSpawner(SelectiveMergeTabName);
}

void FTestPluginModule::InvokeBPAltTab(TSharedPtr<FBlueprintEditor> Editor)
{
	// Old Code
	/*TSharedRef<SAlternativeView> AltView = SNew(SAlternativeView)
		.BlueprintObj(Cast<UObject>(AltMasterList::SharedInstance()->SortedAlternatives[i][0]->AlternativeBP))
		.AltData(AltMasterList::SharedInstance()->SortedAlternatives[i]);

	FGlobalTabmanager::Get()->RestoreDocumentTab("BPAlt",
		FTabManager::ESearchPreference::PreferLiveTab,
		SNew(SDockTab)
		.Label(label)
		.TabRole(ETabRole::DocumentTab)
		[
			AltView
		]);*/
}

void FTestPluginModule::InvokeAltScenarioTab()
{
	if (FGlobalTabmanager::Get()->CanSpawnTab(AlternativeScenarioTabName))
	{
		FGlobalTabmanager::Get()->InvokeTab(AlternativeScenarioTabName);
	}
	else
		UE_LOG(LogTemp, Warning, TEXT("Cannot open scenario tab."));
}

void FTestPluginModule::InvokeSelectiveMergeTab(TSharedPtr<FBlueprintEditor> Editor)
{
	if (FGlobalTabmanager::Get()->CanSpawnTab(SelectiveMergeTabName))
	{
		UE_LOG(LogTemp, Warning, TEXT("Invoking selective merge tab"));
		FGlobalTabmanager::Get()->InvokeTab(SelectiveMergeTabName);
	}
	else
		UE_LOG(LogTemp, Warning, TEXT("Cannot open selective merge tab."));
}

void FTestPluginModule::CloseSelectiveMergeTab() {}

void FTestPluginModule::AddBPMenuExtension(FMenuBuilder& Builder)
{
	Builder.AddMenuEntry(FTestPluginCommands::Get().OpenBPAltWindow);
	Builder.AddMenuEntry(FTestPluginCommands::Get().OpenSelectiveMergeWindow);
}

void FTestPluginModule::AddLevelToolbarExtension(FToolBarBuilder& Builder)
{
	//Builder.AddToolBarButton(FTestPluginCommands::Get().OpenAltScenarioWindow);
	//Builder.AddToolBarButton(FTestPluginCommands::Get().SaveAlternativeData);
	Builder.AddToolBarButton(FTestPluginCommands::Get().LoadAlternativeData);
}

void FTestPluginModule::AddLevelMenuExtension(FMenuBuilder& Builder)
{
	Builder.AddMenuEntry(FTestPluginCommands::Get().OpenAltScenarioWindow);
}

void FTestPluginModule::AddToolbarExtension(FToolBarBuilder& Builder)
{
	Builder.AddToolBarButton(FTestPluginCommands::Get().OpenBPAltWindow);
}

bool FTestPluginModule::VerifyOrCreateDirectory(const FString& TestDir)
{
	// Every function call, unless the function is inline, adds a small 
	// overhead which we can avoid by creating a local variable like so.
	// But beware of making every function inline! 
	IPlatformFile& PlatformFile = FPlatformFileManager::Get().GetPlatformFile();

	// Directory Exists? 
	if (!PlatformFile.DirectoryExists(*TestDir))
	{
		PlatformFile.CreateDirectory(*TestDir);

		if (!PlatformFile.DirectoryExists(*TestDir))
		{
			return false; 
			//~~~~~~~~~~~~~~ 
		}
	}
	return true;
}

void FTestPluginModule::WriteAlternativeFile()
{
	FAssetRegistryModule& AssetRegistryModule = FModuleManager::LoadModuleChecked<FAssetRegistryModule>("AssetRegistry");
	TArray<FAssetData> AssetData;

	//UE_LOG(LogTemp, Log, TEXT("Alt Save Button Pressed"));

	FARFilter Filter;
	Filter.PackagePaths.Add("/Game/StudyBlueprints");

	AssetRegistryModule.Get().GetAssets(Filter, AssetData);

	bool AllSaveCheck = false;

	FString StringToSave;

	for (auto AltList : AltMasterList::SharedInstance()->SortedAlternatives)
	{
		// Delimiter between each set of alternative
		StringToSave.Append(FString(TEXT("--")));
		for (auto AltToSave : AltList)
		{
			// Delimiter between each alternative
			StringToSave.Append(FString(TEXT(",")));
			bool AltFound = false;
			for (auto SingleAssetData : AssetData)
			{
				// Check to see if the asset is infact an alternative that we should save
				if (AltToSave->AlternativeName.Compare(SingleAssetData.AssetName.ToString()) == 0 && Cast<UBlueprint>(SingleAssetData.GetAsset()))
				{
					// Save the blueprint name into the string
					StringToSave.Append(SingleAssetData.AssetName.ToString());

					AltFound = true;
					break;
				}
			}
			if (!AltFound)
			{
				UE_LOG(LogTemp, Log, TEXT("this is bad"));
				AllSaveCheck = true;
			}
		}
	}

	// End of file delimiter
	StringToSave.Append(TEXT("!"));

	if (!FFileHelper::SaveStringToFile(StringToSave, *(FPaths::GameContentDir() + FileName)))
	{
		UE_LOG(LogTemp, Log, TEXT("Alternative Save Failed 1"));
		return;
	}
	if (AllSaveCheck)
	{
		UE_LOG(LogTemp, Log, TEXT("Alternative Save Failed 2"));
		return;
	}

	UE_LOG(LogTemp, Log, TEXT("Alternative Save Successful"));
}

void FTestPluginModule::ReadAlternativeFile()
{
	EAppReturnType::Type LoadAltData = EAppReturnType::No;

	const FText SaveAlternativeTitle = LOCTEXT("LoadAlternativeDataTitle", "Load Alternative Data");

	// Ask the user for confirmation on the action
	LoadAltData = FMessageDialog::Open(EAppMsgType::Type::YesNo,
		FText::FromString(TEXT("Do you want to load your saved alternative data?")),
		&SaveAlternativeTitle);

	if (LoadAltData == EAppReturnType::No)
	{
		return;
	}

	FString LoadedString;

	FFileHelper FileHelper;
	FileHelper.LoadFileToString(LoadedString, *(FPaths::GameContentDir() + FileName));

	TArray<TArray<FString>> AltNames;
	int32 setIndex = 0;

	FString Left, Right, TempL, TempR;

	bool EndOfFile = false;

	// Clear the master list
	AltMasterList::SharedInstance()->ClearMasterList();

	while (true)
	{
		if (!LoadedString.Split(TEXT("--"), &Left, &Right))
		{
			break;
		}

		TArray<FString> SetNames;

		AltNames.Add(SetNames);
		
		if (Right.IsEmpty())
		{
			UE_LOG(LogTemp, Error, TEXT("Loading Failed"));
			break;
		}

		int32 AltCount = 0;
		while (true)
		{
			if (Right.Contains(TEXT("--")))
			{
				Right.Split(TEXT("--"), &TempL, &TempR);

				if (!TempL.IsEmpty())
					AltNames[setIndex].Add(TempL);

				break;
			}
			else if (Right.Contains(TEXT(",")))
			{
				Right.Split(TEXT(","), &TempL, &TempR);
				if (!TempL.IsEmpty())
					AltNames[setIndex].Add(TempL);
				
				if (TempR.IsEmpty())
				{
					break;
				}
			}
			else if (Right.Contains(TEXT("!")))
			{
				Right.Split(TEXT("!"), &TempL, &TempR);
				if (!TempL.IsEmpty())
					AltNames[setIndex].Add(TempL);

				EndOfFile = true;
				break;
			}
			AltCount++;
			Right = TempR;
		}
		if (EndOfFile)
			break;

		LoadedString = TempR;

		setIndex++;
	}

	FAssetRegistryModule& AssetRegistryModule = FModuleManager::LoadModuleChecked<FAssetRegistryModule>("AssetRegistry");
	TArray<FAssetData> AssetData;

	//UE_LOG(LogTemp, Log, TEXT("Alt Load Button Pressed"));

	FARFilter Filter;
	Filter.PackagePaths.Add("/Game/StudyBlueprints");

	AssetRegistryModule.Get().GetAssets(Filter, AssetData);

	TArray<TSharedPtr<UBPAltData>> EmptyAltData;

	TArray<UObject*> AlternativeObjects;
	
	for (int i = 0; i < AltNames.Num(); i++)
	{
		TArray<UObject*> AlternativeObjects;

		TArray<TSharedPtr<UBPAltData>> LoadedAltData;

		for (int j = 0; j < AltNames[i].Num(); j++)
		{
			for (auto Asset : AssetData)
			{
				if (AltNames[i][j] == Asset.AssetName.ToString())
				{
					UBPAltData* Data = new UBPAltData();
					if (j == 0)
					{
						int32 NewIndex = AltMasterList::SharedInstance()->SortedAlternatives.Num();

						Data->ConstructBaseData(AltNames[i][j], NewIndex, Cast<UBlueprint>(Asset.GetAsset()));
						TSharedPtr<UBPAltData> SData = MakeShareable(Data);

						AlternativeObjects.Add(Asset.GetAsset());
						LoadedAltData.Add(SData);
					}
					else
					{
						Data->ConstructData(AltNames[i][j], Cast<UBlueprint>(Asset.GetAsset()),
							AlternativeObjects, i, j, 0);

						TSharedPtr<UBPAltData> NewAlt = MakeShareable(Data);

						AlternativeObjects.Add(Asset.GetAsset());
						LoadedAltData.Add(NewAlt);
					}
					break;
				}
			}
		}
		for (int j = 0; j < LoadedAltData.Num(); j++)
		{
			LoadedAltData[j]->BlueprintAlternatives = AlternativeObjects;
			AltMasterList::SharedInstance()->AddAlternative(LoadedAltData[j]);
		}
	}

	UE_LOG(LogTemp, Log, TEXT("Alt File Load Successful"));
}

#undef LOCTEXT_NAMESPACE
	
IMPLEMENT_MODULE(FTestPluginModule, TestPlugin)

#define LOCTEXT_NAMESPACE "AlternativeViewSummonerC"

FAlternativeViewSummoner::FAlternativeViewSummoner(TSharedPtr<class FAssetEditorToolkit> InHostingApp)
	: FWorkflowTabFactory(AlternativeBlueprintTabs::AltViewID, InHostingApp)
{
	TabLabel = LOCTEXT("AltView Name", "Blueprint Alternatives");
	//TabIcon = FSlateIcon(FEditorStyle::GetStyleSetName(), "Kismet.Tabs.FindResults");

	bIsSingleton = true;

	ViewMenuDescription = LOCTEXT("BPAltMenuDescription", "Blueprint Alternatives");
	ViewMenuTooltip = LOCTEXT("BPAltMenuToolTip", "A tab to create, delete and open Blueprint Alternatives");
}

TSharedRef<SWidget> FAlternativeViewSummoner::CreateTabBody(const FWorkflowTabSpawnInfo& Info) const
{
	TSharedPtr<FBlueprintEditor> BlueprintEditorPtr = StaticCastSharedPtr<FBlueprintEditor>(HostingApp.Pin());

	// Check to see if there is an alternative saved for the blueprint in the editor
	bool Alt = false;
	for (int i = 0; i < AltMasterList::SharedInstance()->SortedAlternatives.Num(); i++)
	{
		for (int j = 0; j < AltMasterList::SharedInstance()->SortedAlternatives[i].Num(); j++)
		{
			// Compare the names
			if (AltMasterList::SharedInstance()->SortedAlternatives[i][j]->AlternativeBP->GetName() == BlueprintEditorPtr.Get()->GetBlueprintObj()->GetName())
			{
				return SNew(SAlternativeView)
					.BPEditor(BlueprintEditorPtr)
					.AltData(AltMasterList::SharedInstance()->SortedAlternatives[i]);
			}
		}
	}
	UE_LOG(LogTemp, Warning, TEXT("First Alt view for: %s"), *BlueprintEditorPtr->GetBlueprintObj()->GetName());

	return SNew(SAlternativeView).BlueprintObj(BlueprintEditorPtr->GetBlueprintObj());
}
#undef LOCTEXT_NAMESPACE

////////////////////////////////////////////////////

#define LOCTEXT_NAMESPACE "AltMergeViewSummonerC"

FAltMergeViewSummoner::FAltMergeViewSummoner(TSharedPtr<class FAssetEditorToolkit> InHostingApp)
	: FWorkflowTabFactory(AlternativeBlueprintTabs::AltSelectiveMergeID, InHostingApp)
{
	TabLabel = LOCTEXT("AltMergeView Name", "Selective Merge");

	bIsSingleton = true;

	ViewMenuDescription = LOCTEXT("AltMergeMenuDescription", "Selective Merge");
	ViewMenuTooltip = LOCTEXT("AltMergeMenuToolTip", "Merge selected nodes over to other alternatives of the current blueprint");
}

TSharedRef<SWidget> FAltMergeViewSummoner::CreateTabBody(const FWorkflowTabSpawnInfo& Info) const
{
	TSharedPtr<FBlueprintEditor> BlueprintEditorPtr = StaticCastSharedPtr<FBlueprintEditor>(HostingApp.Pin());

	UE_LOG(LogTemp, Warning, TEXT("Selective Merge opened for: %s"), *BlueprintEditorPtr->GetBlueprintObj()->GetName());

	return SNew(SAltMergeView)
		.BPEditor(BlueprintEditorPtr);
}

#undef LOCTEXT_NAMESPACE