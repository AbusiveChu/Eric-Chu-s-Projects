// Copyright 1998-2017 Epic Games, Inc. All Rights Reserved.

#pragma once

#include "CoreMinimal.h"
#include "Widgets/DeclarativeSyntaxSupport.h"
#include "Input/Reply.h"
#include "Widgets/SWidget.h"
#include "Styling/SlateBrush.h"
#include "Widgets/SBoxPanel.h"
#include "SBlueprintSubPalette.h"
#include "AlternativeData.h"

class FBlueprintEditor;
class FMenuBuilder;
class FUICommandList;
class SComboButton;

/*******************************************************************************
* SBlueprintLibraryPalette
*******************************************************************************/

class SBlueprintLibraryPalette : public SBlueprintSubPalette
{
public:
	SLATE_BEGIN_ARGS( SBlueprintLibraryPalette )
		: _UseLegacyLayout(false) {}

		SLATE_ATTRIBUTE(bool, UseLegacyLayout)
	SLATE_END_ARGS()

	/**
	 * Creates a sub-palette widget for the blueprint palette UI (this 
	 * contains a subset of the library palette, specifically the user's 
	 * favorites and most used nodes)
	 * 
	 * @param  InArgs				A set of slate arguments, defined above.
	 * @param  InBlueprintEditor	A pointer to the blueprint editor that this palette belongs to.
	 */
	void Construct(const FArguments& InArgs, TWeakPtr<FBlueprintEditor> InBlueprintEditor);

private:
	// SGraphPalette Interface
	virtual void CollectAllActions(FGraphActionListBuilderBase& OutAllActions) override;
	// End SGraphPalette Interface

	// SBlueprintSubPalette Interface
	virtual TSharedRef<SVerticalBox> ConstructHeadingWidget(FSlateBrush const* const Icon, FText const& TitleText, FText const& ToolTip) override;
	virtual void BindCommands(TSharedPtr<FUICommandList> CommandListIn) const override;
	virtual void GenerateContextMenuEntries(FMenuBuilder& MenuBuilder) const override;
	// End SBlueprintSubPalette Interface

	/**
	 * Constructs a slate widget for the class drop-down menu, which is used to
	 * select a filter class.
	 * 
	 * @return A pointer to the newly created menu widget.
	 */
	TSharedRef<SWidget> ConstructClassFilterDropdownContent();

	/**
	 * Retrieves the name of the currently selected class filter (or "All" if
	 * no class filter has been selected).
	 * 
	 * @return A string representing the selected class filter.
	 */
	FText GetFilterClassName() const;

	/**
	 * Clears the current class filter (if one is set), and refreshes the 
	 * displayed action list.
	 * 
	 * @return FReply::Handled()
	 */
	FReply ClearClassFilter();

	/**
	 * Resets the current class filter and refreshes the displayed action list.
	 * 
	 * @param  PickedClass	The new class to filter the library by.
	 */
	void OnClassPicked(UClass* PickedClass);

	/** Used to help ease the transition for users who like the old format */
	bool bUseLegacyLayout;

	/** Class we want to see content of */
	TWeakObjectPtr<UClass> FilterClass;

	/** Combo button used to choose class to filter */
	TSharedPtr<SComboButton> FilterComboButton;
};

/*******************************************************************************
* SBlueprintAlternativesLibrary
*******************************************************************************/

class SBlueprintAlternativesLibrary : public SBlueprintSubPalette
{
public:
	SLATE_BEGIN_ARGS(SBlueprintAlternativesLibrary) 
		: _UseLegacyLayout(false) {}
	SLATE_ATTRIBUTE(bool, UseLegacyLayout)
	SLATE_END_ARGS()

		//virtual ~SBlueprintAlternativesLibrary();
		/**
		* Creates a sub-palette widget for the blueprint palette UI (this
		* contains a subset of the library palette, specifically the user's
		* favorites and most used nodes)
		*
		* @param  InArgs				A set of slate arguments, defined above.
		* @param  InBlueprintEditor	A pointer to the blueprint editor that this palette belongs to.
		*/
		void Construct(const FArguments& InArgs, TWeakPtr<FBlueprintEditor> InBlueprintEditor);

private:
	

