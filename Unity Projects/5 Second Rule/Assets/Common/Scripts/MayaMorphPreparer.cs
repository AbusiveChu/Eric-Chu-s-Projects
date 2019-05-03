using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This class prepares classes exported from maya via our conventions & scripts
/// to be used with the mesh morphing script.
/// It can be applied at runtime (by attaching to an object) or at editor time
/// (via the editor script)
/// </summary>
public class MayaMorphPreparer : MonoBehaviour 
{
    /// <summary>
    /// The object that will be targeted
    /// </summary>
    public Transform targetObject = null;

    /// <summary>
    /// Should blend animations be linked
    /// </summary>
    public bool linkAnimations = true;

    /// <summary>
    /// The prefix between the object name and the blend shape.
    /// MeshName for main mesh, MeshName_BS_BlendShapeName for blend shapes
    /// </summary>
    private const string BLEND_SHAPE_PREFIX = "BlendShape_";

    // Use this for initialization
	void Awake () {
        if (targetObject == null)
        {
            Debug.LogWarning("MayaMorphPreparer used without proper targets. Not doing anything.");
            return;
        }
        
        PrepareMorphTargets(targetObject, linkAnimations);
	}

    /// <summary>
    /// Prepare a model for morphing
    /// </summary>
    /// <param name="targetObject">The object to be targeted</param>
    /// <param name="linkAnimations">Should blend weight animations be linked?</param>
    public static void PrepareMorphTargets(Transform targetObject, bool linkAnimations)
    {
        SkinnedMorphTargets morphTargets = targetObject.GetComponent<SkinnedMorphTargets>();
        if (morphTargets == null)
        {
            morphTargets = targetObject.gameObject.AddComponent<SkinnedMorphTargets>();
        }
        BlendWeightAnimationConnector animLinker = targetObject.GetComponent<BlendWeightAnimationConnector>();
        if (animLinker == null)
        {
            animLinker = targetObject.gameObject.AddComponent<BlendWeightAnimationConnector>();
        }
        animLinker.enabled = linkAnimations;
        
        Transform[] children = targetObject.GetComponentsInChildren<Transform>();
        List<Transform> childMorphTargets = new List<Transform>();
        foreach (Transform child in children)
        {
            if (child.name.StartsWith(BLEND_SHAPE_PREFIX))
            {
                childMorphTargets.Add(child);
            }
        }
        morphTargets.morphTargets = new SkinnedMorphTargets.MeshArray[childMorphTargets.Count];
        for (int i = 0; i < childMorphTargets.Count; i++)
        {
            PrepareMorphTarget(morphTargets, childMorphTargets[i], i);
        }
        morphTargets.blendWeights = new float[childMorphTargets.Count];
    }

    /// <summary>
    /// Utility function to prepare a single morph target
    /// </summary>
    /// <param name="morphTargets">The target script</param>
    /// <param name="child">The current child</param>
    /// <param name="index">The index of the child in the mesh</param>
    private static void PrepareMorphTarget(SkinnedMorphTargets morphTargets, Transform child, int index)
    {
        MeshFilter[] meshes = child.GetComponentsInChildren<MeshFilter>();
        
        SkinnedMorphTargets.MeshArray meshArray = new SkinnedMorphTargets.MeshArray();
        meshArray.name = child.name.Substring(BLEND_SHAPE_PREFIX.Length);
        meshArray.submeshes = new Mesh[meshes.Length];
        for (int i = 0; i < meshes.Length; i++)
        {
            //Not normalizing because the identity transform can't get saved back.
            //NormalizeMesh(meshes[i]);
            meshArray.submeshes[i] = meshes[i].sharedMesh;
        }
		
		morphTargets.morphTargets[index] = meshArray;
        if (Application.isEditor)
        {
            GameObject.DestroyImmediate(child.gameObject);
        }
        else
        {
            GameObject.Destroy(child.gameObject);
        }
    }
	
    /// <summary>
    /// Normalize a mesh - move its transform to identity and bake all the transforms in
    /// </summary>
    /// <param name="mf">The mesh to transform</param>
	private static void NormalizeMesh(MeshFilter mf)
    {
        Transform transform = mf.transform;
        if (transform.localPosition != Vector3.zero ||
            transform.localRotation != Quaternion.identity ||
            transform.localScale != Vector3.one)
        {
            Vector3[] vertices = mf.sharedMesh.vertices;
            Vector3[] normals = mf.sharedMesh.normals;
            for (int i = 0; i < mf.sharedMesh.vertexCount; i++)
            {
                vertices[i] = transform.TransformPoint(vertices[i]);
                normals[i] = transform.TransformDirection(normals[i]);
                //Tangents? UVs?
            }
			
            mf.sharedMesh.vertices = vertices;
            mf.sharedMesh.normals = normals;
            mf.transform.localPosition = Vector3.zero;
            mf.transform.localRotation = Quaternion.identity;
            mf.transform.localScale = Vector3.one;
        }
    }
}
