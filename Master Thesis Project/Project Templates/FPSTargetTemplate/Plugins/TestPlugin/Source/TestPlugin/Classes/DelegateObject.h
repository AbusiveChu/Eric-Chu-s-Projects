// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Engine/Engine.h"
#include "UObject/NoExportTypes.h"
#include "DelegateObject.generated.h"

/**
 * 
 */
UCLASS()
class TESTPLUGIN_API UDelegateObject : public UObject
{
	GENERATED_BODY()
public:
	static UDelegateObject* SharedInstance() { if (!inst) inst = NewObject<UDelegateObject>(); return inst; }

	void BindActorDestroyed(AActor* DestroyedActor, AActor* NewActor, FString Name);

	UFUNCTION()
	void OnActorDestroyed(AActor* DestroyedActor);
	
	TArray<AActor*> DestroyList;
	TArray<AActor*> NewActorList;

	TArray<FString> NewNameList;
	
private:
	static UDelegateObject* inst;

};
