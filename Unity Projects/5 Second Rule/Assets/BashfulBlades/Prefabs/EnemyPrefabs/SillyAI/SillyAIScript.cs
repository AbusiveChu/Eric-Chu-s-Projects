using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SillyAIScript : MonoBehaviour
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
            transform.LookAt(LevelSelectEnemySpawn.PathOneStatic[PathProgress]);
            TargetLookAt = LevelSelectEnemySpawn.PathOneStatic[PathProgress];
        }
        else if (PathChoice == 2)
        {
            transform.LookAt(LevelSelectEnemySpawn.PathTwoStatic[PathProgress]);
            TargetLookAt = LevelSelectEnemySpawn.PathTwoStatic[PathProgress];
        }
        else if (PathChoice == 3)
        {
            transform.LookAt(LevelSelectEnemySpawn.PathThreeStatic[PathProgress]);
            TargetLookAt = LevelSelectEnemySpawn.PathThreeStatic[PathProgress];
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
   





















