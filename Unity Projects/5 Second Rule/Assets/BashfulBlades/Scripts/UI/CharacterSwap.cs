using UnityEngine;
using System.Collections;

public class CharacterSwap : MonoBehaviour
{



    // Use this for initialization
    public void SwitchCharacter(int CharacterSelect)
    {
        SetLevel.Hambo = false;
        SetLevel.Broco = false;
        SetLevel.Chicken = false;
        SetLevel.Carrot = false;
        SetLevel.CharSelect = CharacterSelect;
        if (CharacterSelect == 1)
        {
            SetLevel.Broco = true;           
        }
        else if (CharacterSelect == 2)
        {
            SetLevel.Hambo = true;
        }
        else if (CharacterSelect == 3)
        {
            SetLevel.Carrot = true;
        }
        else if (CharacterSelect == 4)
        {
            SetLevel.Chicken = true;
        }

    }
}
