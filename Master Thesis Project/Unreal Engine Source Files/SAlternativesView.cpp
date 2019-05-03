// By Eric Chu 2017

#include "SAlternativesView.h"
#include "Widgets/IToolTip.h"
#include "Widgets/Layout/SSplitter.h"
#include "Modules/ModuleManager.h"
#include "UObject/UnrealType.h"
#include "Widgets/SOverlay.h"
#include "SlateOptMacros.h"
#include "Framework/Application/SlateApplication.h"
#include "EditorStyleSet.h"
#include "Components/ActorComponent.h"
#include "Engine/Blueprint.h"
#include "EdGraph/EdGraph.h"
#include "EdGraphNode_Comment.h"
#include "Components/TimelineComponent.h"
#include "Kismet2/ComponentEditorUtils.h"
#include "FileHelpers.h"
#include "EdGraphSchema_K2.h"
#include "K2Node.h"
#include "EdGraphSchema_K2_Actions.h"
#include "K2Node_CallFunction.h"
#include "K2Node_Variable.h"
#include "Engine/SCS_Node.h"
#include "Internationalization/Culture.h"
#include "BlueprintEditor.h"
#include "ScopedTransaction.h"
#include "EditorWidgetsModule.h"
#include "AssetRegistryModule.h"
#include "SMyBlueprint.h"
#include "IAssetTools.h"
#include "AssetToolsModule.h"
#include "IDocumentation.h"
#include "SBlueprintLibraryPalette.h"
#include "SBlueprintFavoritesPalette.h"
#include "BlueprintPaletteFavorites.h"
#include "AnimationStateMachineGraph.h"
#include "AnimationStateMachineSchema.h"
#include "AnimationGraph.h"
#include "AnimationStateGraph.h"
#include "AnimStateConduitNode.h"
#include "AnimationTransitionGraph.h"
#include "BlueprintActionMenuItem.h"
#include "BlueprintActionMenuUtils.h"
#include "BlueprintDragDropMenuItem.h"
#include "TutorialMetaData.h"
#include "BlueprintEditorSettings.h"
#include "SPinTypeSelector.h"
// Widget Files
#include "Kismet2/BlueprintEditorUtils.h"
#include "Kismet2/Kismet2NameValidators.h"
#include "Input/SComboBox.h"
#include "MultiBox/MultiBoxBuilder.h"
#include "Widgets/Layout/SScrollBorder.h"
#include "Widgets/Input/SButton.h"
#include "Widgets/Text/SInlineEditableTextBlock.h"
#include "Widgets/Images/SImage.h"
#include "Widgets/SToolTip.h"
#include "Widgets/Input/SCheckBox.h"
#include "Misc/MessageDialog.h"

#include "Toolkits/AssetEditorManager.h"
#define LOCTEXT_NAMESPACE "BlueprintAlternatives"

/** namespace'd to avoid collisions during unified builds */
namespace BlueprintAlternative
{
	static FString const ConfigSection("BlueprintEditor.BlueprintAlternatives");
	static FString const FavoritesHeightConfigKey("FavoritesHeightRatio");
	static FString const LibraryHeightConfigKey("LibraryHeightRatio");
}

/*******************************************************************************
* FBlueprintAlternativesLibraryCommands
*******************************************************************************/

class FBlueprintAlternativesCommands : public TCommands<FBlueprintAlternativesCommands>
{
public:
	FBlueprintAlternativesCommands() : TCommands<FBlueprintAlternativesCommands>
		("BlueprintAlternativesCommands"
			, LOCTEXT("AlternativesLibraryeContext", "Blueprint Alternatives")
			, NAME_None
			, FEditorStyle::GetStyleSetName())
	{
	}

	TSharedPtr<FUICommandInfo> RemoveAlternative;
	TSharedPtr<FUICommandInfo> LoadAlternative;
	TSharedPtr<FUICommandInfo> RefreshAlternatives;

	/** Registers context menu commands for the blueprint library palette. */
	virtual void RegisterCommands() override
	{
		UI_COMMAND(LoadAlternative, "Load Alternative", "Loads the selected Blueprint Alternative", EUserInterfaceActionType::Button, FInputChord());
		UI_COMMAND(RemoveAlternative, "Remove Alternative", "Removes this is item from the altenative list", EUserInterfaceActionType::Button, FInputChord());
		UI_COMMAND(RefreshAlternatives, "Refresh Alternative List", "Refresh the alternatives list", EUserInterfaceActionType::Button, FInputChord());
	}
};


/**
* Constructor.
*/

SBlueprintAlternatives::SBlueprintAlternatives()
{}

/**
* Destructor.
*/
SBlueprintAlternatives::~SBlueprintAlternatives()
{}

