// Fill out your copyright notice in the Description page of Project Settings.

#include "SAlternativeView.h"

const FName AlternativesID = TEXT("BlueprintAlternatives");

#define LOCTEXT_NAMESPACE "AlternativeView"

// DOESN'T SERVE A PURPOSE RIGHT NOW
struct FAltMergeGraphRowEntry
{
	FText Label;

	FName GraphName;

	UEdGraphNode* LocalNode;
	UEdGraphNode* BaseNode;
	UEdGraphNode* RemoteNode;

	UEdGraphPin* LocalPin;
	UEdGraphPin* BasePin;
	UEdGraphPin* RemotePin;

	FLinearColor DisplayColor;

	bool bHasConflicts;
};


SAlternativeView::SAlternativeView() {}

SAlternativeView::~SAlternativeView() {}


BEGIN_SLATE_FUNCTION_BUILD_OPTIMIZATION	 
void SAlternativeView::Construct(const FArguments& InArgs)
{
	// Create the asset discovery indicator
	FEditorWidgetsModule& EditorWidgetsModule = FModuleManager::LoadModuleChecked<FEditorWidgetsModule>("EditorWidgets");
	TSharedRef<SWidget> AssetDiscoveryIndicator = EditorWidgetsModule.CreateAssetDiscoveryIndicator(EAssetDiscoveryIndicatorScaleMode::Scale_Vertical);

	FAssetRegistryModule& AssetRegistryModule = FModuleManager::LoadModuleChecked<FAssetRegistryModule>("AssetRegistry");
	AssetRegistryModule.Get().OnAssetRenamed().AddRaw(this, &SAlternativeView::OnAssetRenamed);

	BPEditor.Reset();
	if (InArgs._BPEditor.IsValid())
	{
		BPEditor = InArgs._BPEditor;
		CurrentBPObj = Cast<UObject>(InArgs._BPEditor->GetBlueprintObj());
	}

	else
		CurrentBPObj = InArgs._BlueprintObj;

	CurrentBP = Cast<UBlueprint>(CurrentBPObj);
	
	AltData = InArgs._AltData;

	// If there is no existing AltData
	if (AltData.Num() == 0)
	{
		// Create a "Base Class" that the alternatives originate from
		FString BaseName = CurrentBPObj->GetName();
		//BaseName += FString(TEXT("(Base Class)"));
		UBPAltData* Data = new UBPAltData();
		int32 NewIndex = AltMasterList::SharedInstance()->SortedAlternatives.Num();
		Data->ConstructBaseData(BaseName, NewIndex, CurrentBP);
		TSharedPtr<UBPAltData> SData = MakeShareable(Data);

		// Add alternative data to the lists
		AltData.Add(SData);
		Alternatives.Add(CurrentBPObj);
		AltMasterList::SharedInstance()->AddAlternative(SData);

		// End Creating "Base Class"
	}
	else
	{
		Alternatives = AltData[0]->BlueprintAlternatives;
	}

	if (AltMasterList::SharedInstance()->SortedAlternatives.Num() != 0)
	{
		MLIndex = AltMasterList::SharedInstance()->GetBaseIndex(AltData[AltData.Num() - 1]->AlternativeBP);
		SortedIndex = AltMasterList::SharedInstance()->GetSortedIndex(CurrentBP);
	}

	// Add ToolBar to the widget
	FToolBarBuilder ToolbarBuilder(TSharedPtr< FUICommandList >(), FMultiBoxCustomization::None);

	// Save Alternative Button
	ToolbarBuilder.AddToolBarButton(
		FUIAction(
			FExecuteAction::CreateSP(this, &SAlternativeView::ToolbarSaveAlternative),
			FIsActionChecked())
		, NAME_None
		, LOCTEXT("SaveAlternativeLabel", "Save Alternative")
		, LOCTEXT("SaveAlternativeTooltip", "Save a copy of the current blueprint into an alternative.")
		, FSlateIcon(FEditorStyle::GetStyleSetName(), "BlueprintMerge.Finish")
	);

	//SAssignNew()

	//ToolbarBuilder.AddWidget();
	//ToolbarBuilder.AddToolBarButton(
	//	FUIAction(
	//		FExecuteAction::CreateSP(this, &SAlternativeView::ToolbarAlternativePIE),
	//		FIsActionChecked())
	//	, NAME_None
	//	, LOCTEXT("AltPlayLabel", "Alternative Play")
	//	, LOCTEXT("AltPlayTooltip", "Play in editor using the current alternative you are editing")
	//	, FSlateIcon(FEditorStyle::GetStyleSetName(), "BlueprintMerge.Finish")
	//);

	// Save Button ToolTip
	TSharedPtr<SToolTip> AlternativeSaveButtonToolTip;
	SAssignNew(AlternativeSaveButtonToolTip, SToolTip)
		.Text(LOCTEXT("AlternativeSaveButtonToolTip", "Save a copy of the current blueprint into an alternative."));

	// Save Button Display Text
	FText SaveButtonText = FText(LOCTEXT("SaveButtonText", "Save Alternative"));

	// Create new list view that will contain all of the alternatives for this blueprint
	SAssignNew(AlternativeListView, SListView<TSharedPtr<UBPAltData>>)
		.SelectionMode(ESelectionMode::Single)
		.ItemHeight(24.0f)
		.ListItemsSource(&AltMasterList::SharedInstance()->SortedAlternatives[MLIndex])
		.OnContextMenuOpening(this, &SAlternativeView::OnMakeAlternativeContextMenu)
		.OnGenerateRow(this, &SAlternativeView::OnGenerateAlternativeRow)
		.OnSelectionChanged(this, &SAlternativeView::OnAlternativeSelectionChanged);

	// Create the tab Widget
	this->ChildSlot
		[
			SNew(SVerticalBox)
			+SVerticalBox::Slot()
			.AutoHeight()
			[
				SNew(SHorizontalBox)
				+ SHorizontalBox::Slot()
				.AutoWidth()
				.Padding(2.f)
				[
					ToolbarBuilder.MakeWidget()
				]
			]
			+ SVerticalBox::Slot()
			.AutoHeight()
			.VAlign(VAlign_Fill)
			[
				// Create a ListView of Alternatives
				SNew(SScrollBorder, AlternativeListView.ToSharedRef())
				[
					AlternativeListView.ToSharedRef()
				]
			]
		];
}

