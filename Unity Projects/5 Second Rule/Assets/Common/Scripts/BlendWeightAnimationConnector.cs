using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//A class for connecting between the locators exported via Maya and
//the morph target blend weights.
//Note : When this is active, it overrides the values every frame

public class BlendWeightAnimationConnector : MonoBehaviour 
{  
    /// The morph target script that will have its values controlled

    private SkinnedMorphTargets morphTargets = null;

    /// The prefix of the blend weight transform. PREFIXabc will map to weight abc
    private const string BLEND_SHAPE_ANIM_PREFIX = "BlendWeight_";

    /// Simple structure to contain 
   
    private class BlendWeightLink
    {
        public Transform sourceTransform;
        public int dstWeightIndex;
    }
    /// <summary>
    /// The links that will be set
    /// </summary>
    private List<BlendWeightLink> blendWeightLinks;

    // Use this for initialization
	void Start () {
        morphTargets = gameObject.GetComponent<SkinnedMorphTargets>();
        if (morphTargets == null)
        {
            Debug.LogWarning("Not connected to morph targets. Not doing anything");
            return;
        }
        InitializeLinks();
	}

    
	// Update is called once per frame
	void Update () {
        if (morphTargets == null)
        {
            return;
        }
        foreach (BlendWeightLink link in blendWeightLinks)
        {
            morphTargets.blendWeights[link.dstWeightIndex] = link.sourceTransform.localPosition.y;
        }
	}

    /// <summary>
    /// Initialize the links between the child transforms and the blend weights
    /// </summary>
    private void InitializeLinks()
    {
        blendWeightLinks = new List<BlendWeightLink>();
        Transform[] childTransforms = gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform transform in childTransforms)
        {
            if (!transform.name.StartsWith(BLEND_SHAPE_ANIM_PREFIX))
            {
                continue;
            }
            string blendShapeAnimName = transform.name.Substring(BLEND_SHAPE_ANIM_PREFIX.Length);
            for (int i = 0; i < morphTargets.morphTargets.Length; i++)
            {
                if (blendShapeAnimName.Equals(morphTargets.morphTargets[i].name,
                    System.StringComparison.OrdinalIgnoreCase))
                {
                    BlendWeightLink link = new BlendWeightLink();
                    link.sourceTransform = transform;
                    link.dstWeightIndex = i;
                    blendWeightLinks.Add(link);
                }
            }
        }
    }
}
