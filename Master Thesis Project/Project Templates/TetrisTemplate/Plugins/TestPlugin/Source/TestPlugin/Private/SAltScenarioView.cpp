// Fill out your copyright notice in the Description page of Project Settings.

#include "SAltScenarioView.h"

#define LOCTEXT_NAMESPACE "AlternativeScenarios"

SAltScenarioView::SAltScenarioView() {}

SAltScenarioView::~SAltScenarioView() {}

BEGIN_SLATE_FUNCTION_BUILD_OPTIMIZATION
void SAltScenarioView::Construct(const FArguments& InArgs)
{
	// If there are no currently scenarios create a placeholder
	if (AltScenarioMasterList::SharedInstance()->MasterList.Num() == 0)
	{
		AltScenarioMasterList::SharedInstance()->isEmpty = true;

		AlternativeScenario* BlankScenario = new AlternativeScenario();

		BlankScenario->Name = TEXT("No Scenarios");

		TSharedPtr<AlternativeScenario> SBlankScenario = MakeShareable(BlankScenario);

		AltScenarioMasterList::SharedInstance()->MasterList.Add(SBlankScenario);
	}
	else
		AltScenarioMasterList::SharedInstance()->isEmpty = false;

	// Save Button ToolTip
	TSharedPtr<SToolTip> AlternativeSaveButtonToolTip;
	SAssignNew(AlternativeSaveButtonToolTip, SToolTip)
		.Text(LOCTEXT("AlternativeSaveButtonToolTip", "Create a new Scenario Alternative"));

	// Save Button Display Text
	FText SaveButtonText = FText(LOCTEXT("SaveScenarioButtonText", "Save Scenario"));

	// Create new list view that will contain all of the alternatives for this blueprint
	SAssignNew(ScenariosListView, SListView<TSharedPtr<AlternativeScenario>>)
		.SelectionMode(ESelectionMode::Single)
		.ItemHeight(24.0f)
		.ListItemsSource(&AltScenarioMasterList::SharedInstance()->MasterList)
		.OnGenerateRow(this, &SAltScenarioView::OnGenerateScenarioRow);

	this->ChildSlot
		[
			SNew(SVerticalBox)
			+ SVerticalBox::Slot()
			.VAlign(VAlign_Top)
			.Padding(2.f, 2.f, 2.f, 2.f)
			.AutoHeight()
			.MaxHeight(120.f)
			[
				// Create the Save button widget
				SNew(SButton)
				.OnClicked(this, &SAltScenarioView::SaveScenario)
				.ToolTip(AlternativeSaveButtonToolTip)
				[
					SNew(STextBlock).Text(SaveButtonText)
				]
			]
			+ SVerticalBox::Slot()
			.AutoHeight()
			.VAlign(VAlign_Fill)
			[
				// Create a ListView of Alternatives
				SNew(SScrollBorder, ScenariosListView.ToSharedRef())
				[
					ScenariosListView.ToSharedRef()
				]
			]
		];
}

END_SLATE_FUNCTION_BUILD_OPTIMIZATION

FReply SAltScenarioView::SaveScenario()
{
	// All of the actors in the persistent level
	TArray<AActor*> LevelActors;
	UGameplayStatics::GetAllActorsOfClass(GWorld, AActor::StaticClass(), LevelActors);

	FString NewName = TEXT("Scenerio ");

	AlternativeScenario* newScen;
	if (AltScenarioMasterList::SharedInstance()->isEmpty)
	{
		NewName += FString::FromInt(AltScenarioMasterList::SharedInstance()->MasterList.Num());
		// Scenario object you are going to add to the list
		newScen = new AlternativeScenario(NewName, AltScenarioMasterList::SharedInstance()->MasterList.Num() - 1);
	}
	else
	{
		NewName += FString::FromInt(AltScenarioMasterList::SharedInstance()->MasterList.Num() + 1);
		newScen = new AlternativeScenario(NewName, AltScenarioMasterList::SharedInstance()->MasterList.Num());
	}
	TSharedPtr<AlternativeScenario> NewScenario = MakeShareable(newScen);

	// Check to make sure that we have at least 1 alternative for a given actor
	bool Check = false;
	for (int k = 0; k < LevelActors.Num(); k++)
	{
		if (LevelActors[k]->ActorHasTag(FName("BPAlternatives")))
		{
			bool Alt = false;
			for (int i = 0; i < AltMasterList::SharedInstance()->SortedAlternatives.Num(); i++)
			{
				for (int j = 0; j < AltMasterList::SharedInstance()->SortedAlternatives[i].Num(); j++)
				{
					if (AltMasterList::SharedInstance()->SortedAlternatives[i][j].Get()->AlternativeBP->IsValidLowLevel())
					{
						FString ClassName = AltMasterList::SharedInstance()->SortedAlternatives[i][j].Get()->AlternativeName;

						if (ClassName.Contains(TEXT("(Base Class)")))
						{
							FString ObjectName, Right;
							ClassName.Split(TEXT("(Base Class)"), &ObjectName, &Right);
							ClassName = ObjectName;
						}
						ClassName += "_C";

						if (LevelActors[k]->GetClass()->GetName() == ClassName)
						{
							UE_LOG(LogTemp, Warning, TEXT("Actor Added to Scenario: %s"), *LevelActors[k]->GetName());

							NewScenario->AddActor(LevelActors[k], j);

							Alt = true;
							Check = true;

							break;
						}
					}
				}
				if (Alt)
				{
					break;
				}
			}
		}
	}

	if (Check)
	{
		UE_LOG(LogTemp, Warning, TEXT("Scenario Added: %s"), *NewScenario->Name);

		if (AltScenarioMasterList::SharedInstance()->isEmpty)
		{
			AltScenarioMasterList::SharedInstance()->MasterList.Empty();
			AltScenarioMasterList::SharedInstance()->isEmpty = false;
		}

		AltScenarioMasterList::SharedInstance()->AddScenario(NewScenario);
		ScenariosListView->RequestListRefresh();
	}
	else
	{
		UE_LOG(LogTemp, Warning, TEXT("Scenario failed to save"));
	}

	return FReply::Handled();
}

