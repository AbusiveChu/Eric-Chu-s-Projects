// Fill out your copyright notice in the Description page of Project Settings.

#include "SAltMergeView.h"

bool SAltMergeView::canStartMerge;
bool SAltMergeView::isCurrentlyMerging;
bool SAltMergeView::isOpen;

SAltMergeView::SAltMergeView() {}

SAltMergeView::~SAltMergeView() {}

#define LOCTEXT_NAMESPACE "AltMergeView"

void SAltMergeView::Construct(const FArguments& InArgs)
{
	isOpen = true;
	canStartMerge = false;
	isCurrentlyMerging = false;

	GraphType = EBlueprintEditorGraph::Type::None;

	Editor = InArgs._BPEditor;
	//PluginInstance = InArgs._PluginInst;

	BaseBP = Editor->GetBlueprintObj();

	if (Editor.IsValid())
	{
		UE_LOG(LogTemp, Warning, TEXT("Blueprint Editor in Selective Merge is Valid"));
	}

	SetIndex = AltMasterList::SharedInstance()->GetBaseIndex(BaseBP);

	TSharedPtr<FString> NewTargetDefault = MakeShareable(new FString(TEXT("Not Selected")));

	// If we found that the blueprint has alternatives we are going to populate the combo box arrays
	if (SetIndex != -1)
	{
		// Clear out the lists of ComboBox Names
		TargetNames.Empty();

		TargetNames.Add(NewTargetDefault);

		for (auto Alt : AltMasterList::SharedInstance()->SortedAlternatives[SetIndex])
		{
			TargetNames.Add(MakeShareable(new FString(Alt->AlternativeName)));
		}

		UE_LOG(LogTemp, Warning, TEXT("Selective Merge Opened for %s"),
			*AltMasterList::SharedInstance()->SortedAlternatives[SetIndex][0]->AlternativeName);

		BackupSubDir = FPaths::GameSavedDir() / TEXT("Backup") / TEXT("Resolve_Backup[") + FDateTime::Now().ToString(TEXT("%Y-%m-%d-%H-%M-%S")) + TEXT("]");

		FToolBarBuilder ToolbarBuilder(TSharedPtr< FUICommandList >(), FMultiBoxCustomization::None);
		ToolbarBuilder.AddToolBarButton(
			FUIAction(
				FExecuteAction::CreateSP(this, &SAltMergeView::OnStartMerge),
				FCanExecuteAction::CreateSP(this, &SAltMergeView::CanStartMerge),
				FIsActionChecked())
			, NAME_None
			, LOCTEXT("StartSelectiveMergeLabel", "Start Selective Merge")
			, LOCTEXT("StartSelectiveMergeTooltip", "Loads the selected blueprints and switches to an active merge (using your selections for the remote/Target2/local)")
			, FSlateIcon(FEditorStyle::GetStyleSetName(), "BlueprintMerge.StartMerge")
		);
		ToolbarBuilder.AddToolBarButton(
			FUIAction(
				FExecuteAction::CreateSP(this, &SAltMergeView::OnTest),
				FCanExecuteAction::CreateSP(this, &SAltMergeView::CanStartMerge),
				FIsActionChecked())
			, NAME_None
			, LOCTEXT("TestMergeLabel", "Test Button")
			, LOCTEXT("TestMergeTooltip", "Logs stats that I want to output")
			, FSlateIcon(FEditorStyle::GetStyleSetName(), "BlueprintMerge.Finish")
		);

		// TODO: Add check box for auto merge where possible
		// ToolbarBuilder.AddWidget()
		ToolbarBuilder.AddSeparator();
		ToolbarBuilder.AddToolBarButton(
			FUIAction(
				FExecuteAction::CreateSP(this, &SAltMergeView::OnFinishMerge),
				FCanExecuteAction(), FIsActionChecked(),
				FIsActionButtonVisible::CreateSP(this, &SAltMergeView::IsCurrentlyMerging))
			, NAME_None
			, LOCTEXT("FinishMergeLabel", "Finish Selective Merge")
			, LOCTEXT("FinishMergeTooltip", "Complete the merge operation - saves the blueprint and resolves the conflict with the SCC provider")
			, FSlateIcon(FEditorStyle::GetStyleSetName(), "BlueprintMerge.Finish")
		);


		if (AltMasterList::SharedInstance()->SortedAlternatives.Num() > 0)
		{
			FString BaseText = TEXT("Base Blueprint Alternative: ") + BaseBP->GetName();
			FString TargetText = TEXT("Select the Target Alternative");
			TSharedRef<SWidget> Overlay = SNew(SHorizontalBox);
			Overlay = SNew(SHorizontalBox)
				+ SHorizontalBox::Slot()
				.HAlign(HAlign_Center)
				[
					SNew(STextBlock)
					.Text(FText::FromString(BaseText))
				]
				+ SHorizontalBox::Slot()
				.HAlign(HAlign_Center)
				[
					SNew(SVerticalBox)
					+ SVerticalBox::Slot()
					.VAlign(VAlign_Center)
					[
						SNew(STextBlock)
						.Text(FText::FromString(TargetText))
					]
					+ SVerticalBox::Slot()
					.VAlign(VAlign_Center)
					.AutoHeight()
					[					
						SAssignNew(TargetMergeComboBox, SComboBox<TSharedPtr<FString>>)
						.OptionsSource(&TargetNames)
						.InitiallySelectedItem(TargetItem)
						.OnGenerateWidget(this, &SAltMergeView::OnGenerateAlternativesComboBox)
						.OnSelectionChanged(this, &SAltMergeView::OnTargetSelectionChanged)
						.ContentPadding(2.0f)
						.Content()
						[
							SNew(STextBlock)
							.Text(this, &SAltMergeView::CreateTargetMergeComboBoxContent)
						]
					]
				];
			//
			ChildSlot
				[
					SNew(SVerticalBox)
					+ SVerticalBox::Slot()
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
					[
						SNew(SSplitter)
						+ SSplitter::Slot()
						.Value(1.0f)
						[
							SNew(SOverlay)
							+ SOverlay::Slot()
							[
								SAssignNew(MainView, SBox)
							]
							+ SOverlay::Slot()
							.VAlign(VAlign_Center)
							[
								Overlay
							]
						]
					]
				];

			TargetMergeComboBox->SetSelectedItem(NewTargetDefault);

			TargetMergeComboBox->RefreshOptions();
		}

		// If there are no alternatives saved
		else
		{
			UE_LOG(LogTemp, Warning, TEXT("Cannot merge, no alternatives exist"));
		}
	}
	else
	{
		UE_LOG(LogTemp, Warning, TEXT("No Alternative exists for this blueprint, cannot open menu"));
	}
}

void SAltMergeView::OnTest()
{
	check(Editor.IsValid());

	UE_LOG(LogTemp, Warning, TEXT("Current Graph Name: %s"), *Editor->GetFocusedGraph()->GetName());

	TArray<UEdGraph*> BPGraphs;
	BaseBP->GetAllGraphs(BPGraphs);

	for (auto Graph : BPGraphs)
	{
		UE_LOG(LogTemp, Warning, TEXT("Graph Name: %s"), *Graph->GetName())
	}
}

