// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Engine/Engine.h"
#include "Editor.h"
#include "Runtime/Core/Public/Misc/MessageDialog.h"

#include "GraphEditAction.h"
#include "BlueprintEditorUtils.h"
#include "AssetRegistryModule.h"
#include "ObjectTools.h"
#include "AssetData.h"
#include "AssetToolsModule.h"

#include "TestPlugin/Classes/BPAltData.h"
#include "TestPlugin/Classes/AlternativeScenarios.h"

/*
* Just to keep the node pointer and ID together
*/
struct FNodeAltID
{
	UEdGraphNode* Node;
	FString ID;

	FNodeAltID() {}
	FNodeAltID(UEdGraphNode* InNode, FString InID)
	{
		Node = InNode;
		ID = InID;
	}
};

/*
 *
 */
struct FMergePin
{
	UEdGraphPin* Pin;
	FNodeAltID RemoteNodeID;

	int32 LocalPinIndex;

	// Indexes for the pins in the 
	int32 RemotePinIndex;

	FMergePin() {}
	FMergePin(UEdGraphPin* inPin, FNodeAltID inRemoteNodeID, int32 inLocalPinIndex, int32 inRemotePinIndex)
	{
		Pin = inPin;
	    RemoteNodeID = inRemoteNodeID;

		LocalPinIndex = inLocalPinIndex;
		RemotePinIndex = inRemotePinIndex;
	}
};

/*
 * Master list of ALL alternatives created across all blueprints (use SharedInstance() to access)
 */
class TESTPLUGIN_API AltMasterList
{
public:
	AltMasterList();
	~AltMasterList();

	// Singleton Public Interface for the Master List
	static AltMasterList* SharedInstance() { if (!inst) inst = new AltMasterList(); return inst; }

public:
	// A list of ALL alternatives saved, unsorted (no base blueprints included).
	TArray<TSharedPtr<UBPAltData>> TheMasterList;
	
	// A list of the base Blueprints that the alternatives are made from
	TArray<UBlueprint*> BaseBPs;
	
	TArray<int32> AlternativeCounts;

	/* 
	 * The Alternatives sorted based on the base blueprint 
	 * Indexs of the first dimension of the array should corespond with the BaseBP indexs
	 * The second dimension should corespond with the order of the alternatives for a given blueprint
     */
	TArray<TArray<TSharedPtr<UBPAltData>>> SortedAlternatives;
	
	TArray<TArray<TSharedPtr<FBlueprintEditor>>> OpenAltViews;

	// Add Alternative to the MasterList
	void AddAlternative(TSharedPtr<UBPAltData> Alt);
	
	// Delete alternative by AlternativeName, return if succesful 
	//(DOESN'T WORK 100% NEED TO UPDATE THE OTHER ALTERNATIVES IN THE SAME "SORTED" LIST)
	bool DeleteAlternative(FString AlternativeName);

	bool DeleteAlternative(TSharedPtr<UBPAltData> Alt);

	bool RenameAlternative(TSharedPtr<UBPAltData> Alt, FString NewName);

	bool OnRenameAlternative(TSharedPtr<UBPAltData> Alt, FString NewName);

	UObject* GetAlternativeObject(TSharedPtr<UBPAltData> AltData);

	//bool IsAltViewOpen(TSharedPtr<FBlueprintEditor>);

	/* Get the base blueprint index (for SortedAlternatives) of a given blueprint, 
	returns -1 the Blueprint doesn't have any alternatives. Confirmed Works */
	int32 GetBaseIndex(UBlueprint* inBP);

	// Confirmed Works
	int32 GetBaseIndex(AActor* inActor);

	// Works, I think...
	int32 GetBaseIndex(FString inAlternativeName);

	// Gets the sorted alterantive index given the base index and the blueprint object
	int32 GetSortedIndex(UBlueprint* inBP);

	// Completely clear the master list
	void ClearMasterList();
private:

	// Static instance of the master list and it's functions, accessible using SharedInstance()
	static AltMasterList* inst;
};

