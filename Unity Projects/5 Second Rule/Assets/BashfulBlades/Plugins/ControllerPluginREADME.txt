Controller Plugin by, Eric Chu 	version: November 4

*Disclaimer* This only works for 1 controller right now will add support for more controllers as required.

Functions in the dll:

void Initialize(int controllerNum);						//Initialize the controller instance

void Vibrate(int leftRumble, int RightRumble);			//Vibrate the controller 

bool IsConnected();										//Checks to see if the controller is connected

int GetLeftJoyStickX();									/////
int GetLeftJoyStickY();									//Returns a value between -32768 and 32767
int GetRightJoyStickX();								//For the Corresonding Stick and Axis
int GetRightJoyStickY();								/////

int GetLeftTriggerThreshold();							//Get how much the trigger is being pressed currently
int GetRightTriggerThreshold();							//

bool ControllerManager.GetButtonDown(int buttonName);						
bool GetButtonReleased(int buttonName);

Buttons go as follows:
0	1	2	3	4	5	6	7	8		9		10		11		12			13			14			15			16
A	B	X	Y	LT	LB	RT	RB	LStick	        RStick	        Back	        Start	     XboxButton               Dpad-Up		     Dpad-Down	               Dpad-Left	  Dpad-Right

I did have an enum representing each button name, but transfering an enum to Unity = :(



How to use the controller plug-in:

1. Put Somthing that looks like this in your controller script:
	 [DllImport("ControllerPlugin")]
		static extern [function type] [function name(function parameters)]

2. Use the Initiatize(int controllerNum); function in Start() or Awake() 

3. Use the functions as needed 


Enjoy ;)