void SAltMergeView::OnStartMerge()
{
	check(Editor.IsValid());
	isCurrentlyMerging = true;
	canStartMerge = false;

	if (Editor.IsValid())
	{
		UE_LOG(LogTemp, Log, TEXT("Starting Selective Merge"));

		TArray<UEdGraph*> FunctionGraphs = BaseBP->FunctionGraphs;
		TArray<UEdGraph*> MacroGraphs = BaseBP->MacroGraphs;
		TArray<UEdGraph*> EventGraphs = BaseBP->EventGraphs;
		TArray<UEdGraph*> UberGraphPages = BaseBP->UbergraphPages;

		RootNodes.Empty();

		//Editor->GetFocusedGraph()->Nodes[0];

		// We only need the currently focused graph if we are only going to selectively merge some of the parts of the graph
		if (Editor->GetSelectedNodes().Num() > 0)
		{
			if (TargetItem.IsValid() && *TargetItem.Get() != TEXT("Not Selected"))
			{
				// Get the type of graph the merge is being made in
				{
					if (BaseBP->FunctionGraphs.Contains(Editor->GetFocusedGraph()))
						GraphType = EBlueprintEditorGraph::Type::Function;

					else if (BaseBP->MacroGraphs.Contains(Editor->GetFocusedGraph()))
						GraphType = EBlueprintEditorGraph::Type::Event;

					else if (BaseBP->EventGraphs.Contains(Editor->GetFocusedGraph()))
						GraphType = EBlueprintEditorGraph::Type::Macro;

					else if (BaseBP->UbergraphPages.Contains(Editor->GetFocusedGraph()))
						GraphType = EBlueprintEditorGraph::Type::UberGraphPages;

					else if (BaseBP->DelegateSignatureGraphs.Contains(Editor->GetFocusedGraph()))
						GraphType = EBlueprintEditorGraph::Type::DelegateSignature;

					else
					{
						GraphType = EBlueprintEditorGraph::Type::None;
						UE_LOG(LogTemp, Warning, TEXT("Graph type is unknown: %s"), *Editor->GetFocusedGraph()->GetName());
						return;
					}
				}
				// Get the Target Blueprint object
				TargetBP = AltMasterList::SharedInstance()->SortedAlternatives[SetIndex][TargetNames.Find(TargetItem) - 1]->AlternativeBP;

				TArray<UEdGraph*> TargetBPGraphs;
				// Get the applicable graphs
				switch (GraphType)
				{
				case EBlueprintEditorGraph::Type::Function:
					TargetBPGraphs = AltMasterList::SharedInstance()->SortedAlternatives[SetIndex][TargetNames.Find(TargetItem) - 1]->AlternativeBP->FunctionGraphs;

					// Currently we don't support function graphs
					FMessageDialog::Open(EAppMsgType::Type::Ok,
						LOCTEXT("FunctionGraphException", "Selective Merging for Function Graphs are not currently supported."));
					return;
					break;

				case EBlueprintEditorGraph::Type::Event:
					TargetBPGraphs = AltMasterList::SharedInstance()->SortedAlternatives[SetIndex][TargetNames.Find(TargetItem) - 1]->AlternativeBP->EventGraphs;
					break;

				case EBlueprintEditorGraph::Type::Macro:
					TargetBPGraphs = AltMasterList::SharedInstance()->SortedAlternatives[SetIndex][TargetNames.Find(TargetItem) - 1]->AlternativeBP->MacroGraphs;

					// Currently we don't support macro graphs
					FMessageDialog::Open(EAppMsgType::Type::Ok,
						LOCTEXT("MacroGraphException", "Selective Merging for Macro Graphs are not currently supported."));
					return;
					break;

				case EBlueprintEditorGraph::Type::UberGraphPages:
					TargetBPGraphs = AltMasterList::SharedInstance()->SortedAlternatives[SetIndex][TargetNames.Find(TargetItem) - 1]->AlternativeBP->UbergraphPages;
					break;

				case EBlueprintEditorGraph::Type::DelegateSignature:
					TargetBPGraphs = AltMasterList::SharedInstance()->SortedAlternatives[SetIndex][TargetNames.Find(TargetItem) - 1]->AlternativeBP->DelegateSignatureGraphs;
					break;

				default:
					UE_LOG(LogTemp, Warning, TEXT("NO VALID GRAPH TYPE"));

					break;
				}

				// Clear the target graph pointer
				TargetGraph = NULL;

				UEdGraph* TempGraph = NULL;

				// Find the specfic graph we are looking for
				for (auto Graph : TargetBPGraphs)
				{
					if (Graph->GetName() == Editor->GetFocusedGraph()->GetName())
					{
						TempGraph = Graph;
						break;
					}
				}

				// If we don't find the right graph cancel the check for the first target
				if (!TempGraph)
				{
					UE_LOG(LogTemp, Warning, TEXT("TargetGraph not found"));

					FMessageDialog::Open(EAppMsgType::Type::Ok,
						LOCTEXT("FunctionGraphException", "Focused Graph Not Found in Target Blueprint"));
					return;
					// If the ubergraph doesn't exist
					if (GraphType == EBlueprintEditorGraph::Type::Event || GraphType == EBlueprintEditorGraph::Type::UberGraphPages)
					{
						// Find the event graph and make that the target graph
						TargetGraph = AltMasterList::SharedInstance()->SortedAlternatives[SetIndex][TargetNames.Find(TargetItem) - 1]->AlternativeBP->EventGraphs[0];
					}
					else if (GraphType == EBlueprintEditorGraph::Type::Function)
					{
						// When the selected 
						FMessageDialog::Open(EAppMsgType::Type::Ok,
							LOCTEXT("FunctionGraphException", "Selective Merging for Function Graphs are not currently supported."));

						return;
					}
					else if (GraphType == EBlueprintEditorGraph::Type::Macro)
					{
						// Create a new macro to add the selection to
						FMessageDialog::Open(EAppMsgType::Type::Ok,
							LOCTEXT("MacroGraphException", "Selective Merging for Macro Graphs are not currently supported."));
						return;
					}
					/* SHOULD WE CREATE A WHOLE NEW GRAPH?
					Maybe only if we find that it is a function or macro that doesn't exist, we can just use
					an existing event graph to put the nodes */
					//goto Target;
				}
				else
					TargetGraph = TempGraph;

				// Deal with the graph type now that we have it
				{
					if (GraphType == EBlueprintEditorGraph::Type::Function)
					{
						UE_LOG(LogTemp, Log, TEXT("This is a Function Graph"));
						// Ask the user which graph to merge the selection to
						// Create the function?
					}
					else if (GraphType == EBlueprintEditorGraph::Type::Macro)
					{
						UE_LOG(LogTemp, Log, TEXT("This is a Macro Graph"));
						// Ask the user which graph to merge the selection to
						// Create the Macro?
					}
					else if (GraphType == EBlueprintEditorGraph::Type::Event)
					{
						UE_LOG(LogTemp, Log, TEXT("This is a Event Graph"));
						// Note of this and present the user with the 
						// if there are no
						// Create the event graph?

					}
					else if (GraphType == EBlueprintEditorGraph::Type::UberGraphPages)
					{
						UE_LOG(LogTemp, Log, TEXT("This is a UberGraphPages Graph"));
						// Not really sure what an "UberGraphPage" is just throw an error just in case
					}
					else
					{
						UE_LOG(LogTemp, Error, TEXT("No graph type or graph type not recognized"));
					}
				}

				LooseNodes.Empty();

				AdjacentMergePins.Empty();

				ExistingNodesInSelection.Empty();

				TArray<UEdGraphNode*> SNodes;

				// For all of the selected nodes
				TSet<UObject*> SelectedNodes = Editor->GetSelectedNodes();
				for (TSet<UObject*>::TIterator SelectedIter(SelectedNodes); SelectedIter; ++SelectedIter)
				{
					UEdGraphNode* Node = Cast<UEdGraphNode>(*SelectedIter);
					UObject* Object = *SelectedIter;
					// Make sure the node is valid
					if (!Node) { continue; }

					SNodes.Add(Node);

					// List of UK2Node Types:
					/*
					UK2Node_Variable Get/Set
					UK2Node_StructOperation
					UK2Node_EnumEquality
					UK2Node_EnumInequality
					UK2Node_EnumLiteral
					UK2Node_Self

					UK2Node_MakeVariable
					UK2Node_GetArrayItem

					UK2Node_Function Entry/Result
					UK2Node_MacroInstance
					UK2Node_EditablePinBase
					UK2Node_Event
					UK2Node_ActorBoundEvent
					UK2Node_ComponentBoundEvent
					UK2Node_CustomEvent
					UK2Node_InputActionEvent
					UK2Node_InputAxisEvent
					UK2Node_InputAxisKeyEvent
					UK2Node_InputVectorAxisEvent
					UK2Node_InputKeyEvent
					UK2Node_InputTouchEvent
					UK2Node_FunctionTerminator
					UK2Node_Tunnel
					UK2Node_FunctionTerminator
					UK2Node_FunctionEntry
					UK2Node_FunctionResult

					UK2Node_IfThenElse
					UK2Node_Switch
					UK2Node_ForEachElementInEnum
					UK2Node_ExecutionSequence
					UK2Node_DoOnceMultiInput

					UK2Node_CallFunction
					UK2Node_AddComponent
					UK2Node_AnimGetter
					UK2Node_CallArrayFunction
					UK2Node_CallDataTableFunction
					UK2Node_CallFunctionOnMember
					UK2Node_CallMaterialParameterCollectionFunction
					UK2Node_CallParentFunction
					UK2Node_CommutativeAssociativeBinaryOperator
					UK2Node_GetInputAxisKeyValue
					UK2Node_GetInputVectorAxisValue
					UK2Node_GetInputAxisValue
					UK2Node_Message

					UK2Node_ConvertAsset

					UK2Node_Literal
					UK2Node_Timeline

					*****
					More for the animation blueprints: (Only including 1 node type that all animation blueprint nodes derive from)
					UAnimGraphNode_Base
					*****
					*/

					// List of Pin Types:
					/*
					All pin types which you access via Pin->PinType.PinCategory are stored in UEdGraphSchema_K2

					Go to (EdGraphSchema_K2.h) for more details on Pin Types

					Used to access the pin categories for comparisons "const UEdGraphSchema_K2* K2Schema = GetDefault<UEdGraphSchema_K2>();"
					"if (Pin->PinType.PinCategory == K2Schema->SOMEPINTYPE)

					Applicable types that I'm using are denoted using *

					// Allowable PinType.PinCategory values //
					PC_Exec* // For flow tabs
					PC_Boolean
					PC_Byte
					PC_Class
					PC_AssetClass
					PC_Int
					PC_Float
					PC_Name
					PC_Delegate* // Maybe used for functions?
					PC_MCDelegate* // Maybe used for functions?
					PC_Object
					PC_Interface
					PC_Asset
					PC_String
					PC_Text
					PC_Struct
					PC_Wildcard
					PC_Enum

					// Common PinType.PinSubCategory values //
					PSC_Self
					PSC_Index
					PSC_Bitmask

					// Pin names that have special meaning and required types in some contexts (depending on the node type) //
					PN_Execute
					PN_Then
					PN_Completed
					PN_DelegateEntry
					PN_EntryPoint
					PN_Self
					PN_Else
					PN_Loop
					PN_After
					PN_ReturnValue
					PN_ObjectToCast
					PN_Condition
					PN_Start
					PN_Stop
					PN_Index
					PN_Item
					PN_CastSucceeded
					PN_CastFailed
					PN_CastedValuePrefix
					PN_MatineeFinished
					*/

					// Output the name/type of the node
					if (true)
					{
						// Variable nodes
						if (Object->IsA<UK2Node_Variable>() || Object->IsA<UK2Node_VariableGet>() || Object->IsA<UK2Node_VariableSet>())
						{
							UE_LOG(LogTemp, Warning, TEXT("Variable Node %s"), *Object->GetName());
						}
						// Event Nodes
						else if (Object->IsA<UK2Node_Event>() || Object->IsA<UK2Node_CustomEvent>())
						{
							if (Object->IsA<UK2Node_CustomEvent>())
							{
								UE_LOG(LogTemp, Warning, TEXT("Custom Event Node %s"), *Object->GetName());
							}
							else
								UE_LOG(LogTemp, Warning, TEXT("Event Node %s"), *Object->GetName());
						}
						else if (Object->IsA<UK2Node_FunctionEntry>())
						{
							UE_LOG(LogTemp, Warning, TEXT("Function Entry Node %s"), *Object->GetName());
						}
						else if (Object->IsA<UK2Node_MacroInstance>())
						{
							UE_LOG(LogTemp, Warning, TEXT("Macro Instance Node %s"), *Object->GetName());
						}
						// Function calls
						else if (Object->IsA<UK2Node_CallFunction>())
						{
							UE_LOG(LogTemp, Warning, TEXT("Function Call Node %s"), *Object->GetName());
						}
						else if (Object->IsA<UK2Node_CallFunctionOnMember>())
						{
							UE_LOG(LogTemp, Warning, TEXT("Function Call On Member Node %s"), *Object->GetName());
						}
						// Node types we aren't explictly taking into account
						else
						{
							UE_LOG(LogTemp, Warning, TEXT("Other Type of Node %s"), *Node->GetNodeTitle(ENodeTitleType::Type::ListView).ToString());
						}
						if (Object->IsA<UK2Node_Literal>())
						{
							UE_LOG(LogTemp, Warning, TEXT("+ Literal Node %s"), *Object->GetName());
						}
					}

					// Add all of the nodes that already exist in the target blueprint
					if (AltNodeMasterList::SharedInstance()->DoesAlternativeHaveNode(Node, TargetBP))
					{
						// Add the node that exists in the target blueprint
						ExistingNodesInSelection.Add(Node);

						//UE_LOG(LogTemp, Log, TEXT("Node Added To Existing Node List: %s"), *Node->GetNodeTitle(ENodeTitleType::Type::ListView).ToString());
					}

					// Get the pins from the given node
					TArray<UEdGraphPin*> NodePins = Node->Pins;
					for (int i = 0; i < NodePins.Num(); i++)
					{
						TArray<UEdGraphPin*> LinkedPins = NodePins[i]->LinkedTo;

						for (int j = 0; j < LinkedPins.Num(); j++)
						{
							UEdGraphNode* LinkedNode = LinkedPins[j]->GetOwningNode();
							//UE_LOG(LogTemp, Log, TEXT("LinkedNode: %s"), *LinkedNode->GetName());

							// Remote Pin Index
							int32 PinIndex = LinkedNode->Pins.Find(LinkedPins[j]);

							// check to see if the adjacent node is a variable (Need to fix) VARIABLE AUTO INCLUDE
							/*if (LinkedNode->IsA<UK2Node_Variable>())
							{
								UE_LOG(LogTemp, Warning, TEXT("Adjacent variable node: %s"), *LinkedNode->GetNodeTitle(ENodeTitleType::Type::ListView).ToString());

								UEdGraphNode* NewVarNode = CheckForVariableInTarget(LinkedPins[j]);

								if (NewVarNode == NULL)
									continue;

								UK2Node* K2Node = Cast<UK2Node>(NewVarNode);

								if (K2Node->NodeCausesStructuralBlueprintChange())
								{
									FBlueprintEditorUtils::MarkBlueprintAsStructurallyModified(TargetBP);
								}
								else
								{
									FBlueprintEditorUtils::MarkBlueprintAsModified(TargetBP);
								}

								TargetGraph->AddNode(NewVarNode, true, false);

								AltNodeMasterList::SharedInstance()->AddNode(NewVarNode, LinkedPins[j]->GetOwningNode(), TargetBP);

								AdjacentMergePins.Add(FMergePin(NodePins[i], FNodeAltID(NewVarNode, AltNodeMasterList::SharedInstance()->FindNodeID(NewVarNode)), i, PinIndex));
							}
							else*/
							{
								if (AltNodeMasterList::SharedInstance()->DoesAlternativeHaveNode(LinkedNode, TargetBP))
								{
									//UE_LOG(LogTemp, Log, TEXT("Merge Pin added from: %s to: %s"), *Node->GetName(), *LinkedNode->GetName());

									AdjacentMergePins.Add(FMergePin(NodePins[i], FNodeAltID(LinkedNode, AltNodeMasterList::SharedInstance()->FindNodeID(LinkedNode)), i, PinIndex));
								}
								// If the adjacent node doesn't exist in the target blueprint find the nearest adjacent pin
								else
								{
									//
									TArray<UEdGraphPin*> NearestAdjPins = CheckForNearestAdjacentPin(NodePins[i], NodePins[i]->Direction);

									//UE_LOG(LogTemp, Warning, TEXT("%d Checked for nearest adjacent pin for node: %s, pin %s"), NearestAdjPins.Num(), *LinkedNode->GetName(), *NodePins[i]->GetName());

									// If there were no nearest adj pins avaliable
									if (NodePins[i]->Direction == EGPD_Input && NearestAdjPins.Num() == 0)
									{
										if (LinkedNode->IsA<UK2Node_Variable>())
										{
											if (UK2Node_Variable* VariableNode = Cast<UK2Node_Variable>(LinkedNode))
											{
												FName VariableName = VariableNode->GetFName();
												FGuid VarCheckGuid = FBlueprintEditorUtils::FindMemberVariableGuidByName(TargetBP, VariableName);

												// if the property doesn't exist in the target blueprint
												if (VarCheckGuid == FGuid())
												{
													continue;
												}
											}
										}
										// The pin that the node is ultimately connected to the root node by
										UEdGraphPin* RootNodePin = NULL;
										// Check to see if the node is attached to a root node 
										UEdGraphNode* RootNode = CheckForRootNode(Node, RootNodePin);
										if (RootNode != NULL)
										{
											// Copy over the root node
											TSet<UObject*> RootNodeToMerge;
											FString RootNodeText;
											RootNodeToMerge.Add(Cast<UObject>(RootNode));
											FEdGraphUtilities::ExportNodesToText(RootNodeToMerge, /*out*/ RootNodeText);

											TargetGraph->Modify();

											TSet<UEdGraphNode*> PastedNodes;
											// Paste all of the "NodesToMerge" into the target graph
											FEdGraphUtilities::ImportNodesFromText(TargetGraph, RootNodeText, PastedNodes);
											for (TSet<UEdGraphNode*>::TIterator It(PastedNodes); It; ++It)
											{
												UEdGraphNode* Node = *It;

												Node->SnapToGrid(16.f);

												// Give new node a different Guid from the old one
												Node->CreateNewGuid();
												bool bNeedToModifyStructurally = false;

												UK2Node* K2Node = Cast<UK2Node>(Node);
												if ((K2Node != NULL) && K2Node->NodeCausesStructuralBlueprintChange())
												{
													bNeedToModifyStructurally = true;
												}
												FBlueprintEditorUtils::MarkBlueprintAsModified(TargetBP);
											}
											if (RootNodePin != NULL)
											{
												int32 RootNodeIndex = RootNode->Pins.Find(RootNodePin);

												UE_LOG(LogTemp, Warning, TEXT("Root Node: %s Merge Pin to: %s"), *RootNode->GetName(), *Node->GetName());
												// Make and adjacent merge pin out of the root node and this node
												AdjacentMergePins.Add(FMergePin(NodePins[i], FNodeAltID(RootNode, AltNodeMasterList::SharedInstance()->FindNodeID(RootNode)), i, RootNodeIndex));
											}
											else
											{
												UE_LOG(LogTemp, Warning, TEXT("Root Node Pin for Root Node: %s is NULL"), *RootNode->GetName());
											}
										}
									}

									// go through all of the nearest adjacent pins
									for (auto NearestAdjPin : NearestAdjPins)
									{
										UEdGraphNode* NearestAdjNode = NearestAdjPin->GetOwningNode();
										int32 NearestPinIndex = NearestAdjNode->Pins.Find(NearestAdjPin);

										UE_LOG(LogTemp, Warning, TEXT("Adding Nearest Adjacent Pin: %s, to Merge Pin"), *NearestAdjNode->GetName());

										AdjacentMergePins.Add(FMergePin(NodePins[i],
											FNodeAltID(NearestAdjNode, AltNodeMasterList::SharedInstance()->FindNodeID(NearestAdjNode)), i, NearestPinIndex));
									}
								}
							}
						}
					}

					bool HasInput = false;
					
					for (auto Pin : Node->Pins)
					{
						if (Pin->PinType.PinCategory == K2Schema->PC_Exec && Pin->Direction == EGPD_Input)
						{
							HasInput = true;
							break;
						}
					}

					if (HasInput)
					{
						UEdGraphNode* RootNode = CheckForRootNode(Node);
						if (!RootNodes.Contains(RootNode))
						{
							if (RootNode == NULL)
							{
								LooseNodes.Add(Node);
							}
							else
							{
								RootNodes.Add(RootNode);
							}
						}
					}
				}

				// Remove the nodes from the adjacent nodes list if they already exist in the selection
				for (int i = 0; i < SNodes.Num(); i++)
				{
					for (int j = 0; j < AdjacentMergePins.Num(); j++)
					{
						if (AdjacentMergePins[j].RemoteNodeID.Node == SNodes[i])
						{
							bool Exists = false;
							for (int k = 0; k < ExistingNodesInSelection.Num(); k++)
							{
								if (ExistingNodesInSelection[k] == SNodes[i])
								{
									Exists = true;
									UE_LOG(LogTemp, Warning, TEXT("Adjacent Node: %s is contained in ExistingNodesInSelection"), *SNodes[i]->GetNodeTitle(ENodeTitleType::Type::ListView).ToString());
									break;
								}
							}
							if (Exists)
								continue;

							AdjacentMergePins.RemoveAt(j);
						}
					}
				}
				/* After we are done going through all of the selected nodes, check the target graph for the adjacent nodes */

				// Either the selected nodes are a disconnected segment or are only variables, no root nodes
				if (RootNodes.Num() == 0)
				{
					UE_LOG(LogTemp, Warning, TEXT("No Root Nodes Found"));
				}

				// All selected nodes belong to the same flow 
				else if (RootNodes.Num() == 1)
				{
					UE_LOG(LogTemp, Warning, TEXT("1 Root node found: %s"), *RootNodes[0]->GetNodeTitle(ENodeTitleType::Type::ListView).ToString());
				}

				// Selected nodes belong to different flows
				else
				{
					UE_LOG(LogTemp, Warning, TEXT("%d root nodes found"), RootNodes.Num());
					for (auto RootNode : RootNodes)
					{
						UE_LOG(LogTemp, Warning, TEXT("Root Node: %s"), *RootNode->GetNodeTitle(ENodeTitleType::Type::ListView).ToString());
					}
				}

				// Start to handle the target graph
				HandleTargetGraph();
			}
		}
		else { UE_LOG(LogTemp, Warning, TEXT("No nodes selected")); }
	}
}

