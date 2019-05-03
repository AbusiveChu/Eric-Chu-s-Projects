// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Engine/Engine.h"
#include "LevelEditor.h"
#include "Editor.h"
#include "Editor/EditorWidgets/Public/EditorWidgetsModule.h"
#include "Engine/GameViewportClient.h"
#include "HAL/FileManager.h"
#include "FileHelpers.h"

// Widget Files
#include "Widgets/SBoxPanel.h"
#include "Widgets/SWidget.h"
#include "Widgets/SToolTip.h"
#include "Widgets/SOverlay.h"
#include "Widgets/SCompoundWidget.h"
#include "Widgets/DeclarativeSyntaxSupport.h"
#include "Widgets/Images/SImage.h"
#include "Widgets/Docking/SDockTab.h"

#include "Widgets/Input/SNumericEntryBox.h"
#include "Widgets/Input/SButton.h"
#include "Widgets/Input/SComboBox.h"

#include "Widgets/Layout/SSpacer.h"
#include "Widgets/Layout/SBox.h"
#include "Widgets/Layout/SScrollBorder.h"

#include "Widgets/Text/STextBlock.h"
#include "Widgets/Text/SInlineEditableTextBlock.h"

#include "SlateBasics.h"
#include "SlateOptMacros.h"
#include "EditorStyleSet.h"

// Blueprint Files
#include "Editor/Kismet/Private/SBlueprintSubPalette.h"
#include "Editor/Kismet/Public/BlueprintEditor.h"
#include "BlueprintEditorModule.h"
#include "Kismet2/BlueprintEditorUtils.h"
#include "Kismet2/KismetEditorUtilities.h"
#include "Kismet2/Kismet2NameValidators.h"

#include "Editor/Kismet/Private/SMyBlueprint.h"

#include "Editor/KismetCompiler/Public/KismetCompilerMisc.h"
#include "Editor/KismetCompiler/Public/KismetCompiler.h"

#include "EdGraph/EdGraphNodeUtils.h"
#include "Runtime/Engine/Classes/Kismet/GameplayStatics.h"
#include "AssetRegistryModule.h"
#include "IAssetTools.h"
#include "AssetToolsModule.h"
#include "Toolkits/AssetEditorToolkit.h"
#include "Toolkits/AssetEditorManager.h"
#include "EdGraphUtilities.h"
#include "EdGraph/EdGraphPin.h"
#include "SNodePanel.h"

#include "VariableSetHandler.h"

// K2 Node Files
#include "K2Node.h"
#include "EdGraphSchema_K2_Actions.h"
#include "K2Node_Event.h"
#include "K2Node_ActorBoundEvent.h"
#include "K2Node_CallFunction.h"
#include "K2Node_Variable.h"
#include "K2Node_CallFunctionOnMember.h"
#include "K2Node_CallParentFunction.h"
#include "K2Node_Tunnel.h"
#include "K2Node_Composite.h"
#include "K2Node_CustomEvent.h"
#include "K2Node_ExecutionSequence.h"
#include "K2Node_FunctionEntry.h"
#include "K2Node_FunctionResult.h"
#include "K2Node_IfThenElse.h"
#include "K2Node_Literal.h"
#include "K2Node_MacroInstance.h"
#include "K2Node_Select.h"
#include "K2Node_Switch.h"
#include "K2Node_Self.h"
#include "K2Node_SwitchInteger.h"
#include "K2Node_SwitchName.h"
#include "K2Node_Timeline.h"
#include "K2Node_VariableGet.h"
#include "K2Node_VariableSet.h"
#include "K2Node_SetFieldsInStruct.h"
#include "K2Node_GetArrayItem.h"
#include "K2Node_MakeVariable.h"
#include "EdGraphNode_Comment.h"

#include "EdGraphSchema_K2.h"

// Plugin Files
#include "TestPlugin/Classes/AltMasterList.h"
#include "TestPlugin/Classes/DelegateObject.h"
#include "TestPlugin.h"

