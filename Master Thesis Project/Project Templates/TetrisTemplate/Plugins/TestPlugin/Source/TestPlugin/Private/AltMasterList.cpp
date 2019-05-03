// Fill out your copyright notice in the Description page of Project Settings.

#include "TestPlugin/Classes/AltMasterList.h"

AltMasterList* AltMasterList::inst;

AltMasterList::AltMasterList(){}

AltMasterList::~AltMasterList(){}

#define LOCTEXT_NAMESPACE "AltMasterList"

void AltMasterList::AddAlternative(TSharedPtr<UBPAltData> Alt)
{
	// Make sure that we are not adding a null pointer
	if (Alt.IsValid())
	{
		UE_LOG(LogTemp, Warning, TEXT("Adding Alternative: %s"), *Alt->AlternativeName);
		EAppReturnType::Type MakeAlt = EAppReturnType::Type::No;

		//  NEED TO FIX THIS will never proc Check to see if the MasterList already contains the alternative you are trying to add
		if (TheMasterList.Contains(Alt))
		{
			// Make an option to create a copy of something of an alternative that already exists.
			MakeAlt = FMessageDialog::Open(EAppMsgType::Type::YesNo, 
				LOCTEXT("AltenativeCopyException", "Alternative already exists would you like to create an Alternative anyways?"));
		}

		// Add a new alternative to the list
		if (!TheMasterList.Contains(Alt) || MakeAlt == EAppReturnType::Type::Yes)
		{
			// Check to see if the base blueprint exists in our master list

			// check to see if the alternative base exists
			if (SortedAlternatives.Num() - 1 >= Alt->index)
			{
				SortedAlternatives[Alt->index].Add(Alt);

				AlternativeCounts[Alt->index]++;
			}
			
			// if this is the first alternative being added for this blueprint create a base
			else
			{
				// Add new list to SortedAlternatives
				TArray<TSharedPtr<UBPAltData>> NewAltList;
				NewAltList.Add(Alt);
				SortedAlternatives.Add(NewAltList);

				Alt->BlueprintAlternatives.Add(Alt->AlternativeBP);

				AlternativeCounts.Add(1);
			}

			// Add the alternative to the master list
			TheMasterList.Add(Alt);

			AltNodeMasterList::SharedInstance()->AddAlternative(Alt);
		}
	}
}

bool AltMasterList::DeleteAlternative(FString AlternativeName)
{
	// Check to see if the name is valid
	bool Alt = false;
	
	for (auto List : SortedAlternatives)
	{
		for (auto Alternative : List)
		{
			if (Alternative->AlternativeName == AlternativeName)
			{
				Alt = true;
				UE_LOG(LogTemp, Warning, TEXT("Found alternative to delete: %s"), *AlternativeName);

				SortedAlternatives[SortedAlternatives.Find(List)].Remove(Alternative);

				TheMasterList.Remove(Alternative);

				break;
			}
		}
	}

	return Alt;
}

// Works?
bool AltMasterList::DeleteAlternative(TSharedPtr<UBPAltData> Alt)
{
	bool AltCheck = false;

	if (Alt.IsValid())
	{
		if (Alt->index != -1 && Alt->sIndex != -1)
		{
			UE_LOG(LogTemp, Log, TEXT("Delete Alternative %s"), *Alt->AlternativeBP->GetName());

			// Remove Alternative from the master lists
			TheMasterList.Remove(Alt);
			SortedAlternatives[Alt->index].Remove(Alt);

			// Close all editors for the given alternative blueprints
			FAssetEditorManager::Get().CloseAllEditorsForAsset(Alt->AlternativeBP);

			// Destroy the blueprint
			FAssetRegistryModule& AssetRegistryModule = FModuleManager::LoadModuleChecked<FAssetRegistryModule>("AssetRegistry");

			AssetRegistryModule.AssetDeleted(Alt->AlternativeBP);

			ObjectTools::DeleteSingleObject(Alt->AlternativeBP);

			Alt->AlternativeBP->ConditionalBeginDestroy();

			AltCheck = true;
		}
		else
		{
			UE_LOG(LogTemp, Error, TEXT("%d %d"), Alt->index, Alt->sIndex);
		}
	}

	else
	{
		UE_LOG(LogTemp, Error, TEXT("Alt in DeleteAlternative not valid"));
	}

	return AltCheck;
}