FReply SAltScenarioView::LoadScenario(int32 index, TSharedPtr<AlternativeScenario> Scenario)
{
	TArray<TSharedPtr<AlternativeScenario>> scenList = AltScenarioMasterList::SharedInstance()->MasterList;

	UE_LOG(LogTemp, Warning, TEXT("Attempting to load scenario: %d"), index);

	if (!AltScenarioMasterList::SharedInstance()->isEmpty)
	{
		AltScenarioMasterList::SharedInstance()->CurrentScenario = Scenario;

		TArray<AActor*> LevelActors;

		UGameplayStatics::GetAllActorsOfClass(GWorld, AActor::StaticClass(), LevelActors);
		UObject* ClassPackage = ANY_PACKAGE;

		int32 ScenarioActorCount = 0;
		FName altTag = "BPAlternatives";
		// Check all actors in the level
		for (auto Actor : LevelActors)
		{
			int32 i = AltMasterList::SharedInstance()->GetBaseIndex(Actor);
			if (Actor->ActorHasTag(altTag) && i != -1)
			{
				for (int j = 0; j < AltMasterList::SharedInstance()->SortedAlternatives[i].Num(); j++)
				{
					// Check to see if the class alternative for a specific actor already exists in the level
					if (AltMasterList::SharedInstance()->SortedAlternatives[i][j]->ClassName != Actor->GetClass()->GetName())
					{
						UE_LOG(LogTemp, Warning, TEXT("Found Scenario Actor: %s"), *Actor->GetName());

						UClass* c = FindObject<UClass>(ClassPackage,
							*AltMasterList::SharedInstance()->SortedAlternatives[i]
							[AltScenarioMasterList::SharedInstance()->MasterList[index]->ScenarioAltList[ScenarioActorCount]]->ClassName);

						// Grab all of the information from the current actor to be applied to the new actor
						FTransform* ActorTransform = new FTransform(Actor->GetActorTransform());

						FActorSpawnParameters SpawnInfo;

						// Create the new actor and spawn it into the persistent level
						AActor* NewActor = GWorld->SpawnActor(c, ActorTransform, SpawnInfo);

						FString ActorLabel = Actor->GetActorLabel();

						// Needs to be called before EditorDestroyActor()
						GWorld->PersistentLevel->Modify();

						// Destroy the current actor
						if (GWorld->EditorDestroyActor(Actor, true))
						{
							AltScenarioMasterList::SharedInstance()->MasterList[index]->ScenarioActorIDs[ScenarioActorCount] = NewActor->GetName();

							// Make sure it's destroyed for realz...
							Actor->Destroy();
							// Make sure that the label of the actor stays consistent
							NewActor->SetActorLabel(ActorLabel, true);
							// Add alternative tag
							NewActor->Tags.Add(altTag);
						}

						ScenarioActorCount++;
						break;
					}
					else
						UE_LOG(LogTemp, Warning, TEXT("Actor is the same in this scenario: %s"), *Actor->GetName());

				}
			}
		}
		if (ScenarioActorCount == 0)
			UE_LOG(LogTemp, Warning, TEXT("Scenario # %d Failed Check"), index);
	}
	else
		UE_LOG(LogTemp, Warning, TEXT("Scenario View is empty"));

	return FReply::Handled();
}


TSharedRef<ITableRow> SAltScenarioView::OnGenerateScenarioRow(TSharedPtr<AlternativeScenario> InItem, const TSharedRef<STableViewBase>& OwnerTable)
{
	// Create new row for the scenario tab
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
				SNew(STextBlock).Text(FText::FromString(InItem->Name))
				.OnDoubleClicked(this, &SAltScenarioView::LoadScenario, InItem->Index, InItem)
			]
		];
}

#undef LOCTEXT_NAMESPACE