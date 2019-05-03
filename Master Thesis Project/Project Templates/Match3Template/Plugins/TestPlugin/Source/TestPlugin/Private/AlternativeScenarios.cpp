// Fill out your copyright notice in the Description page of Project Settings.

#include "AlternativeScenarios.h"

void AlternativeScenario::AddActor(AActor* Actor, int32 AltIndex)
{
	ScenarioActorIDs.Add(Actor->GetName());
	ScenarioAltList.Add(AltIndex);
}


void AlternativeScenario::Construct(FString ScenarioName, int32 index)
{
	Name = ScenarioName;

	Index = index;
}

int32 AlternativeScenario::FindActorAltIndex(FString ActorID)
{
	if (ScenarioActorIDs.Contains(ActorID))
		return ScenarioActorIDs.Find(ActorID);

	else
		return -1;
}