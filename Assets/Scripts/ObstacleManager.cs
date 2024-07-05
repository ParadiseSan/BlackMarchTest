using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] private ObstacleData obstacleData;
    [SerializeField] private GameObject obstaclePrefab;

    private void Start()
    {
        GenerateObstacles();
    }
    internal void GenerateObstacles()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                int index = i * 10 + j;
                if (obstacleData.obstacleGrid[index])
                {
                    Vector3 position = new Vector3(i, 0.5f, j); // Adjust height as needed
                    Instantiate(obstaclePrefab, position, Quaternion.identity);
                }
            }
        }
    }
}

