using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class itemnow : MonoBehaviour
{

    public Text[] ItemCost;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ItemCost[0].text = "Blackpepper bombs cost: " + ItemScript.BPCost;
        ItemCost[1].text = "Garlic Cloves cost: " + ItemScript.GCCost;
        ItemCost[2].text = "Mousetraps cost: " + ItemScript.MTCost;
        ItemCost[3].text = "Honeycombs cost: " + ItemScript.HCCost;

    }
}
