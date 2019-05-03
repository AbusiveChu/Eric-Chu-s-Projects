using UnityEngine;
using System.Collections;

public class PathTrail : MonoBehaviour {

    public int PathProgress = 0;
    public int PathChoice = 0;
    public float Speed = 10;
    private float DistanceBetweenObject;
    private Transform TargetLookAt;
    private float DisDiff;
    private bool pathtoggle;
    // Use this for initialization
    void Start () {
        pathtoggle = false;
        PathChoice = PathSpawner.PathPick;
        if (PathChoice == 1)
        {
            transform.LookAt(EnemySpawn.PathOneStatic[PathProgress]);
        }
        else if (PathChoice == 2)
        {
            transform.LookAt(EnemySpawn.PathTwoStatic[PathProgress]);
        }
        else if (PathChoice == 3)
        {
            transform.LookAt(EnemySpawn.PathThreeStatic[PathProgress]);
        }
        else if (PathChoice == 4)
        {
            transform.LookAt(EnemySpawn.PathFlyOneStatic[PathProgress]);
           
        }
        else if (PathChoice == 5)
        {
            transform.LookAt(EnemySpawn.PathFlyTwoStatic[PathProgress]);
            
        }
        else if (PathChoice == 6)
        {
            transform.LookAt(EnemySpawn.PathFlyThreeStatic[PathProgress]);
          
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (PathChoice == 1)
        {

            transform.LookAt(EnemySpawn.PathOneStatic[PathProgress]);
            TargetLookAt = EnemySpawn.PathOneStatic[PathProgress];


        }
        else if (PathChoice == 2)
        {

            transform.LookAt(EnemySpawn.PathTwoStatic[PathProgress]);
            TargetLookAt = EnemySpawn.PathTwoStatic[PathProgress];

        }
        else if (PathChoice == 3)
        {

            transform.LookAt(EnemySpawn.PathThreeStatic[PathProgress]);
            TargetLookAt = EnemySpawn.PathThreeStatic[PathProgress];

        }
        else if(PathChoice == 4)
        {
            transform.LookAt(EnemySpawn.PathFlyOneStatic[PathProgress]);
            TargetLookAt = EnemySpawn.PathFlyOneStatic[PathProgress];
        }
        else if (PathChoice == 5)
        {
            transform.LookAt(EnemySpawn.PathFlyTwoStatic[PathProgress]);
            TargetLookAt = EnemySpawn.PathFlyTwoStatic[PathProgress];
        }
        else if (PathChoice == 6)
        {
            transform.LookAt(EnemySpawn.PathFlyThreeStatic[PathProgress]);
            TargetLookAt = EnemySpawn.PathFlyThreeStatic[PathProgress];
        }

        DisDiff = 10;

    

    //Enemy tries not to collide with food 

    DistanceBetweenObject = Vector3.Distance(TargetLookAt.position, transform.position);
        if (DistanceBetweenObject > DisDiff)
        {
            transform.position += transform.forward* Speed * Time.deltaTime;
        }
        else if (DistanceBetweenObject<DisDiff)
        {
            if(pathtoggle == true)
            {
                PathProgress--;
            }
            else if(pathtoggle == false)
            {
                PathProgress++;
            }
        }
        if (PathProgress <= 0)
        {
            pathtoggle = false;
        }
        if (PathProgress > 4)
        {
            pathtoggle = true;
            PathProgress--;
        }
    }
}