void SAltMergeView::HandleTargetGraph()
{
	// NOTE: Should probably make this functional for multiple targets by adding the "Target Graph" as a parameter
	if (!TargetGraph) { UE_LOG(LogTemp, Error, TEXT("Target Graph Not Valid")); return; }

	TSet<UObject*> SelectedNodes = Editor->GetSelectedNodes();

	TSet<UObject*> NodesToMerge;

	// If there are root nodes associated with the merged nodes
	if (RootNodes.Num() >= 0)
	{
		UE_LOG(LogTemp, Log, TEXT("Start Merge Number of Selected Nodes: %d"), SelectedNodes.Num());
		// Save all of the nodes as text to transfer over to the target graph
		FString NodeText;
		for (TSet<UObject*>::TIterator SelectedIter(SelectedNodes); SelectedIter; ++SelectedIter)
		{
			UEdGraphNode* Node = Cast<UEdGraphNode>(*SelectedIter);
			UObject* Object = *SelectedIter;
			// Make sure the node is valid
			if (!Node) { continue; }

			bool bCreatedVariable = false;

			Node->PrepareForCopying();

			// Check for pasted variable nodes
			if (Node->IsA<UK2Node_VariableGet>() || Node->IsA<UK2Node_VariableSet>())
			{
				AddMemberVariableFromNode(Node);

				//UEdGraphNode* NewVarNode = CheckForVariableInTarget(Node);

				//if (NewVarNode == NULL)
				//	goto AfterVar;

				//UK2Node* K2Node = Cast<UK2Node>(NewVarNode);

				//if (K2Node->NodeCausesStructuralBlueprintChange())
				//{
				//	FBlueprintEditorUtils::MarkBlueprintAsStructurallyModified(TargetBP);
				//}
				//else
				//{
				//	FBlueprintEditorUtils::MarkBlueprintAsModified(TargetBP);
				//}

				//if (NewVarNode != NULL)
				//{
				//	//TargetGraph->AddNode(NewVarNode, true, false);

				//	AltNodeMasterList::SharedInstance()->AddNode(NewVarNode, Node, TargetBP);
				//	
				//	//for (auto Pin : Node->Pins)
				//	//{
				//	//	for (auto LinkedPin : Pin->LinkedTo)
				//	//	{
				//	//		int32 LocalPinIndex = Node->Pins.Find(Pin);
				//	//		int32 RemotePinIndex = LinkedPin->GetOwningNode()->Pins.Find(LinkedPin);
				//	//
				//	//		UE_LOG(LogTemp, Log, TEXT("Variable Node Merge Pin Added from: %s to: %s"), *Node->GetName(), *LinkedPin->GetOwningNode()->GetName());
				//	//
				//	//		AdjacentMergePins.Add(FMergePin(Pin, FNodeAltID(NewVarNode, AltNodeMasterList::SharedInstance()->FindNodeID(NewVarNode)), LocalPinIndex, RemotePinIndex));
				//	//	}
				//	//}
				//}
				//continue;
			}
			// Check to see if the node exists in the other alternative
			if (!AltNodeMasterList::SharedInstance()->DoesAlternativeHaveNode(Node, TargetBP))
			{
				NodesToMerge.Add(Object);
				AltNodeMasterList::SharedInstance()->AddNode(Node, BaseBP);
				UE_LOG(LogTemp, Log, TEXT("Node Added To Merge: %s Friendly Name: %s"), *Node->GetName(), *Node->GetNodeTitle(ENodeTitleType::Type::ListView).ToString());
			}
		}

		FEdGraphUtilities::ExportNodesToText(NodesToMerge, /*out*/ NodeText);

		bool bNeedToModifyStructurally = false;
		TargetGraph->Modify();

		TSet<UEdGraphNode*> PastedNodes;

		// Paste all of the "NodesToMerge" into the target graph
		FEdGraphUtilities::ImportNodesFromText(TargetGraph, NodeText, PastedNodes);

		UE_LOG(LogTemp, Log, TEXT("Number of pasted nodes: %d"), PastedNodes.Num());

		for (auto PastedNode : PastedNodes)
		{
			UE_LOG(LogTemp, Log, TEXT("Pasted node in the target BP: %s %s"), *PastedNode->GetName(), *PastedNode->GetNodeTitle(ENodeTitleType::ListView).ToString());
		}

		UE_LOG(LogTemp, Log, TEXT("Number of Existing nodes: %d"), ExistingNodesInSelection.Num());

		for (auto ExistingNode : ExistingNodesInSelection)
		{
			UE_LOG(LogTemp, Log, TEXT("Existing node in the target BP: %s %s"), *ExistingNode->GetName(), *ExistingNode->GetNodeTitle(ENodeTitleType::ListView).ToString());
		}

		// Handle the selected nodes that already existed
		for (int i = 0; i < ExistingNodesInSelection.Num(); i++)
		{
			//UE_LOG(LogTemp, Log, TEXT("Number of AdjacentMergePins: %d"), AdjacentMergePins.Num());
			// Find the original node from the base blueprint
			UEdGraphNode* TargetNode =
				AltNodeMasterList::SharedInstance()->GetNodeWithID(AltNodeMasterList::SharedInstance()->FindNodeID(ExistingNodesInSelection[i]), TargetBP);

			for (int j = 0; j > ExistingNodesInSelection[i]->Pins.Num(); j++)
			{
				// Set the default values to be the same
				TargetNode->Pins[j]->DefaultValue = ExistingNodesInSelection[i]->Pins[j]->DefaultValue;
			}

			for (int j = 0; j < AdjacentMergePins.Num(); j++)
			{
				// Handle all of the adjacent variables that might need to be added
				UEdGraphNode* AdjacentNode = AltNodeMasterList::SharedInstance()->GetNodeWithID(AdjacentMergePins[j].RemoteNodeID.ID, TargetBP);

				// Make sure that we are working with the correct node
				if (AltNodeMasterList::SharedInstance()->FindNodeID(TargetNode) ==
					AltNodeMasterList::SharedInstance()->FindNodeID(AdjacentMergePins[j].Pin->GetOwningNode()))
				{
					if (TargetNode->Pins[AdjacentMergePins[j].LocalPinIndex]->Direction == EGPD_Output)
					{
						if (TargetNode->Pins[AdjacentMergePins[j].LocalPinIndex]->PinType.PinCategory == K2Schema->PC_Exec)
						{
							TargetNode->Pins[AdjacentMergePins[j].LocalPinIndex]->BreakAllPinLinks();
							AdjacentNode->Pins[AdjacentMergePins[j].RemotePinIndex]->BreakAllPinLinks();
						}
						TargetNode->Pins[AdjacentMergePins[j].LocalPinIndex]->MakeLinkTo(AdjacentNode->Pins[AdjacentMergePins[j].RemotePinIndex]);

						//UE_LOG(LogTemp, Warning, TEXT("%d %d ENode: %s attached to nodeEO: %s"), AdjacentMergePins[j].LocalPinIndex, AdjacentMergePins[j].RemotePinIndex, *TargetNode->GetName(), *AdjacentNode->GetName());
					}
					else if (TargetNode->Pins[AdjacentMergePins[j].LocalPinIndex]->Direction == EGPD_Input)
					{
						TargetNode->Pins[AdjacentMergePins[j].LocalPinIndex]->BreakAllPinLinks();
						AdjacentNode->Pins[AdjacentMergePins[j].RemotePinIndex]->BreakAllPinLinks();

						AdjacentNode->Pins[AdjacentMergePins[j].RemotePinIndex]->MakeLinkTo(TargetNode->Pins[AdjacentMergePins[j].LocalPinIndex]);

						//UE_LOG(LogTemp, Warning, TEXT("%d %d Node: %s attached to nodeEI: %s"), AdjacentMergePins[j].LocalPinIndex, AdjacentMergePins[j].RemotePinIndex, *AdjacentNode->GetName(), *TargetNode->GetName());
					}
				}
				else
				{
					UE_LOG(LogTemp, Error, TEXT("Node is not right E: %s"), *TargetNode->GetName());
				}
			}
		}

		// Handle the pasted nodes
		for (TSet<UEdGraphNode*>::TIterator It(PastedNodes); It; ++It)
		{
			UEdGraphNode* Node = *It;

			int32 i = It.ElementIt.GetIndex();

			if (!AltNodeMasterList::SharedInstance()->AddNode(Node, Cast<UEdGraphNode>(NodesToMerge[FSetElementId::FromInteger(i)]), TargetBP))
			{
				continue;
			}
			// Account for the differing values in the pins if there are Adjacent Nodes
			if (AdjacentMergePins.Num() > 0)
			{
				//UE_LOG(LogTemp, Log, TEXT("Number of AdjacentMergePins: %d"), AdjacentMergePins.Num());
				for (int j = 0; j < AdjacentMergePins.Num(); j++)
				{
					UEdGraphNode* AdjacentNode = AltNodeMasterList::SharedInstance()->GetNodeWithID(AdjacentMergePins[j].RemoteNodeID.ID, TargetBP);

					// Make sure that we are working with the correct node
					if (AltNodeMasterList::SharedInstance()->FindNodeID(Node) ==
						AltNodeMasterList::SharedInstance()->FindNodeID(AdjacentMergePins[j].Pin->GetOwningNode()))
					{
						// Make the connections between the nodes that were and weren't in the selection
						if (Node->Pins[AdjacentMergePins[j].LocalPinIndex]->Direction == EGPD_Output)
						{
							if (Node->Pins[AdjacentMergePins[j].LocalPinIndex]->PinType.PinCategory == K2Schema->PC_Exec)
							{
								Node->Pins[AdjacentMergePins[j].LocalPinIndex]->BreakAllPinLinks();
								AdjacentNode->Pins[AdjacentMergePins[j].RemotePinIndex]->BreakAllPinLinks();

								// Check to see if there are more than one input pins for the original node
								if (AltNodeMasterList::SharedInstance()->GetNodeWithID(AdjacentMergePins[j].RemoteNodeID.ID, BaseBP))
								{

								}
							}

							Node->Pins[AdjacentMergePins[j].LocalPinIndex]->MakeLinkTo(AdjacentNode->Pins[AdjacentMergePins[j].RemotePinIndex]);

							//UE_LOG(LogTemp, Warning, TEXT("%d %d Node: %s attached to nodePO: %s"), AdjacentMergePins[j].LocalPinIndex, AdjacentMergePins[j].RemotePinIndex, *Node->GetName(), *AdjacentNode->GetName());
						}
						else if (Node->Pins[AdjacentMergePins[j].LocalPinIndex]->Direction == EGPD_Input)
						{
							Node->Pins[AdjacentMergePins[j].LocalPinIndex]->BreakAllPinLinks();
							AdjacentNode->Pins[AdjacentMergePins[j].RemotePinIndex]->BreakAllPinLinks();

							AdjacentNode->Pins[AdjacentMergePins[j].RemotePinIndex]->MakeLinkTo(Node->Pins[AdjacentMergePins[j].LocalPinIndex]);

							//UE_LOG(LogTemp, Warning, TEXT("%d %d Node: %s attached to nodePI: %s"), AdjacentMergePins[j].LocalPinIndex, AdjacentMergePins[j].RemotePinIndex, *AdjacentNode->GetName(), *Node->GetName());
						}
						//else
							//UE_LOG(LogTemp, Error, TEXT("Node: %s doesn't have a direction"), *Node->GetName());
					}
					else
					{
						//UE_LOG(LogTemp, Error, TEXT("Node is not right P: %s"), *Node->GetName());
					}
				}
			}
			// MAYBE FIX THIS LATER (needed for some cases)
			// Adjust Node positions
			//Node->NodePosX = (GraphLocation.X - AvgNodePosition.X);
			//Node->NodePosY = (GraphLocation.Y - AvgNodePosition.Y);

			Node->SnapToGrid(16.0f);

			// Give new node a different Guid from the old one
			Node->CreateNewGuid();

			UK2Node* K2Node = Cast<UK2Node>(Node);
			if ((K2Node != NULL) && K2Node->NodeCausesStructuralBlueprintChange())
			{
				bNeedToModifyStructurally = true;
			}
			FBlueprintEditorUtils::MarkBlueprintAsModified(TargetBP);
		}
	}

	// Check to see if the Root node exists in the target graph 
	for (auto Node : TargetGraph->Nodes)
	{
		for (auto RootNode : RootNodes)
		{
			if (GraphType == EBlueprintEditorGraph::Type::Function)
			{
				// Ask the user which graph to merge the selection to
				// Create the function?
			}
			else if (GraphType == EBlueprintEditorGraph::Type::Macro)
			{
				// Ask the user which graph to merge the selection to
				// Create the Macro?
			}
			else if (GraphType == EBlueprintEditorGraph::Type::Event)
			{
				// Does the root node exist in the target blueprint?
				if (Node->GetNodeTitle(ENodeTitleType::Type::ListView).EqualTo(RootNode->GetNodeTitle(ENodeTitleType::Type::ListView)))
				{
					// If there is the same graph find the existing node in the target graph

				}
				else
				{
					// Is the root node a custom event?
					if (RootNode->IsA<UK2Node_CustomEvent>())
					{

					}
					else
					{

					}
				}
			}
			else if (GraphType == EBlueprintEditorGraph::Type::UberGraphPages)
			{
				// Uber graph is the master graph that the blueprint ultimately uses
				// For pasted Event nodes, we need to see if there is an already existing node in a disabled state that needs to be cleaned up
				if (UK2Node_Event* EventNode = Cast<UK2Node_Event>(Node))
				{
					// Gather all existing event nodes
					TArray<UK2Node_Event*> ExistingEventNodes;
					FBlueprintEditorUtils::GetAllNodesOfClass<UK2Node_Event>(TargetBP, ExistingEventNodes);

					for (UK2Node_Event* ExistingEventNode : ExistingEventNodes)
					{
						check(ExistingEventNode);

						bool bIdenticalNode = EventNode != ExistingEventNode && ExistingEventNode->bOverrideFunction && UK2Node_Event::AreEventNodesIdentical(EventNode, ExistingEventNode);

						// Check if the nodes are identical, if they are we need to delete the original because it is disabled. Identical nodes that are in an enabled state will never make it this far and still be enabled.
						if (bIdenticalNode)
						{
							// Should not have made it to being a pasted node if the pre-existing node wasn't disabled or was otherwise explicitly disabled by the user.
							ensure(!ExistingEventNode->IsNodeEnabled());
							ensure(!ExistingEventNode->bUserSetEnabledState);

							// Destroy the pre-existing node, we do not need it.
							ExistingEventNode->DestroyNode();
						}
					}
				}
			}
			else
			{
				UE_LOG(LogTemp, Error, TEXT("Idk what the root node is supposed to be"));
				break;
			}
		}
	}

	UE_LOG(LogTemp, Log, TEXT("Number of loose nodes to merge: %d"), LooseNodes.Num());

	// Handle all of the "loose" nodes (nodes that aren't connected to a root node)
	if (LooseNodes.Num() > 0)
	{
		TSet<UObject*> LooseNodesToMerge;

		FString NodeText;
		for (auto LooseNode : LooseNodes)
		{
			UObject* Object = Cast<UObject>(LooseNode);

			// Make sure the node is valid
			if (!LooseNode) { continue; }

			LooseNode->PrepareForCopying();

			// Check to see if the node exists in the other alternative
			if (!AltNodeMasterList::SharedInstance()->DoesAlternativeHaveNode(LooseNode, TargetBP))
			{
				LooseNodesToMerge.Add(Object);
				UE_LOG(LogTemp, Log, TEXT("Loose Node Added To Merge: %s"), *LooseNode->GetNodeTitle(ENodeTitleType::Type::ListView).ToString());
			}
		}

		FEdGraphUtilities::ExportNodesToText(LooseNodesToMerge, /*out*/ NodeText);

		bool bNeedToModifyStructurally = false;
		TargetGraph->Modify();

		TSet<UEdGraphNode*> PastedNodes;
		FEdGraphUtilities::ImportNodesFromText(TargetGraph, NodeText, PastedNodes);

		for (TSet<UEdGraphNode*>::TIterator It(PastedNodes); It; ++It)
		{
			UEdGraphNode* Node = *It;

			Node->SnapToGrid(16.f);

			// Give new node a different Guid from the old one
			Node->CreateNewGuid();

			UK2Node* K2Node = Cast<UK2Node>(Node);
			if ((K2Node != NULL) && K2Node->NodeCausesStructuralBlueprintChange())
			{
				bNeedToModifyStructurally = true;
			}
			FBlueprintEditorUtils::MarkBlueprintAsModified(TargetBP);
		}
	}
}