/*
* Master list of ALL blueprint alternative level editor scneraios created (use SharedInstance() to access)
*/
class TESTPLUGIN_API AltScenarioMasterList
{
public:
	AltScenarioMasterList();
	~AltScenarioMasterList();

	// Singleton Public Interface for the Master List
	static AltScenarioMasterList* SharedInstance() { if (!inst) inst = new AltScenarioMasterList(); return inst; }
public:

	// List of all of the stored Scenarios
	TArray<TSharedPtr<AlternativeScenario>> MasterList;
	
	// Current Scenario we are in right now
	TSharedPtr<AlternativeScenario> CurrentScenario;

	// List of the actor unique IDs, First dimension of array is which scenario each set of actor IDs belong to
	TArray<TArray<FString>> SortedActorIDs;

	// List of all of the actor unqiue IDs unsorted
	TArray<FString> UnsortedActorIDs;

	bool isEmpty;

	void OnLevelActorDeleted(AActor* ActorDeleted);

	// NOT IMPLEMENTED! Old Actor: actor being replaced, NewActor: actor that is replacing
	void OnActorReplacedViaAlternative(AActor* OldActor, AActor* NewActor);

	// Add scenario to the master list
	void AddScenario(TSharedPtr<AlternativeScenario> Scenario);

	// Add Actor ID info to the list
	void AddActorID(FString ID);

private:
	// Static instance of the master list and it's functions, accessible using SharedInstance()
	static AltScenarioMasterList* inst;

};

/*
 * Made to keep track of nodes that exist within alternatives
 */
class TESTPLUGIN_API AltNodeMasterList
{
public:
	AltNodeMasterList() {}
	~AltNodeMasterList() {}

	// Singleton Public Interface for the Master List
	static AltNodeMasterList* SharedInstance() { if (!inst) inst = new  AltNodeMasterList(); return inst; }
public:

	// List of all of the stored blueprint alternative nodes
	TArray<TArray<TArray<FNodeAltID>>> MasterList;

	// List of all of the Nodes that are added and are not in other alternatives
	TArray<TArray<UEdGraphNode*>> IndependentNodes;

	// List of the actor unique IDs, First dimension of array is which scenario each set of actor IDs belong to

	TArray<UBlueprint*> BaseBPs;

	TArray<TArray<UBlueprint*>> BlueprintList;

	bool isEmpty = true;

	void AddAlternative(TSharedPtr<UBPAltData> AltData);

	FString FindNodeID(UEdGraphNode* Node);

	int32 FindNodeIndex(FString NodeID, UBlueprint* Blueprint);

	UEdGraphNode* GetNodeWithID(FString NodeID, UBlueprint* Blueprint);

	bool DoesAlternativeHaveNode(UEdGraphNode* Node, UBlueprint* Blueprint);

	// Add node to the master list, return true if node added
	bool AddNode(UEdGraphNode* AddedNode, UBlueprint* Blueprint);

	// Overload for const node
	//bool AddNode(const UEdGraphNode* AddedNode, UBlueprint* Blueprint);

	bool AddNode(UEdGraphNode* AddedNode, UEdGraphNode* AddedFromNode, UBlueprint* Blueprint);

	void OnAlternativeDeleted(UBlueprint* DeletedAlternativeBlueprint);

	// 
	void OnGraphChanged(const struct FEdGraphEditAction& InAction);

	// When a node is deleted from a blueprint alternative
	void OnNodesDeleted(TSet<const UEdGraphNode*> DeletedNodes, UBlueprint* Blueprint);

	// When a new node is added to a blueprint alternative
	void OnNodesAdded(TSet<const UEdGraphNode*> AddedNodes, UBlueprint* Blueprint);

	// Generates an ID, WILL PROBABLY MAKE THIS MORE COMPLEX
	FString GenerateNodeAltID();

private:
	// Static instance of the master list and it's functions, accessible using SharedInstance()
	static AltNodeMasterList* inst;

	/** The handle to the graph changed delegate. */
	TArray<FDelegateHandle> OnGraphChangedHandles;

	int32 IDIterator = 0;
};