bool AltMasterList::RenameAlternative(TSharedPtr<UBPAltData> AltData, FString NewName)
{
	int32 BaseIndex = GetBaseIndex(AltData->AlternativeBP);
	int32 SortedIndex = GetSortedIndex(AltData->AlternativeBP);

	if (BaseIndex != -1 || SortedIndex != -1)
	{
		AltData->AlternativeName;
	}
	//AltData->AlternativeName;
	//
	//AltData;
	return false;
}

bool AltMasterList::OnRenameAlternative(TSharedPtr<UBPAltData> AltData, FString NewName)
{
	int32 BaseIndex = GetBaseIndex(AltData->AlternativeBP);
	int32 SortedIndex = GetSortedIndex(AltData->AlternativeBP);

	if (BaseIndex != -1 || SortedIndex != -1)
	{
		UE_LOG(LogTemp, Log, TEXT("Alternative: %s renamed to: %s"), *AltData->AlternativeName, *NewName);

		AltData->AlternativeName = NewName;

		return true;
	}

	UE_LOG(LogTemp, Error, TEXT("Rename failed"));
	return false;
}

UObject* AltMasterList::GetAlternativeObject(TSharedPtr<UBPAltData> AltData)
{
	if (!AltData.IsValid())
	{
		UE_LOG(LogTemp, Error, TEXT("GetAlternativeObject was given an Invalid value"));
	}
	return SortedAlternatives[AltData->index][AltData->sIndex]->AlternativeBP;
}

int32 AltMasterList::GetBaseIndex(UBlueprint* inBP)
{
	for (int i = 0; i < SortedAlternatives.Num(); i++)
	{
		for (int j = 0; j < SortedAlternatives[i].Num(); j++)
		{
			if (SortedAlternatives[i][j].Get()->AlternativeBP == inBP)
			{
				return i;
			}
		}
	}
	// if the inputed blueprint isn't a saved alternative return -1
	return -1;
}

int32 AltMasterList::GetSortedIndex(UBlueprint* inBP)
{
	for (int i = 0; i < SortedAlternatives.Num(); i++)
	{
		for (int j = 0; j < SortedAlternatives[i].Num(); j++)
		{
			if (SortedAlternatives[i][j].Get()->AlternativeBP == inBP)
			{
				return j;
			}
		}
	}
	// if the inputed blueprint isn't a saved alternative return -1
	return -1;
}

int32 AltMasterList::GetBaseIndex(FString inAlternativeName)
{
	for (int i = 0; i < SortedAlternatives.Num(); i++)
	{
		for (int j = 0; j < SortedAlternatives[i].Num(); j++)
		{
			if (SortedAlternatives[i][j]->AlternativeName == inAlternativeName)
			{
				return i;
			}
		}
	}
	// if the inputed actor's class isn't a saved alternative return -1
	return -1;
}

int32 AltMasterList::GetBaseIndex(AActor* inActor)
{
	for (int i = 0; i < SortedAlternatives.Num(); i++)
	{
		for (int j = 0; j < SortedAlternatives[i].Num(); j++)
		{
			if (SortedAlternatives[i][j].Get()->ClassName == inActor->GetClass()->GetName())
			{
				return i;
			}
		}
	}
	// if the inputed actor's class isn't a saved alternative return -1
	return -1;
}

void AltMasterList::ClearMasterList()
{
	SortedAlternatives.Empty();
	TheMasterList.Empty();
}

AltScenarioMasterList* AltScenarioMasterList::inst;

AltScenarioMasterList::AltScenarioMasterList() 
{
	GEditor->OnLevelActorDeleted().AddRaw(this, &AltScenarioMasterList::OnLevelActorDeleted);
}

AltScenarioMasterList::~AltScenarioMasterList() {}

void AltScenarioMasterList::AddScenario(TSharedPtr<AlternativeScenario> Scenario)
{
	MasterList.Add(Scenario);

	SortedActorIDs.Add(Scenario->ScenarioActorIDs);
	UnsortedActorIDs.Append(Scenario->ScenarioActorIDs);
}

