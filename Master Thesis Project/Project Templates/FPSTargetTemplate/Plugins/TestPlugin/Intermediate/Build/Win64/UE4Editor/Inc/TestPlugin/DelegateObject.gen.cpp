// Copyright 1998-2017 Epic Games, Inc. All Rights Reserved.
/*===========================================================================
	Generated code exported from UnrealHeaderTool.
	DO NOT modify this manually! Edit the corresponding .h files instead!
===========================================================================*/

#include "GeneratedCppIncludes.h"
#include "Classes/DelegateObject.h"
PRAGMA_DISABLE_OPTIMIZATION
#ifdef _MSC_VER
#pragma warning (push)
#pragma warning (disable : 4883)
#endif
PRAGMA_DISABLE_DEPRECATION_WARNINGS
void EmptyLinkFunctionForGeneratedCodeDelegateObject() {}
// Cross Module References
	TESTPLUGIN_API UFunction* Z_Construct_UFunction_UDelegateObject_OnActorDestroyed();
	TESTPLUGIN_API UClass* Z_Construct_UClass_UDelegateObject();
	ENGINE_API UClass* Z_Construct_UClass_AActor_NoRegister();
	TESTPLUGIN_API UClass* Z_Construct_UClass_UDelegateObject_NoRegister();
	COREUOBJECT_API UClass* Z_Construct_UClass_UObject();
	UPackage* Z_Construct_UPackage__Script_TestPlugin();
// End Cross Module References
	void UDelegateObject::StaticRegisterNativesUDelegateObject()
	{
		UClass* Class = UDelegateObject::StaticClass();
		static const TNameNativePtrPair<ANSICHAR> AnsiFuncs[] = {
			{ "OnActorDestroyed", (Native)&UDelegateObject::execOnActorDestroyed },
		};
		FNativeFunctionRegistrar::RegisterFunctions(Class, AnsiFuncs, ARRAY_COUNT(AnsiFuncs));
	}
	UFunction* Z_Construct_UFunction_UDelegateObject_OnActorDestroyed()
	{
		struct DelegateObject_eventOnActorDestroyed_Parms
		{
			AActor* DestroyedActor;
		};
		UObject* Outer = Z_Construct_UClass_UDelegateObject();
		static UFunction* ReturnFunction = nullptr;
		if (!ReturnFunction)
		{
			ReturnFunction = new(EC_InternalUseOnlyConstructor, Outer, TEXT("OnActorDestroyed"), RF_Public|RF_Transient|RF_MarkAsNative) UFunction(FObjectInitializer(), nullptr, (EFunctionFlags)0x00020401, 65535, sizeof(DelegateObject_eventOnActorDestroyed_Parms));
			UProperty* NewProp_DestroyedActor = new(EC_InternalUseOnlyConstructor, ReturnFunction, TEXT("DestroyedActor"), RF_Public|RF_Transient|RF_MarkAsNative) UObjectProperty(CPP_PROPERTY_BASE(DestroyedActor, DelegateObject_eventOnActorDestroyed_Parms), 0x0010000000000080, Z_Construct_UClass_AActor_NoRegister());
			ReturnFunction->Bind();
			ReturnFunction->StaticLink();
#if WITH_METADATA
			UMetaData* MetaData = ReturnFunction->GetOutermost()->GetMetaData();
			MetaData->SetValue(ReturnFunction, TEXT("ModuleRelativePath"), TEXT("Classes/DelegateObject.h"));
#endif
		}
		return ReturnFunction;
	}
	UClass* Z_Construct_UClass_UDelegateObject_NoRegister()
	{
		return UDelegateObject::StaticClass();
	}
	UClass* Z_Construct_UClass_UDelegateObject()
	{
		static UClass* OuterClass = NULL;
		if (!OuterClass)
		{
			Z_Construct_UClass_UObject();
			Z_Construct_UPackage__Script_TestPlugin();
			OuterClass = UDelegateObject::StaticClass();
			if (!(OuterClass->ClassFlags & CLASS_Constructed))
			{
				UObjectForceRegistration(OuterClass);
				OuterClass->ClassFlags |= (EClassFlags)0x20100080u;

				OuterClass->LinkChild(Z_Construct_UFunction_UDelegateObject_OnActorDestroyed());

				OuterClass->AddFunctionToFunctionMapWithOverriddenName(Z_Construct_UFunction_UDelegateObject_OnActorDestroyed(), "OnActorDestroyed"); // 2245586856
				static TCppClassTypeInfo<TCppClassTypeTraits<UDelegateObject> > StaticCppClassTypeInfo;
				OuterClass->SetCppTypeInfo(&StaticCppClassTypeInfo);
				OuterClass->StaticLink();
#if WITH_METADATA
				UMetaData* MetaData = OuterClass->GetOutermost()->GetMetaData();
				MetaData->SetValue(OuterClass, TEXT("IncludePath"), TEXT("DelegateObject.h"));
				MetaData->SetValue(OuterClass, TEXT("ModuleRelativePath"), TEXT("Classes/DelegateObject.h"));
#endif
			}
		}
		check(OuterClass->GetClass());
		return OuterClass;
	}
	IMPLEMENT_CLASS(UDelegateObject, 3229746555);
	static FCompiledInDefer Z_CompiledInDefer_UClass_UDelegateObject(Z_Construct_UClass_UDelegateObject, &UDelegateObject::StaticClass, TEXT("/Script/TestPlugin"), TEXT("UDelegateObject"), false, nullptr, nullptr, nullptr);
	DEFINE_VTABLE_PTR_HELPER_CTOR(UDelegateObject);
PRAGMA_ENABLE_DEPRECATION_WARNINGS
#ifdef _MSC_VER
#pragma warning (pop)
#endif
PRAGMA_ENABLE_OPTIMIZATION