void SAltMergeView::ResolveMerge(UBlueprint* TargetBlueprint)
{
	UPackage* Package = TargetBlueprint->GetOutermost();
	TArray<UPackage*> PackagesToSave;
	PackagesToSave.Add(Package);

	FEditorFileUtils::EPromptReturnCode const SaveResult = FEditorFileUtils::PromptForCheckoutAndSave(PackagesToSave, /*bCheckDirty=*/ false, /*bPromptToSave=*/ false);
	if (SaveResult != FEditorFileUtils::PR_Success)
	{
		const FText ErrorMessage = FText::Format(LOCTEXT("MergeWriteFailedError", "Failed to write merged files, please look for backups in {0}"), FText::FromString(BackupSubDir));

		FNotificationInfo ErrorNotification(ErrorMessage);
		FSlateNotificationManager::Get().AddNotification(ErrorNotification);
	}

	// Open the Target Blueprint to see the changes
	FAssetEditorManager::Get().OpenEditorForAsset(TargetBlueprint);

	// Close the merge tab
	//PluginInstance->CloseSelectiveMergeTab();
}

bool SAltMergeView::CanMergeNodes() const
{
	check(Editor.IsValid());

	const FGraphPanelSelectionSet SelectedNodes = Editor->GetSelectedNodes();

	for (FGraphPanelSelectionSet::TConstIterator SelectedIter(SelectedNodes); SelectedIter; ++SelectedIter)
	{
		UEdGraphNode* Node = Cast<UEdGraphNode>(*SelectedIter);
		if ((Node != NULL) && Node->CanDuplicateNode())
		{
			return true;
		}
	}
	return false;
}