void AltScenarioMasterList::OnActorReplacedViaAlternative(AActor* OldActor, AActor* NewActor)
{
	// Nothing right now
}

void AltScenarioMasterList::OnLevelActorDeleted(AActor* ActorDeleted)
{
	FString ActorName = ActorDeleted->GetName();
	
	if (UnsortedActorIDs.Contains(ActorName))
	{
		for (auto &S : SortedActorIDs)
		{
			if (S.Contains(ActorName))
			{
				S.Remove(ActorName);
			}
		}
	}
}

void AltScenarioMasterList::AddActorID(FString ID)
{
	UnsortedActorIDs.Add(ID);

	//SortedActorIDs
}

AltNodeMasterList* AltNodeMasterList::inst;

void AltNodeMasterList::AddAlternative(TSharedPtr<UBPAltData> AltData)
{
	if (MasterList.Num() - 1 >= AltData->index)
	{
		// Grab all of the graphs 
		TArray<UEdGraph*> ParentBPGraphs;
		TArray<UEdGraph*> BPGraphs;
		if (AltData->index != -1 && AltData->parentIndex != -1)
		{
			// Update the nodes list

			
			AltMasterList::SharedInstance()->SortedAlternatives[AltData->index][AltData->parentIndex]->AlternativeBP->GetAllGraphs(ParentBPGraphs);
			AltData->AlternativeBP->GetAllGraphs(BPGraphs);
		}
		TArray<FNodeAltID> TempNodeIDList;
		for (int i = 0; i < ParentBPGraphs.Num(); i++)
		{
			for (int j = 0; j < ParentBPGraphs[i]->Nodes.Num(); j++)
			{
				FString ID = FindNodeID(ParentBPGraphs[i]->Nodes[j]);
				if (ID == FString::FromInt(-1))
				{
					UE_LOG(LogTemp, Error, TEXT("ParentBP did something weird, a node id from the parent didn't exist (this isn't an issue yet)"));
					ID = GenerateNodeAltID();
				}

				// Add all of the existing IDs from the parent to all of the nodes 
				FNodeAltID NodeAltID(BPGraphs[i]->Nodes[j], ID);

				//UE_LOG(LogTemp, Warning, TEXT("Node ID: %s added to Node: %s"), *ID, *BPGraphs[i]->Nodes[j]->GetName());
				TempNodeIDList.Add(NodeAltID);
			}
		}
		MasterList[AltData->index].Add(TempNodeIDList);

		//UE_LOG(LogTemp, Log, TEXT("BP Node IDs Added (BaseBP Existed): %s"), *AltData->AlternativeBP->GetName());
	}
	// if this is the first alternative being added for this blueprint create a base
	else
	{
		// Update the nodes list
		TArray<UEdGraph*> BPGraphs;

		AltData->AlternativeBP->GetAllGraphs(BPGraphs);

		TArray<TArray<FNodeAltID>> TempNodeListIDListofLists;
		TArray<FNodeAltID> TempNodeIDList;
		for (auto Graph : BPGraphs)
		{
			for (auto Node : Graph->Nodes)
			{
				FString ID = FindNodeID(Node);
				if (ID == FString::FromInt(-1))
				{
					ID = GenerateNodeAltID();
				}

				FNodeAltID NodeAltID(Node, ID);
				TempNodeIDList.Add(NodeAltID);
			}
		}
		TempNodeListIDListofLists.Add(TempNodeIDList);
		MasterList.Add(TempNodeListIDListofLists);

		//UE_LOG(LogTemp, Log, TEXT("BP Node IDs Added (New BaseBP): %s"), *AltData->AlternativeBP->GetName());
	}

	// Add the OnGraphChanged Delegate
	TArray<UEdGraph*> BPGraphs;
	AltData->AlternativeBP->GetAllGraphs(BPGraphs);

	for (auto Graph : BPGraphs)
	{
		OnGraphChangedHandles.Add(Graph->AddOnGraphChangedHandler(FOnGraphChanged::FDelegate::CreateRaw(this, &AltNodeMasterList::OnGraphChanged)));
	}

}

FString AltNodeMasterList::GenerateNodeAltID()
{
	FString ID = FString::FromInt(IDIterator);
	IDIterator++;
	return ID;
}

