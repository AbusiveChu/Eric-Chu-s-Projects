// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Engine/Engine.h"
#include "LevelEditor.h"
#include "Editor.h"

/**
 * Structure that holds all of the Alternative Scenario Data
 */
class TESTPLUGIN_API AlternativeScenario
{
public:
	AlternativeScenario() {}

	AlternativeScenario(FString ScenarioName, int32 index) { Name = ScenarioName; Index = index; }

	~AlternativeScenario() {}

	// int32: index of each actor's alternative
	TArray<int32> ScenarioAltList;

	// FString: The name of the actor
	TArray<FString> ScenarioActorIDs;

	int32 FindActorAltIndex(FString ActorID);

	FString Name;

	int32 Index;

	void AddActor(AActor* Actor, int32 AltIndex);

	// Create all of the required data for the scenario
	void Construct(FString ScenarioName, int32 index);
};