void SAltMergeView::RefreshLists()
{
	TargetMergeComboBox->RefreshOptions();
}

UEdGraphNode* SAltMergeView::CheckForRootNode(UEdGraphNode* N, UEdGraphPin* P)
{
	bool noInput = true;

	// Check all of the Pins of the given node
	for (auto Pin : N->Pins)
	{
		if (Pin->Direction == EGPD_Input)
		{
			// Check all of the linked pins
			for (auto LinkedPin : Pin->LinkedTo)
			{
				UEdGraphNode* LinkedNode = LinkedPin->GetOwningNode();
				UObject* LinkedObject = Cast<UObject>(LinkedNode);

				//
				// Probably need to add a few more node types to these cases
				//

				// Check to see if the linked node is the root node
				if (LinkedObject->IsA<UK2Node_FunctionEntry>() || LinkedObject->IsA<UK2Node_Event>() || LinkedObject->IsA<UK2Node_Tunnel>())
				{
					return LinkedNode;
				}
				// If the linked node isn't any node types that are specified, check the next pin
				else if (LinkedObject->IsA<UK2Node_VariableGet>() || LinkedObject->IsA<UK2Node_Self>() || LinkedObject->IsA<UK2Node_GetArrayItem>()
					|| LinkedObject->IsA<UK2Node_MakeVariable>())
				{
					UE_LOG(LogTemp, Log, TEXT("Linked Node %s's Pin %s cannot check for the root node"), *LinkedNode->GetName(), *Pin->PinName);
				}
				// Is this a node that we can keep looking through?
				else
				{ 
					// Recursion is spooky
					UEdGraphNode* DeeperNode = CheckForRootNode(LinkedNode, P);

					noInput = false;

					if (DeeperNode != NULL)
					{
						P = LinkedPin;
						return DeeperNode;
					}
				}
			}
		}
		else
		{
			// if the node is just an entry point return itself
			if (N->IsA<UK2Node_FunctionEntry>() || N->IsA<UK2Node_Event>() || N->IsA<UK2Node_Tunnel>())
				return N;

			continue;
		}
	}

	// If we get here, that means that there are no input pins for the node and we didn't reach a proper root node
	if (noInput)
	{
		UE_LOG(LogTemp, Warning, TEXT("No root node found for: %s"), *N->GetName());
		return NULL;
	}
	UE_LOG(LogTemp, Warning, TEXT("ROOT NODE CHECK WTF"));
	return NULL;
}