FString AltNodeMasterList::FindNodeID(UEdGraphNode* Node)
{
	for (auto List : MasterList)
	{
		for (auto NodeList : List)
		{
			for (auto AltNode : NodeList)
			{
				if (AltNode.Node == Node)
				{
					return AltNode.ID;
				}
			}
		}
	}

	//UE_LOG(LogTemp, Error, TEXT("Couldn't find ID for node: %s"), *Node->GetName());
	return FString::FromInt(-1);
}

int32 AltNodeMasterList::FindNodeIndex(FString NodeID, UBlueprint* Blueprint)
{
	int32 BaseIndex = AltMasterList::SharedInstance()->GetBaseIndex(Blueprint);
	int32 SortedIndex = AltMasterList::SharedInstance()->GetSortedIndex(Blueprint);

	if (BaseIndex != -1 || SortedIndex != -1)
	{
		for (int i = 0; i < MasterList[BaseIndex][SortedIndex].Num(); i++)
		{
			if (MasterList[BaseIndex][SortedIndex][i].ID == NodeID)
			{
				return i;
			}
		}
	}
	// If there isn't a node with the id
	//UE_LOG(LogTemp, Warning, TEXT("No node found with given NodeID and Blueprint: %s"), *Blueprint->GetName());
	return -1;
}

UEdGraphNode* AltNodeMasterList::GetNodeWithID(FString NodeID, UBlueprint* Blueprint)
{
	int32 BaseIndex = AltMasterList::SharedInstance()->GetBaseIndex(Blueprint);
	int32 SortedIndex = AltMasterList::SharedInstance()->GetSortedIndex(Blueprint);
	if (BaseIndex != -1 || SortedIndex != -1)
	{
		for (int i = 0; i < MasterList[BaseIndex][SortedIndex].Num(); i++)
		{
			if (MasterList[BaseIndex][SortedIndex][i].ID == NodeID)
			{
				return MasterList[BaseIndex][SortedIndex][i].Node;
			}
		}
	}
	// If there isn't a node with the id
	//UE_LOG(LogTemp, Warning, TEXT("No node found with given NodeID and Blueprint: %s"), *Blueprint->GetName());
	return NULL;
}

bool AltNodeMasterList::DoesAlternativeHaveNode(UEdGraphNode* Node, UBlueprint* Blueprint)
{
	int32 BaseIndex = AltMasterList::SharedInstance()->GetBaseIndex(Blueprint);
	int32 SortedIndex = AltMasterList::SharedInstance()->GetSortedIndex(Blueprint);

	if (BaseIndex == -1 || SortedIndex == -1)
	{
		UE_LOG(LogTemp, Error, TEXT("The blueprint you are checking in DoesAlternativeHaveNode isn't an alternative"));
		return false;
	}

	FString InNodeID = FindNodeID(Node);

	// If the node doesn't currently exist in the given blueprint (there are some exceptions to this, but that is okay)
	if (InNodeID == FString::FromInt(-1))
	{
		//UE_LOG(LogTemp, Log, TEXT("This node doesn't have a node ID"));

		// Add node to the master list
		// AddNode(Node, Blueprint);

		return false;
	}

	for (auto NodeID : MasterList[BaseIndex][SortedIndex])
	{
		if (InNodeID == NodeID.ID)
		{
			UE_LOG(LogTemp, Log, TEXT("Node: %s exists in the target blueprint with an ID"), *Node->GetName());

			return true;
		}
	}

	//UE_LOG(LogTemp, Error, TEXT("Node: %s with ID %s doesn't exist in the target blueprint"), *Node->GetName(), *InNodeID);

	return false;
}