END_SLATE_FUNCTION_BUILD_OPTIMIZATION

TSharedRef<ITableRow> SAlternativeView::OnGenerateAlternativeRow(TSharedPtr<UBPAltData> InItem, const TSharedRef<STableViewBase>& OwnerTable)
{
	// Save Button ToolTip
	TSharedPtr<SToolTip> AlternativeRowToolTip;
	SAssignNew(AlternativeRowToolTip, SToolTip)
		.Text(LOCTEXT("AlternativeRowToolTip", "Double Click to open this Blueprint Alternative"));

	return
		SNew(STableRow<TSharedRef<UBPAltData>>, OwnerTable)
		.ShowSelection(true)
		[
			SNew(SHorizontalBox)
			+ SHorizontalBox::Slot()
			.VAlign(VAlign_Fill)
			.HAlign(HAlign_Left)
			.FillWidth(1.0f)
			[
				SNew(STextBlock).Text(FText::FromString(InItem->AlternativeName))
				.OnDoubleClicked(this, &SAlternativeView::LoadAlternative, InItem->sIndex, InItem)
				.ToolTip(AlternativeRowToolTip)
			]
		];
}

bool SAlternativeView::RefreshAltData()
{
	if (AltMasterList::SharedInstance()->SortedAlternatives.Num() > 0)
	{
		AltData = AltMasterList::SharedInstance()->SortedAlternatives[0];

		return true;
	}
	return false;
}