// From blueprint merging
#include "Widgets/Input/SHyperlink.h"
#include "Framework/Commands/GenericCommands.h"
#include "Framework/Notifications/NotificationManager.h"
#include "Widgets/Notifications/SNotificationList.h"

/* 
 * Type of graph that we are editing in the blueprint
 */
namespace EBlueprintEditorGraph
{
	enum Type
	{
		None,
		Event,
		Function,
		Macro,
		UberGraphPages,
		DelegateSignature,
		IntermediateGenerated,
	};
}

/**
 * Widget class that handles the selective merging between alternatives of a blueprint.
 */
class TESTPLUGIN_API SAltMergeView : public SCompoundWidget 
{
public:
	SLATE_BEGIN_ARGS(SAltMergeView) {}
		SLATE_ARGUMENT(TSharedPtr<FBlueprintEditor>, BPEditor)
	SLATE_END_ARGS()

	SAltMergeView();
	~SAltMergeView();

	void Construct(const FArguments& InArgs);

	bool IsCurrentlyMerging() { return isCurrentlyMerging; }

	bool CanStartMerge() { return canStartMerge; }

	void RefreshLists();

private:
	TSharedRef<SWidget> OnGenerateAlternativesComboBox(TSharedPtr<FString> InItem);

	// Start the merge process
	void OnStartMerge();

	// Just a test function for debugging
	void OnTest();

	FText CreateTargetMergeComboBoxContent() const;

	// On Target Merge ComboBox Selection Changed
	void OnTargetSelectionChanged(TSharedPtr<FString> NewValue, ESelectInfo::Type);

	// 
	void ResolveMerge(UBlueprint* TargetBlueprint);

	// 
	bool CanMergeNodes() const; 

	// Seperate function to handle the creation of the nodes in the target graph
	void HandleTargetGraph();

	// Called when "Finish Merge" is clicked
	void OnFinishMerge();

	// Back up subdirectory 
	FString BackupSubDir;

	/*  
		Check to find if there is a root node
		RETURN: the root node
		if there is isn't a root node return NULL
		OPTIONAL: You can get the exact pin P that the node would be connected to the root node from
	*/
	UEdGraphNode* CheckForRootNode(UEdGraphNode* N, UEdGraphPin* P = NULL);

	//
	TArray<UEdGraphPin*> CheckForNearestAdjacentPin(UEdGraphPin* P, EEdGraphPinDirection Dir);

	//
	UEdGraphNode* CheckForVariableInTarget(UEdGraphPin* P);
	
	//
	UEdGraphNode* CheckForVariableInTarget(UEdGraphNode* N);

	//
	void AddMemberVariableFromNode(UEdGraphNode* N);

	//
	TArray<UEdGraphNode*> RootNodes;

	// Nodes that aren't connected to a root node
	TArray<UEdGraphNode*> LooseNodes;

	// Nodes that already exist in the target blueprint but are selected for the merge
	TArray<UEdGraphNode*> ExistingNodesInSelection;

	TArray<FMergePin> AdjacentMergePins;

	// Blueprint that you are starting the merge from
	UBlueprint* BaseBP;

	// Blueprint that you are going to merge to
	UBlueprint* TargetBP;

	// The specific graph that you are going to merge to
	UEdGraph* TargetGraph;

	// Blueprint editor instance for the selection we are taking from
	TSharedPtr<FBlueprintEditor> Editor;

	TSharedPtr<SBox> MainView;

	// Index of the set of alternatives
	int32 SetIndex;

	const UEdGraphSchema_K2* K2Schema = GetDefault<UEdGraphSchema_K2>();

	EBlueprintEditorGraph::Type GraphType;

	TArray<TSharedPtr<FString>> TargetNames;

	// Currently Selected Target Merge Item
	TSharedPtr<FString> TargetItem;

	// Reference to the combo box which lists alternatives
	TSharedPtr<SComboBox<TSharedPtr<FString>>> TargetMergeComboBox;

	static bool isCurrentlyMerging;

	static bool canStartMerge;

	// is the Selective merge tab open?
	static bool isOpen;
};
