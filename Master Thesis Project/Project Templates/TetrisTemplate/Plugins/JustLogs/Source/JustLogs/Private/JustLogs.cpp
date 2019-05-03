// Copyright 1998-2017 Epic Games, Inc. All Rights Reserved.

#include "JustLogs.h"

#define LOCTEXT_NAMESPACE "FJustLogsModule"

void FJustLogsModule::StartupModule()
{
	// This code will execute after your module is loaded into memory; the exact timing is specified in the .uplugin file per-module

	UE_LOG(LogTemp, Log, TEXT("AltCC Just Logs Start Up"));

	FBlueprintEditorModule& BlueprintEditorModule = FModuleManager::LoadModuleChecked<FBlueprintEditorModule>("Kismet");
	{
		BlueprintEditorModule.OnRegisterTabsForEditor().AddRaw(this, &FJustLogsModule::OnRegisterBlueprintAlternativesTabForBlueprintEditor);

	}

	FLevelEditorModule& LevelEditorModule = FModuleManager::LoadModuleChecked<FLevelEditorModule>("LevelEditor");
	{
		//LevelEditorModule.OnRegisterTabs().AddRaw(this, &FTestPluginModule::OnRegisterLevelEditorTabs);
		//LevelEditorModule.StartPlayInEditorSession
	}

	FAssetRegistryModule& AssetRegistryModule = FModuleManager::LoadModuleChecked<FAssetRegistryModule>("AssetRegistry");
	{
		AssetRegistryModule.Get().OnAssetRenamed().AddRaw(this, &FJustLogsModule::OnAssetRenamed);
		AssetRegistryModule.Get().OnAssetAdded().AddRaw(this, &FJustLogsModule::OnAssetAdded);
		AssetRegistryModule.Get().OnAssetRemoved().AddRaw(this, &FJustLogsModule::OnAssetRemoved);
	}

	{
		//GEditor->OnLevelActorAttached
		GEditor->OnLevelActorDeleted().AddRaw(this, &FJustLogsModule::OnLevelActorDeleted);
		GEditor->OnLevelActorAdded().AddRaw(this, &FJustLogsModule::OnLevelActorAdded);
		GEditor->OnBlueprintPreCompile().AddRaw(this, &FJustLogsModule::OnBlueprintPreCompiled);
	}

	FPropertyEditorModule& PropertyEditorModule = FModuleManager::LoadModuleChecked<FPropertyEditorModule>("PropertyEditor");
	{
	}
}

void FJustLogsModule::OnGraphChanged(const struct FEdGraphEditAction& InAction)
{
	if (InAction.Action == EEdGraphActionType::GRAPHACTION_AddNode)
	{
		UE_LOG(LogTemp, Log, TEXT("AltCC Graph: %s Adding %d Nodes"), *InAction.Graph->GetName(), InAction.Nodes.Num());
		for (auto Node : InAction.Nodes)
		{
			UE_LOG(LogTemp, Log, TEXT("AltCC Graph %s Added Node: %s"), *InAction.Graph->GetName(), *Node->GetName());
		}
	}
	else if (InAction.Action == EEdGraphActionType::GRAPHACTION_RemoveNode)
	{
		UE_LOG(LogTemp, Log, TEXT("AltCC Graph: %s Removing %d Nodes"), *InAction.Graph->GetName(), InAction.Nodes.Num());
		for (auto Node : InAction.Nodes)
		{
			UE_LOG(LogTemp, Log, TEXT("AltCC Graph %s Removed Node: %s"), *InAction.Graph->GetName(), *Node->GetName());
		}
	}
	else if (InAction.Action == EEdGraphActionType::GRAPHACTION_SelectNode)
	{
		UE_LOG(LogTemp, Log, TEXT("AltCC Graph: %s Selecting %d Nodes"), *InAction.Graph->GetName(), InAction.Nodes.Num());

		
	}
	else if (InAction.Action == EEdGraphActionType::GRAPHACTION_Default)
	{
		// Will crash the engine Graph isn't valid
		//UE_LOG(LogTemp, Log, TEXT("Graph: %s Default Action"), *InAction.Graph->GetName());
	}
	else
	{
		UE_LOG(LogTemp, Log, TEXT("AltCC Graph %s Invalid Action"), *InAction.Graph->GetName());
	
		if (InAction.Nodes.Num() > 0)
		{
			UE_LOG(LogTemp, Log, TEXT("AltCC Graph: %s INVALID %d Nodes"), *InAction.Graph->GetName(), InAction.Nodes.Num());
			for (auto Node : InAction.Nodes)
			{
				UE_LOG(LogTemp, Log, TEXT("AltCC Graph %s INVALIDING Node: %s"), *InAction.Graph->GetName(), *Node->GetName());
			}
		}
	}
}

//
void FJustLogsModule::OnBlueprintPreCompiled(UBlueprint* CompiledBP)
{
	UE_LOG(LogTemp, Log, TEXT("AltCC Blueprint Compiled: %s"), *CompiledBP->GetName());
}

//
void FJustLogsModule::OnEditorClosed()
{}

void FJustLogsModule::OnRegisterBlueprintAlternativesTabForBlueprintEditor(FWorkflowAllowedTabSet & WFSet, FName BlueprintName, TSharedPtr< FBlueprintEditor > Editor)
{}

// 
void FJustLogsModule::OnAssetRenamed(const FAssetData& NewData, const FString& OldPath)
{
	UE_LOG(LogTemp, Log, TEXT("AltCC Asset Renamed from: %s to: %s"), *OldPath, *NewData.AssetName.ToString());
}

//
void FJustLogsModule::OnAssetAdded(const FAssetData& NewData)
{
	UE_LOG(LogTemp, Log, TEXT("AltCC Asset Added: %s"), *NewData.AssetName.ToString());
}

//
void FJustLogsModule::OnAssetRemoved(const FAssetData& NewData)
{
	UE_LOG(LogTemp, Log, TEXT("AltCC Asset Deleted: %s"), *NewData.AssetName.ToString());
}

void FJustLogsModule::OnLevelActorAdded(AActor* ActorAdded)
{
	UE_LOG(LogTemp, Log, TEXT("AltCC Level Actor Added: %s"), *ActorAdded->GetName());
}

void FJustLogsModule::OnLevelActorDeleted(AActor* ActorAdded)
{
	UE_LOG(LogTemp, Log, TEXT("AltCC Level Actor Deleted: %s"), *ActorAdded->GetName());
}

void FJustLogsModule::ShutdownModule()
{
	// This function may be called during shutdown to clean up your module.  For modules that support dynamic reloading,
	// we call this function before unloading the module.

	UE_LOG(LogTemp, Log, TEXT("AltCC Just Logs Shut Down"));
}

#undef LOCTEXT_NAMESPACE
	
IMPLEMENT_MODULE(FJustLogsModule, JustLogs)