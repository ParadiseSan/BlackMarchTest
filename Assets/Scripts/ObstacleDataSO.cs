using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleData", menuName = "ScriptableObjects/ObstacleData", order = 1)]
public class ObstacleData : ScriptableObject
{
    public bool[] obstacleGrid = new bool[100]; // 10x10 grid

    public void RandomizeObstacles()
    {
        // Reset the grid
        for (int i = 0; i < obstacleGrid.Length; i++)
        {
            obstacleGrid[i] = false;
        }

        // Create a list of possible indexes
        List<int> availableIndexes = new List<int>();
        for (int i = 0; i < obstacleGrid.Length; i++)
        {
            availableIndexes.Add(i);
        }

        // Randomly select 10 indexes
        System.Random random = new System.Random();
        for (int i = 0; i < 10; i++)
        {
            if (availableIndexes.Count == 0) break;

            int randomIndex = random.Next(availableIndexes.Count);
            int chosenIndex = availableIndexes[randomIndex];
            obstacleGrid[chosenIndex] = true;

            // Remove the chosen index to prevent duplicates
            availableIndexes.RemoveAt(randomIndex);
        }
    }
}