	// SGraphPalette Interface
	virtual void CollectAllActions(FGraphActionListBuilderBase& OutAllActions) override;
	// End SGraphPalette Interface

	// SBlueprintSubPalette Interface
	virtual TSharedRef<SVerticalBox> ConstructHeadingWidget(FSlateBrush const* const Icon, FText const& TitleText, FText const& ToolTip) override;
	virtual void BindCommands(TSharedPtr<FUICommandList> CommandListIn) const override;
	virtual void GenerateContextMenuEntries(FMenuBuilder& MenuBuilder) const override;
	// End SBlueprintSubPalette Interface

	/** Used to help ease the transition for users who like the old format */
	bool bUseLegacyLayout;

	
private:

	/*******************************************************************************
	* Alternative Functions
	*******************************************************************************/
	void AddAlternative();

	FReply LoadAlternative();
	FReply SaveAlternative();

	TSharedRef<ITableRow> OnGenerateAlternativeRow(TSharedPtr<FString> InItem, const TSharedRef<STableViewBase>& OwnerTable);

	void OnPickedAlternative(int AlternativeItem);

	TSharedPtr<SWidget> MakeAlternativeContextMenu();

	/*******************************************************************************
	* Alternative variables
	*******************************************************************************/

	/** The total number of Alternatives created **/
	int AlternativeCount = 2;

	TSharedPtr<FUICommandList> AltCommandList;
	//FAlternativeData NewAlt("Alternative" + AlternativeCount, "", "");
	
	TSharedPtr<FString> AltPtr1;
	TSharedPtr<FString> NewAltPtr;
	TArray<TSharedPtr<FString>> AltItems;
};

/*******************************************************************************
* SBlueprintLibraryPalette
*******************************************************************************/

//class SBlueprintAlternativesLibrary : public SBlueprintSubPalette
//{
//public:
//	SLATE_BEGIN_ARGS(SBlueprintAlternativesLibrary)
//		: _UseLegacyLayout(false) {}
//
//		SLATE_ATTRIBUTE(bool, UseLegacyLayout)
//	SLATE_END_ARGS()
//
//	/**
//	* Creates a sub-palette widget for the blueprint palette UI (this
//	* contains a subset of the library palette, specifically the user's
//	* favorites and most used nodes)
//	*
//	* @param  InArgs				A set of slate arguments, defined above.
//	* @param  InBlueprintEditor	A pointer to the blueprint editor that this palette belongs to.
//	*/
//	void Construct(const FArguments& InArgs, TWeakPtr<FBlueprintEditor> InBlueprintEditor);
//
//private:
//	void OnPickedAlternative(int AlternativeItem);
//
//	// SGraphPalette Interface
//	virtual void CollectAllActions(FGraphActionListBuilderBase& OutAllActions) override;
//	// End SGraphPalette Interface
//
//	// SBlueprintSubPalette Interface
//	virtual TSharedRef<SVerticalBox> ConstructHeadingWidget(FSlateBrush const* const Icon, FText const& TitleText, FText const& ToolTip) override;
//	virtual void BindCommands(TSharedPtr<FUICommandList> CommandListIn) const override;
//	virtual void GenerateContextMenuEntries(FMenuBuilder& MenuBuilder) const override;
//	// End SBlueprintSubPalette Interface
//
//	//Construct Alternative Buttons
//	TSharedRef<SWidget> ConstructAlternativeButtons();
//
//	FReply LoadAlternative();
//	FReply SaveAlternative();
//	
//
//	void AddAlternative(TSharedRef<SVerticalBox> InBox);
//
//	/** Used to help ease the transition for users who like the old format */
//	bool bUseLegacyLayout;
//
//	/** Class we want to see content of */
//	TWeakObjectPtr<UClass> FilterClass;
//
//	/** Combo button used to choose class to filter */
//	TSharedPtr<SComboButton> AlternativeButton;
//
//	/** The total number of Alternatives created **/
//	int AlternativeCount = 2;
//};

