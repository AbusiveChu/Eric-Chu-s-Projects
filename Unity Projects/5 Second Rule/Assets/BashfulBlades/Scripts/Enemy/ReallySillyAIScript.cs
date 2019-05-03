using UnityEngine;
using System.Collections;

public class ReallySillyAIScript : MonoBehaviour {
    public int PathProgress = 0;
    public int PathChoice = 0;
    public float EnemySpeed = 10;
    private float DistanceBetweenObject;
    private Transform TargetLookAt;
    private float DisDiff;
	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
        
       
       
            transform.LookAt(EnemySpawn.PathFlyTwoStatic[PathProgress]);
            TargetLookAt = EnemySpawn.PathFlyTwoStatic[PathProgress];
      

        DisDiff = 20;
        DistanceBetweenObject = Vector3.Distance(TargetLookAt.position, transform.position);
        if (DistanceBetweenObject > DisDiff)
        {
            transform.position += transform.forward * EnemySpeed * Time.deltaTime;
        }
        else if (DistanceBetweenObject < DisDiff)
        {
            PathProgress++;
        }
	}
}