TArray<UEdGraphPin*> SAltMergeView::CheckForNearestAdjacentPin(UEdGraphPin* P, EEdGraphPinDirection Dir)
{
	TArray<UEdGraphPin*> APins;

	// Check to see if the pin is not a flow pin, if not don't go any deeper
	if (P->PinType.PinCategory != K2Schema->PC_Exec)
	{
		for (auto LPin : P->LinkedTo)
		{
			if (AltNodeMasterList::SharedInstance()->DoesAlternativeHaveNode(LPin->GetOwningNode(), TargetBP))
			{
				APins.Add(LPin);
			}
		}
		return APins;
	}

	// If it IS a flow pin
	else
	{
		if (Dir == EGPD_Input)
		{
			// Check all of the linked pins
			for (auto LinkedPin : P->LinkedTo)
			{
				UEdGraphNode* LinkedNode = LinkedPin->GetOwningNode();
				for (auto LinkedNodePin : LinkedNode->Pins)
				{
					if (LinkedNodePin->PinType.PinCategory != K2Schema->PC_Exec)
					{
						// if we are looking at a pin with the wrong direction just look at the next
						UE_LOG(LogTemp, Log, TEXT("Adj Skip In Type"));
						continue;
					}

					// Get the node that is connected to the node that you are already connected to
					UEdGraphNode* LinkedNodePinNode = LinkedNodePin->GetOwningNode();

					if (AltNodeMasterList::SharedInstance()->DoesAlternativeHaveNode(LinkedNodePinNode, TargetBP))
					{
						if (LinkedNodePin->Direction == P->Direction)
						{
							// if we are looking at a pin with the wrong direction just look at the next
							UE_LOG(LogTemp, Log, TEXT("Adj Skip In Dir"));
							continue;
						}

						// if the target blueprint does have a node just return it
						UE_LOG(LogTemp, Log, TEXT("Add Pin: %s to Nearest from node: %s"), *LinkedNodePin->GetName(), *LinkedNodePinNode->GetName());
						APins.Add(LinkedNodePin);

						break;
					}
					else
					{
						if (LinkedNodePin->Direction != P->Direction)
						{
							// if we are looking at a pin with the wrong direction just look at the next
							UE_LOG(LogTemp, Log, TEXT("Adj Skip In Dir"));
							continue;
						}

						// Recursion is spooky
						TArray<UEdGraphPin*> DeeperPins = CheckForNearestAdjacentPin(LinkedNodePin, Dir);

						for (auto DeeperPin : DeeperPins)
						{
							if (DeeperPin != NULL)
								APins.Add(DeeperPin);
						}
					}
				}
			}
			return APins;
		}
		else if (Dir == EGPD_Output)
		{
			// Check all of the linked pins
			for (auto LinkedPin : P->LinkedTo)
			{
				UEdGraphNode* LinkedNode = LinkedPin->GetOwningNode();
				for (auto LinkedNodePin : LinkedNode->Pins)
				{
					if (LinkedNodePin->PinType.PinCategory != K2Schema->PC_Exec)
					{
						// if we are looking at a pin with the wrong direction just look at the next
						UE_LOG(LogTemp, Log, TEXT("Adj Skip Out Type"));
						continue;
					}

					// Get the node that is connected to the node that you are already connected to
					UEdGraphNode* LinkedNodePinNode = LinkedNodePin->GetOwningNode();

					if (AltNodeMasterList::SharedInstance()->DoesAlternativeHaveNode(LinkedNodePinNode, TargetBP))
					{
						if (LinkedNodePin->Direction == P->Direction)
						{
							// if we are looking at a pin with the wrong direction just look at the next
							UE_LOG(LogTemp, Log, TEXT("Adj Skip Out Dir"));
							continue;
						}

						// if the target blueprint does have a node just return it
						UE_LOG(LogTemp, Log, TEXT("Add Pin: %s to Nearest from node: %s"), *LinkedNodePin->GetName(), *LinkedNodePinNode->GetName());
						APins.Add(LinkedNodePin);

						break;
					}
					else
					{
						if (LinkedNodePin->Direction != P->Direction)
						{
							// if we are looking at a pin with the wrong direction just look at the next
							UE_LOG(LogTemp, Log, TEXT("Adj Skip In Dir"));
							continue;
						}

						// Recursion is spooky
						TArray<UEdGraphPin*> DeeperPins = CheckForNearestAdjacentPin(LinkedNodePin, Dir);

						for (auto DeeperPin : DeeperPins)
						{
							if (DeeperPin != NULL)
								APins.Add(DeeperPin);
						}
					}
				}
			}
			return APins;
		}
	}

	// If we get here just return an empty array
	UE_LOG(LogTemp, Error, TEXT("ADJACENT NODE CHECK FAILED ON PIN: %s"), *P->GetName());
	return TArray<UEdGraphPin*>();

}

