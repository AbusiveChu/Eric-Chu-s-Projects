// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "UObject/NoExportTypes.h"
#include "Modules/ModuleManager.h"
#include "UObject/UObjectBase.h"

class UBlueprint;

#define LOCTEXT_NAMESPACE "BPAltData"

/**
 * Structure that holds all of the nessisary Blueprint Alternative Data 
 */

class TESTPLUGIN_API UBPAltData
{
public:
	/** Default Constructor */
	UBPAltData();

///////////////////
//// Variables ////
///////////////////
public:
	UBlueprint* AlternativeBP;

	/** Name of this alternative object */
	FString AlternativeName;

	/** Name of this alternative object's class */
	const FName OriginalClassName;

	/** Is parallel editing enabled for this blueprint? ( Not Implemented yet) */
	bool ParallelEditingEnabled = false;

	/** Is this a base class' data or an alternative's? */
	bool isBaseClass = false;

	/** Is this alternative data good to use? */
	bool isValid;

	/** The index of the parent blueprint alternative */
	int32 parentIndex = -1;

	/** Keep track of the index that this alternative is relative to its base class */
	int32 sIndex = -1;

	/** Keep track of the index that this alternative is in the master list */
	int32 index = -1;

	/** Name of the class of this alternative */
	FString ClassName;

	/** The blueprints that we can switch between, for this particular alternative */
	TArray<UObject*> BlueprintAlternatives;

///////////////////
//// Functions ////
///////////////////
public:

	/** Construct specifically for Base Data being contructed */
	void ConstructBaseData(FString InAlternativeName, int32 InIndex, UBlueprint* InAlternativeBlueprint);

	/* Grab data using this function because UObjects can't use constructors with parameters */
	void ConstructData(FString InAlternativeName, UBlueprint* InAlternativeBP, 
		TArray<UObject*> Alts, int32 InIndex, int32 InSortedIndex, int32 InParentIndex);

	/* Called when blueprints are compiled do before the other constructs */
	void ReconstructData(FString InAlternativeName, UBlueprint* InAlternativeBP, int32 Index);

};

#undef LOCTEXT_NAMESPACE