// Copyright 1998-2017 Epic Games, Inc. All Rights Reserved.
/*===========================================================================
	Generated code exported from UnrealHeaderTool.
	DO NOT modify this manually! Edit the corresponding .h files instead!
===========================================================================*/

#include "ObjectMacros.h"
#include "ScriptMacros.h"

PRAGMA_DISABLE_DEPRECATION_WARNINGS
class AActor;
#ifdef TESTPLUGIN_DelegateObject_generated_h
#error "DelegateObject.generated.h already included, missing '#pragma once' in DelegateObject.h"
#endif
#define TESTPLUGIN_DelegateObject_generated_h

#define HostProject_Plugins_TestPlugin_Source_TestPlugin_Classes_DelegateObject_h_16_RPC_WRAPPERS \
 \
	DECLARE_FUNCTION(execOnActorDestroyed) \
	{ \
		P_GET_OBJECT(AActor,Z_Param_DestroyedActor); \
		P_FINISH; \
		P_NATIVE_BEGIN; \
		this->OnActorDestroyed(Z_Param_DestroyedActor); \
		P_NATIVE_END; \
	}


#define HostProject_Plugins_TestPlugin_Source_TestPlugin_Classes_DelegateObject_h_16_RPC_WRAPPERS_NO_PURE_DECLS \
 \
	DECLARE_FUNCTION(execOnActorDestroyed) \
	{ \
		P_GET_OBJECT(AActor,Z_Param_DestroyedActor); \
		P_FINISH; \
		P_NATIVE_BEGIN; \
		this->OnActorDestroyed(Z_Param_DestroyedActor); \
		P_NATIVE_END; \
	}


#define HostProject_Plugins_TestPlugin_Source_TestPlugin_Classes_DelegateObject_h_16_INCLASS_NO_PURE_DECLS \
private: \
	static void StaticRegisterNativesUDelegateObject(); \
	friend TESTPLUGIN_API class UClass* Z_Construct_UClass_UDelegateObject(); \
public: \
	DECLARE_CLASS(UDelegateObject, UObject, COMPILED_IN_FLAGS(0), 0, TEXT("/Script/TestPlugin"), NO_API) \
	DECLARE_SERIALIZER(UDelegateObject) \
	enum {IsIntrinsic=COMPILED_IN_INTRINSIC};


#define HostProject_Plugins_TestPlugin_Source_TestPlugin_Classes_DelegateObject_h_16_INCLASS \
private: \
	static void StaticRegisterNativesUDelegateObject(); \
	friend TESTPLUGIN_API class UClass* Z_Construct_UClass_UDelegateObject(); \
public: \
	DECLARE_CLASS(UDelegateObject, UObject, COMPILED_IN_FLAGS(0), 0, TEXT("/Script/TestPlugin"), NO_API) \
	DECLARE_SERIALIZER(UDelegateObject) \
	enum {IsIntrinsic=COMPILED_IN_INTRINSIC};


#define HostProject_Plugins_TestPlugin_Source_TestPlugin_Classes_DelegateObject_h_16_STANDARD_CONSTRUCTORS \
	/** Standard constructor, called after all reflected properties have been initialized */ \
	NO_API UDelegateObject(const FObjectInitializer& ObjectInitializer = FObjectInitializer::Get()); \
	DEFINE_DEFAULT_OBJECT_INITIALIZER_CONSTRUCTOR_CALL(UDelegateObject) \
	DECLARE_VTABLE_PTR_HELPER_CTOR(NO_API, UDelegateObject); \
DEFINE_VTABLE_PTR_HELPER_CTOR_CALLER(UDelegateObject); \
private: \
	/** Private move- and copy-constructors, should never be used */ \
	NO_API UDelegateObject(UDelegateObject&&); \
	NO_API UDelegateObject(const UDelegateObject&); \
public:


#define HostProject_Plugins_TestPlugin_Source_TestPlugin_Classes_DelegateObject_h_16_ENHANCED_CONSTRUCTORS \
	/** Standard constructor, called after all reflected properties have been initialized */ \
	NO_API UDelegateObject(const FObjectInitializer& ObjectInitializer = FObjectInitializer::Get()) : Super(ObjectInitializer) { }; \
private: \
	/** Private move- and copy-constructors, should never be used */ \
	NO_API UDelegateObject(UDelegateObject&&); \
	NO_API UDelegateObject(const UDelegateObject&); \
public: \
	DECLARE_VTABLE_PTR_HELPER_CTOR(NO_API, UDelegateObject); \
DEFINE_VTABLE_PTR_HELPER_CTOR_CALLER(UDelegateObject); \
	DEFINE_DEFAULT_OBJECT_INITIALIZER_CONSTRUCTOR_CALL(UDelegateObject)


#define HostProject_Plugins_TestPlugin_Source_TestPlugin_Classes_DelegateObject_h_16_PRIVATE_PROPERTY_OFFSET
#define HostProject_Plugins_TestPlugin_Source_TestPlugin_Classes_DelegateObject_h_13_PROLOG
#define HostProject_Plugins_TestPlugin_Source_TestPlugin_Classes_DelegateObject_h_16_GENERATED_BODY_LEGACY \
PRAGMA_DISABLE_DEPRECATION_WARNINGS \
public: \
	HostProject_Plugins_TestPlugin_Source_TestPlugin_Classes_DelegateObject_h_16_PRIVATE_PROPERTY_OFFSET \
	HostProject_Plugins_TestPlugin_Source_TestPlugin_Classes_DelegateObject_h_16_RPC_WRAPPERS \
	HostProject_Plugins_TestPlugin_Source_TestPlugin_Classes_DelegateObject_h_16_INCLASS \
	HostProject_Plugins_TestPlugin_Source_TestPlugin_Classes_DelegateObject_h_16_STANDARD_CONSTRUCTORS \
public: \
PRAGMA_ENABLE_DEPRECATION_WARNINGS


#define HostProject_Plugins_TestPlugin_Source_TestPlugin_Classes_DelegateObject_h_16_GENERATED_BODY \
PRAGMA_DISABLE_DEPRECATION_WARNINGS \
public: \
	HostProject_Plugins_TestPlugin_Source_TestPlugin_Classes_DelegateObject_h_16_PRIVATE_PROPERTY_OFFSET \
	HostProject_Plugins_TestPlugin_Source_TestPlugin_Classes_DelegateObject_h_16_RPC_WRAPPERS_NO_PURE_DECLS \
	HostProject_Plugins_TestPlugin_Source_TestPlugin_Classes_DelegateObject_h_16_INCLASS_NO_PURE_DECLS \
	HostProject_Plugins_TestPlugin_Source_TestPlugin_Classes_DelegateObject_h_16_ENHANCED_CONSTRUCTORS \
private: \
PRAGMA_ENABLE_DEPRECATION_WARNINGS


#undef CURRENT_FILE_ID
#define CURRENT_FILE_ID HostProject_Plugins_TestPlugin_Source_TestPlugin_Classes_DelegateObject_h


PRAGMA_ENABLE_DEPRECATION_WARNINGS
