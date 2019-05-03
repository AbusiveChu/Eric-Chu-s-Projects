// Fill out your copyright notice in the Description page of Project Settings.

#include "DelegateObject.h"

UDelegateObject* UDelegateObject::inst;

void UDelegateObject::BindActorDestroyed(AActor* DestroyedActor, AActor* NewActor, FString Name)
{
	DestroyList.Add(DestroyedActor);

	NewActorList.Add(NewActor);
	NewNameList.Add(Name);

	DestroyedActor->OnDestroyed.AddDynamic(this, &UDelegateObject::OnActorDestroyed);
}

void UDelegateObject::OnActorDestroyed(AActor* DestroyedActor)
{
	if (DestroyList.Contains(DestroyedActor))
	{
		int32 i = DestroyList.Find(DestroyedActor);

		NewActorList[i]->Rename(*NewNameList[i]);

		DestroyList.RemoveAt(i, 1, true);

		NewActorList.RemoveAt(i, 1, true);
		NewNameList.RemoveAt(i, 1, true);
	}
}