BEGIN_SLATE_FUNCTION_BUILD_OPTIMIZATION
void SBlueprintAlternatives::Construct(const FArguments& InArgs, TWeakPtr<FBlueprintEditor> InBlueprintEditor)
{
	// Create the asset discovery indicator
	FEditorWidgetsModule& EditorWidgetsModule = FModuleManager::LoadModuleChecked<FEditorWidgetsModule>("EditorWidgets");
	TSharedRef<SWidget> AssetDiscoveryIndicator = EditorWidgetsModule.CreateAssetDiscoveryIndicator(EAssetDiscoveryIndicatorScaleMode::Scale_Vertical);

	HostingApp = InBlueprintEditor;

	TSharedPtr<FBlueprintEditor> BlueprintEditorPtr = StaticCastSharedPtr<FBlueprintEditor>(HostingApp.Pin());
	CurrentBP = BlueprintEditorPtr.Get()->GetBlueprintObj();
	CurrentBPObj = Cast<UObject>(CurrentBP);
	
	// If this is the base class for the set of alternatives
	if (!CurrentBP->IsAlternative && !CurrentBP->AltListCreated)
	{
		// Create name for the base class item
		FString BaseName = CurrentBPObj->GetName();
		BaseName += FString(TEXT("(Base Class)"));

		TSharedPtr<UAlternativeData> NewDataPtr(new UAlternativeData(BaseName, NULL, NULL, true));

		// Add the Base Blueprint to the list of alternatives
		AltData.Add(NewDataPtr);
		AlternativeCount++;
		Alternatives.Add(Cast<UObject>(BlueprintEditorPtr.Get()->GetBlueprintObj()));

		CurrentBP->AltListCreated = true;
	}
	
	else
	{
		//AltData = CurrentBP->AlternativeList;
	}
	TArray<FString> someStr;
	// Save Button ToolTip
	TSharedPtr<SToolTip> AlternativeSaveButtonToolTip;
	SAssignNew(AlternativeSaveButtonToolTip, SToolTip).Text(LOCTEXT("Alternatives", "Create a new Blueprint Alternative from this alternative"));

	SAssignNew(AlternativeListView, SListView<TSharedPtr<UAlternativeData>>)
		.SelectionMode(ESelectionMode::Single)
		.ItemHeight(24.0f)
		.ListItemsSource(&AltData)
		.OnGenerateRow(this, &SBlueprintAlternatives::OnGenerateAlternativeRow)
		.OnContextMenuOpening(FOnContextMenuOpening::CreateSP(this, &SBlueprintAlternatives::MakeAlternativeContextMenu))
		.OnSelectionChanged(this, &SBlueprintAlternatives::AlternativeSelectionChange);
		;

	this->ChildSlot
		[
			SNew(SVerticalBox)
			+ SVerticalBox::Slot()
			.VAlign(VAlign_Top)
			.Padding(2.f, 2.f, 2.f, 2.f)
			.AutoHeight()
			[
				SNew(SButton)
				.OnClicked(this, &SBlueprintAlternatives::SaveAlternative)
				.ToolTip(AlternativeSaveButtonToolTip)
				[
					SNew(STextBlock).Text(LOCTEXT("Save Alternative", "Save Alternative"))
				]
			]
			+ SVerticalBox::Slot()
			.AutoHeight()
			.VAlign(VAlign_Fill)
			[
				SNew(SScrollBorder, AlternativeListView.ToSharedRef())
				[
					AlternativeListView.ToSharedRef()
				]
			]
		];
}
END_SLATE_FUNCTION_BUILD_OPTIMIZATION


TSharedPtr<SWidget> SBlueprintAlternatives::MakeAlternativeContextMenu()
{
	const bool bCloseAfterSelection = true;
	FMenuBuilder MenuBuilder(bCloseAfterSelection, TSharedPtr<FUICommandList>());

	MenuBuilder.BeginSection("MainSection");
	{
		MenuBuilder.AddMenuEntry(LOCTEXT("LoadBlueprintAlternative", "Load Alternative"), LOCTEXT("LoadBlueprintAlternative_ToolTip", "Load the selected Blueprint Alternative"),
			FSlateIcon(FEditorStyle::GetStyleSetName(), "ClassIcon.BlueprintCore"), FExecuteAction::CreateSP(this, &SBlueprintAlternatives::LoadAlt));

		MenuBuilder.AddMenuEntry(LOCTEXT("RefreshAlternativeList", "Refresh List"), LOCTEXT("RefreshAlternativeList_ToolTip", "Refresh Alternative List"),
			FSlateIcon(FEditorStyle::GetStyleSetName(), NAME_None), FExecuteAction::CreateSP(this, &SBlueprintAlternatives::RefreshAlternativeList));

		MenuBuilder.AddMenuEntry(LOCTEXT("RemoveAlternative", "Remove Alternative"), LOCTEXT("RemoveAlternative_ToolTip", "Remove Alternative"),
			FSlateIcon(FEditorStyle::GetStyleSetName(), NAME_None), FExecuteAction::CreateSP(this, &SBlueprintAlternatives::RemoveAlternative));
	}
	MenuBuilder.EndSection();

	return MenuBuilder.MakeWidget();
}

