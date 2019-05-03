// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "CoreUObject.h"
#include "Engine/Engine.h"
#include "LevelEditor.h"
#include "Editor.h"
#include "Editor/EditorWidgets/Public/EditorWidgetsModule.h"
#include "Engine/GameViewportClient.h"
#include "Editor/BlueprintGraph/Classes/K2Node_Literal.h"
#include "Runtime/Core/Public/Misc/MessageDialog.h"
#include "Editor/GraphEditor/Public/SGraphPanel.h"
#include "Editor/UnrealEd/Public/EdGraphUtilities.h"
#include "Editor/UnrealEd/Public/GraphEditor.h"
#include "EdGraph/EdGraphNode.h"

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

// Property Editor Files
#include "PropertyHandle.h"
#include "PropertyEditorModule.h"
#include "PropertyEditorDelegates.h"
#include "PropertyCustomizationHelpers.h"
#include "DetailCustomizations.h"
#include "DetailLayoutBuilder.h"
#include "DetailWidgetRow.h"
#include "DetailCategoryBuilder.h"
#include "IDetailChildrenBuilder.h"
#include "IDetailCustomization.h"
#include "IDetailPropertyRow.h"

// Blueprint Files
#include "Editor/Kismet/Private/SBlueprintSubPalette.h"
#include "Editor/Kismet/Public/BlueprintEditor.h"
#include "BlueprintEditorModule.h"
#include "EdGraph/EdGraphNodeUtils.h"

#include "Kismet2/BlueprintEditorUtils.h"
#include "Kismet2/Kismet2NameValidators.h"
#include "Kismet2/KismetEditorUtilities.h"
#include "Kismet2/KismetDebugUtilities.h"
#include "Kismet2/KismetReinstanceUtilities.h"

#include "AssetRegistryModule.h"
#include "IAssetTools.h"
#include "AssetToolsModule.h"
#include "Toolkits/AssetEditorToolkit.h"
#include "Toolkits/AssetEditorManager.h"

#include "WorkflowOrientedApp/WorkflowTabFactory.h"
#include "WorkflowOrientedApp/WorkflowTabManager.h"
#include "EdGraph/EdGraphSchema.h"
#include "WorkflowOrientedApp/WorkflowUObjectDocuments.h"
#include "Engine/TimelineTemplate.h"

// Plugin Files
#include "TestPlugin/Classes/BPAltData.h"
#include "TestPlugin/Classes/AltMasterList.h"
#include "AlternativeBlueprintTabs.h"
#include "TestPlugin.h"

class FBlueprintEditor;
class SSplitter;
class SToolTip;
class UBlueprint;
class FWorkflowTabFactory;

class IDetailChildrenBuilder;

// UK2Node Types
class UK2Node_Variable;
class UK2NodeGet;
class UK2NodeSet;
class UK2Node_FunctionEntry;
class UK2Node_FucntionResult;
class UK2Node_Event;
class UK2Node_Literal;
class UK2Node_Timeline;
class UK2Node_EditablePinBase;

/**
 * Create the widget for the blueprint editor alternative menu
 */
class TESTPLUGIN_API SAlternativeView : public SCompoundWidget
{
public:
	SLATE_BEGIN_ARGS(SAlternativeView) {}
		SLATE_ARGUMENT(TSharedPtr<FBlueprintEditor>, BPEditor)
		SLATE_ARGUMENT(UObject*, BlueprintObj)
		SLATE_ARGUMENT(TArray<TSharedPtr<UBPAltData>>, AltData)
	SLATE_END_ARGS()
		
	SAlternativeView();
	~SAlternativeView();

	/**
	* Creates the slate widget that represents a list of available actions for
	* the specified blueprint.
	*
	* @param  InArgs				A set of slate arguments, defined above.
	*/
	void Construct(const FArguments& InArgs);

	/** A list of alternative data for the current set of Alternatives */
	TArray<TSharedPtr<UBPAltData>> AltData;

private:
	/**
	//// Blueprint Alternative Variables ////
	*/

