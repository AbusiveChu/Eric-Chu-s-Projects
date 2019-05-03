// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Engine/Engine.h"
#include "LevelEditor.h"
#include "Editor.h"
#include "Editor/EditorWidgets/Public/EditorWidgetsModule.h"
#include "Engine/GameViewportClient.h"

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

// Blueprint Files
#include "Editor/Kismet/Private/SBlueprintSubPalette.h"
#include "Editor/Kismet/Public/BlueprintEditor.h"
#include "BlueprintEditorModule.h"
#include "Kismet2/BlueprintEditorUtils.h"
#include "Kismet2/Kismet2NameValidators.h"
#include "EdGraph/EdGraphNodeUtils.h"
#include "Runtime/Engine/Classes/Kismet/GameplayStatics.h"
#include "AssetRegistryModule.h"
#include "IAssetTools.h"
#include "AssetToolsModule.h"
#include "Toolkits/AssetEditorToolkit.h"
#include "Toolkits/AssetEditorManager.h"

// Plugin Files
#include "TestPlugin/Classes/AltMasterList.h"
#include "TestPlugin/Classes/DelegateObject.h"

DECLARE_DELEGATE_OneParam(FOnScenarioActorDestroyed, AActor*);
/**
 * blah blah blah something something scernarios
 */
class TESTPLUGIN_API SAltScenarioView : public SCompoundWidget
{
public:
	FOnScenarioActorDestroyed OnScenarioActorDestroyed;
public:
	SLATE_BEGIN_ARGS(SAltScenarioView) {}
	//SLATE_ARGUMENT()
	SLATE_END_ARGS()

	SAltScenarioView();
	~SAltScenarioView();

	void Construct(const FArguments& InArgs);

private:

	// Listview to show all of the scenarios
	TSharedPtr<SListView<TSharedPtr<AlternativeScenario>>> ScenariosListView;

	FReply SaveScenario();
	FReply LoadScenario(int32 index, TSharedPtr<AlternativeScenario> Scenario);

	/** ListView Row Widget Generation */
	TSharedRef<ITableRow> OnGenerateScenarioRow(TSharedPtr<AlternativeScenario> InItem, const TSharedRef<STableViewBase>& OwnerTable);
};