// Add node to the master list
bool AltNodeMasterList::AddNode(UEdGraphNode* AddedNode, UBlueprint* Blueprint)
{
	int32 AltBaseIndex = AltMasterList::SharedInstance()->GetBaseIndex(Blueprint);
	int32 SortedIndex = AltMasterList::SharedInstance()->GetSortedIndex(Blueprint);

	FString NodeID = FindNodeID(AddedNode);

	if (AltBaseIndex != -1 && SortedIndex != -1)
	{
		if (NodeID == FString::FromInt(-1))
		{
			NodeID = GenerateNodeAltID();
		}
		else
		{
			UE_LOG(LogTemp, Warning, TEXT("Added Node: %s already has a Node ID: %s"), *AddedNode->GetName(), *NodeID);
			return false;
		}
		FNodeAltID NodeAltID(AddedNode, NodeID);
		MasterList[AltBaseIndex][SortedIndex].Add(NodeAltID);

		for (int i = IndependentNodes.Num(); i < AltBaseIndex; i++)
		{
			TArray<UEdGraphNode*> NewNodeList;
			IndependentNodes.Add(NewNodeList);
		}
		IndependentNodes[AltBaseIndex].Add(AddedNode);

		UE_LOG(LogTemp, Log, TEXT("Node: %s Added to AltNodeMasterList"), *AddedNode->GetName());
		return true;
	}
	else
	{
		UE_LOG(LogTemp, Error, TEXT("Blueprint: %s , NOT VALID to add node"), *Blueprint->GetName());
		return false;
	}
}

bool AltNodeMasterList::AddNode(UEdGraphNode* AddedNode, UEdGraphNode* AddedFromNode, UBlueprint* Blueprint)
{
	int32 AltBaseIndex = AltMasterList::SharedInstance()->GetBaseIndex(Blueprint);
	int32 SortedIndex = AltMasterList::SharedInstance()->GetSortedIndex(Blueprint);

	FString FromID = FindNodeID(AddedFromNode);
	FString NodeID = FindNodeID(AddedNode);

	if (FromID == FString::FromInt(-1))
	{
		//UE_LOG(LogTemp, Error, TEXT("Added From Node: %s doesn't have an ID"), *AddedFromNode->GetName());
		return false;
	}
	if (NodeID != FString::FromInt(-1))
	{
		//UE_LOG(LogTemp, Warning, TEXT("Added Node: %s already has a Node ID: %s"), *AddedNode->GetName(), *NodeID);
		return false;
	}
	FNodeAltID NodeAltID(AddedNode, FromID);

	MasterList[AltBaseIndex][SortedIndex].Add(NodeAltID);

	return true;
}

void AltNodeMasterList::OnAlternativeDeleted(UBlueprint* DeletedAlternativeBlueprint)
{
	int32 Set = AltMasterList::SharedInstance()->GetBaseIndex(DeletedAlternativeBlueprint);
	int32 Alt = AltMasterList::SharedInstance()->GetSortedIndex(DeletedAlternativeBlueprint);

	if (Set == -1 || Alt == -1)
	{
		UE_LOG(LogTemp, Error, TEXT("The blueprint you are checking in OnAlternativeDeleted isn't an alternative"));
		return;
	}

	MasterList[Set].RemoveAt(Alt);
}

void AltNodeMasterList::OnGraphChanged(const struct FEdGraphEditAction& InAction)
{
	//if (InAction.Action == EEdGraphActionType::GRAPHACTION_AddNode)
	//{
	//	for (int i = 0; i < InAction.Nodes.Num(); i++)
	//	{
	//		UE_LOG(LogTemp, Log, TEXT("Graph: %s Added Node: %s"), *InAction.Graph->GetName(), *InAction.Nodes[FSetElementId::FromInteger(i)]->GetName());
	//	}
	//	OnNodesAdded(InAction.Nodes, FBlueprintEditorUtils::FindBlueprintForGraph(InAction.Graph));
	//}
	//else if (InAction.Action == EEdGraphActionType::GRAPHACTION_RemoveNode)
	//{
	//	for (int i = 0; i < InAction.Nodes.Num(); i++)
	//	{
	//		UE_LOG(LogTemp, Log, TEXT("Graph: %s Deleted Node: %s"), *InAction.Graph->GetName(), *InAction.Nodes[FSetElementId::FromInteger(i)]->GetName());
	//	}
	//	OnNodesDeleted(InAction.Nodes, FBlueprintEditorUtils::FindBlueprintForGraph(InAction.Graph));
	//}

	//else if (InAction.Action == EEdGraphActionType::GRAPHACTION_SelectNode)
	//{
	//	UE_LOG(LogTemp, Log, TEXT("Graph: %s Select Node"), *InAction.Graph->GetName());
	//}
	//else if (InAction.Action == EEdGraphActionType::GRAPHACTION_Default)
	//{
	//	// Will crash the engine Graph isn't valid
	//	//UE_LOG(LogTemp, Log, TEXT("Graph: %s Default Action"), *InAction.Graph->GetName());
	//}
	//else
	//{
	//	UE_LOG(LogTemp, Log, TEXT("Graph: %s Invalid ACTION"), *InAction.Graph->GetName());

	//	if (InAction.Nodes.Num() > 0)
	//	{
	//		for (int i = 0; i < InAction.Nodes.Num(); i++)
	//		{
	//			UE_LOG(LogTemp, Log, TEXT("Graph: %s I Added Node: %s"), *InAction.Graph->GetName(), *InAction.Nodes[FSetElementId::FromInteger(i)]->GetName());
	//		}
	//		OnNodesAdded(InAction.Nodes, FBlueprintEditorUtils::FindBlueprintForGraph(InAction.Graph));
	//	}
	//}
}