FReply SAlternativeView::LoadAlternative(int32 Index, TSharedPtr<UBPAltData> Alt)
{
	CurrentAlternative = Alt;

	Alternatives = AltMasterList::SharedInstance()->SortedAlternatives[MLIndex]
		[AltMasterList::SharedInstance()->SortedAlternatives[MLIndex].Num() - 1]->BlueprintAlternatives;

	TArray<TArray<TSharedPtr<UBPAltData>>> SortedAlternativesTest = AltMasterList::SharedInstance()->SortedAlternatives;

	FAssetEditorManager::Get().OpenEditorForAsset(Alt->AlternativeBP);

	UE_LOG(LogTemp, Warning, TEXT("Alternative %s Opened"), *Alt->AlternativeName);
	return FReply::Handled();
}

void SAlternativeView::LoadAlternative(TSharedPtr<UBPAltData> Alt) 
{
	CurrentAlternative = Alt;

	Alternatives = AltMasterList::SharedInstance()->SortedAlternatives[MLIndex]
		[AltMasterList::SharedInstance()->SortedAlternatives[MLIndex].Num() - 1]->BlueprintAlternatives;

	TArray<TArray<TSharedPtr<UBPAltData>>> SortedAlternativesTest = AltMasterList::SharedInstance()->SortedAlternatives;

	FAssetEditorManager::Get().OpenEditorForAsset(Alt->AlternativeBP);

	UE_LOG(LogTemp, Warning, TEXT("Alternative %s Opened"), *Alt->AlternativeName);
}

// Not used anymore
FReply SAlternativeView::SaveAlternative()
{
	AlternativeCount = AltMasterList::SharedInstance()->SortedAlternatives[MLIndex].Num();

	FString NewAltName = AltData[0]->AlternativeBP->GetName();
	NewAltName += FString(TEXT("_Alternative "));
	NewAltName += FString::FromInt(AlternativeCount);

	// Make new blueprint object
	UObject* AlternativeBlueprint = DuplicateObject(CurrentBPObj, CurrentBPObj->GetOuter(), FName(*NewAltName));

	Alternatives = AltMasterList::SharedInstance()->SortedAlternatives[MLIndex]
		[AltMasterList::SharedInstance()->SortedAlternatives[MLIndex].Num() - 1]->BlueprintAlternatives;

	UBPAltData* Data = new UBPAltData();
	Alternatives.Add(AlternativeBlueprint);
	Data->ConstructData(NewAltName, Cast<UBlueprint>(AlternativeBlueprint),
		 Alternatives, MLIndex, AlternativeCount, SortedIndex);

	TSharedPtr<UBPAltData> SData = MakeShareable(Data);

	// Add the Base Blueprint to the list of alternatives
	AltMasterList::SharedInstance()->AddAlternative(SData);
	
	AltData.Add(SData);

	FAssetRegistryModule& AssetRegistryModule = FModuleManager::LoadModuleChecked<FAssetRegistryModule>("AssetRegistry");
	AssetRegistryModule.AssetCreated(AlternativeBlueprint);

	// Add the alternative Property to the new alternative Class
	FPropertyEditorModule& PropertyModule = FModuleManager::LoadModuleChecked<FPropertyEditorModule>("PropertyEditor");

	FString NameTemp = NewAltName + "_C";

	PropertyModule.RegisterCustomClassLayout(
		FName(*NameTemp), FOnGetDetailCustomizationInstance::CreateStatic(&FAlternativeDetails::MakeInstance));

	UE_LOG(LogTemp, Warning, TEXT("Alternative: %s Saved"));
	return FReply::Handled();
}

