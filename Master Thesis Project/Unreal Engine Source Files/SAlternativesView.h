#pragma once

#include "CoreMinimal.h"
#include "Widgets/DeclarativeSyntaxSupport.h"
#include "Widgets/SWidget.h"
#include "Widgets/SCompoundWidget.h"
#include "Widgets/Views/STableRow.h"
#include "Widgets/Views/SListView.h"
#include "EdGraph/EdGraphNodeUtils.h"
#include "Toolkits/AssetEditorToolkit.h"
#include "AlternativeData.h"
#include "SBlueprintSubPalette.h"

class FBlueprintEditor;
class SSplitter;
class SToolTip;
class UBlueprint;

/*******************************************************************************
* SBlueprintAlternatives
*******************************************************************************/

class KISMET_API SBlueprintAlternatives : public SCompoundWidget
{
public:
	SLATE_BEGIN_ARGS(SBlueprintAlternatives) {}
	SLATE_END_ARGS()

		/**
		* Creates the slate widget that represents a list of available actions for
		* the specified blueprint.
		*
		* @param  InArgs				A set of slate arguments, defined above.
		* @param  InBlueprintEditor	A pointer to the blueprint editor that this tab belongs to.
		*/
		void Construct(const FArguments& InArgs, TWeakPtr<FBlueprintEditor> InBlueprintEditor);

public:
	/**
	* Constructor.
	*/
	SBlueprintAlternatives();

	/**
	* Destructor.
	*/
	~SBlueprintAlternatives();
private:
	// ListView Row Widget Generation
	TSharedRef<ITableRow> OnGenerateAlternativeRow(TSharedPtr<UAlternativeData> InItem, const TSharedRef<STableViewBase>& OwnerTable);

	// Create Context Menu (right click menu)
	TSharedPtr<SWidget> MakeAlternativeContextMenu();

	// ListView Widget
	TSharedPtr<SListView<TSharedPtr<UAlternativeData>>> AlternativeListView;

	// Context Menu Functions
	void RemoveAlternative();
	void LoadAlt();
	// End Context Menu Functions

	// ListView Item Functions 
	FReply LoadAlternative(int32 Index);
	FReply SaveAlternative();
	// End ListView Item Functions

	// Fires when a selection is made in the list view
	void AlternativeSelectionChange(TSharedPtr<UAlternativeData> SelectedItem, ESelectInfo::Type SelectInfo);

	// For parallel editing (not finsihed)
	ECheckBoxState SetParallelEditingState() const;
	EVisibility IsVisible() const;
	void OnCheckBoxToggled(ECheckBoxState InNewState);

	// Master list of all of the alternatives created
	static AlternativeMasterList MasterList;

	// A list of alternative data
	TArray<TSharedPtr<UAlternativeData>> AltData;

	// Number of Alternatives for a given blueprint
	int32 AlternativeCount = 0;

	// The index of currently selected Blueprint Alternative Item
	int32 SelectionIndex= -1;

	// Item data that is currently selecting in the ListView
	TSharedPtr<UAlternativeData> CurrentSelection;

	/** Add a new alternative object to the list, to update the listview */
	void AddAlternative();

	/** Current focused Blueprint */
	UObject* CurrentBPObj;
	UBlueprint* CurrentBP;

	// Array of Alternative Blueprint objects that are created
	TArray<UObject*> Alternatives;

	// Used to get the current blueprint object
	TWeakPtr<FAssetEditorToolkit> HostingApp;

	const TArray<TSharedPtr<UAlternativeData>>& GetAllAlternatives();

protected:
	//Refresh the Alternatives list on the next tick
	void RefreshAlternativeList();
};