UEdGraphNode* SAltMergeView::CheckForVariableInTarget(UEdGraphPin* P)
{
	UEdGraphNode* N = P->GetOwningNode();

	check(N);
	if (!TargetGraph || !TargetBP)
	{
		UE_LOG(LogTemp, Error, TEXT("Target Blueprint or Target Graph is invalid."));
		return NULL;
	}

	if (UK2Node_Variable* VariableNode = Cast<UK2Node_Variable>(N))
	{
		FName VariableName = VariableNode->GetFName();
		FGuid VarCheckGuid = FBlueprintEditorUtils::FindMemberVariableGuidByName(TargetBP, VariableName);

		UStruct* Scope = BaseBP->GetClass();

		UProperty* Prop = VariableNode->GetPropertyForVariable();
		FName NewVarMemberName = Prop->GetFName();
		FString VarDefaultValue;
		uint8* ContainerPtr = reinterpret_cast<uint8*>(TargetBP->GeneratedClass->GetDefaultObject());
		FBlueprintEditorUtils::PropertyValueToString(Prop, ContainerPtr, VarDefaultValue);

		// if the property doesn't exist in the target blueprint add a new member variable
		if (VarCheckGuid == FGuid())
		{
			UE_LOG(LogTemp, Warning, TEXT("Property: %s not found in Target Blueprint"), *NewVarMemberName.ToString());

			// Used for promoting to local variable
			UEdGraph* FunctionGraph = nullptr;

			// Prep for modification
			TargetBP->Modify();
			TargetGraph->Modify();

			bool bWasSuccessful = false;
			FEdGraphPinType NewPinType = P->PinType;
			NewPinType.bIsConst = false;
			NewPinType.bIsReference = false;
			NewPinType.bIsWeakPointer = false;

			bWasSuccessful = FBlueprintEditorUtils::AddMemberVariable(TargetBP, NewVarMemberName, NewPinType, VarDefaultValue);

			if (bWasSuccessful)
			{
				// Create the new setter node
				FEdGraphSchemaAction_K2NewNode NodeInfo;

				// Create get or set node, depending on whether we clicked on an input or output pin
				UK2Node_Variable* TemplateNode = NULL;
				if (P->Direction == EGPD_Input)
				{
					TemplateNode = NewObject<UK2Node_VariableSet>();
				}
				else
				{
					TemplateNode = NewObject<UK2Node_VariableGet>();
				}
				TemplateNode->VariableReference.SetSelfMember(NewVarMemberName);
				NodeInfo.NodeTemplate = TemplateNode;

				// Set position of new node to be close to node we clicked on
				FVector2D NewNodePos;
				NewNodePos.X = N->NodePosX;
				NewNodePos.Y = N->NodePosY;

				UEdGraphNode* NewNode = NodeInfo.PerformAction(TargetGraph, P, NewNodePos, false);

				Editor->RenameNewlyAddedAction(NewVarMemberName);

				return NewNode;
			}
		}
		else
		{
			/*
			if the target property does exist in the target blueprint but the node doesn't exist in the target graph
			return a new node that was made from the property
			*/

			// Prep for modification of the graph
			TargetGraph->Modify();

			// Create the new setter node
			FEdGraphSchemaAction_K2NewNode NodeInfo;

			// Create get or set node, depending on whether we clicked on an input or output pin
			UK2Node_Variable* TemplateNode = NULL;
			if (P->Direction == EGPD_Input)
			{
				TemplateNode = NewObject<UK2Node_VariableSet>();
			}
			else
			{
				TemplateNode = NewObject<UK2Node_VariableGet>();
			}
			TemplateNode->VariableReference.SetSelfMember(NewVarMemberName);

			NodeInfo.NodeTemplate = TemplateNode;

			// Set position of new node to be close to node we clicked on
			FVector2D NewNodePos;
			//NewNodePos.X = (P->Direction == EGPD_Output) ? N->NodePosX - 200 : N->NodePosX + 400;
			
			NewNodePos.X = N->NodePosX;
			NewNodePos.Y = N->NodePosY;

			UEdGraphNode* NewNode = NodeInfo.PerformAction(TargetGraph, P, NewNodePos, false);

			Editor->RenameNewlyAddedAction(NewVarMemberName);

			return NewNode;
		}
	}
	else
	{
		UE_LOG(LogTemp, Error, TEXT("Node: %s is not a variable type"), *N->GetName());
		return NULL;
	}
	UE_LOG(LogTemp, Error, TEXT("CheckForVariableInTarget went really wrong"));
	return NULL;
}