void SAlternativeView::ToolbarSaveAlternative()
{
	// Don't allow asset renaming during PIE
	if (GIsEditor)
	{
		EAppReturnType::Type SaveAlt = EAppReturnType::No;

		const FText SaveAlternativeTitle = LOCTEXT("SaveAlternativeTitle", "New Alternative");

		// Ask the user for confirmation on the action
		SaveAlt = FMessageDialog::Open(EAppMsgType::Type::YesNo,
			FText::FromString(TEXT("Are you sure you want to save an Alternative of ") + CurrentBP->GetName() + TEXT("?")),
			&SaveAlternativeTitle);

		if (SaveAlt == EAppReturnType::Yes)
		{
			AlternativeCount = AltMasterList::SharedInstance()->AlternativeCounts[MLIndex];

			FString NewAltName = AltData[0]->AlternativeBP->GetName();
			NewAltName += FString(TEXT("_Alternative "));
			NewAltName += FString::FromInt(AlternativeCount);

			// Make new blueprint object
			UObject* AlternativeBlueprint = DuplicateObject(CurrentBPObj, CurrentBPObj->GetOuter(), FName(*NewAltName));

			Alternatives = AltMasterList::SharedInstance()->SortedAlternatives[MLIndex]
				[AltMasterList::SharedInstance()->SortedAlternatives[MLIndex].Num() - 1]->BlueprintAlternatives;

			UBPAltData* Data = new UBPAltData();
			Alternatives.Add(AlternativeBlueprint);
			Data->ConstructData(NewAltName, Cast<UBlueprint>(AlternativeBlueprint), Alternatives, 
				MLIndex, AltMasterList::SharedInstance()->SortedAlternatives[MLIndex].Num(), SortedIndex);

			TSharedPtr<UBPAltData> SData = MakeShareable(Data);

			// Add the Base Blueprint to the list of alternatives
			AltMasterList::SharedInstance()->AddAlternative(SData);

			FAssetRegistryModule& AssetRegistryModule = FModuleManager::LoadModuleChecked<FAssetRegistryModule>("AssetRegistry");
			AssetRegistryModule.AssetCreated(AlternativeBlueprint);

			// Add the alternative Property to the new alternative Class
			FPropertyEditorModule& PropertyModule = FModuleManager::LoadModuleChecked<FPropertyEditorModule>("PropertyEditor");

			FString NameTemp = NewAltName + "_C";

			PropertyModule.RegisterCustomClassLayout(
				FName(*NameTemp), FOnGetDetailCustomizationInstance::CreateStatic(&FAlternativeDetails::MakeInstance));

			UE_LOG(LogTemp, Warning, TEXT("Alternative: %s Saved"), *NewAltName);

			//
			// Ask to open the newly saved alternative
			//
			EAppReturnType::Type OpenNewAlt = EAppReturnType::No;

			const FText OpenNewAlternativeTitle = LOCTEXT("OpenNewAlternativeTitle", "New Alternative");

			// Ask the user for confirmation on the action
			OpenNewAlt = FMessageDialog::Open(EAppMsgType::Type::YesNo,
				FText::FromString(TEXT("Would you like to open the new alternative?")),
				&OpenNewAlternativeTitle);

			if (OpenNewAlt == EAppReturnType::Yes)
			{
				LoadAlternative(SData);
			}
		}
	}
}

void SAlternativeView::ToolbarAlternativePIE()
{	
	if (GEditor)
	{
		GEditor->PlayInEditor(GWorld->PersistentLevel->GetWorld(), true);
	}
}

//
void SAlternativeView::OnAlternativeSelectionChanged(TSharedPtr<UBPAltData> InItem, ESelectInfo::Type InSelectInfo)
{
	if (InItem.IsValid())
	{
		UE_LOG(LogTemp, Log, TEXT("Alt View %d Selection Changed to: %s"), InItem->index, *InItem->AlternativeName);

		SelectedAlternative = InItem;
	}
}

// 
void SAlternativeView::OnAssetRenamed(const FAssetData& NewData, const FString& OldPath)
{
	AlternativeListView->RebuildList();
}

void SAlternativeView::RefreshAlternativeList()
{
	AlternativeListView->RebuildList();
}

void SAlternativeView::DeleteAlternative(TSharedPtr<UBPAltData> Alt)
{
	// Don't allow asset deletion during PIE
	if (GIsEditor)
	{
		UEditorEngine* Editor = GEditor;
		FWorldContext* PIEWorldContext = GEditor->GetPIEWorldContext();
		if (PIEWorldContext)
		{
			FNotificationInfo Notification(LOCTEXT("CannotDeleteAssetInPIE", "Assets cannot be deleted while in PIE."));
			Notification.ExpireDuration = 3.0f;
			FSlateNotificationManager::Get().AddNotification(Notification);
			return;
		}
	}
	EAppReturnType::Type DeleteAlt = EAppReturnType::No;

	const FText DeleteAlternativeTitle = LOCTEXT("DeletingAlternative","Delete Alternative");

	// Ask the user for confirmation on the action
	DeleteAlt = FMessageDialog::Open(EAppMsgType::Type::YesNo,
		FText::FromString(TEXT("Are you sure you want to delete ") + CurrentBP->GetName() + TEXT("?")),
		&DeleteAlternativeTitle);

	if (DeleteAlt == EAppReturnType::Yes)
	{
		AltMasterList::SharedInstance()->DeleteAlternative(Alt);
	}
}