void SBlueprintAlternatives::AlternativeSelectionChange(TSharedPtr<UAlternativeData> SelectedItem, ESelectInfo::Type SelectInfo)
{
	if (SelectedItem.IsValid())
	{
		CurrentSelection = SelectedItem;
		SelectionIndex = SelectedItem->index;
	}
}

void SBlueprintAlternatives::AddAlternative()
{
	if (CurrentBPObj == nullptr)
		return;

	// Create a new name for the Alternative
	FString NewAltName = CurrentBPObj->GetName();
	NewAltName += FString(TEXT("_Alternative"));
	NewAltName += FString::FromInt(AlternativeCount - 1);

	TSharedPtr<UAlternativeData> NewDataPtr(new UAlternativeData(NewAltName, AltData[0]->OriginalBlueprint, CurrentBPObj, false));
	NewDataPtr->index = AltData.Num();

	// Add a new item to AltData
	AltData.Add(NewDataPtr);
	// Refresh the list view
	RefreshAlternativeList();
	
	MasterList.AddAlternative(NewDataPtr);

	// Create new blueprint object that the Alternative will be stored in
	UObject* AlternativeBlueprint = DuplicateObject(CurrentBPObj, NULL, FName(*NewAltName));
	Cast<UBlueprint>(AlternativeBlueprint)->IsAlternative = true;
	Alternatives.Add(AlternativeBlueprint);

	UE_LOG(LogTemp, Warning, TEXT("ALTERNATIVE CREATED"));
}

TSharedRef<ITableRow> SBlueprintAlternatives::OnGenerateAlternativeRow(TSharedPtr<UAlternativeData> InItem, const TSharedRef<STableViewBase>& OwnerTable)
{
	return
		SNew(STableRow<TSharedRef<UAlternativeData>>, OwnerTable)
		.ShowSelection(true)
		[
			
			SNew(SHorizontalBox)
			+ SHorizontalBox::Slot()
				.VAlign(VAlign_Fill)
				.HAlign(HAlign_Left)
				.FillWidth(1.0f)
				[
					SNew(STextBlock).Text(FText::FromString(InItem->AlternativeName))
						.OnDoubleClicked(this, &SBlueprintAlternatives::LoadAlternative, InItem->index)
				]
			+ SHorizontalBox::Slot()
				.VAlign(VAlign_Fill)
				.HAlign(HAlign_Right)
				[
					SNew(SCheckBox)
						.Visibility(this, &SBlueprintAlternatives::IsVisible)
						.IsChecked(this, &SBlueprintAlternatives::SetParallelEditingState)
						.OnCheckStateChanged(this, &SBlueprintAlternatives::OnCheckBoxToggled)
				]
		];
}

void SBlueprintAlternatives::OnCheckBoxToggled(ECheckBoxState InNewState)
{
	if (InNewState == ECheckBoxState::Checked)
	{
		AltData[AlternativeCount - 1]->ParallelEditingEnabled = true;
	}
	else
	{
		AltData[AlternativeCount - 1]->ParallelEditingEnabled = false;
	}
}

ECheckBoxState SBlueprintAlternatives::SetParallelEditingState() const
{
	ECheckBoxState ParallelEditingState = ECheckBoxState::Unchecked;

	if (AltData[AlternativeCount - 1].IsValid())
	{
		ParallelEditingState = AltData[AlternativeCount - 1]->ParallelEditingEnabled ? ECheckBoxState::Checked : ECheckBoxState::Unchecked;
	}

	return ECheckBoxState::Unchecked;
}

EVisibility SBlueprintAlternatives::IsVisible() const
{
	return EVisibility::Visible;
}

void SBlueprintAlternatives::LoadAlt()
{
	FAssetEditorManager::Get().OpenEditorForAsset(Alternatives[SelectionIndex]);

	UE_LOG(LogTemp, Warning, TEXT("CONTEXT MENU LOAD WORKED"));
}

void SBlueprintAlternatives::RemoveAlternative()
{
	// Need to check if an Alternative Blueprint is open and prompt for action.
	bool AlternativeIsClosed = true;

	if (AlternativeIsClosed)
	{
		AltData.RemoveAt(SelectionIndex, 1, false);
	}
}

FReply SBlueprintAlternatives::LoadAlternative(int32 Index)
{
	FAssetEditorManager::Get().OpenEditorForAsset(Alternatives[Index]);

	UE_LOG(LogTemp, Warning, TEXT("LOAD WORKED"));
	return FReply::Handled();
}

FReply SBlueprintAlternatives::SaveAlternative()
{
	AlternativeCount++;
	
	AddAlternative();

	UE_LOG(LogTemp, Warning, TEXT("SAVE WORKED"));
	return FReply::Handled();
}

const TArray<TSharedPtr<UAlternativeData>>& SBlueprintAlternatives::GetAllAlternatives()
{
	return MasterList.MasterList;
}

void SBlueprintAlternatives::RefreshAlternativeList()
{
	AlternativeListView->RequestListRefresh();
}