UEdGraphNode* SAltMergeView::CheckForVariableInTarget(UEdGraphNode* N)
{
	check(N);
	if (!TargetGraph || !TargetBP)
	{
		UE_LOG(LogTemp, Error, TEXT("Target Blueprint or Target Graph is invalid."));
		return NULL;
	}

	if (UK2Node_Variable* VariableNode = Cast<UK2Node_Variable>(N))
	{
		FName VariableName = VariableNode->GetFName();
		FGuid VarCheckGuid = FBlueprintEditorUtils::FindMemberVariableGuidByName(TargetBP, VariableName);

		UStruct* Scope = BaseBP->GetClass();

		UProperty* Prop = VariableNode->GetPropertyForVariable();
		FName NewVarMemberName = Prop->GetFName();
		FString VarDefaultValue;
		uint8* ContainerPtr = reinterpret_cast<uint8*>(TargetBP->GeneratedClass->GetDefaultObject());
		FBlueprintEditorUtils::PropertyValueToString(Prop, ContainerPtr, VarDefaultValue);

		UEdGraphPin* P = NULL;

		for (auto Pin : N->Pins)
		{
			if (Pin->PinType.PinCategory != K2Schema->PC_Exec)
			{
				P = Pin;
				break;
			}
		}
		// if the property doesn't exist in the target blueprint add a new member variable
		if (VarCheckGuid == FGuid())
		{
			UE_LOG(LogTemp, Warning, TEXT("Property: %s not found in Target Blueprint"), *NewVarMemberName.ToString());

			// Used for promoting to local variable
			UEdGraph* FunctionGraph = nullptr;

			// Prep for modification
			TargetBP->Modify();
			TargetGraph->Modify();

			bool bWasSuccessful = false;
			FEdGraphPinType NewPinType = P->PinType;
			NewPinType.bIsConst = false;
			NewPinType.bIsReference = false;
			NewPinType.bIsWeakPointer = false;

			bWasSuccessful = FBlueprintEditorUtils::AddMemberVariable(TargetBP, NewVarMemberName, NewPinType, VarDefaultValue);

			if (bWasSuccessful)
			{
				UE_LOG(LogTemp, Log, TEXT("New property: %s added to target blueprint: %s"), *Prop->GetName(), *TargetBP->GetName());
				// Create the new setter node
				FEdGraphSchemaAction_K2NewNode NodeInfo;

				// Create get or set node, depending on whether we clicked on an input or output pin
				UK2Node_Variable* TemplateNode = NULL;
				if (N->IsA<UK2Node_VariableGet>())
				{
					TemplateNode = NewObject<UK2Node_VariableGet>();
				}
				else if (N->IsA<UK2Node_VariableSet>())
				{
					TemplateNode = NewObject<UK2Node_VariableSet>();
				}
				TemplateNode->VariableReference.SetSelfMember(NewVarMemberName);
				NodeInfo.NodeTemplate = TemplateNode;

				// Set position of new node to be close to node we clicked on
				FVector2D NewNodePos;
				NewNodePos.X = N->NodePosX;
				NewNodePos.Y = N->NodePosY;

				return NodeInfo.PerformAction(TargetGraph, P, NewNodePos, false);
			}
		}
		else
		{
			/*
			if the target property does exist in the target blueprint but the node doesn't exist in the target graph
			return a new node that was made from the property
			*/

			// Prep for modification of the graph
			TargetBP->Modify();
			TargetGraph->Modify();

			// Create the new setter node
			FEdGraphSchemaAction_K2NewNode NodeInfo;

			// Create get or set node, depending on whether we clicked on an input or output pin
			UK2Node_Variable* TemplateNode = NULL;
			if (N->IsA<UK2Node_VariableGet>())
			{
				TemplateNode = NewObject<UK2Node_VariableGet>();
			}
			else if (N->IsA<UK2Node_VariableSet>())
			{
				TemplateNode = NewObject<UK2Node_VariableSet>();
			}
			TemplateNode->VariableReference.SetSelfMember(NewVarMemberName);

			NodeInfo.NodeTemplate = TemplateNode;

			// Set position of new node to be close to node we clicked on
			FVector2D NewNodePos;
			//NewNodePos.X = (P->Direction == EGPD_Output) ? N->NodePosX - 200 : N->NodePosX + 400;

			NewNodePos.X = N->NodePosX;
			NewNodePos.Y = N->NodePosY;

			return NodeInfo.PerformAction(TargetGraph, P, NewNodePos, false);
		}
	}
	else
	{
		UE_LOG(LogTemp, Error, TEXT("Node: %s is not a variable type"), *N->GetName());
		return NULL;
	}
	UE_LOG(LogTemp, Error, TEXT("CheckForVariableInTargetN went really wrong"));
	return NULL;
}

void SAltMergeView::AddMemberVariableFromNode(UEdGraphNode* N)
{
	check(N);
	if (!TargetGraph || !TargetBP)
	{
		UE_LOG(LogTemp, Error, TEXT("Target Blueprint or Target Graph is invalid."));
		return;
	}

	if (UK2Node_Variable* VariableNode = Cast<UK2Node_Variable>(N))
	{
		FName VariableName = VariableNode->GetFName();
		FGuid VarCheckGuid = FBlueprintEditorUtils::FindMemberVariableGuidByName(TargetBP, VariableName);

		UStruct* Scope = BaseBP->GetClass();

		UProperty* Prop = VariableNode->GetPropertyForVariable();
		FName NewVarMemberName = Prop->GetFName();
		FString VarDefaultValue;
		uint8* ContainerPtr = reinterpret_cast<uint8*>(TargetBP->GeneratedClass->GetDefaultObject());
		FBlueprintEditorUtils::PropertyValueToString(Prop, ContainerPtr, VarDefaultValue);

		UEdGraphPin* P = NULL;

		for (auto Pin : N->Pins)
		{
			if (Pin->PinType.PinCategory != K2Schema->PC_Exec)
			{
				P = Pin;
				break;
			}
		}
		// if the property doesn't exist in the target blueprint add a new member variable
		if (VarCheckGuid == FGuid())
		{
			UE_LOG(LogTemp, Warning, TEXT("Property: %s not found in Target Blueprint"), *NewVarMemberName.ToString());

			// Used for promoting to local variable
			UEdGraph* FunctionGraph = nullptr;

			// Prep for modification
			TargetBP->Modify();
			TargetGraph->Modify();

			bool bWasSuccessful = false;
			FEdGraphPinType NewPinType = P->PinType;
			NewPinType.bIsConst = false;
			NewPinType.bIsReference = false;
			NewPinType.bIsWeakPointer = false;

			bWasSuccessful = FBlueprintEditorUtils::AddMemberVariable(TargetBP, NewVarMemberName, NewPinType, VarDefaultValue);
		}
		else
		{
			/*
			if the target property does exist in the target blueprint but the node doesn't exist in the target graph
			return a new node that was made from the property
			*/

			UE_LOG(LogTemp, Warning, TEXT("Property: %s already EXISTS in Target Blueprint"), *NewVarMemberName.ToString());
		}
	}
}

void SAltMergeView::OnTargetSelectionChanged(TSharedPtr<FString> NewValue, ESelectInfo::Type)
{
	UE_LOG(LogTemp, Warning, TEXT("Selection Changed to: %s"), **NewValue.Get());

	TargetItem = NewValue;

	FString Name = *NewValue.Get();

	if (Name != FString("Not Selected"))
	{
		UE_LOG(LogTemp, Warning, TEXT("Selection is valid, good to merge!"));
		canStartMerge = true;
	}
	else
	{
		UE_LOG(LogTemp, Warning, TEXT("Selection is NOT valid, CANNOT merge!"));
	}
}

TSharedRef<SWidget> SAltMergeView::OnGenerateAlternativesComboBox(TSharedPtr<FString> InItem)
{
	if (InItem.IsValid())
	{
		UE_LOG(LogTemp, Warning, TEXT("Generate Merge ComboBox Item: %s"), **InItem.Get());
		return SNew(STextBlock).Text(FText::FromString(*InItem.Get()));
	}

	else
		return SNew(STextBlock).Text(FText::FromString(TEXT("Merge Bad Box")));
}

void SAltMergeView::OnFinishMerge()
{

}

FText SAltMergeView::CreateTargetMergeComboBoxContent() const
{
	return TargetItem.IsValid() ? FText::FromString(*TargetItem.Get()) : LOCTEXT("TargetAltNotSelected", "Not selected");
}

#undef LOCTEXT_NAMESPACE