TSharedPtr<SWidget> SAlternativeView::OnMakeAlternativeContextMenu()
{
	const bool bCloseAfterSelection = true;
	FMenuBuilder MenuBuilder(bCloseAfterSelection, TSharedPtr<FUICommandList>());

	MenuBuilder.BeginSection("MainSection");
	{
		MenuBuilder.AddMenuEntry(LOCTEXT("LoadBlueprintAlternative", "Open Alternative"), LOCTEXT("LoadBlueprintAlternative_ToolTip", "Open the selected Blueprint Alternative"),
			FSlateIcon(FEditorStyle::GetStyleSetName(), "ClassIcon.BlueprintCore"), FExecuteAction::CreateSP(this, &SAlternativeView::LoadAlternative, SelectedAlternative));
		
		MenuBuilder.AddMenuEntry(LOCTEXT("DeleteBlueprintAlternative", "Delete Alternative"), LOCTEXT("DeleteBlueprintAlternative_ToolTip", "Delete the selected Blueprint Alternative"),
			FSlateIcon(FEditorStyle::GetStyleSetName(), "ContentBrowser.AssetActions.Delete"), FExecuteAction::CreateSP(this, &SAlternativeView::DeleteAlternative, SelectedAlternative));

		MenuBuilder.AddMenuEntry(LOCTEXT("RefreshAltList", "Refresh List"), LOCTEXT("RefreshAltList_ToolTip", "Refresh Alt List"),
			FSlateIcon(FEditorStyle::GetStyleSetName(), "ContentBrowser.AssetActions.Delete"), FExecuteAction::CreateSP(this, &SAlternativeView::RefreshAlternativeList));

	}
	MenuBuilder.EndSection();

	return MenuBuilder.MakeWidget();
}

#undef LOCTEXT_NAMESPACE


/////////////////////////////////////
// IDetailsCustomization interface //
/////////////////////////////////////

#define LOCTEXT_NAMESPACE "AlternativeDetails"