// When a node is deleted from a blueprint alternative
void AltNodeMasterList::OnNodesDeleted(TSet<const UEdGraphNode*> DeletedNodes, UBlueprint* Blueprint)
{
	int32 Set = AltMasterList::SharedInstance()->GetBaseIndex(Blueprint);
	int32 Alt = AltMasterList::SharedInstance()->GetSortedIndex(Blueprint);

	if (Set == -1 || Alt == -1)
	{
		UE_LOG(LogTemp, Error, TEXT("The blueprint you are checking in OnNodeDeleted isn't an alternative"));
		return;
	}

	for (auto DeletedNode : DeletedNodes)
	{
		int32 NodeIndex = FindNodeIndex(FindNodeID(const_cast<UEdGraphNode*>(DeletedNode)), Blueprint);

		if (NodeIndex == -1)
		{
			continue;
		}
		// Remove the node from the master list
		MasterList[Set][Alt].RemoveAt(NodeIndex);
	}
}

// When a new node is added to a blueprint alternative
void AltNodeMasterList::OnNodesAdded(TSet<const UEdGraphNode*> AddedNodes, UBlueprint* Blueprint)
{
	int32 Set = AltMasterList::SharedInstance()->GetBaseIndex(Blueprint);
	int32 Alt = AltMasterList::SharedInstance()->GetSortedIndex(Blueprint);

	if (Set == -1 || Alt == -1)
	{
		UE_LOG(LogTemp, Error, TEXT("The blueprint you are checking in OnNodeDeleted isn't an alternative"));
		return;
	}

	for (auto AddedNode : AddedNodes)
	{
		int32 NodeIndex = FindNodeIndex(FindNodeID(const_cast<UEdGraphNode*>(AddedNode)), Blueprint);

		if (NodeIndex != -1)
		{
			continue;
		}
		AddNode(const_cast<UEdGraphNode*>(AddedNode), Blueprint);
	}
}

// Return if a given node exists in a given alternative
/*bool AltNodeMasterList::DoesNodeExistIn2Graphs(UEdGraphNode* InNode, UBlueprint* BaseBlueprint, UBlueprint* TargetBlueprint)
{
	int32 BaseSet = AltMasterList::SharedInstance()->GetBaseIndex(BaseBlueprint);
	int32 BaseAlt = AltMasterList::SharedInstance()->GetSortedIndex(BaseBlueprint);

	int32 TargetSet = AltMasterList::SharedInstance()->GetBaseIndex(TargetBlueprint);
	int32 TargetAlt = AltMasterList::SharedInstance()->GetSortedIndex(TargetBlueprint);

	if (BaseSet == -1 || BaseAlt == -1 || TargetSet == -1 || TargetAlt == -1)
	{
		UE_LOG(LogTemp, Error, TEXT("One of the two blueprints you are checking isn't an alternative"));
		return false;
	}

	FString ID = FindNodeID(InNode);

	if (ID == FString::FromInt(-1))
	{
		UE_LOG(LogTemp, Error, TEXT("The node that you are checking in DoesNodeExistIn2Graphs isn't valid"));
		return false;
	}

	for (auto BNodeIDList : MasterList[BaseSet][BaseAlt])
	{
		
	}

	for (auto TNodeIDList : MasterList[TargetSet][TargetAlt])
	{

	}

	return false;
} */

#undef LOCTEXT_NAMESPACE