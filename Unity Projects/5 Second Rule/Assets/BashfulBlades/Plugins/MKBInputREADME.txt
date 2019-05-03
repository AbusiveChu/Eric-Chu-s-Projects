MKBInputREADME.txt
by, Eric Chu	
	
	//Set the active window
	bool INPUTDLL_API WINAPI SetHookTarget(HWND wHnd);

	//Deallocate the Hook Target
	void INPUTDLL_API WINAPI Shutdown();

	//Keyboard functions
	bool INPUTDLL_API WINAPI GetKDown(char key);			//All 256 Ascii keys
	
	bool INPUTDLL_API WINAPI GetSKDown(char* key);			//For spectial keys that aren't ascii
	*Disclaimer: Only "Space" and "Escape" have been implemented*
	
	//////---------------------
	
	//Mouse Functions

	int INPUTDLL_API WINAPI GetMouseY();
	int INPUTDLL_API WINAPI GetMouseX();

	bool INPUTDLL_API WINAPI IsMouseMoving();

	bool INPUTDLL_API WINAPI GetMBDown(int button);
	/*
	Left Mouse Button   = 0
	Right Mouse Button  = 1
	Middle Mouse Button = 2
	*/
	//////---------------------

*ProTip: when using C# replace all HWND types with System.IntPtr instead*