void FAlternativeDetails::CustomizeDetails(IDetailLayoutBuilder& DetailBuilder)
{
	BuilderRef = &DetailBuilder;

	DetailBuilder.GetObjectsBeingCustomized(Objects);

	IDetailCategoryBuilder& Cat = DetailBuilder.EditCategory(TEXT("Blueprint Alternatives"));

	UE_LOG(LogTemp, Warning, TEXT("Alternative Details Created"));

	// Getting the Altnernative Name list for the given blueprint
	FString ObjectName, Right;
	if (Objects.Num() != 1)
	{
		UE_LOG(LogTemp, Warning, TEXT("More than one Objects that are being edited"));
		return;
	}
	else
	{
		Objects[0]->GetClass()->GetName().Split(TEXT("_C"), &ObjectName, &Right);

		UE_LOG(LogTemp, Warning, TEXT("One Object being edited: %s"), *ObjectName);

		if (Objects[0].Get()->IsInBlueprint())
			return;
	}

	UBlueprint* bp;
	TArray<TSharedPtr<UBPAltData>> arry;
	bool Alt = false;
	for (int i = 0; i < AltMasterList::SharedInstance()->SortedAlternatives.Num(); i++)
	{
		for (int j = 0; j < AltMasterList::SharedInstance()->SortedAlternatives[i].Num(); j++)
		{
			bp = AltMasterList::SharedInstance()->SortedAlternatives[i][j].Get()->AlternativeBP;
			
			arry = AltMasterList::SharedInstance()->SortedAlternatives[i];
			if (AltMasterList::SharedInstance()->SortedAlternatives[i][j].Get()->AlternativeBP->IsValidLowLevel())
			{
				if (AltMasterList::SharedInstance()->SortedAlternatives[i][j].Get()->AlternativeBP->GetName().Compare(ObjectName) == 0 && !Alt)
				{
					Alt = true;
					UE_LOG(LogTemp, Warning, TEXT("We got the alternative!"));
					break;
				}
			}
		}
		if (Alt)
		{
			for (int k = 0; k < AltMasterList::SharedInstance()->SortedAlternatives[i].Num(); k++)
			{
				AlternativeNames.Add(MakeShareable(new FString(AltMasterList::SharedInstance()->SortedAlternatives[i][k].Get()->AlternativeName)));
			}
			break;
		}
	}

	int nameIndex = 0;
	if (!Alt)
	{
		UE_LOG(LogTemp, Warning, TEXT("Cannot make alternative menu, no alternatives exist for this actor"));

		// Adding the first item to the ComboBox if there is no alternatives for a given object
		AlternativeNames.Add(MakeShareable(new FString(TEXT("No Alternatives"))));
	}

	else
	{
		// Set the CurrentSelection if there is an alternative set
		for (int i = 0; i < AlternativeNames.Num(); i++)
		{
			if (*AlternativeNames[i].Get() == ObjectName)
			{
				CurrentItem = AlternativeNames[i];
				nameIndex = i;
				break;
			}
		}
	}

	// Assign a custom combo to show all of the alternatives for a given set of alternatives
	Cat.AddCustomRow(LOCTEXT("AlternativeCatComboBox", "AltList"))
	.WholeRowContent()
	[
		SAssignNew(AlternativesComboBox, SComboBox<TSharedPtr<FString>>)
			.OptionsSource(&AlternativeNames)
			.InitiallySelectedItem(AlternativeNames[nameIndex])
			.OnGenerateWidget(this, &FAlternativeDetails::OnGenerateAlternativesComboBox)
			.OnSelectionChanged(this, &FAlternativeDetails::OnAlternativeSelectionChange)
			.ContentPadding(2.0f)
			.Content()
			[
				SNew(STextBlock)
				.Text(this, &FAlternativeDetails::CreateAlternativesComboBoxContent)
				.Font(IDetailLayoutBuilder::GetDetailFont())
			]
	];

	Cat.AddCustomRow(LOCTEXT("MyButtonRowFilterString", "Search Filter Keywords"))
		.WholeRowContent().HAlign(EHorizontalAlignment::HAlign_Left)
		[
			SNew(SButton)
			.Text(LOCTEXT("RegenerateBtnText", "Regenerate Alts"))
			.OnClicked(this, &FAlternativeDetails::OnRegenerate, &AlternativeNames)
		];

	// Hide this category when inside of the Blueprint Editor
	if (Objects[0].Get()->IsInBlueprint())
		DetailBuilder.HideCategory("Blueprint Alternatives");

	if (AlternativeNames.Num() > 0)
	{
		AActor* SomeObject = Cast<AActor>(Objects[0].Get());
		FName newTag = "BPAlternatives";
		if (!SomeObject->ActorHasTag(newTag))
		{
			SomeObject->Tags.Add(newTag);
		}
	}
}

FReply FAlternativeDetails::OnCheckBP()
{
	// Get all of the assets for the project
	FAssetRegistryModule& AssetRegistryModule = FModuleManager::LoadModuleChecked<FAssetRegistryModule>("AssetRegistry");
	TArray<FAssetData> AssetData;
	
	// Sort to see if the asset is in a blueprint
	for (auto Asset : AssetData)
	{
		if (Cast<UBlueprint>(Asset.GetAsset()))
		{
			UE_LOG(LogTemp, Warning, TEXT("%s"), *Asset.GetAsset()->GetName());
		}
		else
			UE_LOG(LogTemp, Warning, TEXT("No blueprint cast"))
	}
	return FReply::Handled();
}

