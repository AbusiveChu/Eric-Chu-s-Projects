using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SillyAIFlying : MonoBehaviour
{
    public int PathProgress = 0;
    public int PathChoice = 0;
    public float EnemySpeed = 10;
    private float DistanceBetweenObject;
    private Transform TargetLookAt;
    private float DisDiff;

    void Start()
    {
        PathChoice = LevelSelectEnemySpawn.randomspawn;
    }
    // Update is called once per frame
    void Update()
    {

        if (PathChoice == 1)
        {
            transform.LookAt(LevelSelectEnemySpawn.PathFlyOneStatic[PathProgress]);
            TargetLookAt = LevelSelectEnemySpawn.PathFlyOneStatic[PathProgress];
        }
        else if (PathChoice == 2)
        {
            transform.LookAt(LevelSelectEnemySpawn.PathFlyTwoStatic[PathProgress]);
            TargetLookAt = LevelSelectEnemySpawn.PathFlyTwoStatic[PathProgress];
        }
        else if (PathChoice == 3)
        {
            transform.LookAt(LevelSelectEnemySpawn.PathFlyThreeStatic[PathProgress]);
            TargetLookAt = LevelSelectEnemySpawn.PathFlyThreeStatic[PathProgress];
        }

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






















