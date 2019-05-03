// Fill out your copyright notice in the Description page of Project Settings.

#include "TestPlugin/Classes/BPAltData.h"

UBPAltData::UBPAltData()
{}

/** Add values after you've already created an instance of UBPAltData */
void UBPAltData::ConstructBaseData(FString InAlternativeName, int32 InIndex, UBlueprint* InAlternativeBlueprint)
{
	AlternativeName = InAlternativeName;
	AlternativeBP = InAlternativeBlueprint;

	index = InIndex;

	if (InAlternativeName.Contains(TEXT("(Base Class)")))
	{
		FString ObjectName, Right;

		InAlternativeName.Split(TEXT("(Base Class)"), &ObjectName, &Right);
		ClassName = ObjectName;
	}
	else
		ClassName = InAlternativeName;

	ClassName += TEXT("_C");

	// if this is the base alternative
	sIndex = 0;
}

void UBPAltData::ConstructData(FString InAlternativeName, UBlueprint* InAlternativeBP,
	TArray<UObject*> Alts, int32 InIndex, int32 InSortedIndex, int32 InParentIndex)
{
	AlternativeName = InAlternativeName;
	BlueprintAlternatives = Alts;
	AlternativeBP = InAlternativeBP;

	if (InAlternativeName.Contains(TEXT("(Base Class)")))
	{
		FString ObjectName, Right;

		InAlternativeName.Split(TEXT("(Base Class)"),&ObjectName, &Right);
		ClassName = ObjectName;
	}
	else
		ClassName = InAlternativeName;

	ClassName += TEXT("_C");

	index = InIndex;

	sIndex = InSortedIndex;

	parentIndex = InParentIndex;
}

void UBPAltData::ReconstructData(FString InAlternativeName, UBlueprint* InAlternativeBP, int32 Index)
{
	AlternativeName = InAlternativeName;

	AlternativeBP = InAlternativeBP;

	index = Index;
}