FReply FAlternativeDetails::OnRegenerate(TArray<TSharedPtr<FString>> *AltNames)
{
	FPropertyEditorModule& PropertyModule = FModuleManager::LoadModuleChecked<FPropertyEditorModule>("PropertyEditor");

	// Get all of the assets for the project
	FAssetRegistryModule& AssetRegistryModule = FModuleManager::LoadModuleChecked<FAssetRegistryModule>("AssetRegistry");
	TArray<FAssetData> AssetData;
	// Asset registry filter to find blueprints
	FARFilter Filter;
	Filter.PackagePaths.Add("/Game/Blueprints");

	AssetRegistryModule.Get().GetAssets(Filter, AssetData);

	// 'Right' isn't supposed to be used
	FString ObjectName, Right;
	if (Objects.Num() > 1)
	{
		//UE_LOG(LogTemp, Warning, TEXT("More than one Objects that are being edited"));
	}
	else if (Objects.Num() == 1)
	{
		Objects[0]->GetClass()->GetName().Split(TEXT("_C"), &ObjectName, &Right);

		//UE_LOG(LogTemp, Warning, TEXT("One Object being edited: %s"), *ObjectName);
	}

	TArray<UObject*> BlueprintObjects;
	// Sort to see if the asset is in a blueprint
	for (auto Asset : AssetData)
	{
		if (Cast<UBlueprint>(Asset.GetAsset()))
		{
			//UE_LOG(LogTemp, Warning, TEXT("%s"), *Asset.GetAsset()->GetName());
			BlueprintObjects.Add(Asset.GetAsset());
		}
		//else
			//UE_LOG(LogTemp, Warning, TEXT("No blueprint"));
	}

	for (auto BPObj : BlueprintObjects)
	{
		FString NameTemp = BPObj->GetName();
		if (BPObj->GetName().Contains(TEXT("(Base Class)")))
		{
			BPObj->GetName().Split(TEXT("(Base Class)"), &NameTemp, &Right);
		}
		NameTemp += TEXT("_C");

		PropertyModule.RegisterCustomClassLayout(
			FName(*NameTemp), FOnGetDetailCustomizationInstance::CreateStatic(&FAlternativeDetails::MakeInstance));

		//UE_LOG(LogTemp, Warning, TEXT("Added property to object: %s"), *NameTemp);
	}
	
	AlternativesComboBox->RefreshOptions();
	PropertyModule.NotifyCustomizationModuleChanged();
	
	for (auto name : (*AltNames))
	{
		FString temp = *name.Get();

		//UE_LOG(LogTemp, Warning, TEXT("Alt name: %s"), *temp);
	}

	UE_LOG(LogTemp, Warning, TEXT("Regeneration Complete"));
	return FReply::Handled();
};

TSharedRef<SWidget> FAlternativeDetails::OnGenerateAlternativesComboBox(TSharedPtr<FString> InItem)
{
	if (InItem.IsValid())
	{
		//UE_LOG(LogTemp, Warning, TEXT("Generate ComboBox Item: %s"), **InItem.Get());
		return SNew(STextBlock).Text(FText::FromString(*InItem.Get()));
	}

	else
		return SNew(STextBlock).Text(FText::FromString(TEXT("Bad Box")));
}

void FAlternativeDetails::OnAlternativeComboBoxOpening()
{
	FPropertyEditorModule& PropertyModule = FModuleManager::LoadModuleChecked<FPropertyEditorModule>("PropertyEditor");

	AlternativesComboBox->RefreshOptions();
	PropertyModule.NotifyCustomizationModuleChanged();

	BuilderRef->ForceRefreshDetails();
}

