using UnityEngine;
using System.Collections;

public class MorphDemoGUI : MonoBehaviour {

    public Transform morphObject;
    public GameObject modelObject;

    void OnGUI()
    {
        SkinnedMorphTargets morphTarget = morphObject.GetComponent<SkinnedMorphTargets>();
		if (morphTarget == null) 
		{
			return;
		}
        GUI.Label(new Rect(120, 20, 100, 40), "Morph Targets");

        
        GUI.Label(new Rect(20, 70, 220, 30), "Neutral");
        GUI.HorizontalSlider(new Rect(200, 70, 250, 30), morphTarget.neutralWeight, 0, 1);

        int start = 110;

        for (int i = 0; i < morphTarget.morphTargets.Length; i++)
        {
            GUI.Label(new Rect(20, start, 200, 30), morphTarget.morphTargets[i].name);
            morphTarget.blendWeights[i] =
                GUI.HorizontalSlider(new Rect(220, start, 250, 30), morphTarget.blendWeights[i], 0, 1);
            start += 40;
        }

        GUI.Label(new Rect(Screen.width - 120, 20, 100, 40), "Animations");
        if (GUI.Button(new Rect(Screen.width - 120, 70, 100, 40), "headbump"))
        {
            modelObject.GetComponent<Animation>().CrossFade("headBump");
        }
        if (GUI.Button(new Rect(Screen.width - 120, 120, 100, 40), "jogging"))
        {
            modelObject.GetComponent<Animation>().CrossFade("jogging");
        }
        if (GUI.Button(new Rect(Screen.width - 120, 170, 100, 40), "RotAnim"))
        {
            modelObject.GetComponent<Animation>().CrossFade("RotAnim");
        }
        if (GUI.Button(new Rect(Screen.width - 120, 220, 100, 40), "TransAnim"))
        {
            modelObject.GetComponent<Animation>().CrossFade("TransAnim");
        }

    }
}
