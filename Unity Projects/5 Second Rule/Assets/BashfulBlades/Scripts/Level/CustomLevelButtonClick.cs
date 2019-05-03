using UnityEngine;
using System.Collections;

public class CustomLevelButtonClick : MonoBehaviour {

    public void loadVert(int Vert)
    {      
        CustomLevelScript.VerticalGrid = Vert;
    }
    public void loadHori(int Hori)
    {
        CustomLevelScript.HorizontalGrid = Hori;     
    }
}