void FAlternativeDetails::OnAlternativeSelectionChange(TSharedPtr<FString> NewValue, ESelectInfo::Type)
{
	UE_LOG(LogTemp, Warning, TEXT("Alt Selection Changed to: %s"), **NewValue.Get());

	FString CName = *NewValue.Get();

	// Change the combo box current item
	CurrentItem = NewValue;

	// Check to see if you are trying to switch back to the base class
	if (NewValue->Contains(TEXT("(Base Class)")))
	{
		FString Right;
		NewValue->Split(TEXT("(Base Class)"), &CName, &Right);
	}

	// Get the actor object for the object being edited
	AActor* AltActor = Cast<AActor>(Objects[0].Get());
	FName altTag = "BPAlternatives";

	// Make sure that the selction exists as a Blueprint Alternative
	bool Alt = false;
	for (int i = 0; i < AltMasterList::SharedInstance()->SortedAlternatives.Num(); i++)
	{
		for (int j = 0; j < AltMasterList::SharedInstance()->SortedAlternatives[i].Num(); j++)
		{
			if (AltMasterList::SharedInstance()->SortedAlternatives[i][j].Get()->AlternativeBP->GetName().Compare(CName) == 0 && !Alt)
			{
				// Make sure that the Actor exists in the level
				if (GWorld->ContainsActor(AltActor))
				{
					// Get Class Name for the new selection
					CName.Append("_C");
					UObject* ClassPackage = ANY_PACKAGE;
					UE_LOG(LogTemp, Warning, TEXT("Alternative Switch Class Name: %s"), *CName);

					// Find the class object for the new selection
					UClass* c = FindObject<UClass>(ClassPackage, *CName);

					FActorSpawnParameters SpawnInfo;
					// SpawnInfo.Template = Cast<AActor>(AltMasterList::SharedInstance()->SortedAlternatives[i][j]->BlueprintAlternatives[j]);

					FReferencerInformationList Refs;
					UObject* Object = Objects[0].Get();

					// Check to see if this actor is reference and get the assets that are referencing this object
					if (IsReferenced(Object, RF_Public, EInternalObjectFlags::Native, true, &Refs))
					{
						FStringOutputDevice Ar;
						Object->OutputReferencers(Ar, &Refs);

						// Output the referencers information
						//UE_LOG(LogTemp, Warning, TEXT("%s"), *Ar);

						for (auto Ref : Refs.ExternalReferences)
						{
							///UE_LOG(LogTemp, Warning, TEXT("Class Name: %s"), *Ref.Referencer->GetClass()->GetName());
						
							// Check to see if there if the object is referened explictly in a blueprint (probably the level blueprint)
							if (Ref.Referencer->GetClass()->GetName() == TEXT("K2Node_Literal"))
							{
								// DO SOMETHING TO PREVENT THE ERROR, REPLACE THE REFERENCE?
							}
						}
						for (auto Ref : Refs.InternalReferences)
						{
							//UE_LOG(LogTemp, Warning, TEXT("Class Name: %s"), *Ref.Referencer->GetClass()->GetName());
						}
					}
					//else
						//UE_LOG(LogTemp, Warning, TEXT("Object not referenced: %s"), *Object->GetName());

					// Grab all of the information from the current actor to be applied to the new actor
					FTransform* ActorTransform = new FTransform(AltActor->GetActorTransform());

					// Create the new actor and spawn it into the persistent Slevel
					AActor* NewActor = AltActor->GetWorld()->SpawnActor(c, ActorTransform, SpawnInfo);

					NewActor->EditorReplacedActor(AltActor);

					FString ActorLabel = AltActor->GetActorLabel();

					// Needs to be called before EditorDestroyActor()
					GWorld->PersistentLevel->Modify();

					// Destroy the current actor
					if (GWorld->EditorDestroyActor(AltActor, true))
					{
						NewActor->SetActorLabel(ActorLabel);

						// Add AlternativeBlueprint Tag to the Actor
						NewActor->Tags.Add(altTag);

						// Select the new actor
						GEditor->SelectNone(true, true);
						GEditor->SelectActor(NewActor, true, true);
					}
					Alt = true;
					UE_LOG(LogTemp, Warning, TEXT("Actor swtiched to selection"));
				}
				else
					UE_LOG(LogTemp, Warning, TEXT("Alternative doesn't exist as an Actor"));

				break;
			}
		}
		if (Alt)
			break;
	}
}

FText FAlternativeDetails::CreateAlternativesComboBoxContent() const
{
	return CurrentItem.IsValid() ? FText::FromString(*CurrentItem.Get()) : LOCTEXT("AlternativeNotSelected", "Not selected");
}

#undef LOCTEXT_NAMESPACE