	// Index of the set in the Master List SortedAlternatives
	int32 MLIndex;

	// ListView Widget for avaliable alternatives for this blueprint
	TSharedPtr<SListView<TSharedPtr<UBPAltData>>> AlternativeListView;

	// Array of Alternative Blueprint objects that are created
	TArray<UObject*> Alternatives;

	/** Current focused Blueprint */
	UObject* CurrentBPObj;
	UBlueprint* CurrentBP;

	TSharedPtr<FBlueprintEditor> BPEditor;

	// Number of Alternatives for a given blueprint
	int32 AlternativeCount = 0;

	// Sorted Index in the given set of alternatives
	int32 SortedIndex = -1;

	// Current Alternative Being Edited (Doesn't currently work as intended)
	TSharedPtr<UBPAltData> CurrentAlternative;

	// The Alterantive object that is the current selectio in the Blueprint alternative menu
	TSharedPtr<UBPAltData> SelectedAlternative;

	/**
	//// Blueprint Alternative Functions ////
	*/

	// Get alternative data from the master list, return true if the refresh is successful
	bool RefreshAltData();

	// Toolbar Button Functions
	void ToolbarSaveAlternative();	
	void ToolbarAlternativePIE();

	// @TODO: Check box to indicate if the user wants to switch actors out in the blueprint editor on PIE
	//TSharedRef<SCheckBox> BPEditorCheckBox;

	// ListView Item Functions 
	FReply LoadAlternative(int32 Index, TSharedPtr<UBPAltData> Alt);
	FReply SaveAlternative();

	// Just set the currently selected alternative
	void OnAlternativeSelectionChanged(TSharedPtr<UBPAltData> InItem, ESelectInfo::Type InSelectInfo);

	// End ListView Item Functions

	/** ListView Row Widget Generation */
	TSharedRef<ITableRow> OnGenerateAlternativeRow(TSharedPtr<UBPAltData> InItem, const TSharedRef<STableViewBase>& OwnerTable);

	/** Create Context Menu (right click menu) */
	TSharedPtr<SWidget> OnMakeAlternativeContextMenu();

	// Context Menu Functions
	void LoadAlternative(TSharedPtr<UBPAltData> Alt);
	void DeleteAlternative(TSharedPtr<UBPAltData> Alt);
	void RefreshAlternativeList();

	void OnAssetRenamed(const FAssetData& NewData, const FString& OldPath);
	// End Context Menu Functions


protected:
};

// Blueprint Alternative Details Panel Customization
class FAlternativeDetails : public IDetailCustomization
{
public:
	/**
	* Creates a new instance.
	*
	* @return A new property type customization.
	*/
	static TSharedRef<IDetailCustomization> MakeInstance()
	{
		return MakeShareable(new FAlternativeDetails());
	}

	// IDetailCustomization Interface (Do everything here)
	virtual void CustomizeDetails(IDetailLayoutBuilder& DetailBuilder) override;

	// Reference to the DetailBuilder for this object
	IDetailLayoutBuilder* BuilderRef;

	// List of the names of the alternatives for a given object
	TArray<TSharedPtr<FString>> AlternativeNames;

	// Currently Selected Item
	TSharedPtr<FString> CurrentItem;

	// Reference to the combo box which lists alternatives
	TSharedPtr<SComboBox<TSharedPtr<FString>>> AlternativesComboBox;

	// Objects being edited in the Level Editor
	TArray<TWeakObjectPtr<UObject>> Objects;

	// List of all of the alternatives for a given object
	TArray<TSharedPtr<UBPAltData>> ObjectAlts;

	void OnAlternativeSelectionChange(TSharedPtr<FString> NewValue, ESelectInfo::Type);

	void OnAlternativeComboBoxOpening();

	TSharedRef<SWidget> OnGenerateAlternativesComboBox(TSharedPtr<FString> InItem);

	FText CreateAlternativesComboBoxContent() const;

	FReply OnRegenerate(TArray<TSharedPtr<FString>> *AltNames);

	FReply OnCheckBP();
};

