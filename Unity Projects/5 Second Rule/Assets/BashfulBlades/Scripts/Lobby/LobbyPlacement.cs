using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LobbyPlacement : MonoBehaviour
{
    public int slotNum;

    public static int locallyOccupied = 0;
    public bool Occupied = false;

    public static bool changePlaces = false;
    public Text LocalLobbyName;
    public Text LocalChar;

    void LateUpdate()
    {
        //clicked = false;
        if (changePlaces)
            changePlaces = false;
    }

    public void slotRecv(int SlotNum, string Name, int Character)
    {
        if (SlotNum == slotNum)
        {
            Occupied = true;
            LocalLobbyName.text = Name;
            if (Character == 1)
            {
                LocalChar.text = "Broc Lee";
            }
            if (Character == 2)
            {
                LocalChar.text = "Hambo";
            }
            if (Character == 3)
            {
                LocalChar.text = "Carrot";
            }
            if (Character == 4)
            {
                LocalChar.text = "Chicken";
            }
        }
        else
        {
            if (locallyOccupied != slotNum)
            {
                ClearSlot();
            }
            Occupied = false;
        }
    }

    public void ClearSlot()
    {
        LocalLobbyName.text = "Open";
        LocalChar.text = "";
        Occupied = false;
    }

    public void MoveToSlot()
    {
        if (SetLevel.CharSelect > 0)
        {
            if (!Occupied)
            {
                if (ChangeName.NameChanged == false)
                {
                    LocalLobbyName.text = "Player " + slotNum;
                    ChangeName.Name = LocalLobbyName.text;
                }
                else if (ChangeName.NameChanged == true)
                {
                    LocalLobbyName.text = ChangeName.Name;
                }
                if (SetLevel.CharSelect == 0)
                {
                    LocalChar.text = "None";
                }
                if (SetLevel.CharSelect == 1)
                {
                    LocalChar.text = "Broc Lee";
                }
                if (SetLevel.CharSelect == 2)
                {
                    LocalChar.text = "Hambo";
                }
                if (SetLevel.CharSelect == 3)
                {
                    LocalChar.text = "Carrot";
                }
                if (SetLevel.CharSelect == 4)
                {
                    LocalChar.text = "Chicken";
                }

                locallyOccupied = slotNum;

                //clicked = true;
                Occupied = true;
                changePlaces = true;
            }
        }
    }
}