using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObstacleEditor : EditorWindow
{
    private ObstacleData obstacleData;

    [MenuItem("Window/Obstacle Editor")]
    public static void ShowWindow()
    {
        GetWindow<ObstacleEditor>("Obstacle Editor");
    }

    private void OnGUI()
    {
        if (obstacleData == null)
        {
            if (GUILayout.Button("Load Obstacle Data"))
            {
                LoadObstacleData();
            }
        }
        else
        {
            if (GUILayout.Button("Randomize Obstacles"))
            {
                obstacleData.RandomizeObstacles();
            }

            DrawGrid();

            if (GUILayout.Button("Save Obstacle Data"))
            {
                EditorUtility.SetDirty(obstacleData);
                AssetDatabase.SaveAssets();
            }
        }
    }

    private void LoadObstacleData()
    {
        string path = EditorUtility.OpenFilePanel("Select Obstacle Data", "Assets", "asset");
        if (path.StartsWith(Application.dataPath))
        {
            string relativePath = "Assets" + path.Substring(Application.dataPath.Length);
            obstacleData = AssetDatabase.LoadAssetAtPath<ObstacleData>(relativePath);
        }
    }

    private void DrawGrid()
    {
        GUILayout.Label("Obstacle Grid", EditorStyles.boldLabel);
        for (int y = 0; y < 10; y++)
        {
            GUILayout.BeginHorizontal();
            for (int x = 0; x < 10; x++)
            {
                int index = y * 10 + x;
                obstacleData.obstacleGrid[index] = GUILayout.Toggle(obstacleData.obstacleGrid[index], "");
            }
            GUILayout.EndHorizontal();
        }
    }
}
