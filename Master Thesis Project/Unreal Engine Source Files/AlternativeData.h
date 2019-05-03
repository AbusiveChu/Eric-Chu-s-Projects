#pragma once

#include "CoreMinimal.h"
#include "UObject/ObjectMacros.h"
#include "UObject/Object.h"
#include "UObject/UObjectGlobals.h"
#include "Templates/SubclassOf.h"
#include "UObject/Class.h"
#include "Misc/StringAssetReference.h"
#include "UObject/Package.h"
#include "UObject/ObjectRedirector.h"
#include "Misc/PackageName.h"
#include "UObject/LinkerLoad.h"
#include "SharedMapView.h"
#include "Engine/Engine.h"
#include "Misc/MessageDialog.h"

namespace EAlternativeActivationMethod
{
	enum Type
	{
		DoubleClicked,
		EnterPressed,
		Previewed
	};
}


// Storing data about each alternative saved
// UCLASS(config=Engine)
class UAlternativeData
{
	//GENERATED_UCLASS_BODY()
public:
	UBlueprint* AlternativeBP;
	
	/** Name of this alternative object */
	FString AlternativeName;

	/** Name of this alternative object's class */
	const FName OriginalClassName;

	/** Is parallel editing enabled for this blueprint? ( Not Implemented yet)*/
	bool ParallelEditingEnabled = false;

	/** Is this a base class' data or an alternative's? */
	bool isBaseClass = false;

	/** Keep track of the index that this alternative is in the master list*/
	int32 mIndex = -1;

	/** Keep track of the index that this alternative is relative to its base class */
	int32 index = -1;

	/** Used to make sure that all alternatives of a base class have the same list of alternatives */
	TArray<UAlternativeData> BaseBPAlternatives;

	/** The Blueprint that this Blueprint Alternative was created from */
	const UBlueprint* ParentBlueprint;

	/** The Original Blueprint that this alternative is derived from */
	const UBlueprint* OriginalBlueprint;

public:
	/** Default Constructor */
	UAlternativeData()
	{}

	/** Prototype Constructor */
	UAlternativeData(FString InAlternativeName)
	{
		AlternativeName = InAlternativeName;
	}

	/** Constructor */
	UAlternativeData(FString InAlternativeName, const UObject* InOriginalBP, const UObject* InParentBP, bool IsBaseClass)
	{
		AlternativeName = InAlternativeName;
		OriginalBlueprint = Cast<UBlueprint>(InOriginalBP);
		isBaseClass = IsBaseClass;
		ParentBlueprint = Cast<UBlueprint>(InParentBP);
	}

	void GetBaseBlueprint(const UBlueprint* OriginalBP)
	{
		OriginalBP = OriginalBlueprint;
	}

	// Switch the actor that would be spawn from the world outliner with the proper alternative
	void AlternativeSwitch()
	{
		// Always check GEngine because it can be 0
		check(GEngine)
		//GEngine->OnPlayInEditorStart().AddSP(this, );
	}
};


// Master list of all alternatives created (static instance of this class in SAlternativeView)
class AlternativeMasterList
{
public:
	TArray<TSharedPtr<UAlternativeData>> MasterList;

	void AddAlternative(TSharedPtr<UAlternativeData> Alt)
	{
		if (Alt.IsValid())
		{
			EAppReturnType::Type MakeAlt = EAppReturnType::Type::No;
			if (MasterList.Contains(Alt))
			{
				MakeAlt = FMessageDialog::Open(EAppMsgType::YesNo, FText::FromString("Alternative already exists would you like to create an Alternative anyways?"), &FText::FromString("Alternative Already Exists"));	
			}

			if (!MasterList.Contains(Alt) || MakeAlt == EAppReturnType::Type::Yes);
			{
				MasterList.Add(Alt);

				bool ContainsBase = false;

				for (auto BP : BaseBPs)
				{
					if (BP == Alt->OriginalBlueprint)
					{
						int32 index;
						BaseBPs.Find(BP, index);
						ContainsBase = true;
						break;
					}
				}

				if (!ContainsBase)
				{
					// Used to copy the blueprint data
					UBlueprint* temp;
					memcpy(temp, Alt->OriginalBlueprint, sizeof(Alt->OriginalBlueprint));
					// Add a new base blueprint 
					BaseBPs.Add(temp);
					TArray<TSharedPtr<UAlternativeData>> NewAltList;
					NewAltList.Add(Alt);
					SortedAlternatives.Add(NewAltList);
				}
			}
		}
	}

private:
	TArray<UBlueprint*> BaseBPs;

	TArray<TArray<TSharedPtr<UAlternativeData>>> SortedAlternatives;
};